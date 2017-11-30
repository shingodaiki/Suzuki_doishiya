﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace GitHub.Unity
{
    [Serializable]
    public class TreeNodeDictionary : SerializableDictionary<string, TreeNode> { }

    [Serializable]
    public abstract class Tree
    {
        public static float ItemHeight { get { return EditorGUIUtility.singleLineHeight; } }
        public static float ItemSpacing { get { return EditorGUIUtility.standardVerticalSpacing; } }

        [SerializeField] public Rect Margin = new Rect();
        [SerializeField] public Rect Padding = new Rect();

        [SerializeField] public string PathIgnoreRoot;
        [SerializeField] public string PathSeparator = "/";
        [SerializeField] public bool DisplayRootNode = true;
        [SerializeField] public bool Selectable = false;
        [SerializeField] public GUIStyle FolderStyle;
        [SerializeField] public GUIStyle TreeNodeStyle;
        [SerializeField] public GUIStyle ActiveTreeNodeStyle;

        [SerializeField] private List<TreeNode> nodes = new List<TreeNode>();
        [SerializeField] private TreeNode selectedNode = null;
        [SerializeField] private TreeNode activeNode = null;
        [SerializeField] private TreeNodeDictionary folders = new TreeNodeDictionary();

        [NonSerialized] private Stack<bool> indents = new Stack<bool>();

        public bool IsInitialized { get { return nodes != null && nodes.Count > 0 && !String.IsNullOrEmpty(nodes[0].Name); } }
        public bool RequiresRepaint { get; private set; }

        public TreeNode SelectedNode
        {
            get
            {
                if (selectedNode != null && String.IsNullOrEmpty(selectedNode.Name))
                    selectedNode = null;
                return selectedNode;
            }
            private set
            {
                selectedNode = value;
            }
        }

        public TreeNode ActiveNode { get { return activeNode; } }

        public void Load(IEnumerable<ITreeData> data, string title)
        {
            var collapsedFoldersEnumerable = folders.Where(pair => pair.Value.IsCollapsed).Select(pair => pair.Key);
            var collapsedFolders = new HashSet<string>(collapsedFoldersEnumerable);

            folders.Clear();
            nodes.Clear();

            var displayRootLevel = DisplayRootNode ? 1 : 0;

            var titleNode = new TreeNode()
            {
                Name = title,
                Label = title,
                Level = -1 + displayRootLevel,
                IsFolder = true,
                Selectable = Selectable
            };
            SetNodeIcon(titleNode);
            nodes.Add(titleNode);

            var hideChildren = false;
            var hideChildrenBelowLevel = 0;

            foreach (var d in data)
            {
                var fullName = d.Name;
                if (PathIgnoreRoot != null)
                {
                    var indexOf = fullName.IndexOf(PathIgnoreRoot);
                    if (indexOf != -1)
                    {
                        fullName = fullName.Substring(indexOf + PathIgnoreRoot.Length);
                    }
                }

                var parts = fullName.Split(new [] {PathSeparator}, StringSplitOptions.None);
                for (int i = 0; i < parts.Length; i++)
                {
                    var label = parts[i];
                    var level = i + 1;
                    var name = String.Join(PathSeparator, parts, 0, level);
                    var isFolder = i < parts.Length - 1;
                    var alreadyExists = folders.ContainsKey(name);
                    if (!alreadyExists)
                    {
                        var node = new TreeNode
                        {
                            Name = name,
                            IsActive = d.IsActive,
                            Label = label,
                            Level = i + displayRootLevel,
                            IsFolder = isFolder,
                            Selectable = Selectable
                        };

                        if (node.IsActive)
                        {
                            activeNode = node;
                        }

                        if (hideChildren)
                        {
                            if (level <= hideChildrenBelowLevel)
                            {
                                hideChildren = false;
                            }
                            else
                            {
                                node.IsHidden = true;
                            }
                        }

                        SetNodeIcon(node);

                        nodes.Add(node);
                        if (isFolder)
                        {
                            if (collapsedFolders.Contains(name))
                            {
                                node.IsCollapsed = true;

                                if (!hideChildren)
                                {
                                    hideChildren = true;
                                    hideChildrenBelowLevel = level;
                                }
                            }

                            folders.Add(name, node);
                        }
                    }
                }
            }
        }

        public Rect Render(Rect rect, Vector2 scroll, Action<TreeNode> singleClick = null, Action<TreeNode> doubleClick = null, Action<TreeNode> rightClick = null)
        {
            RequiresRepaint = false;
            rect = new Rect(0f, rect.y, rect.width, ItemHeight);

            var level = 0;

            if (DisplayRootNode)
            {
                var titleNode = nodes[0];
                var selectionChanged = titleNode.Render(rect, Styles.TreeIndentation, selectedNode == titleNode, FolderStyle, TreeNodeStyle, ActiveTreeNodeStyle);

                if (selectionChanged)
                {
                    ToggleNodeVisibility(0, titleNode);
                }

                RequiresRepaint = HandleInput(rect, titleNode, 0);
                rect.y += ItemHeight + ItemSpacing;

                Indent();
                level = 1;
            }

            int i = 1;
            for (; i < nodes.Count; i++)
            {
                var node = nodes[i];
                if (node.Level > level && !node.IsHidden)
                {
                    Indent();
                }
                var changed = node.Render(rect, Styles.TreeIndentation, selectedNode == node, FolderStyle, TreeNodeStyle, ActiveTreeNodeStyle);

                if (node.IsFolder && changed)
                {
                    // toggle visibility for all the nodes under this one
                    ToggleNodeVisibility(i, node);
                }

                if (node.Level < level)
                {
                    for (; node.Level > level && indents.Count > 1; level--)
                    {
                        Unindent();
                    }
                }
                level = node.Level;

                if (!node.IsHidden)
                {
                    RequiresRepaint = HandleInput(rect, node, i, singleClick, doubleClick, rightClick);
                    rect.y += ItemHeight + ItemSpacing;
                }
            }

            Unindent();

            return rect;
        }

        public void Focus()
        {
            bool selectionChanged = false;
            if (Event.current.type == EventType.KeyDown)
            {
                int directionY = Event.current.keyCode == KeyCode.UpArrow ? -1 : Event.current.keyCode == KeyCode.DownArrow ? 1 : 0;
                int directionX = Event.current.keyCode == KeyCode.LeftArrow ? -1 : Event.current.keyCode == KeyCode.RightArrow ? 1 : 0;

                if (directionY < 0 || directionX < 0)
                {
                    SelectedNode = nodes[nodes.Count - 1];
                    selectionChanged = true;
                    Event.current.Use();
                }
                else if (directionY > 0 || directionX > 0)
                {
                    SelectedNode = nodes[0];
                    selectionChanged = true;
                    Event.current.Use();
                }
            }
            RequiresRepaint = selectionChanged;
        }

        public void Blur()
        {
            SelectedNode = null;
            RequiresRepaint = true;
        }

        private int ToggleNodeVisibility(int idx, TreeNode rootNode)
        {
            var nodeLevel = node.Level;
            node.IsCollapsed = !node.IsCollapsed;
            idx++;
            for (; idx < nodes.Count && nodes[idx].Level > nodeLevel; idx++)
            {
                nodes[idx].IsHidden = node.IsCollapsed;
                if (nodes[idx].IsFolder && !node.IsCollapsed && nodes[idx].IsCollapsed)
                {
                    var level = nodes[idx].Level;
                    for (idx++; idx < nodes.Count && nodes[idx].Level > level; idx++) { }
                    idx--;
                }
            }
            if (SelectedNode != null && SelectedNode.IsHidden)
            {
                SelectedNode = node;
            }
            return idx;
        }

        private bool HandleInput(Rect rect, TreeNode currentNode, int index, Action<TreeNode> singleClick = null, Action<TreeNode> doubleClick = null, Action<TreeNode> rightClick = null)
        {
            bool selectionChanged = false;
            var clickRect = new Rect(0f, rect.y, rect.width, rect.height);
            if (Event.current.type == EventType.MouseDown && clickRect.Contains(Event.current.mousePosition))
            {
                Event.current.Use();
                SelectedNode = currentNode;
                selectionChanged = true;
                var clickCount = Event.current.clickCount;
                var mouseButton = Event.current.button;

                if (mouseButton == 0 && clickCount == 1 && singleClick != null)
                {
                    singleClick(currentNode);
                }
                if (mouseButton == 0 && clickCount > 1 && doubleClick != null)
                {
                    doubleClick(currentNode);
                }
                if (mouseButton == 1 && clickCount == 1 && rightClick != null)
                {
                    rightClick(currentNode);
                }
            }

            // Keyboard navigation if this child is the current selection
            if (currentNode == selectedNode && Event.current.type == EventType.KeyDown)
            {
                int directionY = Event.current.keyCode == KeyCode.UpArrow ? -1 : Event.current.keyCode == KeyCode.DownArrow ? 1 : 0;
                int directionX = Event.current.keyCode == KeyCode.LeftArrow ? -1 : Event.current.keyCode == KeyCode.RightArrow ? 1 : 0;
                if (directionY != 0 || directionX != 0)
                {
                    if (directionY > 0)
                    {
                        selectionChanged = SelectNext(index, false) != index;
                    }
                    else if (directionY < 0)
                    {
                        selectionChanged = SelectPrevious(index, false) != index;
                    }
                    else if (directionX > 0)
                    {
                        if (currentNode.IsFolder && currentNode.IsCollapsed)
                        {
                            ToggleNodeVisibility(index, currentNode);
                            Event.current.Use();
                        }
                        else
                        {
                            selectionChanged = SelectNext(index, true) != index;
                        }
                    }
                    else if (directionX < 0)
                    {
                        if (currentNode.IsFolder && !currentNode.IsCollapsed)
                        {
                            ToggleNodeVisibility(index, currentNode);
                            Event.current.Use();
                        }
                        else
                        {
                            selectionChanged = SelectPrevious(index, true) != index;
                        }
                    }
                }
            }
            return selectionChanged;
        }

        private int SelectNext(int index, bool foldersOnly)
        {
            for (index++; index < nodes.Count; index++)
            {
                if (nodes[index].IsHidden)
                    continue;
                if (!nodes[index].IsFolder && foldersOnly)
                    continue;
                break;
            }

            if (index < nodes.Count)
            {
                SelectedNode = nodes[index];
                Event.current.Use();
            }
            else
            {
                SelectedNode = null;
            }
            return index;
        }

        private int SelectPrevious(int index, bool foldersOnly)
        {
            for (index--; index >= 0; index--)
            {
                if (nodes[index].IsHidden)
                    continue;
                if (!nodes[index].IsFolder && foldersOnly)
                    continue;
                break;
            }

            if (index >= 0)
            {
                SelectedNode = nodes[index];
                Event.current.Use();
            }
            else
            {
                SelectedNode = null;
            }
            return index;
        }

        private void Indent()
        {
            indents.Push(true);
        }

        private void Unindent()
        {
            indents.Pop();
        }

        private void SetNodeIcon(TreeNode node)
        {
            node.Icon = GetNodeIcon(node);
            node.Load();
        }

        protected abstract Texture2D GetNodeIcon(TreeNode node);

        protected void LoadNodeIcons()
        {
            foreach (var treeNode in nodes)
            {
                SetNodeIcon(treeNode);
            }
        }
    }

    [Serializable]
    public class TreeNode
    {
        public string Name;
        public string Label;
        public int Level;
        public bool IsFolder;
        public bool IsCollapsed;
        public bool IsHidden;
        public bool IsActive;
        public GUIContent content;
        [NonSerialized] public Texture2D Icon;
        public bool Selectable;

        public void Load()
        {
            content = new GUIContent(Label, Icon);
        }

        public bool Render(Rect rect, float indentation, bool isSelected, GUIStyle toggleStyle, GUIStyle nodeStyle, GUIStyle activeNodeStyle)
        {
            if (IsHidden)
                return false;

            var changed = false;
            var fillRect = rect;
            var nodeStartX = Level * indentation * (Selectable ? 2 : 1);

            if (Selectable && Level > 0)
            {
                nodeStartX += 2 * Level;
            }

            var nodeRect = new Rect(nodeStartX, rect.y, rect.width, rect.height);

            var data = string.Format("Label: {0} ", Label);
            data += string.Format("Start: {0} ", nodeStartX);

            if (Event.current.type == EventType.repaint)
            {
                nodeStyle.Draw(fillRect, GUIContent.none, false, false, false, isSelected);
            }

            var styleOn = false;
            if (IsFolder)
            {
                data += string.Format("FolderStart: {0} ", nodeStartX);

                var toggleRect = new Rect(nodeStartX, nodeRect.y, indentation, nodeRect.height);
                nodeStartX += toggleRect.width;

                styleOn = !IsCollapsed;

                if (Event.current.type == EventType.repaint)
                {
                    toggleStyle.Draw(toggleRect, GUIContent.none, false, false, styleOn, isSelected);
                }

                EditorGUI.BeginChangeCheck();
                {
                    GUI.Toggle(toggleRect, !IsCollapsed, GUIContent.none, GUIStyle.none);
                }
                changed = EditorGUI.EndChangeCheck();
            }

            if (Selectable)
            {
                data += string.Format("SelectStart: {0} ", nodeStartX);

                var selectRect = new Rect(nodeStartX, nodeRect.y, indentation, nodeRect.height);

                nodeStartX += selectRect.width + 2;

                EditorGUI.BeginChangeCheck();
                {
                    GUI.Toggle(selectRect, false, GUIContent.none, Styles.ToggleMixedStyle);
                }
                EditorGUI.EndChangeCheck();
            }

            data += string.Format("ContentStart: {0} ", nodeStartX);
            var contentStyle = IsActive ? activeNodeStyle : nodeStyle;

            var contentRect = new Rect(nodeStartX, rect.y, rect.width, rect.height);
            if (Event.current.type == EventType.repaint)
            {
                contentStyle.Draw(contentRect, content, false, false, styleOn, isSelected);
            }

            Debug.Log(data);

            return changed;
        }

        public override string ToString()
        {
            return String.Format("name:{0} label:{1} level:{2} isFolder:{3} isCollapsed:{4} isHidden:{5} isActive:{6}",
                Name, Label, Level, IsFolder, IsCollapsed, IsHidden, IsActive);
        }
    }

    [Serializable]
    public class BranchesTree: Tree
    {
        [SerializeField] public bool IsRemote;
        
        [NonSerialized] public Texture2D ActiveBranchIcon;
        [NonSerialized] public Texture2D BranchIcon;
        [NonSerialized] public Texture2D FolderIcon;
        [NonSerialized] public Texture2D GlobeIcon;

        protected override Texture2D GetNodeIcon(TreeNode node)
        {
            Texture2D nodeIcon;
            if (node.IsActive)
            {
                nodeIcon = ActiveBranchIcon;
            }
            else if (node.IsFolder)
            {
                nodeIcon = IsRemote && node.Level == 1
                    ? GlobeIcon
                    : FolderIcon;
            }
            else
            {
                nodeIcon = BranchIcon;
            }
            return nodeIcon;
        }


        public void UpdateIcons(Texture2D activeBranchIcon, Texture2D branchIcon, Texture2D folderIcon, Texture2D globeIcon)
        {
            var needsLoad = ActiveBranchIcon == null || BranchIcon == null || FolderIcon == null || GlobeIcon == null;
            if (needsLoad)
            {
                ActiveBranchIcon = activeBranchIcon;
                BranchIcon = branchIcon;
                FolderIcon = folderIcon;
                GlobeIcon = globeIcon;

                LoadNodeIcons();
            }
        }
    }
}
