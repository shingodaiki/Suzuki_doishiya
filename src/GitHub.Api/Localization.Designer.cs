﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GitHub.Unity {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Localization {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Localization() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GitHub.Unity.Localization", typeof(Localization).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}.
        /// </summary>
        public static string BasePathLabel {
            get {
                return ResourceManager.GetString("BasePathLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Branches.
        /// </summary>
        public static string BranchesTitle {
            get {
                return ResourceManager.GetString("BranchesTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to cancel.
        /// </summary>
        public static string Cancel {
            get {
                return ResourceManager.GetString("Cancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} changed files.
        /// </summary>
        public static string ChangedFilesLabel {
            get {
                return ResourceManager.GetString("ChangedFilesLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Changes.
        /// </summary>
        public static string ChangesTitle {
            get {
                return ResourceManager.GetString("ChangesTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        public static string CommitButton {
            get {
                return ResourceManager.GetString("CommitButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All.
        /// </summary>
        public static string CommitSelectAllButton {
            get {
                return ResourceManager.GetString("CommitSelectAllButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to None.
        /// </summary>
        public static string CommitSelectNoneButton {
            get {
                return ResourceManager.GetString("CommitSelectNoneButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No remote configured.
        /// </summary>
        public static string DefaultRepoUrl {
            get {
                return ResourceManager.GetString("DefaultRepoUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Commit description.
        /// </summary>
        public static string DescriptionLabel {
            get {
                return ResourceManager.GetString("DescriptionLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fetch Changes.
        /// </summary>
        public static string FetchActionTitle {
            get {
                return ResourceManager.GetString("FetchActionTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fetch.
        /// </summary>
        public static string FetchButtonText {
            get {
                return ResourceManager.GetString("FetchButtonText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not fetch changes.
        /// </summary>
        public static string FetchFailureDescription {
            get {
                return ResourceManager.GetString("FetchFailureDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ....
        /// </summary>
        public static string GitInitBrowseButton {
            get {
                return ResourceManager.GetString("GitInitBrowseButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pick desired repository root.
        /// </summary>
        public static string GitInitBrowseTitle {
            get {
                return ResourceManager.GetString("GitInitBrowseTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Set up git.
        /// </summary>
        public static string GitInitButton {
            get {
                return ResourceManager.GetString("GitInitButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to We could not find Git in the system..
        /// </summary>
        public static string GitLFSNotFound {
            get {
                return ResourceManager.GetString("GitLFSNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The detected LFS at {0} has version {1}, which is too low. The minimum LFS version is {2}..
        /// </summary>
        public static string GitLfsVersionTooLow {
            get {
                return ResourceManager.GetString("GitLfsVersionTooLow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to We could not find Git in the system..
        /// </summary>
        public static string GitNotFound {
            get {
                return ResourceManager.GetString("GitNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The detected Git at {0} has version {1}, which is too low. The minimum Git version is {2}..
        /// </summary>
        public static string GitVersionTooLow {
            get {
                return ResourceManager.GetString("GitVersionTooLow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (All).
        /// </summary>
        public static string HistoryFocusAll {
            get {
                return ResourceManager.GetString("HistoryFocusAll", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        public static string HistoryFocusSingle {
            get {
                return ResourceManager.GetString("HistoryFocusSingle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to History.
        /// </summary>
        public static string HistoryTitle {
            get {
                return ResourceManager.GetString("HistoryTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initialize a git repository for this project.
        /// </summary>
        public static string InitializeRepositoryButtonText {
            get {
                return ResourceManager.GetString("InitializeRepositoryButtonText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initialize.
        /// </summary>
        public static string InitializeTitle {
            get {
                return ResourceManager.GetString("InitializeTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your selected folder &apos;{0}&apos; is not a valid repository root for your current project..
        /// </summary>
        public static string InvalidInitDirectoryMessage {
            get {
                return ResourceManager.GetString("InvalidInitDirectoryMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OK.
        /// </summary>
        public static string InvalidInitDirectoryOK {
            get {
                return ResourceManager.GetString("InvalidInitDirectoryOK", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid repository root.
        /// </summary>
        public static string InvalidInitDirectoryTitle {
            get {
                return ResourceManager.GetString("InvalidInitDirectoryTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Maximum number of logins exceeded. Please wait a few minutes before trying again..
        /// </summary>
        public static string LockedOut {
            get {
                return ResourceManager.GetString("LockedOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Login failed.
        /// </summary>
        public static string LoginFailed {
            get {
                return ResourceManager.GetString("LoginFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your current project is not currently in an active git repository:.
        /// </summary>
        public static string NoActiveRepositoryMessage {
            get {
                return ResourceManager.GetString("NoActiveRepositoryMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No repository found.
        /// </summary>
        public static string NoActiveRepositoryTitle {
            get {
                return ResourceManager.GetString("NoActiveRepositoryTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No changed files.
        /// </summary>
        public static string NoChangedFilesLabel {
            get {
                return ResourceManager.GetString("NoChangedFilesLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No changes found.
        /// </summary>
        public static string NoChangesLabel {
            get {
                return ResourceManager.GetString("NoChangesLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tried to run git task while git was not found..
        /// </summary>
        public static string NoGitError {
            get {
                return ResourceManager.GetString("NoGitError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You need to log in..
        /// </summary>
        public static string NotLoggedIn {
            get {
                return ResourceManager.GetString("NotLoggedIn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ok.
        /// </summary>
        public static string Ok {
            get {
                return ResourceManager.GetString("Ok", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1 changed file.
        /// </summary>
        public static string OneChangedFileLabel {
            get {
                return ResourceManager.GetString("OneChangedFileLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Publish.
        /// </summary>
        public static string PublishButton {
            get {
                return ResourceManager.GetString("PublishButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pull.
        /// </summary>
        public static string PullActionTitle {
            get {
                return ResourceManager.GetString("PullActionTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pull.
        /// </summary>
        public static string PullButton {
            get {
                return ResourceManager.GetString("PullButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        public static string PullButtonCount {
            get {
                return ResourceManager.GetString("PullButtonCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        public static string PullConfirmCancel {
            get {
                return ResourceManager.GetString("PullConfirmCancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Would you like to pull changes from remote &apos;{0}&apos;?.
        /// </summary>
        public static string PullConfirmDescription {
            get {
                return ResourceManager.GetString("PullConfirmDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pull Changes?.
        /// </summary>
        public static string PullConfirmTitle {
            get {
                return ResourceManager.GetString("PullConfirmTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pull.
        /// </summary>
        public static string PullConfirmYes {
            get {
                return ResourceManager.GetString("PullConfirmYes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not update local branch..
        /// </summary>
        public static string PullFailureDescription {
            get {
                return ResourceManager.GetString("PullFailureDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Local branch is up to date with {0}.
        /// </summary>
        public static string PullSuccessDescription {
            get {
                return ResourceManager.GetString("PullSuccessDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Push.
        /// </summary>
        public static string PushActionTitle {
            get {
                return ResourceManager.GetString("PushActionTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Push.
        /// </summary>
        public static string PushButton {
            get {
                return ResourceManager.GetString("PushButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        public static string PushButtonCount {
            get {
                return ResourceManager.GetString("PushButtonCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        public static string PushConfirmCancel {
            get {
                return ResourceManager.GetString("PushConfirmCancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Would you like to push changes to remote &apos;{0}&apos;?.
        /// </summary>
        public static string PushConfirmDescription {
            get {
                return ResourceManager.GetString("PushConfirmDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Push Changes?.
        /// </summary>
        public static string PushConfirmTitle {
            get {
                return ResourceManager.GetString("PushConfirmTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Push.
        /// </summary>
        public static string PushConfirmYes {
            get {
                return ResourceManager.GetString("PushConfirmYes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not push branch.
        /// </summary>
        public static string PushFailureDescription {
            get {
                return ResourceManager.GetString("PushFailureDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Branch pushed.
        /// </summary>
        public static string PushSuccessDescription {
            get {
                return ResourceManager.GetString("PushSuccessDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Refresh.
        /// </summary>
        public static string RefreshButton {
            get {
                return ResourceManager.GetString("RefreshButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Release Lock.
        /// </summary>
        public static string ReleaseLockActionTitle {
            get {
                return ResourceManager.GetString("ReleaseLockActionTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Access.
        /// </summary>
        public static string RemoteAccessTitle {
            get {
                return ResourceManager.GetString("RemoteAccessTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Host.
        /// </summary>
        public static string RemoteHostTitle {
            get {
                return ResourceManager.GetString("RemoteHostTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Name.
        /// </summary>
        public static string RemoteNameTitle {
            get {
                return ResourceManager.GetString("RemoteNameTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Remotes.
        /// </summary>
        public static string RemotesTitle {
            get {
                return ResourceManager.GetString("RemotesTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User.
        /// </summary>
        public static string RemoteUserTitle {
            get {
                return ResourceManager.GetString("RemoteUserTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Request Lock.
        /// </summary>
        public static string RequestLockActionTitle {
            get {
                return ResourceManager.GetString("RequestLockActionTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Settings.
        /// </summary>
        public static string SettingsTitle {
            get {
                return ResourceManager.GetString("SettingsTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Commit summary.
        /// </summary>
        public static string SummaryLabel {
            get {
                return ResourceManager.GetString("SummaryLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not switch to branch {0}.
        /// </summary>
        public static string SwitchBranchFailedDescription {
            get {
                return ResourceManager.GetString("SwitchBranchFailedDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Switch branch.
        /// </summary>
        public static string SwitchBranchTitle {
            get {
                return ResourceManager.GetString("SwitchBranchTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GitHub.
        /// </summary>
        public static string Title {
            get {
                return ResourceManager.GetString("Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported view mode: {0}.
        /// </summary>
        public static string UnknownViewModeError {
            get {
                return ResourceManager.GetString("UnknownViewModeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Changes.
        /// </summary>
        public static string ViewModeChangesTab {
            get {
                return ResourceManager.GetString("ViewModeChangesTab", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to History.
        /// </summary>
        public static string ViewModeHistoryTab {
            get {
                return ResourceManager.GetString("ViewModeHistoryTab", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Settings.
        /// </summary>
        public static string ViewModeSettingsTab {
            get {
                return ResourceManager.GetString("ViewModeSettingsTab", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Active branch.
        /// </summary>
        public static string Window_RepoBranchTooltip {
            get {
                return ResourceManager.GetString("Window_RepoBranchTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add a remote in the Settings tab.
        /// </summary>
        public static string Window_RepoNoUrlTooltip {
            get {
                return ResourceManager.GetString("Window_RepoNoUrlTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Url of the {0} remote.
        /// </summary>
        public static string Window_RepoUrlTooltip {
            get {
                return ResourceManager.GetString("Window_RepoUrlTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong 2FA code.
        /// </summary>
        public static string Wrong2faCode {
            get {
                return ResourceManager.GetString("Wrong2faCode", resourceCulture);
            }
        }
    }
}
