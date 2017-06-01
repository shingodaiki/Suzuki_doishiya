using System.Linq;
using GitHub.Unity;

namespace IntegrationTests
{
    class BaseGitEnvironmentTest : BaseGitRepoTest
    {
        protected IEnvironment InitializeEnvironment(NPath repoPath, NPath environmentPath = null, bool enableEnvironmentTrace = false)
        {
            TaskManager = new TaskManager();
            var sc = new ThreadSynchronizationContext(TaskManager.Token);
            TaskManager.UIScheduler = new SynchronizationContextTaskScheduler(sc);

            Environment = new IntegrationTestEnvironment(repoPath, SolutionDirectory, environmentPath, enableEnvironmentTrace);

            Platform = new Platform(Environment);
            GitEnvironment = Platform.GitEnvironment;
            ProcessManager = new ProcessManager(Environment, GitEnvironment);

            Platform.Initialize(ProcessManager, TaskManager);


            GitClient = new GitClient(Environment, ProcessManager, Platform.CredentialManager, TaskManager);

            var repositoryManagerFactory = new RepositoryManagerFactory();
            RepositoryManager = repositoryManagerFactory.CreateRepositoryManager(Platform, TaskManager, GitClient, repoPath);
            RepositoryManager.Initialize();
            RepositoryManager.Start();

            Environment.Repository = RepositoryManager.Repository;

            DotGitPath = repoPath.Combine(".git");

            if (DotGitPath.FileExists())
            {
                DotGitPath =
                    DotGitPath.ReadAllLines()
                              .Where(x => x.StartsWith("gitdir:"))
                              .Select(x => x.Substring(7).Trim().ToNPath())
                              .First();
            }

            BranchesPath = DotGitPath.Combine("refs", "heads");
            RemotesPath = DotGitPath.Combine("refs", "remotes");
            DotGitIndex = DotGitPath.Combine("index");
            DotGitHead = DotGitPath.Combine("HEAD");
            DotGitConfig = DotGitPath.Combine("config");
            return Environment;
        }

        protected IEnvironment Initialize(NPath repoPath, NPath environmentPath = null, bool enableEnvironmentTrace = false)
        {
            InitializeEnvironment(repoPath, environmentPath, enableEnvironmentTrace);
            var gitSetup = new GitSetup(Environment, TaskManager.Token);
            gitSetup.SetupIfNeeded().Wait();

            Environment.GitExecutablePath = gitSetup.GitExecutablePath;
            return Environment;
        }

        protected override void OnTearDown()
        {
            RepositoryManager?.Stop();
            RepositoryManager?.Dispose();
            RepositoryManager = null;
            base.OnTearDown();
        }

        public IRepositoryManager RepositoryManager { get; private set; }

        protected IPlatform Platform { get; private set; }

        protected IProcessManager ProcessManager { get; private set; }
        protected ITaskManager TaskManager { get; private set; }

        protected IProcessEnvironment GitEnvironment { get; private set; }
        protected IGitClient GitClient { get; set; }

        protected NPath DotGitConfig { get; private set; }

        protected NPath DotGitHead { get; private set; }

        protected NPath DotGitIndex { get; private set; }

        protected NPath RemotesPath { get; private set; }

        protected NPath BranchesPath { get; private set; }

        protected NPath DotGitPath { get; private set; }
    }
}
