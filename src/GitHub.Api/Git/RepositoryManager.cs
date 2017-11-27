﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GitHub.Unity
{
    public interface IRepositoryManager : IDisposable
    {
        event Action<bool> IsBusyChanged;
        event Action<ConfigBranch?, ConfigRemote?> CurrentBranchUpdated;
        event Action<GitStatus> GitStatusUpdated;
        event Action<List<GitLock>> GitLocksUpdated;
        event Action<List<GitLogEntry>> GitLogUpdated;
        event Action<Dictionary<string, ConfigBranch>> LocalBranchesUpdated;
        event Action<Dictionary<string, ConfigRemote>, Dictionary<string, Dictionary<string, ConfigBranch>>> RemoteBranchesUpdated;

        void Initialize();
        void Start();
        void Stop();
        ITask CommitAllFiles(string message, string body);
        ITask CommitFiles(List<string> files, string message, string body);
        ITask Fetch(string remote);
        ITask Pull(string remote, string branch);
        ITask Push(string remote, string branch);
        ITask Revert(string changeset);
        ITask RemoteAdd(string remote, string url);
        ITask RemoteRemove(string remote);
        ITask RemoteChange(string remote, string url);
        ITask SwitchBranch(string branch);
        ITask DeleteBranch(string branch, bool deleteUnmerged = false);
        ITask CreateBranch(string branch, string baseBranch);
        ITask LockFile(string file);
        ITask UnlockFile(string file, bool force);
        void UpdateGitLog();
        void UpdateGitStatus();
        void UpdateLocks();
        int WaitForEvents();

        IGitConfig Config { get; }
        IGitClient GitClient { get; }
        bool IsBusy { get; }
    }

    interface IRepositoryPathConfiguration
    {
        NPath RepositoryPath { get; }
        NPath DotGitPath { get; }
        NPath BranchesPath { get; }
        NPath RemotesPath { get; }
        NPath DotGitIndex { get; }
        NPath DotGitHead { get; }
        NPath DotGitConfig { get; }
    }

    class RepositoryPathConfiguration : IRepositoryPathConfiguration
    {
        public RepositoryPathConfiguration(NPath repositoryPath)
        {
            RepositoryPath = repositoryPath;

            DotGitPath = repositoryPath.Combine(".git");
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
            DotGitCommitEditMsg = DotGitPath.Combine("COMMIT_EDITMSG");
        }

        public NPath RepositoryPath { get; }
        public NPath DotGitPath { get; }
        public NPath BranchesPath { get; }
        public NPath RemotesPath { get; }
        public NPath DotGitIndex { get; }
        public NPath DotGitHead { get; }
        public NPath DotGitConfig { get; }
        public NPath DotGitCommitEditMsg { get; }
    }

    class RepositoryManager : IRepositoryManager
    {
        private readonly IGitConfig config;
        private readonly IGitClient gitClient;
        private readonly IPlatform platform;
        private readonly IRepositoryPathConfiguration repositoryPaths;
        private readonly IRepositoryWatcher watcher;

        private bool isBusy;

        public event Action<ConfigBranch?, ConfigRemote?> CurrentBranchUpdated;
        public event Action<bool> IsBusyChanged;
        public event Action<GitStatus> GitStatusUpdated;
        public event Action<List<GitLock>> GitLocksUpdated;
        public event Action<List<GitLogEntry>> GitLogUpdated;
        public event Action<Dictionary<string, ConfigBranch>> LocalBranchesUpdated;
        public event Action<Dictionary<string, ConfigRemote>, Dictionary<string, Dictionary<string, ConfigBranch>>> RemoteBranchesUpdated;

        public RepositoryManager(IPlatform platform, IGitConfig gitConfig,
            IRepositoryWatcher repositoryWatcher, IGitClient gitClient,
            IRepositoryPathConfiguration repositoryPaths)
        {
            this.repositoryPaths = repositoryPaths;
            this.platform = platform;
            this.gitClient = gitClient;
            this.watcher = repositoryWatcher;
            this.config = gitConfig;

            SetupWatcher();
        }

        public static RepositoryManager CreateInstance(IPlatform platform, ITaskManager taskManager,
            IGitClient gitClient, NPath repositoryRoot)
        {
            var repositoryPathConfiguration = new RepositoryPathConfiguration(repositoryRoot);
            string filePath = repositoryPathConfiguration.DotGitConfig;
            var gitConfig = new GitConfig(filePath);

            var repositoryWatcher = new RepositoryWatcher(platform, repositoryPathConfiguration, taskManager.Token);

            return new RepositoryManager(platform, gitConfig, repositoryWatcher,
                gitClient, repositoryPathConfiguration);
        }

        public void Initialize()
        {
            Logger.Trace("Initialize");
            watcher.Initialize();
        }

        public void Start()
        {
            Logger.Trace("Start");

            UpdateConfigData();
            watcher.Start();
        }

        public void Stop()
        {
            Logger.Trace("Stop");
            watcher.Stop();
        }

        /// <summary>
        /// Never ever call this from any callback that might be triggered by events
        /// raised here. This is not reentrancy safe and will deadlock if you do.
        /// Call this only from a non-callback main thread or preferably only for tests
        /// </summary>
        /// <returns></returns>
        public int WaitForEvents()
        {
            return watcher.CheckAndProcessEvents();
        }

        public ITask CommitAllFiles(string message, string body)
        {
            var task = GitClient
                .AddAll()
                .Then(GitClient.Commit(message, body));

            return HookupHandlers(task, true, true);
        }

        public ITask CommitFiles(List<string> files, string message, string body)
        {
            var task = GitClient
                .Add(files)
                .Then(GitClient.Commit(message, body));

            return HookupHandlers(task, true, true);
        }

        public ITask Fetch(string remote)
        {
            var task = GitClient.Fetch(remote);
            return HookupHandlers(task, true, false);
        }

        public ITask Pull(string remote, string branch)
        {
            var task = GitClient.Pull(remote, branch);
            return HookupHandlers(task, true, true);
        }

        public ITask Push(string remote, string branch)
        {
            var task = GitClient.Push(remote, branch);
            return HookupHandlers(task, true, false);
        }

        public ITask Revert(string changeset)
        {
            var task = GitClient.Revert(changeset);
            return HookupHandlers(task, true, true);
        }

        public ITask RemoteAdd(string remote, string url)
        {
            var task = GitClient.RemoteAdd(remote, url);
            task = HookupHandlers(task, true, false);
            if (!platform.Environment.IsWindows)
            {
                task.Then(_ => {
                    UpdateConfigData(true);
                });
            }
            return task;
        }

        public ITask RemoteRemove(string remote)
        {
            var task = GitClient.RemoteRemove(remote);
            task = HookupHandlers(task, true, false);
            if (!platform.Environment.IsWindows)
            {
                task.Then(_ => {
                    UpdateConfigData(true);
                });
            }
            return task;
        }

        public ITask RemoteChange(string remote, string url)
        {
            var task = GitClient.RemoteChange(remote, url);
            return HookupHandlers(task, true, false);
        }

        public ITask SwitchBranch(string branch)
        {
            var task = GitClient.SwitchBranch(branch);
            return HookupHandlers(task, true, true);
        }

        public ITask DeleteBranch(string branch, bool deleteUnmerged = false)
        {
            var task = GitClient.DeleteBranch(branch, deleteUnmerged);
            return HookupHandlers(task, true, false);
        }

        public ITask CreateBranch(string branch, string baseBranch)
        {
            var task = GitClient.CreateBranch(branch, baseBranch);
            return HookupHandlers(task, true, false);
        }

        public ITask LockFile(string file)
        {
            var task = GitClient.Lock(file);
            return HookupHandlers(task, true, false);
        }

        public ITask UnlockFile(string file, bool force)
        {
            var task = GitClient.Unlock(file, force);
            return HookupHandlers(task, true, false);
        }

        public void UpdateGitLog()
        {
            var task = GitClient.Log();
            task = HookupHandlers(task, false, false);
            task.Then((success, logEntries) =>
            {
                if (success)
                {
                    GitLogUpdated?.Invoke(logEntries);
                }
            }).Start();
        }

        public void UpdateGitStatus()
        {
            var task = GitClient.Status();
            task = HookupHandlers(task, true, false);
            task.Then((success, status) =>
            {
                if (success)
                {
                    GitStatusUpdated?.Invoke(status);
                }
            }).Start();
        }

        public void UpdateLocks()
        {
            var task = GitClient.ListLocks(false);
            HookupHandlers(task, false, false);
            task.Then((success, locks) =>
            {
                if (success)
                {
                    GitLocksUpdated?.Invoke(locks);
                }
            }).Start();
        }

        private ITask<T> HookupHandlers<T>(ITask<T> task, bool isExclusive, bool filesystemChangesExpected)
        {
            return new ActionTask(CancellationToken.None, () => {
                    if (isExclusive)
                    {
                        Logger.Trace("Starting Operation - Setting Busy Flag");
                        IsBusy = true;
                    }

                    if (filesystemChangesExpected)
                    {
                        Logger.Trace("Starting Operation - Disable Watcher");
                        watcher.Stop();
                    }
                })
                .Then(task)
                .Finally((success, exception, result) => {
                    if (filesystemChangesExpected)
                    {
                        Logger.Trace("Ended Operation - Enable Watcher");
                        watcher.Start();
                    }

                    if (isExclusive)
                    {
                        Logger.Trace("Ended Operation - Clearing Busy Flag");
                        IsBusy = false;
                    }

                    if (success)
                    {
                        return result;
                    }

                    throw exception;
                });
        }

        private void SetupWatcher()
        {
            watcher.HeadChanged += WatcherOnHeadChanged;
            watcher.IndexChanged += WatcherOnIndexChanged;
            watcher.ConfigChanged += WatcherOnConfigChanged;
            watcher.RepositoryCommitted += WatcherOnRepositoryCommitted;
            watcher.RepositoryChanged += WatcherOnRepositoryChanged;
            watcher.LocalBranchesChanged += WatcherOnLocalBranchesChanged;
            watcher.RemoteBranchesChanged += WatcherOnRemoteBranchesChanged;
        }

        private void UpdateHead()
        {
            var head = repositoryPaths.DotGitHead.ReadAllLines().FirstOrDefault();
            Logger.Trace("UpdateHead: {0}", head ?? "[NULL]");
            UpdateCurrentBranchAndRemote(head);
        }

        private void UpdateCurrentBranchAndRemote(string head)
        {
            ConfigBranch? branch = null;

            if (head.StartsWith("ref:"))
            {
                var branchName = head.Substring(head.IndexOf("refs/heads/") + "refs/heads/".Length);
                branch = config.GetBranch(branchName);

                if (!branch.HasValue)
                {
                    branch = new ConfigBranch { Name = branchName };
                }
            }

            var defaultRemote = "origin";
            ConfigRemote? remote = null;

            if (branch.HasValue && branch.Value.IsTracking)
            {
                remote = branch.Value.Remote;
            }

            if (!remote.HasValue)
            {
                remote = config.GetRemote(defaultRemote);
            }

            if (!remote.HasValue)
            {
                var configRemotes = config.GetRemotes().ToArray();
                if (configRemotes.Any())
                {
                    remote = configRemotes.FirstOrDefault();
                }
            }

            Logger.Trace("CurrentBranch: {0}", branch.HasValue ? branch.Value.ToString() : "[NULL]");
            Logger.Trace("CurrentRemote: {0}", remote.HasValue ? remote.Value.ToString() : "[NULL]");
            CurrentBranchUpdated?.Invoke(branch, remote);
        }

        private void WatcherOnRemoteBranchesChanged()
        {
            Logger.Trace("WatcherOnRemoteBranchesChanged");
            UpdateRemoteBranches();
        }

        private void WatcherOnLocalBranchesChanged()
        {
            Logger.Trace("WatcherOnLocalBranchesChanged");
            UpdateLocalBranches();
        }

        private void WatcherOnRepositoryCommitted()
        {
            Logger.Trace("WatcherOnRepositoryCommitted");
            UpdateGitLog();
        }

        private void WatcherOnRepositoryChanged()
        {
            Logger.Trace("WatcherOnRepositoryChanged");
            UpdateGitStatus();
        }

        private void WatcherOnConfigChanged()
        {
            Logger.Trace("WatcherOnConfigChanged");
            UpdateConfigData(true);
        }

        private void WatcherOnHeadChanged()
        {
            Logger.Trace("WatcherOnHeadChanged");
            UpdateHead();
        }

        private void WatcherOnIndexChanged()
        {
            Logger.Trace("WatcherOnIndexChanged");
            UpdateGitStatus();
        }

        private void UpdateConfigData(bool resetConfig = false)
        {
            Logger.Trace("UpdateConfigData reset:{0}", resetConfig);

            if (resetConfig)
            {
                config.Reset();
            }

            UpdateLocalBranches();
            UpdateRemoteBranches();
            UpdateHead();
        }

        private void UpdateLocalBranches()
        {
            Logger.Trace("UpdateLocalBranches");

            var branches = new Dictionary<string, ConfigBranch>();
            UpdateLocalBranches(branches, repositoryPaths.BranchesPath, config.GetBranches().Where(x => x.IsTracking), "");

            Logger.Trace("OnLocalBranchListUpdated {0} branches", branches.Count);
            LocalBranchesUpdated?.Invoke(branches);
        }

        private void UpdateLocalBranches(Dictionary<string, ConfigBranch> branches, NPath path, IEnumerable<ConfigBranch> configBranches, string prefix)
        {
            foreach (var file in path.Files())
            {
                var branchName = prefix + file.FileName;
                var branch =
                    configBranches.Where(x => x.Name == branchName).Select(x => x as ConfigBranch?).FirstOrDefault();
                if (!branch.HasValue)
                {
                    branch = new ConfigBranch { Name = branchName };
                }
                branches.Add(branchName, branch.Value);
            }

            foreach (var dir in path.Directories())
            {
                UpdateLocalBranches(branches, dir, configBranches, prefix + dir.FileName + "/");
            }
        }

        private void UpdateRemoteBranches()
        {
            Logger.Trace("UpdateRemoteBranches");

            var remotes = config.GetRemotes().ToArray().ToDictionary(x => x.Name, x => x);
            var remoteBranches = new Dictionary<string, Dictionary<string, ConfigBranch>>();

            foreach (var remote in remotes.Keys)
            {
                var branchList = new Dictionary<string, ConfigBranch>();
                var basedir = repositoryPaths.RemotesPath.Combine(remote);
                if (basedir.Exists())
                {
                    foreach (var branch in
                        basedir.Files(true)
                               .Select(x => x.RelativeTo(basedir))
                               .Select(x => x.ToString(SlashMode.Forward)))
                    {
                        branchList.Add(branch, new ConfigBranch { Name = branch, Remote = remotes[remote] });
                    }

                    remoteBranches.Add(remote, branchList);
                }
            }

            Logger.Trace("OnRemoteBranchListUpdated {0} remotes", remotes.Count);
            RemoteBranchesUpdated?.Invoke(remotes, remoteBranches);
        }

        private bool disposed;

        private void Dispose(bool disposing)
        {
            if (disposed) return;
            disposed = true;

            if (disposing)
            {
                Stop();
                watcher.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IGitConfig Config => config;

        public IGitClient GitClient => gitClient;

        public bool IsBusy
        {
            get { return isBusy; }
            private set
            {
                if (isBusy != value)
                {
                    Logger.Trace("IsBusyChanged Value:{0}", value);
                    isBusy = value;
                    IsBusyChanged?.Invoke(isBusy);
                }
            }
        }

        protected static ILogging Logger { get; } = Logging.GetLogger<RepositoryManager>();
    }
}
