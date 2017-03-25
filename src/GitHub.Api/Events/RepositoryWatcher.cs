﻿using System;
using System.Linq;
using System.Threading;
using sfw.net;

namespace GitHub.Unity
{
    interface IRepositoryWatcher : IDisposable
    {
        void Start();
        void Stop();
        event Action<string> HeadChanged;
        event Action IndexChanged;
        event Action ConfigChanged;
        event Action<string> LocalBranchCreated;
        event Action<string> LocalBranchDeleted;
        event Action RepositoryChanged;
        event Action<string, string> RemoteBranchCreated;
        event Action<string, string> RemoteBranchDeleted;
    }

    class RepositoryWatcher : IRepositoryWatcher
    {
        private readonly RepositoryPathConfiguration paths;
        private readonly NPath[] ignoredPaths;

        private bool disposed;
        private NativeInterface nativeInterface;
        private bool running;
        private Thread thread;

        public RepositoryWatcher(IPlatform platform, RepositoryPathConfiguration paths)
        {
            this.paths = paths;

            ignoredPaths = new[] {
                platform.Environment.UnityProjectPath.ToNPath().Combine("Library"),
                platform.Environment.UnityProjectPath.ToNPath().Combine("Temp")
            };

            nativeInterface = new NativeInterface(paths.RepositoryPath);
            thread = new Thread(ThreadLoop);
        }

        public void Start()
        {
            running = true;
            thread.Start();
        }

        public void Stop()
        {
            running = false;
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            nativeInterface.Dispose();
            nativeInterface = null;
        }

        public event Action<string> HeadChanged;
        public event Action IndexChanged;
        public event Action ConfigChanged;
        public event Action<string> LocalBranchCreated;
        public event Action<string> LocalBranchDeleted;
        public event Action RepositoryChanged;
        public event Action<string, string> RemoteBranchCreated;
        public event Action<string, string> RemoteBranchDeleted;

        private void ThreadLoop()
        {
            while (running)
            {
                foreach (var fileEvent in nativeInterface.GetEvents())
                {
                    if (!running)
                    {
                        break;
                    }

                    var fileA = new NPath(fileEvent.Directory).Combine(fileEvent.FileA);

                    NPath fileB = null;
                    if (fileEvent.FileB != null)
                    {
                        fileB = new NPath(fileEvent.Directory).Combine(fileEvent.FileB);
                    }

                    if (fileA.IsChildOf(paths.DotGitPath))
                    {
                        if (fileA.Equals(paths.DotGitConfig))
                        {
                            Logger.Debug("ConfigChanged");

                            ConfigChanged?.Invoke();
                        }
                        else if (fileA.Equals(paths.DotGitHead))
                        {
                            string headContent = null;
                            if (fileEvent.Type != EventType.DELETED)
                            {
                                headContent = paths.DotGitHead.ReadAllLines().FirstOrDefault();
                            }

                            Logger.Debug("HeadChanged: {0}", headContent ?? "[null]");
                            HeadChanged?.Invoke(headContent);
                        }
                        else if (fileA.Equals(paths.DotGitIndex))
                        {
                            Logger.Debug("IndexChanged");
                            IndexChanged?.Invoke();
                        }
                        else if (fileA.IsChildOf(paths.RemotesPath))
                        {
                            if (fileA.ExtensionWithDot == ".lock")
                            {
                                continue;
                            }

                            var relativePath = fileA.RelativeTo(paths.RemotesPath);
                            var relativePathElements = relativePath.Elements.ToArray();

                            if (fileEvent.Type == EventType.DELETED && relativePathElements.Length > 1)
                            {
                                var origin = relativePathElements[0];
                                var branch = string.Join(@"/", relativePathElements.Skip(1).ToArray());

                                Logger.Debug("RemoteBranchDeleted: {0}", branch);
                                RemoteBranchDeleted?.Invoke(origin, branch);
                            }
                            else if (fileEvent.Type == EventType.CREATED)
                            {}
                            else if (fileEvent.Type == EventType.MODIFIED)
                            {}
                            else if (fileEvent.Type == EventType.RENAMED)
                            {}
                            else
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                        }
                        else if (fileA.IsChildOf(paths.BranchesPath))
                        {
                            if (fileA.ExtensionWithDot == ".lock" && fileEvent.Type != EventType.RENAMED)
                            {
                                continue;
                            }

                            if (fileEvent.Type == EventType.DELETED)
                            {
                                var relativePath = fileA.RelativeTo(paths.BranchesPath);
                                var relativePathElements = relativePath.Elements.ToArray();

                                if (!relativePathElements.Any())
                                {
                                    continue;
                                }

                                var branch = string.Join(@"/", relativePathElements.ToArray());

                                Logger.Debug("LocalBranchDeleted: {0}", branch);
                                LocalBranchDeleted?.Invoke(branch);
                            }
                            else if (fileEvent.Type == EventType.CREATED)
                            {}
                            else if (fileEvent.Type == EventType.MODIFIED)
                            {}
                            else if (fileEvent.Type == EventType.RENAMED)
                            {
                                if (fileB != null && fileB.FileExists())
                                {
                                    if (fileA.FileNameWithoutExtension == fileB.FileNameWithoutExtension)
                                    {
                                        var relativePath = fileB.RelativeTo(paths.BranchesPath);
                                        var relativePathElements = relativePath.Elements.ToArray();

                                        if (!relativePathElements.Any())
                                        {
                                            continue;
                                        }

                                        var branch = string.Join(@"/", relativePathElements.ToArray());

                                        Logger.Debug("LocalBranchCreated: {0}", branch);
                                        LocalBranchCreated?.Invoke(branch);
                                    }
                                }
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                        }
                    }
                    else
                    {
                        if (ignoredPaths.Any(ignoredPath => fileA.IsChildOf(ignoredPath)))
                        {
                            continue;
                        }

                        Logger.Debug("RepositoryChanged {0}: {1} {2}", fileEvent.Type, fileA.ToString(),
                            fileB?.ToString() ?? "[NULL]");
                        RepositoryChanged?.Invoke();
                    }
                }

                Thread.Sleep(200);
            }
        }

        protected static ILogging Logger { get; } = Logging.GetLogger<RepositoryWatcher>();
    }
}
