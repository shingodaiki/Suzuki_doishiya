using System;
using GitHub.Api;

namespace GitHub.Unity
{
    class GitConfigGetTask : GitTask
    {
        private readonly string arguments;
        private string result;

        public GitConfigGetTask(IEnvironment environment, IProcessManager processManager, ITaskResultDispatcher resultDispatcher,
            string key, GitConfigSource configSource, Action<string> onSuccess, Action onFailure)
            : base(environment, processManager, resultDispatcher,
                  str =>
                  {
                      var logger = Logging.GetLogger<GitConfigGetTask>();
                      logger.Debug("WTF {0}", str);
                      onSuccess(str);
                  }, onFailure)
        {
            var source = "";
            source +=
                configSource == GitConfigSource.NonSpecified ? "--get-all" :
                configSource == GitConfigSource.Local ? "--get --local" :
                configSource == GitConfigSource.User ? "--get --global" :
                "--get --system";
            arguments = String.Format("config {0} {1}", source, key);
        }

        protected override ProcessOutputManager HookupOutput(IProcess process)
        {
            var processor = new BaseOutputProcessor();
            processor.OnData += s =>
            {
                if (String.IsNullOrEmpty(result))
                {
                    result = s;
                }
            };
            return new ProcessOutputManager(process, processor);
        }

        protected override void RaiseOnSuccess(string msg)
        {
            if (String.IsNullOrEmpty(result))
            {
                RaiseOnFailure("No value returned for " + arguments);
            }
            else
            {
                base.RaiseOnSuccess(result);
            }
        }

        public override bool Blocking { get { return false; } }
        public override bool Critical { get { return false; } }
        public override bool Cached { get { return false; } }
        public override string Label { get { return "git config get"; } }
        protected override string ProcessArguments { get { return arguments; } }
    }
}