using System.Threading;

namespace GitHub.Unity
{
    class GitLfsInstallTask : ProcessTask<string>
    {
        private const string TaskName = "git lsf install";

        public GitLfsInstallTask(CancellationToken token, IOutputProcessor<string> processor = null)
            : base(token, processor ?? new SimpleOutputProcessor())
        {
            Name = TaskName;
        }

        public override string ProcessArguments { get { return "lfs install"; } }
        public override TaskAffinity Affinity { get { return TaskAffinity.Exclusive; } }
    }
}