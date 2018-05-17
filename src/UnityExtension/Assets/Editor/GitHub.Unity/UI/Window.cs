using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GitHub.Unity
{
    [Serializable]
    class Window : BaseWindow
    {
        private const float DefaultNotificationTimeout = 4f;
        private const string Title = "GitHub";
        private const string Menu_Window_GitHub = "Window/GitHub";
        private const string Menu_Window_GitHub_Command_Line = "Window/GitHub Command Line";
        private const string LocksTitle = "Locks";

        [NonSerialized] private double notificationClearTime = -1;
        [NonSerialized] private double timeSinceLastRotation = -1f;
        [NonSerialized] private Spinner spinner;
        [NonSerialized] private IProgress progress;
        [NonSerialized] private float progressValue;
        [NonSerialized] private string progressMessage;

        [SerializeField] private bool currentBranchAndRemoteHasUpdate;
        [SerializeField] private bool currentTrackingStatusHasUpdate;
        [SerializeField] private bool currentStatusEntriesHasUpdate;
        [SerializeField] private SubTab changeTab = SubTab.InitProject;
        [SerializeField] private SubTab activeTab = SubTab.InitProject;
        [SerializeField] private InitProjectView initProjectView = new InitProjectView();
        [SerializeField] private BranchesView branchesView = new BranchesView();
        [SerializeField] private ChangesView changesView = new ChangesView();
        [SerializeField] private HistoryView historyView = new HistoryView();
        [SerializeField] private SettingsView settingsView = new SettingsView();
        [SerializeField] private LocksView locksView = new LocksView();
        [SerializeField] private bool hasRemote;
        [SerializeField] private string currentRemoteName;
        [SerializeField] private string currentBranch;
        [SerializeField] private string currentRemoteUrl;
        [SerializeField] private int statusAhead;
        [SerializeField] private int statusBehind;
        [SerializeField] private bool hasItemsToCommit;
        [SerializeField] private GUIContent currentBranchContent;
        [SerializeField] private GUIContent currentRemoteUrlContent;
        [SerializeField] private CacheUpdateEvent lastCurrentBranchAndRemoteChangedEvent;
        [SerializeField] private CacheUpdateEvent lastTrackingStatusChangedEvent;
        [SerializeField] private CacheUpdateEvent lastStatusEntriesChangedEvent;

        [MenuItem(Menu_Window_GitHub)]
        public static void Window_GitHub()
        {
            ShowWindow(EntryPoint.ApplicationManager);
        }

        [MenuItem(Menu_Window_GitHub_Command_Line)]
        public static void GitHub_CommandLine()
        {
            EntryPoint.ApplicationManager.ProcessManager.RunCommandLineWindow(NPath.CurrentDirectory);
            EntryPoint.ApplicationManager.TaskManager.Run(EntryPoint.ApplicationManager.UsageTracker.IncrementApplicationMenuMenuItemCommandLine);
        }

#if DEBUG
        [MenuItem("GitHub/Select Window")]
        public static void GitHub_SelectWindow()
        {
            var window = Resources.FindObjectsOfTypeAll(typeof(Window)).FirstOrDefault() as Window;
            Selection.activeObject = window;
        }

        [MenuItem("GitHub/Restart")]
        public static void GitHub_Restart()
        {
            EntryPoint.Restart();
        }
#endif

        public static void ShowWindow(IApplicationManager applicationManager)
        {
            var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
            var window = GetWindow<Window>(type);
            window.InitializeWindow(applicationManager);
            window.Show();
        }

        public static Window GetWindow()
        {
            return Resources.FindObjectsOfTypeAll(typeof(Window)).FirstOrDefault() as Window;
        }

        public override void Initialize(IApplicationManager applicationManager)
        {
            base.Initialize(applicationManager);

            applicationManager.OnProgress += OnProgress;

            HistoryView.InitializeView(this);
            ChangesView.InitializeView(this);
            BranchesView.InitializeView(this);
            SettingsView.InitializeView(this);
            LocksView.InitializeView(this);
            InitProjectView.InitializeView(this);

            titleContent = new GUIContent(Title, Styles.SmallLogo);

            if (!HasRepository)
            {
                changeTab = activeTab = SubTab.InitProject;
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();

#if DEVELOPER_BUILD
            Selection.activeObject = this;
#endif
            if (Repository != null)
                ValidateCachedData(Repository);

            if (ActiveView != null)
                ActiveView.OnEnable();

            if (spinner == null)
                spinner = new Spinner();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (ActiveView != null)
                ActiveView.OnDisable();
        }

        public override void OnDataUpdate()
        {
            base.OnDataUpdate();
            MaybeUpdateData();

            if (ActiveView != null)
                ActiveView.OnDataUpdate();
        }

        public override void OnFocusChanged()
        {
            if (ActiveView != null)
                ActiveView.OnFocusChanged();
        }

        public override void OnRepositoryChanged(IRepository oldRepository)
        {
            base.OnRepositoryChanged(oldRepository);

            DetachHandlers(oldRepository);
            AttachHandlers(Repository);

            if (HasRepository)
            {
                if (activeTab == SubTab.InitProject)
                {
                    changeTab = SubTab.History;
                    UpdateActiveTab();
                }
            }
            else
            {
                if (activeTab != SubTab.InitProject)
                {
                    changeTab = SubTab.InitProject;
                    UpdateActiveTab();
                }
            }
        }

        public override void OnSelectionChange()
        {
            base.OnSelectionChange();
            if (ActiveView != null)
                ActiveView.OnSelectionChange();
        }

        public override void Refresh()
        {
            base.Refresh();
            if (ActiveView != null)
                ActiveView.Refresh();
            Repaint();
        }


        public override void OnUI()
        {
            base.OnUI();

            if (HasRepository)
            {
                DoActionbarGUI();
                DoHeaderGUI();
            }

            DoToolbarGUI();

            var rect = GUILayoutUtility.GetLastRect();
            // GUI for the active tab
            if (ActiveView != null)
            {
                ActiveView.OnGUI();
            }

            if (IsBusy && activeTab != SubTab.Settings && Event.current.type == EventType.Repaint)
            {
                if (timeSinceLastRotation < 0)
                {
                    timeSinceLastRotation = EditorApplication.timeSinceStartup;
                }
                else
                {
                    var elapsedTime = (float)(EditorApplication.timeSinceStartup - timeSinceLastRotation);
                    if (spinner == null)
                        spinner = new Spinner();
                    spinner.Start(elapsedTime);
                    spinner.Rotate(elapsedTime);

                    spinner.Render();

                    rect = new Rect(0f, rect.y + rect.height, Position.width, Position.height - (rect.height + rect.y));
                    rect = spinner.Layout(rect);
                    rect.y += rect.height + 30;
                    rect.height = 20;
                    if (!String.IsNullOrEmpty(progressMessage))
                        EditorGUI.ProgressBar(rect, progressValue / 100, progressMessage);
                }
            }
        }

        public override void Update()
        {
            base.Update();

            // Notification auto-clear timer override
            if (notificationClearTime > 0f && EditorApplication.timeSinceStartup > notificationClearTime)
            {
                notificationClearTime = -1f;
                RemoveNotification();
                Redraw();
            }

            if (IsBusy && activeTab != SubTab.Settings)
            {
                Redraw();
            }
            else
            {
                timeSinceLastRotation = -1f;
                spinner.Stop();
            }
        }

        private void ValidateCachedData(IRepository repository)
        {
            repository.CheckAndRaiseEventsIfCacheNewer(CacheType.RepositoryInfo, lastCurrentBranchAndRemoteChangedEvent);
        }

        private void MaybeUpdateData()
        {
            if (progress != null)
            {
                progressValue = progress.Value;
                progressMessage = progress.Message;
            }

            string updatedRepoRemote = null;
            string updatedRepoUrl = Localization.DefaultRepoUrl;

            var shouldUpdateContentFields = false;

            if (currentTrackingStatusHasUpdate)
            {
                currentTrackingStatusHasUpdate = false;
                statusAhead = Repository.CurrentAhead;
                statusBehind = Repository.CurrentBehind;
            }

            if (currentStatusEntriesHasUpdate)
            {
                currentStatusEntriesHasUpdate = false;
                var currentChanges = Repository.CurrentChanges;
                hasItemsToCommit = currentChanges != null &&
                    currentChanges.Any(entry => entry.Status != GitFileStatus.Ignored && !entry.Staged);
            }

            if (currentBranchAndRemoteHasUpdate)
            {
                hasRemote = false;
            }

            if (Repository != null)
            {
                if (currentBranch == null || currentRemoteName == null || currentBranchAndRemoteHasUpdate)
                {
                    currentBranchAndRemoteHasUpdate = false;

                    var repositoryCurrentBranch = Repository.CurrentBranch;
                    var updatedRepoBranch = repositoryCurrentBranch.HasValue ? repositoryCurrentBranch.Value.Name : null;

                    var repositoryCurrentRemote = Repository.CurrentRemote;
                    if (repositoryCurrentRemote.HasValue)
                    {
                        hasRemote = true;
                        updatedRepoRemote = repositoryCurrentRemote.Value.Name;
                        if (!string.IsNullOrEmpty(repositoryCurrentRemote.Value.Url))
                        {
                            updatedRepoUrl = repositoryCurrentRemote.Value.Url;
                        }
                    }

                    if (currentRemoteName != updatedRepoRemote)
                    {
                        currentRemoteName = updatedRepoBranch;
                        shouldUpdateContentFields = true;
                    }

                    if (currentBranch != updatedRepoBranch)
                    {
                        currentBranch = updatedRepoBranch;
                        shouldUpdateContentFields = true;
                    }

                    if (currentRemoteUrl != updatedRepoUrl)
                    {
                        currentRemoteUrl = updatedRepoUrl;
                        shouldUpdateContentFields = true;
                    }
                }
            }
            else
            {
                if (currentRemoteName != null)
                {
                    currentRemoteName = null;
                    shouldUpdateContentFields = true;
                }

                if (currentBranch != null)
                {
                    currentBranch = null;
                    shouldUpdateContentFields = true;
                }

                if (currentRemoteUrl != Localization.DefaultRepoUrl)
                {
                    currentRemoteUrl = Localization.DefaultRepoUrl;
                    shouldUpdateContentFields = true;
                }
            }

            if (shouldUpdateContentFields || currentBranchContent == null || currentRemoteUrlContent == null)
            {
                currentBranchContent = new GUIContent(currentBranch, Localization.Window_RepoBranchTooltip);

                if (currentRemoteName != null)
                {
                    currentRemoteUrlContent = new GUIContent(currentRemoteUrl, string.Format(Localization.Window_RepoUrlTooltip, currentRemoteName));
                }
                else
                {
                    currentRemoteUrlContent = new GUIContent(currentRemoteUrl, Localization.Window_RepoNoUrlTooltip);
                }
            }
        }

        private void AttachHandlers(IRepository repository)
        {
            if (repository == null)
                return;
            repository.CurrentBranchAndRemoteChanged += RepositoryOnCurrentBranchAndRemoteChanged;
            repository.TrackingStatusChanged += RepositoryOnTrackingStatusChanged;
            repository.StatusEntriesChanged += RepositoryOnStatusEntriesChanged;
        }

        private void RepositoryOnCurrentBranchAndRemoteChanged(CacheUpdateEvent cacheUpdateEvent)
        {
            if (!lastCurrentBranchAndRemoteChangedEvent.Equals(cacheUpdateEvent))
            {
                lastCurrentBranchAndRemoteChangedEvent = cacheUpdateEvent;
                currentBranchAndRemoteHasUpdate = true;
                Redraw();
            }
        }

        private void RepositoryOnTrackingStatusChanged(CacheUpdateEvent cacheUpdateEvent)
        {
            if (!lastTrackingStatusChangedEvent.Equals(cacheUpdateEvent))
            {
                lastTrackingStatusChangedEvent = cacheUpdateEvent;
                currentTrackingStatusHasUpdate = true;
                Redraw();
            }
        }

        private void RepositoryOnStatusEntriesChanged(CacheUpdateEvent cacheUpdateEvent)
        {
            if (!lastStatusEntriesChangedEvent.Equals(cacheUpdateEvent))
            {
                lastStatusEntriesChangedEvent = cacheUpdateEvent;
                currentStatusEntriesHasUpdate = true;
                Redraw();
            }
        }

        private void OnProgress(IProgress progr)
        {
            progress = progr;
        }

        private void DetachHandlers(IRepository repository)
        {
            if (repository == null)
                return;
            repository.CurrentBranchAndRemoteChanged -= RepositoryOnCurrentBranchAndRemoteChanged;
            repository.TrackingStatusChanged -= RepositoryOnTrackingStatusChanged;
            repository.StatusEntriesChanged -= RepositoryOnStatusEntriesChanged;
        }

        private void DoHeaderGUI()
        {
            GUILayout.BeginHorizontal(Styles.HeaderBoxStyle);
            {
                GUILayout.Space(3);
                GUILayout.BeginVertical(GUILayout.Width(16));
                {
                    GUILayout.Space(9);
                    GUILayout.Label(Styles.RepoIcon, GUILayout.Height(20), GUILayout.Width(20));
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    GUILayout.Space(3);

                    GUILayout.Label(currentRemoteUrlContent, Styles.HeaderRepoLabelStyle);
                    GUILayout.Space(-2);
                    GUILayout.Label(currentBranchContent, Styles.HeaderBranchLabelStyle);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void DoToolbarGUI()
        {
            // Subtabs & toolbar
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                EditorGUI.BeginChangeCheck();
                {
                    if (HasRepository)
                    {
                        changeTab = TabButton(SubTab.Changes, Localization.ChangesTitle, changeTab);
                        changeTab = TabButton(SubTab.History, Localization.HistoryTitle, changeTab);
                        changeTab = TabButton(SubTab.Branches, Localization.BranchesTitle, changeTab);
                        changeTab = TabButton(SubTab.Locks, LocksTitle, changeTab);
                    }
                    else if (!HasRepository)
                    {
                        changeTab = TabButton(SubTab.InitProject, Localization.InitializeTitle, changeTab);
                    }
                    changeTab = TabButton(SubTab.Settings, Localization.SettingsTitle, changeTab);
                }

                if (EditorGUI.EndChangeCheck())
                {
                    UpdateActiveTab();
                }

                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DoActionbarGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                if (hasRemote)
                {
                    EditorGUI.BeginDisabledGroup(currentRemoteName == null);
                    {
                        // Fetch button
                        var fetchClicked = GUILayout.Button(Localization.FetchButtonText, Styles.ToolbarButtonStyle);
                        if (fetchClicked)
                        {
                            Fetch();
                        }

                        // Pull button
                        var pullButtonText = statusBehind > 0 ? String.Format(Localization.PullButtonCount, statusBehind) : Localization.PullButton;
                        var pullClicked = GUILayout.Button(pullButtonText, Styles.ToolbarButtonStyle);

                        if (pullClicked &&
                            EditorUtility.DisplayDialog(Localization.PullConfirmTitle,
                                String.Format(Localization.PullConfirmDescription, currentRemoteName),
                                Localization.PullConfirmYes,
                                Localization.PullConfirmCancel)
                        )
                        {
                            Pull();
                        }
                    }
                    EditorGUI.EndDisabledGroup();

                    // Push button
                    EditorGUI.BeginDisabledGroup(currentRemoteName == null || statusBehind != 0);
                    {
                        var pushButtonText = statusAhead > 0 ? String.Format(Localization.PushButtonCount, statusAhead) : Localization.PushButton;
                        var pushClicked = GUILayout.Button(pushButtonText, Styles.ToolbarButtonStyle);

                        if (pushClicked &&
                            EditorUtility.DisplayDialog(Localization.PushConfirmTitle,
                                String.Format(Localization.PushConfirmDescription, currentRemoteName),
                                Localization.PushConfirmYes,
                                Localization.PushConfirmCancel)
                        )
                        {
                            Push();
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    // Publishing a repo
                    if (GUILayout.Button(Localization.PublishButton, Styles.ToolbarButtonStyle))
                    {
                        PopupWindow.OpenWindow(PopupWindow.PopupViewType.PublishView);
                    }
                }

                if (GUILayout.Button(Localization.RefreshButton, Styles.ToolbarButtonStyle))
                {
                    Refresh();
                }

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Account", EditorStyles.toolbarDropDown))
                    DoAccountDropdown();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void Pull()
        {
            if (hasItemsToCommit)
            {
                EditorUtility.DisplayDialog("Pull", "You need to commit your changes before pulling.", "Cancel");
            }
            else
            {
                Repository
                    .Pull()
                    .FinallyInUI((success, e) => {
                        if (success)
                        {
                            TaskManager.Run(EntryPoint.ApplicationManager.UsageTracker.IncrementHistoryViewToolbarPull);

                            EditorUtility.DisplayDialog(Localization.PullActionTitle,
                                String.Format(Localization.PullSuccessDescription, currentRemoteName),
                            Localization.Ok);
                        }
                        else
                        {
                            EditorUtility.DisplayDialog(Localization.PullActionTitle,
                                Localization.PullFailureDescription,
                            Localization.Ok);
                        }
                    })
                    .Start();
            }
        }

        private void Push()
        {
            Repository
                .Push()
                .FinallyInUI((success, e) => {
                    if (success)
                    {
                        TaskManager.Run(EntryPoint.ApplicationManager.UsageTracker.IncrementHistoryViewToolbarPush);

                        EditorUtility.DisplayDialog(Localization.PushActionTitle,
                            String.Format(Localization.PushSuccessDescription, currentRemoteName),
                        Localization.Ok);
                    }
                    else
                    {
                        EditorUtility.DisplayDialog(Localization.PushActionTitle,
                            Localization.PushFailureDescription,
                        Localization.Ok);
                    }
                })
                .Start();
        }

        private void Fetch()
        {
            Repository
                .Fetch()
                .FinallyInUI((success, e) => {
                    if (!success)
                    {
                        TaskManager.Run(EntryPoint.ApplicationManager.UsageTracker.IncrementHistoryViewToolbarFetch);

                        EditorUtility.DisplayDialog(Localization.FetchActionTitle, Localization.FetchFailureDescription,
                            Localization.Ok);
                    }
                })
                .Start();
        }

        private void UpdateActiveTab()
        {
            if (changeTab != activeTab)
            {
                var fromView = ActiveView;
                activeTab = changeTab;
                var toView = ActiveView;
                SwitchView(fromView, toView);
            }
        }

        private void SwitchView(Subview fromView, Subview toView)
        {
            GUI.FocusControl(null);

            if (fromView != null)
                fromView.OnDisable();

            toView.OnEnable();
            toView.OnDataUpdate();

            // this triggers a repaint
            Repaint();
        }

        private void DoAccountDropdown()
        {
            GenericMenu accountMenu = new GenericMenu();

            if (!Platform.Keychain.HasKeys)
            {
                accountMenu.AddItem(new GUIContent("Sign in"), false, SignIn, "sign in");
            }
            else
            {
                accountMenu.AddItem(new GUIContent("Go to Profile"), false, GoToProfile, "profile");
                accountMenu.AddSeparator("");
                accountMenu.AddItem(new GUIContent("Sign out"), false, SignOut, "sign out");
            }
            accountMenu.ShowAsContext();
        }

        private void SignIn(object obj)
        {
            PopupWindow.OpenWindow(PopupWindow.PopupViewType.AuthenticationView);
        }

        private void GoToProfile(object obj)
        {
            //TODO: ONE_USER_LOGIN This assumes only ever one user can login
            var keychainConnection = Platform.Keychain.Connections.First();
            var uriString = new UriString(keychainConnection.Host).Combine(keychainConnection.Username);
            Application.OpenURL(uriString);
        }

        private void SignOut(object obj)
        {
            UriString host;
            if (Repository != null && Repository.CloneUrl != null && Repository.CloneUrl.IsValidUri)
            {
                host = new UriString(Repository.CloneUrl.ToRepositoryUri()
                                               .GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped));
            }
            else
            {
                host = UriString.ToUriString(HostAddress.GitHubDotComHostAddress.WebUri);
            }

            var apiClient = new ApiClient(host, Platform.Keychain, null, null, NPath.Default, NPath.Default);
            apiClient.Logout(host);
        }

        public new void ShowNotification(GUIContent content)
        {
            ShowNotification(content, DefaultNotificationTimeout);
        }

        public void ShowNotification(GUIContent content, float timeout)
        {
            notificationClearTime = timeout < DefaultNotificationTimeout ? EditorApplication.timeSinceStartup + timeout : -1f;
            base.ShowNotification(content);
        }

        private static SubTab TabButton(SubTab tab, string title, SubTab currentTab)
        {
            return GUILayout.Toggle(currentTab == tab, title, EditorStyles.toolbarButton) ? tab : currentTab;
        }

        private Subview ToView(SubTab tab)
        {
            switch (tab)
            {
                case SubTab.InitProject:
                    return initProjectView;
                case SubTab.History:
                    return historyView;
                case SubTab.Changes:
                    return changesView;
                case SubTab.Branches:
                    return branchesView;
                case SubTab.Settings:
                    return settingsView;
                case SubTab.Locks:
                    return locksView;
                default:
                    throw new ArgumentOutOfRangeException("tab");
            }
        }

        public HistoryView HistoryView
        {
            get { return historyView; }
        }

        public ChangesView ChangesView
        {
            get { return changesView; }
        }

        public BranchesView BranchesView
        {
            get { return branchesView; }
        }

        public SettingsView SettingsView
        {
            get { return settingsView; }
        }

        public LocksView LocksView
        {
            get { return locksView; }
        }

        public InitProjectView InitProjectView
        {
            get { return initProjectView; }
        }

        private Subview ActiveView
        {
            get { return ToView(activeTab); }
        }

        public override bool IsBusy
        {
            get { return Manager.IsBusy; }
        }

        private enum SubTab
        {
            None,
            InitProject,
            History,
            Changes,
            Branches,
            Settings,
            Locks
        }
    }
}
