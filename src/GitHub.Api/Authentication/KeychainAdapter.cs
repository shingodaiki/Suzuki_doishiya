﻿using System.Threading.Tasks;
using Octokit;

namespace GitHub.Unity
{
    class KeychainAdapter : ICredentialStore
    {
        public Credentials OctokitCredentials { get; private set; } = Credentials.Anonymous;
        public ICredential Credential { get; private set; }

        public void Set(ICredential credential)
        {
            Credential = credential;
            OctokitCredentials = new Credentials(credential.Username, credential.Token);
        }

        public void UpdateToken(string token)
        {
            Credential.UpdateToken(token);
        }

        /// <summary>
        /// Implementation for Octokit
        /// </summary>
        /// <returns>Octokit credentials</returns>
        Task<Credentials> ICredentialStore.GetCredentials()
        {
            return TaskEx.FromResult(OctokitCredentials);
        }
    }
}
