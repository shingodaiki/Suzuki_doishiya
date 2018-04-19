using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GitHub.Unity
{
    [Serializable]
    class GitPathView : Subview
    {
        private const string GitInstallTitle = "Git installation";
        private const string PathToGit = "Path to Git";
        private const string GitPathSaveButton = "Save Path";
        private const string UseInternalGitButton = "Use internal git";
        private const string FindSystemGitButton = "Find system git";
        private const string BrowseButton = "...";
        private const string GitInstallBrowseTitle = "Select git binary";
        private const string ErrorInvalidPathMessage = "Invalid Path.";
        private const string ErrorInstallingInternalGit = "Error installing portable git.";
        private const string ErrorValidatingGitPath = "Error validating Git Path.";
        private const string ErrorGitNotFoundMessage = "Git not found.";
        private const string ErrorGitLfsNotFoundMessage = "Git LFS not found.";
        private const string ErrorMinimumGitVersionMessageFormat = "Git version {0} found. Git version {1} is required.";
        private const string ErrorMinimumGitLfsVersionMessageFormat = "Git LFS version {0} found. Git LFS version {1} is required.";

        [SerializeField] private string gitExec;
        [SerializeField] private string gitExecParent;
        [SerializeField] private string gitExecExtension;
        [SerializeField] private string newGitExec;
        [SerializeField] private bool isValueChanged;
        [SerializeField] private bool isValueChangedAndFileExists;
        [SerializeField] private string gitFileErrorMessage;
        [SerializeField] private string gitVersionErrorMessage;

        [NonSerialized] private bool isBusy;
        [NonSerialized] private bool gitExecHasChanged;
        [NonSerialized] private bool gitExecutableIsSet;
        [NonSerialized] private string portableGitPath;

        public override void InitializeView(IView parent)
        {
            base.InitializeView(parent);
            gitExecutableIsSet = Environment.GitExecutablePath.IsInitialized;

            var gitInstallDetails = new GitInstaller.GitInstallDetails(Environment.UserCachePath, Environment.IsWindows);
            portableGitPath = gitInstallDetails.GitExecutablePath;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            gitExecHasChanged = true;
        }

        public override void OnDataUpdate()
        {
            base.OnDataUpdate();
            MaybeUpdateData();
        }

        public override void OnGUI()
        {
            // Install path
            GUILayout.Label(GitInstallTitle, EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(!gitExecutableIsSet || IsBusy || Parent.IsBusy);
            {
                // Install path field
                GUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginChangeCheck();
                    {
                        newGitExec = EditorGUILayout.TextField(PathToGit, newGitExec);
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        CheckEnteredGitPath();
                    }

                    if (GUILayout.Button(BrowseButton, EditorStyles.miniButton, GUILayout.Width(Styles.BrowseButtonWidth)))
                    {
                        GUI.FocusControl(null);

                        var newValue = EditorUtility.OpenFilePanel(GitInstallBrowseTitle,
                            gitExecParent,
                            gitExecExtension);

                        if (!string.IsNullOrEmpty(newValue))
                        {
                            newGitExec = newValue;

                            if (Environment.IsWindows)
                            {
                                //Normalizing the path separator in windows
                                newGitExec = newGitExec.ToNPath().ToString();
                            }

                            CheckEnteredGitPath();
                        }
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

                GUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginDisabledGroup(!isValueChangedAndFileExists);
                    {
                        if (GUILayout.Button(GitPathSaveButton, GUILayout.ExpandWidth(false)))
                        {
                            GUI.FocusControl(null);
                            isBusy = true;

                            ValidateAndSetGitInstallPath(newGitExec);
                        }
                    }
                    EditorGUI.EndDisabledGroup();

                    // disable if we are not on windows
                    // disable if the newPath == portableGitPath
                    EditorGUI.BeginDisabledGroup(!Environment.IsWindows || Environment.IsWindows && newGitExec == portableGitPath);
                    if (GUILayout.Button(UseInternalGitButton, GUILayout.ExpandWidth(false)))
                    {
                        GUI.FocusControl(null);

                        Logger.Trace("Expected portableGitPath: {0}", portableGitPath);
                        newGitExec = portableGitPath;
                        CheckEnteredGitPath();
                    }
                    EditorGUI.EndDisabledGroup();

                    //Find button - for attempting to locate a new install
                    if (GUILayout.Button(FindSystemGitButton, GUILayout.ExpandWidth(false)))
                    {
                        GUI.FocusControl(null);
                        isBusy = true;

                        newGitExec = gitExec;
                        CheckEnteredGitPath();

                        new FindExecTask("git", Manager.CancellationToken)
                            .Configure(Manager.ProcessManager, dontSetupGit: true)
                            .Catch(ex => true)
                            .FinallyInUI((success, ex, path) => {
                                if (success)
                                {
                                    Logger.Trace("FindGit Path:{0}", path);
                                    newGitExec = path;
                                    CheckEnteredGitPath();
                                }
                                else
                                {
                                    if (ex != null)
                                    {
                                        Logger.Error(ex, "FindGit Error Path:{0}", path);
                                    }
                                    else
                                    {
                                        Logger.Error("FindGit Failed Path:{0}", path);
                                    }
                                }

                                isBusy = false;
                            }).Start();
                    }
                }
                GUILayout.EndHorizontal();

                if (gitFileErrorMessage != null)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(gitFileErrorMessage, Styles.ErrorLabel);
                    }
                    GUILayout.EndHorizontal();
                }

                if (gitVersionErrorMessage != null)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(gitVersionErrorMessage, Styles.ErrorLabel);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        private void MaybeUpdateData()
        {
            if (gitExecHasChanged)
            {
                if (gitExecExtension == null)
                {
                    gitExecExtension = Environment.ExecutableExtension;

                    if (Environment.IsWindows)
                    {
                        gitExecExtension = gitExecExtension.TrimStart('.');
                    }
                }

                if (Environment.GitExecutablePath.IsInitialized)
                {
                    newGitExec = gitExec = Environment.GitExecutablePath.ToString();
                    gitExecParent = Environment.GitExecutablePath.Parent.ToString();

                    CheckEnteredGitPath();
                }

                if (gitExecParent == null)
                {
                    gitExecParent = Environment.GitInstallPath;
                }

                gitExecHasChanged = false;
            }
        }

        private void CheckEnteredGitPath()
        {
            isValueChanged = !string.IsNullOrEmpty(newGitExec) && newGitExec != gitExec;

            isValueChangedAndFileExists = isValueChanged && newGitExec.ToNPath().FileExists();

            gitFileErrorMessage = isValueChanged && !isValueChangedAndFileExists ? ErrorInvalidPathMessage : null;

            gitVersionErrorMessage = null;
        }

        private void ValidateAndSetGitInstallPath(string value)
        {
            value = value.Trim();

            if (value == portableGitPath)
            {
                Logger.Trace("Attempting to restore portable Git Path:{0}", value);

                var gitInstaller = new GitInstaller(Environment, EntryPoint.ApplicationManager.ProcessManager,
                    EntryPoint.ApplicationManager.TaskManager);

                gitInstaller.SetupGitIfNeeded()
                    .FinallyInUI((success, exception, installationState) =>
                    {
                        Logger.Trace("Setup Git Using the installer:{0}", success);

                        if (!success)
                        {
                            Logger.Error(exception, ErrorInstallingInternalGit);
                            gitVersionErrorMessage = ErrorValidatingGitPath;
                        }
                        else
                        {
                            Manager.SystemSettings.Unset(Constants.GitInstallPathKey);
                            Environment.GitExecutablePath = installationState.GitExecutablePath;
                            Environment.GitLfsExecutablePath = installationState.GitLfsExecutablePath;
                            Environment.IsCustomGitExecutable = false;

                            gitExecHasChanged = true;
                        }

                        isBusy = false;
                    }).Start();
            }
            else
            {
                //Logger.Trace("Validating Git Path:{0}", value);

                gitVersionErrorMessage = null;

                GitClient.ValidateGitInstall(value.ToNPath(), true)
                    .ThenInUI((success, result) =>
                    {
                        if (!success)
                        {
                            Logger.Trace(ErrorValidatingGitPath);
                            gitVersionErrorMessage = ErrorValidatingGitPath;
                        }
                        else if (!result.IsValid)
                        {
                            Logger.Warning(
                                "Software versions do not meet minimums Git:{0} (Minimum:{1}) GitLfs:{2} (Minimum:{3})",
                                result.GitVersion, Constants.MinimumGitVersion, result.GitLfsVersion,
                                Constants.MinimumGitLfsVersion);

                            var errorMessageStringBuilder = new StringBuilder();

                            if (result.GitVersion == null)
                            {
                                errorMessageStringBuilder.Append(ErrorGitNotFoundMessage);
                            }
                            else if (result.GitLfsVersion == null)
                            {
                                errorMessageStringBuilder.Append(ErrorGitLfsNotFoundMessage);
                            }
                            else
                            {
                                if (result.GitVersion < Constants.MinimumGitVersion)
                                {
                                    errorMessageStringBuilder.AppendFormat(ErrorMinimumGitVersionMessageFormat,
                                        result.GitVersion, Constants.MinimumGitVersion);
                                }

                                if (result.GitLfsVersion < Constants.MinimumGitLfsVersion)
                                {
                                    if (errorMessageStringBuilder.Length > 0)
                                    {
                                        errorMessageStringBuilder.Append(Environment.NewLine);
                                    }

                                    errorMessageStringBuilder.AppendFormat(ErrorMinimumGitLfsVersionMessageFormat,
                                        result.GitLfsVersion, Constants.MinimumGitLfsVersion);
                                }
                            }

                            gitVersionErrorMessage = errorMessageStringBuilder.ToString();
                        }
                        else
                        {
                            Logger.Trace("Software versions meet minimums Git:{0} GitLfs:{1}",
                                result.GitVersion,
                                result.GitLfsVersion);

                            Manager.SystemSettings.Set(Constants.GitInstallPathKey, value);
                            Environment.GitExecutablePath = value.ToNPath();
                            Environment.IsCustomGitExecutable = true;

                            gitExecHasChanged = true;
                        }

                        isBusy = false;

                    }).Start();
            }
        }

        public override bool IsBusy
        {
            get { return isBusy; }
        }
    }
}
