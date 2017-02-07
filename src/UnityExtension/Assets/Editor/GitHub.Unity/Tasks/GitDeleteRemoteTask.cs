using System;

namespace GitHub.Unity
{
    class GitDeleteRemoteTask : GitTask
    {
        private readonly string repository;
        private readonly string branch;

        private GitDeleteRemoteTask(Action onSuccess, Action onFailure, string repository, string branch)
            : base(str => onSuccess.SafeInvoke(), onFailure)
        {
            this.repository = repository;
            this.branch = branch;
        }

        public static void Schedule(Action onSuccess, string repository, string branch, Action onFailure = null)
        {
            Tasks.Add(new GitDeleteRemoteTask(onSuccess, onFailure, repository, branch));
        }

        public override bool Blocking
        {
            get { return false; }
        }

        public override TaskQueueSetting Queued
        {
            get { return TaskQueueSetting.Queue; }
        }

        public override bool Critical
        {
            get { return false; }
        }

        public override bool Cached
        {
            get { return true; }
        }

        public override string Label
        {
            get { return "git push --delete"; }
        }

        protected override string ProcessArguments
        {
            get
            {
                return string.Format("push {0} --delete {1}", repository, branch);
            }
        }
    }
}