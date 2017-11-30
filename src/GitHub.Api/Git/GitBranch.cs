using System;

namespace GitHub.Unity
{
    [Serializable]
    public struct GitBranch
    {
        public static GitBranch Default = new GitBranch();

        public string name;
        public string tracking;
        public bool isActive;

        public GitBranch(string name, string tracking, bool active)
        {
            Guard.ArgumentNotNullOrWhiteSpace(name, "name");

            this.name = name;
            this.tracking = tracking;
            this.isActive = active;
        }

        public string Name => name;
        public string Tracking => tracking;
        public bool IsActive => isActive;

        public override string ToString()
        {
            return $"{Name} Tracking? {Tracking} Active? {IsActive}";
        }
    }
}