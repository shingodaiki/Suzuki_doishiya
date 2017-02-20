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
    internal class Localization {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Localization() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
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
        internal static global::System.Globalization.CultureInfo Culture {
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
        internal static string BasePathLabel {
            get {
                return ResourceManager.GetString("BasePathLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} changed files.
        /// </summary>
        internal static string ChangedFilesLabel {
            get {
                return ResourceManager.GetString("ChangedFilesLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string CommitButton {
            get {
                return ResourceManager.GetString("CommitButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All.
        /// </summary>
        internal static string CommitSelectAllButton {
            get {
                return ResourceManager.GetString("CommitSelectAllButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to None.
        /// </summary>
        internal static string CommitSelectNoneButton {
            get {
                return ResourceManager.GetString("CommitSelectNoneButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Commit description.
        /// </summary>
        internal static string DescriptionLabel {
            get {
                return ResourceManager.GetString("DescriptionLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ....
        /// </summary>
        internal static string GitInitBrowseButton {
            get {
                return ResourceManager.GetString("GitInitBrowseButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pick desired repository root.
        /// </summary>
        internal static string GitInitBrowseTitle {
            get {
                return ResourceManager.GetString("GitInitBrowseTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Set up git.
        /// </summary>
        internal static string GitInitButton {
            get {
                return ResourceManager.GetString("GitInitButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (All).
        /// </summary>
        internal static string HistoryFocusAll {
            get {
                return ResourceManager.GetString("HistoryFocusAll", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string HistoryFocusSingle {
            get {
                return ResourceManager.GetString("HistoryFocusSingle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initialize repository.
        /// </summary>
        internal static string InitializeRepositoryButtonText {
            get {
                return ResourceManager.GetString("InitializeRepositoryButtonText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your selected folder &apos;{0}&apos; is not a valid repository root for your current project..
        /// </summary>
        internal static string InvalidInitDirectoryMessage {
            get {
                return ResourceManager.GetString("InvalidInitDirectoryMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OK.
        /// </summary>
        internal static string InvalidInitDirectoryOK {
            get {
                return ResourceManager.GetString("InvalidInitDirectoryOK", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid repository root.
        /// </summary>
        internal static string InvalidInitDirectoryTitle {
            get {
                return ResourceManager.GetString("InvalidInitDirectoryTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Maximum number of logins exceeded. Please wait a few minutes before trying again..
        /// </summary>
        internal static string LockedOut {
            get {
                return ResourceManager.GetString("LockedOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Login failed.
        /// </summary>
        internal static string LoginFailed {
            get {
                return ResourceManager.GetString("LoginFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your current project is not currently in an active git repository:.
        /// </summary>
        internal static string NoActiveRepositoryMessage {
            get {
                return ResourceManager.GetString("NoActiveRepositoryMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No repository found.
        /// </summary>
        internal static string NoActiveRepositoryTitle {
            get {
                return ResourceManager.GetString("NoActiveRepositoryTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No changed files.
        /// </summary>
        internal static string NoChangedFilesLabel {
            get {
                return ResourceManager.GetString("NoChangedFilesLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No changes found.
        /// </summary>
        internal static string NoChangesLabel {
            get {
                return ResourceManager.GetString("NoChangesLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tried to run git task while git was not found..
        /// </summary>
        internal static string NoGitError {
            get {
                return ResourceManager.GetString("NoGitError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You need to log in..
        /// </summary>
        internal static string NotLoggedIn {
            get {
                return ResourceManager.GetString("NotLoggedIn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ok.
        /// </summary>
        internal static string Ok {
            get {
                return ResourceManager.GetString("Ok", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1 changed file.
        /// </summary>
        internal static string OneChangedFileLabel {
            get {
                return ResourceManager.GetString("OneChangedFileLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pull.
        /// </summary>
        internal static string PullActionTitle {
            get {
                return ResourceManager.GetString("PullActionTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pull.
        /// </summary>
        internal static string PullButton {
            get {
                return ResourceManager.GetString("PullButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string PullButtonCount {
            get {
                return ResourceManager.GetString("PullButtonCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        internal static string PullConfirmCancel {
            get {
                return ResourceManager.GetString("PullConfirmCancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Would you like to pull changes from remote &apos;{0}&apos;?.
        /// </summary>
        internal static string PullConfirmDescription {
            get {
                return ResourceManager.GetString("PullConfirmDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pull Changes?.
        /// </summary>
        internal static string PullConfirmTitle {
            get {
                return ResourceManager.GetString("PullConfirmTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pull.
        /// </summary>
        internal static string PullConfirmYes {
            get {
                return ResourceManager.GetString("PullConfirmYes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not update local branch..
        /// </summary>
        internal static string PullFailureDescription {
            get {
                return ResourceManager.GetString("PullFailureDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Local branch is up to date with {0}.
        /// </summary>
        internal static string PullSuccessDescription {
            get {
                return ResourceManager.GetString("PullSuccessDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Push.
        /// </summary>
        internal static string PushActionTitle {
            get {
                return ResourceManager.GetString("PushActionTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Push.
        /// </summary>
        internal static string PushButton {
            get {
                return ResourceManager.GetString("PushButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string PushButtonCount {
            get {
                return ResourceManager.GetString("PushButtonCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        internal static string PushConfirmCancel {
            get {
                return ResourceManager.GetString("PushConfirmCancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Would you like to push changes to remote &apos;{0}&apos;?.
        /// </summary>
        internal static string PushConfirmDescription {
            get {
                return ResourceManager.GetString("PushConfirmDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Push Changes?.
        /// </summary>
        internal static string PushConfirmTitle {
            get {
                return ResourceManager.GetString("PushConfirmTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Push.
        /// </summary>
        internal static string PushConfirmYes {
            get {
                return ResourceManager.GetString("PushConfirmYes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not push branch.
        /// </summary>
        internal static string PushFailureDescription {
            get {
                return ResourceManager.GetString("PushFailureDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Branch pushed.
        /// </summary>
        internal static string PushSuccessDescription {
            get {
                return ResourceManager.GetString("PushSuccessDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Refresh.
        /// </summary>
        internal static string RefreshButton {
            get {
                return ResourceManager.GetString("RefreshButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Access.
        /// </summary>
        internal static string RemoteAccessTitle {
            get {
                return ResourceManager.GetString("RemoteAccessTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Host.
        /// </summary>
        internal static string RemoteHostTitle {
            get {
                return ResourceManager.GetString("RemoteHostTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Name.
        /// </summary>
        internal static string RemoteNameTitle {
            get {
                return ResourceManager.GetString("RemoteNameTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Remotes.
        /// </summary>
        internal static string RemotesTitle {
            get {
                return ResourceManager.GetString("RemotesTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User.
        /// </summary>
        internal static string RemoteUserTitle {
            get {
                return ResourceManager.GetString("RemoteUserTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Commit summary.
        /// </summary>
        internal static string SummaryLabel {
            get {
                return ResourceManager.GetString("SummaryLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GitHub.
        /// </summary>
        internal static string Title {
            get {
                return ResourceManager.GetString("Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported view mode: {0}.
        /// </summary>
        internal static string UnknownViewModeError {
            get {
                return ResourceManager.GetString("UnknownViewModeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Changes.
        /// </summary>
        internal static string ViewModeChangesTab {
            get {
                return ResourceManager.GetString("ViewModeChangesTab", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to History.
        /// </summary>
        internal static string ViewModeHistoryTab {
            get {
                return ResourceManager.GetString("ViewModeHistoryTab", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Settings.
        /// </summary>
        internal static string ViewModeSettingsTab {
            get {
                return ResourceManager.GetString("ViewModeSettingsTab", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong 2FA code.
        /// </summary>
        internal static string Wrong2faCode {
            get {
                return ResourceManager.GetString("Wrong2faCode", resourceCulture);
            }
        }
    }
}
