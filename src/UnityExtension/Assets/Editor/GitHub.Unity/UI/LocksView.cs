using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GitHub.Unity
{
    [Serializable]
    public class GitLockEntryDictionary : SerializableDictionary<string, GitLockEntry> { }

    [Serializable]
    public class GitStatusDictionary : SerializableDictionary<string, GitStatus> { }

    [Serializable]
    public class GitLockEntry
    {
        public static GitLockEntry Default = new GitLockEntry(GitLock.Default, GitFileStatus.None);

        [SerializeField] private GitLock gitLock;
        [SerializeField] private GitFileStatus gitFileStatus;

        [NonSerialized] public Texture Icon;
        [NonSerialized] public Texture IconBadge;

        public GitLockEntry(GitLock gitLock, GitFileStatus gitFileStatus)
        {
            this.gitLock = gitLock;
            this.gitFileStatus = gitFileStatus;
        }

        public GitLock GitLock
        {
            get { return gitLock; }
        }

        public GitFileStatus GitFileStatus
        {
            get { return gitFileStatus; }
        }

        public string PrettyTimeString
        {
            get
            {
                return gitLock.LockedAt.ToLocalTime().CreateRelativeTime(DateTimeOffset.Now);
            }
        }
    }

    [Serializable]
    class LocksControl
    {
        [SerializeField] private Vector2 scroll;
        [SerializeField] private List<GitLockEntry> gitLockEntries = new List<GitLockEntry>();
        [SerializeField] public GitLockEntryDictionary assets = new GitLockEntryDictionary();
        [SerializeField] public GitStatusDictionary gitStatusDictionary = new GitStatusDictionary();

        [NonSerialized] private Action<GitLock> rightClickNextRender;
        [NonSerialized] private GitLockEntry rightClickNextRenderEntry;
        [NonSerialized] private GitLockEntry selectedEntry;
        [NonSerialized] private int controlId;
        [NonSerialized] private UnityEngine.Object lastActivatedObject;

        public GitLockEntry SelectedEntry
        {
            get
            {
                return selectedEntry;
            }
            set
            {
                selectedEntry = value;

                var activeObject = selectedEntry != null
                    ? AssetDatabase.LoadMainAssetAtPath(selectedEntry.GitLock.Path)
                    : null;

                lastActivatedObject = activeObject;

                if (LocksControlHasFocus)
                {
                    Selection.activeObject = activeObject;
                }
            }
        }

        public bool Render(Rect containingRect, Action<GitLock> singleClick = null,
            Action<GitLock> doubleClick = null, Action<GitLock> rightClick = null)
        {
            var requiresRepaint = false;
            scroll = GUILayout.BeginScrollView(scroll);
            {
                controlId = GUIUtility.GetControlID(FocusType.Keyboard);

                if (Event.current.type != EventType.Repaint)
                {
                    if (rightClickNextRender != null)
                    {
                        rightClickNextRender.Invoke(rightClickNextRenderEntry.GitLock);
                        rightClickNextRender = null;
                        rightClickNextRenderEntry = GitLockEntry.Default;
                    }
                }

                var startDisplay = scroll.y;
                var endDisplay = scroll.y + containingRect.height;

                var rect = new Rect(containingRect.x, containingRect.y, containingRect.width, 0);

                for (var index = 0; index < gitLockEntries.Count; index++)
                {
                    var entry = gitLockEntries[index];

                    var entryRect = new Rect(rect.x, rect.y, rect.width, Styles.LocksEntryHeight);

                    var shouldRenderEntry = !(entryRect.y > endDisplay || entryRect.yMax < startDisplay);
                    if (shouldRenderEntry && Event.current.type == EventType.Repaint)
                    {
                        RenderEntry(entryRect, entry);
                    }

                    var entryRequiresRepaint =
                        HandleInput(entryRect, entry, index, singleClick, doubleClick, rightClick);
                    requiresRepaint = requiresRepaint || entryRequiresRepaint;

                    rect.y += Styles.LocksEntryHeight;
                }

                GUILayout.Space(rect.y - containingRect.y);
            }
            GUILayout.EndScrollView();

            return requiresRepaint;
        }

        private void RenderEntry(Rect entryRect, GitLockEntry entry)
        {
            var isSelected = entry == SelectedEntry;

            var iconWidth = 32;
            var iconHeight = 32;
            var iconRect = new Rect(entryRect.x + Styles.BaseSpacing / 2, entryRect.y + (Styles.LocksEntryHeight - iconHeight) / 2, iconWidth, iconHeight);

            var iconBadgeWidth = 16;
            var iconBasgeHeight = 16;
            var iconBadgeRect = new Rect(iconRect.x + iconBadgeWidth, iconRect.y + iconBasgeHeight, iconBadgeWidth, iconBasgeHeight);

            var entryBodyX = iconRect.x + iconRect.width + Styles.BaseSpacing / 2;

            var pathRect = new Rect(entryBodyX, entryRect.y + Styles.BaseSpacing, entryRect.width - entryBodyX, 11f * 2);
            var metaDataRect = new Rect(entryBodyX, pathRect.y + pathRect.height + 2, entryRect.width - entryBodyX, 9f * 2);

            var hasKeyboardFocus = GUIUtility.keyboardControl == controlId;

            Styles.Label.Draw(entryRect, GUIContent.none, false, false, isSelected, hasKeyboardFocus);
            Styles.Label.Draw(iconRect, entry.Icon, false, false, isSelected, hasKeyboardFocus);
            Styles.Label.Draw(iconBadgeRect, entry.IconBadge, false, false, isSelected, hasKeyboardFocus);
            Styles.LockPathStyle.Draw(pathRect, entry.GitLock.Path, false, false, isSelected, hasKeyboardFocus);
            Styles.LockMetaDataStyle.Draw(metaDataRect, string.Format("Locked {0} by {1}", entry.PrettyTimeString, entry.GitLock.Owner.Name), false, false, isSelected, hasKeyboardFocus);
        }

        private bool HandleInput(Rect rect, GitLockEntry entry, int index, Action<GitLock> singleClick = null,
            Action<GitLock> doubleClick = null, Action<GitLock> rightClick = null)
        {
            var requiresRepaint = false;
            var clickRect = new Rect(0f, rect.y, rect.width, rect.height);
            if (Event.current.type == EventType.MouseDown && clickRect.Contains(Event.current.mousePosition))
            {
                Event.current.Use();
                GUIUtility.keyboardControl = controlId;

                SelectedEntry = entry;
                requiresRepaint = true;
                var clickCount = Event.current.clickCount;
                var mouseButton = Event.current.button;

                if (mouseButton == 0 && clickCount == 1 && singleClick != null)
                {
                    singleClick(entry.GitLock);
                }
                if (mouseButton == 0 && clickCount > 1 && doubleClick != null)
                {
                    doubleClick(entry.GitLock);
                }
                if (mouseButton == 1 && clickCount == 1 && rightClick != null)
                {
                    rightClickNextRender = rightClick;
                    rightClickNextRenderEntry = entry;
                }
            }

            // Keyboard navigation if this child is the current selection
            if (GUIUtility.keyboardControl == controlId && entry == SelectedEntry && Event.current.type == EventType.KeyDown)
            {
                var directionY = Event.current.keyCode == KeyCode.UpArrow ? -1 : Event.current.keyCode == KeyCode.DownArrow ? 1 : 0;
                if (directionY != 0)
                {
                    Event.current.Use();

                    if (directionY > 0)
                    {
                        requiresRepaint = SelectNext(index);
                    }
                    else
                    {
                        requiresRepaint = SelectPrevious(index);
                    }
                }
            }

            return requiresRepaint;
        }

        public void Load(List<GitLock> locks, List<GitStatusEntry> gitStatusEntries)
        {
            var statusEntries = gitStatusEntries.ToDictionary(entry => entry.Path.ToNPath().ToString(SlashMode.Forward), entry => entry.status);

            var selectedLockId = SelectedEntry != null && SelectedEntry.GitLock != GitLock.Default
                ? (int?) SelectedEntry.GitLock.ID 
                : null;

            var scrollValue = scroll.y;

            var previousCount = gitLockEntries.Count;

            var scrollIndex = (int)(scrollValue / Styles.LocksEntryHeight);

            assets.Clear();

            gitLockEntries = locks.Select(gitLock => {

                GitFileStatus gitFileStatus;
                if (!statusEntries.TryGetValue(gitLock.Path.ToString(SlashMode.Forward), out gitFileStatus))
                {
                    gitFileStatus = GitFileStatus.None;
                }

                var gitLockEntry = new GitLockEntry(gitLock, gitFileStatus);
                LoadIcon(gitLockEntry, true);

                var assetGuid = AssetDatabase.AssetPathToGUID(gitLock.Path);
                if (!string.IsNullOrEmpty(assetGuid))
                {
                    assets.Add(assetGuid, gitLockEntry);
                }

                return gitLockEntry;
            }).ToList();

            var selectionPresent = false;
            for (var index = 0; index < gitLockEntries.Count; index++)
            {
                var gitLockEntry = gitLockEntries[index];
                if (selectedLockId.HasValue && selectedLockId.Value == gitLockEntry.GitLock.ID)
                {
                    selectedEntry = gitLockEntry;
                    selectionPresent = true;
                    break;
                }
            }

            if (!selectionPresent)
            {
                selectedEntry = GitLockEntry.Default;
            }

            if (scrollIndex > gitLockEntries.Count)
            {
                ScrollTo(0);
            }
            else
            {
                var scrollOffset = scrollValue % Styles.LocksEntryHeight;

                var scrollIndexFromBottom = previousCount - scrollIndex;
                var newScrollIndex = gitLockEntries.Count - scrollIndexFromBottom;

                ScrollTo(newScrollIndex, scrollOffset);
            }
        }

        public void LoadIcons()
        {
            foreach (var gitLockEntry in gitLockEntries)
            {
                LoadIcon(gitLockEntry);
            }
        }

        private void LoadIcon(GitLockEntry gitLockEntry, bool force = false)
        {
            if (force || gitLockEntry.Icon == null)
            {
                gitLockEntry.Icon = GetNodeIcon(gitLockEntry.GitLock);
            }

            if (force || gitLockEntry.IconBadge == null)
            {
                gitLockEntry.IconBadge = Styles.GetFileStatusIcon(gitLockEntry.GitFileStatus, true);
            }
        }

        protected Texture GetNodeIcon(GitLock node)
        {
            Texture nodeIcon = null;

            if (!string.IsNullOrEmpty(node.Path))
            {
                nodeIcon = UnityEditorInternal.InternalEditorUtility.GetIconForFile(node.Path);
            }

            if (nodeIcon != null)
            {
                nodeIcon.hideFlags = HideFlags.HideAndDontSave;
            }

            return nodeIcon;
        }

        protected bool LocksControlHasFocus
        {
            get { return GUIUtility.keyboardControl == controlId; }
        }

        private bool SelectNext(int index)
        {
            index++;

            if (index < gitLockEntries.Count)
            {
                SelectedEntry = gitLockEntries[index];
                return true;
            }

            return false;
        }

        private bool SelectPrevious(int index)
        {
            index--;

            if (index >= 0)
            {
                SelectedEntry = gitLockEntries[index];
                return true;
            }

            return false;
        }

        public void ScrollTo(int index, float offset = 0f)
        {
            scroll.Set(scroll.x, Styles.LocksEntryHeight * index + offset);
        }

        public bool OnSelectionChange()
        {
            if (!LocksControlHasFocus)
            {
                GitLockEntry gitLockEntry = GitLockEntry.Default;

                if (Selection.activeObject != lastActivatedObject)
                {
                    var activeAssetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                    var activeAssetGuid = AssetDatabase.AssetPathToGUID(activeAssetPath);

                    assets.TryGetValue(activeAssetGuid, out gitLockEntry);
                }

                SelectedEntry = gitLockEntry;
                return true;
            }

            return false;
        }
    }

    [Serializable]
    class LocksView : Subview
    {
        [NonSerialized] private bool currentStatusEntriesHasUpdate;
        [NonSerialized] private bool currentLocksHasUpdate;

        [SerializeField] private LocksControl locksControl;
        [SerializeField] private GitLock selectedEntry = GitLock.Default;

        [SerializeField] private CacheUpdateEvent lastLocksChangedEvent;
        [SerializeField] private CacheUpdateEvent lastStatusEntriesChangedEvent;

        [SerializeField] private List<GitLock> lockedFiles = new List<GitLock>();
        [SerializeField] private List<GitStatusEntry> gitStatusEntries = new List<GitStatusEntry>();

        [SerializeField] private string currentUsername;

        public override void OnEnable()
        {
            base.OnEnable();

            if (locksControl != null)
            {
                locksControl.LoadIcons();
            }

            AttachHandlers(Repository);
            ValidateCachedData(Repository);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            DetachHandlers(Repository);
        }

        public override void Refresh()
        {
            base.Refresh();
            Repository.Refresh(CacheType.GitStatus);
            Repository.Refresh(CacheType.GitLocks);
        }

        public override void OnDataUpdate()
        {
            base.OnDataUpdate();
            MaybeUpdateData();
        }

        public override void OnGUI()
        {
            var rect = GUILayoutUtility.GetLastRect();
            if (locksControl != null)
            {
                var lockControlRect = new Rect(0f, 0f, Position.width, Position.height - rect.height);

                var requiresRepaint = locksControl.Render(lockControlRect,
                    entry => {
                        selectedEntry = entry;
                    },
                    entry => { }, 
                    entry => {
                        string unlockFile;
                        GenericMenu.MenuFunction menuFunction;

                        if (entry.Owner.Name == currentUsername)
                        {
                            unlockFile = "Unlock File";
                            menuFunction = UnlockSelectedEntry;
                        }
                        else
                        {
                            unlockFile = "Force Unlock File";
                            menuFunction = ForceUnlockSelectedEntry;
                        }

                        var menu = new GenericMenu();
                        menu.AddItem(new GUIContent(unlockFile), false, menuFunction);
                        menu.ShowAsContext();
                    });

                if (requiresRepaint)
                    Redraw();
            }
        }

        private void UnlockSelectedEntry()
        {
            Repository
                .ReleaseLock(selectedEntry.Path, false)
                .Start();
        }

        private void ForceUnlockSelectedEntry()
        {
            Repository
                .ReleaseLock(selectedEntry.Path, true)
                .Start();
        }

        private void AttachHandlers(IRepository repository)
        {
            if (repository == null)
            {
                return;
            }

            repository.LocksChanged += RepositoryOnLocksChanged;
            repository.LocksChanged += RepositoryOnStatusEntriesChanged;
        }

        private void RepositoryOnLocksChanged(CacheUpdateEvent cacheUpdateEvent)
        {
            if (!lastLocksChangedEvent.Equals(cacheUpdateEvent))
            {
                lastLocksChangedEvent = cacheUpdateEvent;
                currentLocksHasUpdate = true;
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

        private void DetachHandlers(IRepository repository)
        {
            if (repository == null)
            {
                return;
            }

            repository.LocksChanged -= RepositoryOnLocksChanged;
            repository.LocksChanged -= RepositoryOnStatusEntriesChanged;
        }

        private void ValidateCachedData(IRepository repository)
        {
            repository.CheckAndRaiseEventsIfCacheNewer(CacheType.GitLocks, lastLocksChangedEvent);
            repository.CheckAndRaiseEventsIfCacheNewer(CacheType.GitStatus, lastStatusEntriesChangedEvent);
        }

        private void MaybeUpdateData()
        {
            if (Repository == null)
            {
                return;
            }

            if (currentLocksHasUpdate)
            {
                lockedFiles = Repository.CurrentLocks;

                //TODO: ONE_USER_LOGIN This assumes only ever one user can login
                var keychainConnection = Platform.Keychain.Connections.First();
                currentUsername = keychainConnection.Username;
            }

            if (currentStatusEntriesHasUpdate)
            {
                gitStatusEntries = Repository.CurrentChanges.Where(x => x.Status != GitFileStatus.Ignored).ToList();
            }

            if (currentStatusEntriesHasUpdate || currentLocksHasUpdate)
            {
                currentStatusEntriesHasUpdate = false;
                currentLocksHasUpdate = false;

                BuildLocksControl();
            }
        }

        private void BuildLocksControl()
        {
            if (locksControl == null)
            {
                locksControl = new LocksControl();
            }

            locksControl.Load(lockedFiles, gitStatusEntries);

            if (!selectedEntry.Equals(GitLock.Default)
                && selectedEntry.ID != locksControl.SelectedEntry.GitLock.ID)
            {
                selectedEntry = GitLock.Default;
            }
        }
        public override void OnSelectionChange()
        {
            base.OnSelectionChange();
            if (locksControl.OnSelectionChange())
            {
                Redraw();
            }
        }
    }
}
