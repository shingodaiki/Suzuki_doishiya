using System;

namespace GitHub.Unity
{
    [Serializable]
    public struct GitStatusEntry
    {
        public static GitStatusEntry Default = new GitStatusEntry();
        
        public string path;
        public string fullPath;
        public string projectPath;
        public string originalPath;
        public GitFileStatus status;
        public bool staged;

        public GitStatusEntry(string path, string fullPath, string projectPath,
            GitFileStatus status,
            string originalPath = null, bool staged = false)
        {
            Guard.ArgumentNotNullOrWhiteSpace(path, "path");
            Guard.ArgumentNotNullOrWhiteSpace(fullPath, "fullPath");
            Guard.ArgumentNotNullOrWhiteSpace(projectPath, "projectPath");

            this.path = path;
            this.status = status;
            this.fullPath = fullPath;
            this.projectPath = projectPath;
            this.originalPath = originalPath;
            this.staged = staged;
        }

        public string Path => path;

        public string FullPath => fullPath;

        public string ProjectPath => projectPath;

        public string OriginalPath => originalPath;

        public GitFileStatus Status => status;

        public bool Staged => staged;

        public override string ToString()
        {
            return $"Path:'{Path}' Status:'{Status}' FullPath:'{FullPath}' ProjectPath:'{ProjectPath}' OriginalPath:'{OriginalPath}' Staged:'{Staged}'";
        }
    }

    [Serializable]
    public struct GitStatusEntryTreeData : ITreeData
    {
        public static GitStatusEntryTreeData Default = new GitStatusEntryTreeData(GitStatusEntry.Default);

        public GitStatusEntry gitStatusEntry;

        public GitStatusEntryTreeData(GitStatusEntry gitStatusEntry)
        {
            this.gitStatusEntry = gitStatusEntry;
        }

        public string Name => gitStatusEntry.FullPath;
        public bool IsActive => false;
        public GitStatusEntry GitStatusEntry => gitStatusEntry;
    }
}
