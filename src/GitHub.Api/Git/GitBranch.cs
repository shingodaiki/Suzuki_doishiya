using System;

namespace GitHub.Unity
{
    [Serializable]
    public struct GitBranch
    {
        public static GitBranch Default = new GitBranch();

        public string name;
        public string tracking;

        public GitBranch(string name, string tracking = null)
        {
            Guard.ArgumentNotNullOrWhiteSpace(name, "name");

            this.name = name;
            this.tracking = tracking ?? string.Empty;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + (name?.GetHashCode() ?? 0);
            hash = hash * 23 + (tracking?.GetHashCode() ?? 0);
            return hash;
        }

        public override bool Equals(object other)
        {
            if (other is GitBranch)
                return Equals((GitBranch)other);
            return false;
        }

        public bool Equals(GitBranch other)
        {
            return
                String.Equals(name, other.name) &&
                String.Equals(tracking, other.tracking);
        }

        public static bool operator ==(GitBranch lhs, GitBranch rhs)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(lhs, rhs))
                return true;

            // If one is null, but not both, return false.
            if (((object)lhs == null) || ((object)rhs == null))
                return false;

            // Return true if the fields match:
            return lhs.Equals(rhs);
        }

        public static bool operator !=(GitBranch lhs, GitBranch rhs)
        {
            return !(lhs == rhs);
        }

        public string Name => name;
        public string Tracking => tracking;

        public override string ToString()
        {
            return $"{Name} Tracking? {Tracking ?? "[NULL]"}";
        }
    }
}
