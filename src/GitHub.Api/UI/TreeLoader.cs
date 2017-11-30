using System;
using System.Collections.Generic;
using System.Linq;

namespace GitHub.Unity
{
    public interface ITree
    {
        void AddNode(string fullPath, string path, string label, int level, bool isFolder, bool isActive, bool isHidden, bool isCollapsed);
        void Clear();
        HashSet<string> GetCollapsedFolders();
        bool DisplayRootNode { get; }
        bool IsCheckable { get; }
        string PathSeparator { get; }
        string PathIgnoreRoot { get; }
    }

    public static class TreeLoader
    {
        public static void Load(ITree tree, IEnumerable<ITreeData> data, string title)
        {
            var collapsedFolders = tree.GetCollapsedFolders();

            tree.Clear();

            var displayRootLevel = tree.DisplayRootNode ? 1 : 0;

            tree.AddNode(fullPath: title, path: title, label: title, level: -1 + displayRootLevel, isFolder: true, isActive: false, isHidden: false, isCollapsed: false);

            var hideChildren = false;
            var hideChildrenBelowLevel = 0;

            var folders = new HashSet<string>();

            foreach (var d in data)
            {
                var path = d.Path;
                if (tree.PathIgnoreRoot != null)
                {
                    var indexOf = path.IndexOf(tree.PathIgnoreRoot);
                    if (indexOf != -1)
                    {
                        path = path.Substring(indexOf + tree.PathIgnoreRoot.Length);
                    }
                }

                var parts = path.Split(new[] { tree.PathSeparator }, StringSplitOptions.None);
                for (int i = 0; i < parts.Length; i++)
                {
                    var label = parts[i];
                    var level = i + 1;
                    var nodePath = String.Join(tree.PathSeparator, parts, 0, level);
                    var isFolder = i < parts.Length - 1;
                    var alreadyExists = folders.Contains(nodePath);
                    if (!alreadyExists)
                    {
                        var nodeIsHidden = false;
                        if (hideChildren)
                        {
                            if (level <= hideChildrenBelowLevel)
                            {
                                hideChildren = false;
                            }
                            else
                            {
                                nodeIsHidden = true;
                            }
                        }

                        var nodeIsCollapsed = false;
                        if (isFolder)
                        {
                            folders.Add(nodePath);

                            if (collapsedFolders.Contains(nodePath))
                            {
                                nodeIsCollapsed = true;

                                if (!hideChildren)
                                {
                                    hideChildren = true;
                                    hideChildrenBelowLevel = level;
                                }
                            }

                        }

                        tree.AddNode(fullPath: d.FullPath, path: nodePath, label: label, level: i + displayRootLevel, isFolder: isFolder, isActive: d.IsActive, isHidden: nodeIsHidden, isCollapsed: nodeIsCollapsed);
                    }
                }
            }
        }

    }
}