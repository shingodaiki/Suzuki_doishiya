#pragma warning disable 649

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
        private const string LaunchMenu = "Window/GitHub";
        private const string RefreshButton = "Refresh";
        private const string UnknownSubTabError = "Unsupported view mode: {0}";
        private const string BadNotificationDelayError = "A delay of {0} is shorter than the default delay and thus would get pre-empted.";
        private const string HistoryTitle = "History";
        private const string ChangesTitle = "Changes";
        private const string BranchesTitle = "Branches";
        private const string SettingsTitle = "Settings";
        private const string AuthenticationTitle = "Auth";
        private const string DefaultRepoUrl = "No remote configured";
        private const string Window_RepoUrlTooltip = "Url of the {0} remote";
        private const string Window_RepoNoUrlTooltip = "Add a remote in the Settings tab";
        private const string Window_RepoBranchTooltip = "Active branch";

        [NonSerialized] private double notificationClearTime = -1;

        [SerializeField] private SubTab activeTab = SubTab.History;
        [SerializeField] private BranchesView branchesTab = new BranchesView();
        [SerializeField] private ChangesView changesTab = new ChangesView();
        [SerializeField] private HistoryView historyTab = new HistoryView();
        [SerializeField] private SettingsView settingsTab = new SettingsView();

        [SerializeField] private string repoBranch;
        [SerializeField] private string repoUrl;
        [SerializeField] private GUIContent repoBranchContent;
        [SerializeField] private GUIContent repoUrlContent;

        [MenuItem(LaunchMenu)]
        public static void Window_GitHub()
        {
            ShowWindow(EntryPoint.ApplicationManager);
        }

        [MenuItem("GitHub/Show Window")]
        public static void GitHub_ShowWindow()
        {
            ShowWindow(EntryPoint.ApplicationManager);
        }

        [MenuItem("GitHub/Command Line")]
        public static void GitHub_CommandLine()
        {
            EntryPoint.ApplicationManager.ProcessManager.RunCommandLineWindow(NPath.CurrentDirectory);
        }

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

            HistoryTab.InitializeView(this);
            ChangesTab.InitializeView(this);
            BranchesTab.InitializeView(this);
            SettingsTab.InitializeView(this);
        }

        public override void OnEnable()
        {
            base.OnEnable();

#if DEVELOPER_BUILD
            Selection.activeObject = this;
#endif

            // Set window title
            titleContent = new GUIContent(Title, Styles.SmallLogo);

            if (ActiveTab != null)
                ActiveTab.OnEnable();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (ActiveTab != null)
                ActiveTab.OnDisable();
        }

        public override void OnDataUpdate()
        {
            base.OnDataUpdate();

            string repoRemote = null;
            if (MaybeUpdateData(out repoRemote))
            {
                repoBranchContent = new GUIContent(repoBranch, Window_RepoBranchTooltip);
                if (repoUrl != null)
                {
                    repoUrlContent = new GUIContent(repoUrl, string.Format(Window_RepoUrlTooltip, repoRemote));
                }
                else
                {
                    repoUrlContent = new GUIContent(repoUrl, Window_RepoNoUrlTooltip);
                }
            }

            if (ActiveTab != null)
                ActiveTab.OnDataUpdate();
        }

        public override void OnRepositoryChanged(IRepository oldRepository)
        {
            base.OnRepositoryChanged(oldRepository);

            DetachHandlers(oldRepository);
            AttachHandlers(Repository);

            if (ActiveTab != null)
                ActiveTab.OnRepositoryChanged(oldRepository);
        }

        public override void OnSelectionChange()
        {
            base.OnSelectionChange();
            if (ActiveTab != null)
                ActiveTab.OnSelectionChange();
        }

        public override void Refresh()
        {
            base.Refresh();
            if (ActiveTab != null)
                ActiveTab.Refresh();
            Repaint();
        }

        public override void OnUI()
        {
            base.OnUI();

            if (HasRepository)
            {
                DoHeaderGUI();
            }

            DoToolbarGUI();

            // GUI for the active tab
            if (ActiveTab != null)
            {
                ActiveTab.OnGUI();
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
        }

        private void RefreshOnMainThread()
        {
            new ActionTask(TaskManager.Token, Refresh) { Affinity = TaskAffinity.UI }.Start();
        }

        private bool MaybeUpdateData(out string repoRemote)
        {
            repoRemote = null;
            bool repoDataChanged = false;
            if (Repository != null)
            {
                var currentBranchString = (Repository.CurrentBranch.HasValue ? Repository.CurrentBranch.Value.Name : null);
                if (repoBranch != currentBranchString)
                {
                    repoBranch = currentBranchString;
                    repoDataChanged = true;
                }

                var url = Repository.CloneUrl != null ? Repository.CloneUrl.ToString() : DefaultRepoUrl;
                if (repoUrl != url)
                {
                    repoUrl = url;
                    repoDataChanged = true;
                }

                if (Repository.CurrentRemote.HasValue)
                    repoRemote = Repository.CurrentRemote.Value.Name;
            }
            else if (!HasRepository)
            {
                repoBranch = null;
                repoUrl = null;
            }

            if (repoUrl == null)
            {
                repoUrl = DefaultRepoUrl;
                repoDataChanged = true;
            }
            return repoDataChanged;
        }

        private void AttachHandlers(IRepository repository)
        {
            if (repository == null)
                return;
            repository.OnRepositoryInfoChanged += RefreshOnMainThread;
        }

        private void DetachHandlers(IRepository repository)
        {
            if (repository == null)
                return;
            repository.OnRepositoryInfoChanged -= RefreshOnMainThread;
        }


        private void SwitchView(Subview from, Subview to)
        {
            GUI.FocusControl(null);
            if (from != null)
                from.OnDisable();
            to.OnEnable();
            Refresh();
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

                    GUILayout.Label(repoUrlContent, Styles.HeaderRepoLabelStyle);
                    GUILayout.Space(-2);
                    GUILayout.Label(repoBranchContent, Styles.HeaderBranchLabelStyle);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void DoToolbarGUI()
        {
            // Subtabs & toolbar
            Rect mainNavRect = EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                SubTab tab = activeTab;
                EditorGUI.BeginChangeCheck();
                {
                    if (HasRepository)
                    {
                        tab = TabButton(SubTab.Changes, ChangesTitle, tab);
                        tab = TabButton(SubTab.History, HistoryTitle, tab);
                        tab = TabButton(SubTab.Branches, BranchesTitle, tab);
                    }
                    else
                    {
                        tab = TabButton(SubTab.History, HistoryTitle, tab);
                    }
                    tab = TabButton(SubTab.Settings, SettingsTitle, tab);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    var from = ActiveTab;
                    activeTab = tab;
                    SwitchView(from, ActiveTab);
                }

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Account", EditorStyles.toolbarDropDown))
                    DoAccountDropdown();
            }
            EditorGUILayout.EndHorizontal();
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
            PopupWindow.Open(PopupWindow.PopupViewType.AuthenticationView);
        }

        private void GoToProfile(object obj)
        {
            Application.OpenURL(Platform.CredentialManager.CachedCredentials.Host.Combine(Platform.CredentialManager.CachedCredentials.Username));
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

            var apiClient = ApiClient.Create(host, Platform.Keychain);
            apiClient.Logout(host);
        }

        private bool ValidateSettings()
        {
            var settingsIssues = Utility.Issues.Select(i => i as ProjectSettingsIssue).FirstOrDefault(i => i != null);

            // Initial state
            if (!Utility.ActiveRepository || !Utility.GitFound ||
                (settingsIssues != null &&
                    (settingsIssues.WasCaught(ProjectSettingsEvaluation.EditorSettingsMissing) ||
                        settingsIssues.WasCaught(ProjectSettingsEvaluation.BadVCSSettings))))
            {
                return false;
            }

            return true;
        }

        public new void ShowNotification(GUIContent content)
        {
            ShowNotification(content, DefaultNotificationTimeout);
        }

        public void ShowNotification(GUIContent content, float timeout)
        {
            Debug.Assert(timeout <= DefaultNotificationTimeout, String.Format(BadNotificationDelayError, timeout));

            notificationClearTime = timeout < DefaultNotificationTimeout ? EditorApplication.timeSinceStartup + timeout : -1f;
            base.ShowNotification(content);
        }

        private static SubTab TabButton(SubTab tab, string title, SubTab activeTab)
        {
            return GUILayout.Toggle(activeTab == tab, title, EditorStyles.toolbarButton) ? tab : activeTab;
        }

        public HistoryView HistoryTab
        {
            get { return historyTab; }
        }

        public ChangesView ChangesTab
        {
            get { return changesTab; }
        }

        public BranchesView BranchesTab
        {
            get { return branchesTab; }
        }

        public SettingsView SettingsTab
        {
            get { return settingsTab; }
        }

        private Subview ActiveTab
        {
            get
            {
                return ToView(activeTab);
            }
        }

        private Subview ToView(SubTab tab)
        {
            switch (tab)
            {
                case SubTab.History:
                    return historyTab;
                case SubTab.Changes:
                    return changesTab;
                case SubTab.Branches:
                    return branchesTab;
                case SubTab.Settings:
                default:
                    return settingsTab;
            }
        }

        private enum SubTab
        {
            History,
            Changes,
            Branches,
            Settings
        }
    }
}
