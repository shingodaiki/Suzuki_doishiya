﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Octokit;

namespace GitHub.Api
{
    class ApiClient : IApiClient
    {
        private static readonly Unity.ILogging logger = Unity.Logging.GetLogger<ApiClient>();
        public HostAddress HostAddress { get; }
        public UriString OriginalUrl { get; }

        private readonly IGitHubClient githubClient;
        private readonly IGitHubClient githubAppClient;
        private readonly ICredentialManager credentialManager;
        private readonly ILoginManager loginManager;
        private static readonly SemaphoreSlim sem = new SemaphoreSlim(1);

        Octokit.Repository repositoryCache = new Octokit.Repository();
        string owner;
        bool? isEnterprise;

        public ApiClient(UriString hostUrl, ICredentialManager credentialManager, IGitHubClient githubClient, IGitHubClient githubAppClient)
        {
            Guard.ArgumentNotNull(hostUrl, nameof(hostUrl));
            Guard.ArgumentNotNull(credentialManager, nameof(credentialManager));
            Guard.ArgumentNotNull(githubClient, nameof(githubClient));

            HostAddress = HostAddress.Create(hostUrl);
            OriginalUrl = hostUrl;
            this.githubClient = githubClient;
            this.githubAppClient = githubAppClient;
            this.credentialManager = credentialManager;
            loginManager = new LoginManager(credentialManager, ApplicationInfo.ClientId, ApplicationInfo.ClientSecret);
        }

        public async void GetRepository(Action<Octokit.Repository> callback)
        {
            Guard.ArgumentNotNull(callback, "callback");
            var repo = await GetRepositoryInternal();
            callback(repo);
        }

        public async Task Login(string username, string password, Action<LoginResult> need2faCode, Action<bool, string> result)
        {
            Guard.ArgumentNotNull(need2faCode, "need2faCode");
            Guard.ArgumentNotNull(result, "result");

            LoginResultData res = null;
            try
            {
                res = await loginManager.Login(OriginalUrl, githubClient, username, password);
            }
            catch (Exception ex)
            {
                result(false, ex.Message);
                return;
            }

            if (res.Code == LoginResultCodes.CodeRequired)
            {
                var resultCache = new LoginResult(res, result, need2faCode);
                need2faCode(resultCache);
            }
            else
            {
                result(res.Code == LoginResultCodes.Success, res.Message);
            }
        }

        public async Task ContinueLogin(LoginResult loginResult, string code)
        {
            LoginResultData result = null;
            try
            {
                result = await loginManager.ContinueLogin(loginResult.Data, code);
            }
            catch (Exception ex)
            {
                loginResult.Callback(false, ex.Message);
                return;
            }
            if (result.Code == LoginResultCodes.CodeFailed)
            {
                loginResult.TwoFACallback(new LoginResult(result, loginResult.Callback, loginResult.TwoFACallback));
            }
            loginResult.Callback(result.Code == LoginResultCodes.Success, result.Message);
        }

        public async Task<bool> LoginAsync(string username, string password, Func<LoginResult, string> need2faCode)
        {
            Guard.ArgumentNotNull(need2faCode, "need2faCode");

            LoginResultData res = null;
            try
            {
                res = await loginManager.Login(OriginalUrl, githubClient, username, password);
            }
            catch (Exception)
            {
                return false;
            }

            if (res.Code == LoginResultCodes.CodeRequired)
            {
                var resultCache = new LoginResult(res, null, null);
                var code = need2faCode(resultCache);
                return await ContinueLoginAsync(resultCache, need2faCode, code);
            }
            else
            {
                return res.Code == LoginResultCodes.Success;
            }
        }

        public async Task<bool> ContinueLoginAsync(LoginResult loginResult, Func<LoginResult, string> need2faCode, string code)
        {
            LoginResultData result = null;
            try
            {
                result = await loginManager.ContinueLogin(loginResult.Data, code);
            }
            catch (Exception)
            {
                return false;
            }

            if (result.Code == LoginResultCodes.CodeFailed)
            {
                var resultCache = new LoginResult(result, null, null);
                code = need2faCode(resultCache);
                if (String.IsNullOrEmpty(code))
                    return false;
                return await ContinueLoginAsync(resultCache, need2faCode, code);
            }
            return result.Code == LoginResultCodes.Success;
        }

        private async Task<Octokit.Repository> GetRepositoryInternal()
        {
            try
            {
                if (owner == null)
                {
                    var ownerLogin = OriginalUrl.Owner;
                    var repositoryName = OriginalUrl.RepositoryName;

                    if (ownerLogin != null && repositoryName != null)
                    {
                        var repo = await githubClient.Repository.Get(ownerLogin, repositoryName);
                        if (repo != null)
                        {
                            repositoryCache = repo;
                        }
                        owner = ownerLogin;
                    }
                }
            }
            // it'll throw if it's private or an enterprise instance requiring authentication
            catch (ApiException apiex)
            {
                if (!HostAddress.IsGitHubDotComUri(OriginalUrl.ToRepositoryUri()))
                    isEnterprise = apiex.IsGitHubApiException();
            }
            catch {}
            finally
            {
                sem.Release();
            }

            return repositoryCache;
        }

        public async Task<bool> ValidateCredentials()
        {
            try
            {
                var credential = await credentialManager.Load(OriginalUrl);
                if (credential != null)
                {
                    await githubAppClient.Authorization.CheckApplicationAuthentication(ApplicationInfo.ClientId, credential.Token);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
