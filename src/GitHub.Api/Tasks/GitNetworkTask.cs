using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GitHub.Api;

namespace GitHub.Unity
{
    class GitNetworkTask : GitTask
    {
        private readonly ICredentialManager credentialManager;
        private readonly IUIDispatcher uiDispatcher;

        public GitNetworkTask(IEnvironment environment, IProcessManager processManager, ITaskResultDispatcher resultDispatcher,
            ICredentialManager credentialManager, IUIDispatcher uiDispatcher,
            Action<string> onSuccess, Action onFailure)
            : base(environment, processManager, resultDispatcher, onSuccess, onFailure)
        {
            Guard.ArgumentNotNull(credentialManager, nameof(credentialManager));
            Guard.ArgumentNotNull(uiDispatcher, nameof(uiDispatcher));

            this.credentialManager = credentialManager;
            this.uiDispatcher = uiDispatcher;
        }

        public override async void Run()
        {
            await RunAsync(new CancellationToken());
        }

        public override async Task<bool> RunAsync(CancellationToken cancel)
        {
            var canRun = !string.IsNullOrEmpty(Environment.GitExecutablePath);

            if (!canRun)
            {
                RaiseOnFailure(Localization.NoGitError, FailureSeverity.Moderate);
                Abort();
                return false;
            }

            canRun = await credentialManager.Load(Environment.RepositoryHost) == null;
            if (!canRun)
            {
                canRun = await uiDispatcher.RunUI();
            }

            if (!canRun)
            {
                RaiseOnFailure(Localization.NotLoggedIn, FailureSeverity.Moderate);
                Abort();
                return false;
            }
            
            return await base.RunAsync(cancel);
        }
    }

    interface IUIDispatcher
    {
        Task<bool> RunUI();
    }

    class BaseUIDispatcher : IUIDispatcher
    {
        private static readonly ILogging logger = Logging.GetLogger<BaseUIDispatcher>();

        public event Action<bool> OnClose;

        public async Task<bool> RunUI()
        {
            Action<bool> onClose = RaiseOnClose;

            var ret = await TaskExt.FromEvent<Action<bool>, bool>(
                getHandler: (completeAction, cancelAction, rejectAction) =>
                    (eventArgs) => completeAction(eventArgs),
                subscribe: eventHandler =>
                    this.OnClose += eventHandler,
                unsubscribe: eventHandler =>
                    this.OnClose -= eventHandler,
                initiate: (completeAction, cancelAction, rejectAction) =>
                    Run(onClose),
                token: CancellationToken.None);

            logger.Debug("Authentication done");
            return ret;
        }

        protected virtual void Run(Action<bool> onClose)
        {
        }

        protected void RaiseOnClose(bool result)
        {
            OnClose?.Invoke(result);
        }
    }
}