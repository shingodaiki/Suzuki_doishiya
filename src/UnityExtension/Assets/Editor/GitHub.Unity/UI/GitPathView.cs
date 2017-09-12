using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private const string GitInstallFindButton = "Find install";
        private const string BrowseButton = "...";
        private const string GitInstallBrowseTitle = "Select git binary";
        private const string ErrorInvalidPathMessage = "Invalid Path.";
        private const string ErrorGettingSoftwareVersionMessage = "Error getting software versions.";
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

        public override bool IsBusy
        {
            get { return isBusy; }
        }

        public override void OnGUI()
        {
            // Install path
            GUILayout.Label(GitInstallTitle, EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(IsBusy || Parent.IsBusy);
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

                    if (GUILayout.Button(BrowseButton, EditorStyles.miniButton, GUILayout.Width(25)))
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
                    GUILayout.FlexibleSpace();

                    //Find button - for attempting to locate a new install
                    if (GUILayout.Button(GitInstallFindButton, GUILayout.ExpandWidth(false)))
                    {
                        GUI.FocusControl(null);
                        isBusy = true;

                        newGitExec = gitExec;
                        CheckEnteredGitPath();

                        new ProcessTask<NPath>(Manager.CancellationToken, new FirstLineIsPathOutputProcessor())
                            .Configure(Manager.ProcessManager, Environment.IsWindows ? "where" : "which", "git")
                            .FinallyInUI((success, ex, path) =>
                            {
                                if (success)
                                {
                                    Logger.Trace("FindGit Path:{0}", path);
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

                                if (success)
                                {
                                    newGitExec = path;
                                    CheckEnteredGitPath();
                                }

                                isBusy = false;
                            }).Start();
                    }

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

                }
                GUILayout.EndHorizontal();
            }
            EditorGUI.EndDisabledGroup();
        }

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
                if (Environment != null)
                {
                    if (gitExecExtension == null)
                    {
                        gitExecExtension = Environment.ExecutableExtension;

                        if (Environment.IsWindows)
                        {
                            gitExecExtension = gitExecExtension.TrimStart('.');
                        }
                    }

                    if (Environment.GitExecutablePath != null)
                    {
                        newGitExec = gitExec = Environment.GitExecutablePath.ToString();
                        gitExecParent = Environment.GitExecutablePath.Parent.ToString();

                        CheckEnteredGitPath();
                    }

                    if (gitExecParent == null)
                    {
                        gitExecParent = Environment.GitInstallPath;
                    }
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
            Logger.Trace("Validating Git Path:{0}", value);

            gitVersionErrorMessage = null;

            GitClient.ValidateGitInstall(value).ThenInUI((sucess, result) => {
                if (!sucess)
                {
                    Logger.Trace(ErrorGettingSoftwareVersionMessage);
                    gitVersionErrorMessage = ErrorGettingSoftwareVersionMessage;

                    isBusy = false;

                    return;
                }

                if (!result.IsValid)
                {
                    Logger.Warning("Software versions do not meet minimums Git:{0} (Minimum:{1}) GitLfs:{2} (Minimum:{3})",
                        result.GitVersion,
                        Constants.MinimumGitVersion,
                        result.GitLfsVersion,
                        Constants.MinimumGitLfsVersion);

                    var errorMessageStringBuilder = new StringBuilder();

                    if (result.GitVersion < Constants.MinimumGitVersion)
                    {
                        errorMessageStringBuilder.AppendFormat(ErrorMinimumGitVersionMessageFormat, result.GitVersion, Constants.MinimumGitVersion);
                    }

                    if (result.GitLfsVersion < Constants.MinimumGitLfsVersion)
                    {
                        if(errorMessageStringBuilder.Length > 0)
                        {
                            errorMessageStringBuilder.Append(Environment.NewLine);
                        }

                        errorMessageStringBuilder.AppendFormat(ErrorMinimumGitLfsVersionMessageFormat, result.GitLfsVersion, Constants.MinimumGitLfsVersion);
                    }

                    gitVersionErrorMessage = errorMessageStringBuilder.ToString();

                    isBusy = false;
                    return;
                }

                Logger.Trace("Software versions meet minimums Git:{0} GitLfs:{1}",
                    result.GitVersion,
                    result.GitLfsVersion);

                Manager.SystemSettings.Set(Constants.GitInstallPathKey, value);
                Environment.GitExecutablePath = value.ToNPath();

                gitExecHasChanged = true;
                isBusy = false;

            }).Start();
        }
    }
}
