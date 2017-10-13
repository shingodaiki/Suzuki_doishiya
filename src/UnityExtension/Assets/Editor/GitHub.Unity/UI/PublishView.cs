﻿using System;
using System.Collections.Generic;
using System.Linq;
using Octokit;
using UnityEditor;
using UnityEngine;

namespace GitHub.Unity
{
    class PublishView : Subview
    {
        private static readonly Vector2 viewSize = new Vector2(400, 350);

        private const string WindowTitle = "Publish";
        private const string Header = "Publish this repository to GitHub";
        private const string PrivateRepoMessage = "You choose who can see and commit to this repository";
        private const string PublicRepoMessage = "Anyone can see this repository. You choose who can commit";
        private const string PublishViewCreateButton = "Publish";
        private const string OwnersDefaultText = "Select a user or org";
        private const string SelectedOwnerLabel = "Owner";
        private const string RepositoryNameLabel = "Repository Name";
        private const string DescriptionLabel = "Description";
        private const string CreatePrivateRepositoryLabel = "Create as a private repository";
        private const string PublishLimtPrivateRepositoriesError = "You are currently at your limt of private repositories";
        private const string AuthenticationChangedMessageFormat = "You were authenticated as \"{0}\", but you are now authenticated as \"{1}\". Would you like to proceed or logout?";
        private const string AuthenticationChangedTitle = "Authentication Changed";
        private const string AuthenticationChangedProceed = "Proceed";
        private const string AuthenticationChangedLogout = "Logout";

        [SerializeField] private string username;
        [SerializeField] private string[] owners = { OwnersDefaultText };
        [SerializeField] private IList<Organization> organizations;
        [SerializeField] private int selectedOwner;
        [SerializeField] private string repoName = String.Empty;
        [SerializeField] private string repoDescription = "";
        [SerializeField] private bool togglePrivate;

        [NonSerialized] private IApiClient client;
        [NonSerialized] private bool isBusy;
        [NonSerialized] private string error;
        [NonSerialized] private bool ownersNeedLoading;

        public IApiClient Client
        {
            get
            {
                if (client == null)
                {
                    var repository = Environment.Repository;
                    UriString host;
                    if (repository != null && !string.IsNullOrEmpty(repository.CloneUrl))
                    {
                        host = repository.CloneUrl.ToRepositoryUrl();
                    }
                    else
                    {
                        host = UriString.ToUriString(HostAddress.GitHubDotComHostAddress.WebUri);
                    }

                    client = ApiClient.Create(host, Platform.Keychain);
                }

                return client;
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            ownersNeedLoading = organizations == null && !isBusy;
        }

        public override void OnDataUpdate()
        {
            base.OnDataUpdate();
            MaybeUpdateData();
        }

        private void MaybeUpdateData()
        {
            if (ownersNeedLoading)
            {
                ownersNeedLoading = false;
                LoadOwners();
            }
        }

        public override void InitializeView(IView parent)
        {
            base.InitializeView(parent);
            Title = WindowTitle;
            Size = viewSize;
        }

        private void LoadOwners()
        {
            var keychainConnections = Platform.Keychain.Connections;
            //TODO: ONE_USER_LOGIN This assumes only ever one user can login

            isBusy = true;

            //TODO: ONE_USER_LOGIN This assumes only ever one user can login
            username = keychainConnections.First().Username;

            Logger.Trace("Loading Owners");

            Client.GetOrganizations(orgs =>
            {
                organizations = orgs;
                Logger.Trace("Loaded {0} Owners", organizations.Count);

                var organizationLogins = organizations
                    .OrderBy(organization => organization.Login)
                    .Select(organization => organization.Login);

                owners = new[] { OwnersDefaultText, username }.Union(organizationLogins).ToArray();

                isBusy = false;
            }, exception =>
            {
                isBusy = false;

                var tokenUsernameMismatchException = exception as TokenUsernameMismatchException;
                if (tokenUsernameMismatchException != null)
                {
                    Logger.Trace("Token Username Mismatch");

                    var shouldProceed = EditorUtility.DisplayDialog(AuthenticationChangedTitle,
                        string.Format(AuthenticationChangedMessageFormat,
                            tokenUsernameMismatchException.CachedUsername,
                            tokenUsernameMismatchException.CurrentUsername), AuthenticationChangedProceed, AuthenticationChangedLogout);

                    if (shouldProceed)
                    {
                        //Proceed as current user

                    }
                    else
                    {
                        //Logout current user and try again

                    }
                    return;
                }

                var keychainEmptyException = exception as KeychainEmptyException;
                if (keychainEmptyException != null)
                {
                    Logger.Trace("Keychain empty");
                    PopupWindow.OpenWindow(PopupWindow.PopupViewType.AuthenticationView);
                    return;
                }

                Logger.Error(exception, "Unhandled Exception");
            });
        }

        public override void OnGUI()
        {
            GUILayout.BeginHorizontal(Styles.AuthHeaderBoxStyle);
            {
                GUILayout.BeginVertical(GUILayout.Width(16));
                {
                    GUILayout.Space(9);
                    GUILayout.Label(Styles.BigLogo, GUILayout.Height(20), GUILayout.Width(20));
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    GUILayout.Space(11);
                    GUILayout.Label(Title, EditorStyles.boldLabel);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(Styles.PublishViewSpacingHeight);

            EditorGUI.BeginDisabledGroup(isBusy);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.BeginVertical();
                    {
                        GUILayout.Label(SelectedOwnerLabel);
                        selectedOwner = EditorGUILayout.Popup(selectedOwner, owners);
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.Width(8));
                    {
                        GUILayout.Space(20);
                        GUILayout.Label("/");
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                    {
                        GUILayout.Label(RepositoryNameLabel);
                        repoName = EditorGUILayout.TextField(repoName);
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();

                GUILayout.Label(DescriptionLabel);
                repoDescription = EditorGUILayout.TextField(repoDescription);
                GUILayout.Space(Styles.PublishViewSpacingHeight);

                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        togglePrivate = GUILayout.Toggle(togglePrivate, CreatePrivateRepositoryLabel);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(Styles.PublishViewSpacingHeight);
                        var repoPrivacyExplanation = togglePrivate ? PrivateRepoMessage : PublicRepoMessage;
                        GUILayout.Label(repoPrivacyExplanation, Styles.LongMessageStyle);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                GUILayout.Space(Styles.PublishViewSpacingHeight);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    EditorGUI.BeginDisabledGroup(!IsFormValid);
                    if (GUILayout.Button(PublishViewCreateButton))
                    {
                        GUI.FocusControl(null);
                        isBusy = true;

                        var organization = owners[selectedOwner] == username ? null : owners[selectedOwner];

                        var cleanRepoDescription = repoDescription.Trim();
                        cleanRepoDescription = cleanRepoDescription == string.Empty ? null : cleanRepoDescription;

                        Client.CreateRepository(new NewRepository(repoName)
                        {
                            Private = togglePrivate,
                            Description = cleanRepoDescription
                        }, (repository, ex) =>
                        {
                            if (ex != null)
                            {
                                Logger.Error(ex, "Repository Create Error Type:{0}", ex.GetType().ToString());

                                error = ex.Message;
                                isBusy = false;
                                return;
                            }

                            if (repository == null)
                            {
                                Logger.Warning("Returned Repository is null");
                                isBusy = false;
                                return;
                            }

                            Logger.Trace("Repository Created");

                            GitClient.RemoteAdd("origin", repository.CloneUrl)
                                     .Then(GitClient.Push("origin", Repository.CurrentBranch.Value.Name))
                                     .ThenInUI(Finish)
                                     .Start();
                        }, organization);
                    }
                    EditorGUI.EndDisabledGroup();
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(10);

                if (error != null)
                    GUILayout.Label(error, Styles.ErrorLabel);

                GUILayout.FlexibleSpace();
            }
            EditorGUI.EndDisabledGroup();
        }

        public override bool IsBusy
        {
            get { return isBusy; }
        }

        private bool IsFormValid
        {
            get { return !string.IsNullOrEmpty(repoName) && !isBusy && selectedOwner != 0; }
        }
    }
}
