﻿using System;
using System.Threading;
using System.Threading.Tasks;
using GitHub.Logging;

namespace GitHub.Unity
{
    class GitInstallDetails
    {
        public const string DefaultGitZipMd5Url = "https://ghfvs-installer.github.com/unity/portable_git/git.zip.MD5.txt";
        public const string DefaultGitZipUrl = "https://ghfvs-installer.github.com/unity/portable_git/git.zip";
        public const string DefaultGitLfsZipMd5Url = "https://ghfvs-installer.github.com/unity/portable_git/git-lfs.zip.MD5.txt";
        public const string DefaultGitLfsZipUrl = "https://ghfvs-installer.github.com/unity/portable_git/git-lfs.zip";

        public const string GitExtractedMD5 = "e6cfc0c294a2312042f27f893dfc9c0a";
        public const string GitLfsExtractedMD5 = "36e3ae968b69fbf42dff72311040d24a";

        public const string WindowsGitExecutableMD5 = "50570ed932559f294d1a1361801740b9";
        public const string MacGitExecutableMD5 = "";

        public const string WindowsGitLfsExecutableMD5 = "177bb14d0c08f665a24f0d5516c3b080";
        public const string MacGitLfsExecutableMD5 = "f81a1a065a26a4123193e8fd96c561ad";

        private const string PackageVersion = "f02737a78695063deace08e96d5042710d3e32db";
        private const string PackageName = "PortableGit";

        private readonly bool onWindows;

        public GitInstallDetails(NPath baseDataPath, bool onWindows)
        {
            this.onWindows = onWindows;

            ZipPath = baseDataPath.Combine("downloads");
            ZipPath.EnsureDirectoryExists();

            var gitInstallPath = baseDataPath.Combine(PackageNameWithVersion);
            GitInstallationPath = gitInstallPath;

            if (onWindows)
            {
                GitExecutable += "git.exe";
                GitLfsExecutable += "git-lfs.exe";

                GitExecutablePath = gitInstallPath.Combine("cmd", GitExecutable);
            }
            else
            {
                GitExecutable = "git";
                GitLfsExecutable = "git-lfs";

                GitExecutablePath = gitInstallPath.Combine("bin", GitExecutable);
            }

            GitLfsExecutablePath = GetGitLfsExecutablePath(gitInstallPath);
        }

        public NPath GetGitLfsExecutablePath(NPath gitInstallRoot)
        {
            return onWindows
                ? gitInstallRoot.Combine("mingw32", "libexec", "git-core", GitLfsExecutable)
                : gitInstallRoot.Combine("libexec", "git-core", GitLfsExecutable);
        }

        public NPath ZipPath { get; }
        public NPath GitInstallationPath { get; }
        public string GitExecutable { get; }
        public NPath GitExecutablePath { get; }
        public string GitLfsExecutable { get; }
        public NPath GitLfsExecutablePath { get; }
        public UriString GitZipMd5Url { get; set; } = DefaultGitZipMd5Url;
        public UriString GitZipUrl { get; set; } = DefaultGitZipUrl;
        public UriString GitLfsZipMd5Url { get; set; } = DefaultGitLfsZipMd5Url;
        public UriString GitLfsZipUrl { get; set; } = DefaultGitLfsZipUrl;
        public string PackageNameWithVersion => PackageName + "_" + PackageVersion;
    }

    class GitInstaller
    {
        private static readonly ILogging Logger = LogHelper.GetLogger<GitInstaller>();
        private readonly CancellationToken cancellationToken;

        private readonly IEnvironment environment;
        private readonly GitInstallDetails installDetails;
        private readonly IZipHelper sharpZipLibHelper;
        private NPath gitArchiveFilePath;
        private NPath gitLfsArchivePath;

        public GitInstaller(IEnvironment environment, CancellationToken cancellationToken,
            GitInstallDetails installDetails = null, NPath gitArchiveFilePath = null, NPath gitLfsArchivePath = null)
        {
            this.environment = environment;
            this.sharpZipLibHelper = ZipHelper.Instance;
            this.cancellationToken = cancellationToken;
            this.installDetails = installDetails ?? new GitInstallDetails(environment.UserCachePath, environment.IsWindows);
            this.gitArchiveFilePath = gitArchiveFilePath;
            this.gitLfsArchivePath = gitLfsArchivePath;
        }

        public void SetupGitIfNeeded(ActionTask<NPath> onSuccess, ITask onFailure)
        {
            Logger.Trace("SetupGitIfNeeded");

            if (!environment.IsWindows)
            {
                onFailure.Start();
                return;
            }

            var isGitExtractedTask = new FuncTask<NPath>(cancellationToken, () =>
            {
                if (!IsGitExtracted())
                {
                    GrabZipFromResources();
                    return null;
                }
                Logger.Trace("SetupGitIfNeeded: Skipped");
                return installDetails.GitExecutablePath;
            });
            isGitExtractedTask.OnEnd += (t, res, _, __) =>
            {
                if (res == null)
                {
                    var extractTask = ExtractPortableGit();
                    extractTask.Then(onSuccess, TaskRunOptions.OnSuccess, taskIsTopOfChain: true);
                    extractTask.Then(onFailure, TaskRunOptions.OnFailure, taskIsTopOfChain: true);
                    t.Then(extractTask);
                }
                else
                    t.Then(onSuccess);
            };

            isGitExtractedTask.Start();
        }

        private void GrabZipFromResources()
        {
            if (gitArchiveFilePath == null || !gitArchiveFilePath.FileExists())
                gitArchiveFilePath = AssemblyResources.ToFile(ResourceType.Platform, "git.zip", installDetails.ZipPath, environment);
            if (!gitArchiveFilePath.FileExists())
                gitArchiveFilePath = null;

            if (gitLfsArchivePath == null || !gitLfsArchivePath.FileExists())
                gitLfsArchivePath = AssemblyResources.ToFile(ResourceType.Platform, "git-lfs.zip", installDetails.ZipPath, environment);
            if (!gitLfsArchivePath.FileExists())
                gitLfsArchivePath = null;
        }

        private ITask CreateDownloadTask()
        {
            gitArchiveFilePath = installDetails.ZipPath.Combine("git.zip");
            gitLfsArchivePath = installDetails.ZipPath.Combine("git-lfs.zip");

            var downloader = new Downloader();
            downloader.QueueDownload(installDetails.GitZipUrl, installDetails.GitZipMd5Url, installDetails.ZipPath);
            downloader.QueueDownload(installDetails.GitLfsZipUrl, installDetails.GitLfsZipMd5Url, installDetails.ZipPath);
            return downloader;
        }

        private FuncTask<NPath> CreateUnzipTasks(NPath gitExtractPath, NPath gitLfsExtractPath, NPath tempZipExtractPath)
        {
            var unzipGitTask = new UnzipTask(cancellationToken, gitArchiveFilePath, gitExtractPath, sharpZipLibHelper,
                environment.FileSystem, GitInstallDetails.GitExtractedMD5);
            var unzipGitLfsTask = new UnzipTask(cancellationToken, gitLfsArchivePath, gitLfsExtractPath, sharpZipLibHelper,
                environment.FileSystem, GitInstallDetails.GitLfsExtractedMD5);
            
            var moveGitTask = new FuncTask<NPath>(cancellationToken, () => MoveGitAndLfs(gitExtractPath, gitLfsExtractPath, tempZipExtractPath));
            return unzipGitTask
                   .Then(unzipGitLfsTask)
                   .Then(moveGitTask);
        }

        private FuncTask<NPath> ExtractPortableGit()
        {
            var tempZipExtractPath = NPath.CreateTempDirectory("git_zip_extract_zip_paths");
            var gitExtractPath = tempZipExtractPath.Combine("git").CreateDirectory();
            var gitLfsExtractPath = tempZipExtractPath.Combine("git-lfs").CreateDirectory();

            var unzipTasks = CreateUnzipTasks(gitExtractPath, gitLfsExtractPath, tempZipExtractPath);

            if (gitArchiveFilePath == null || gitLfsArchivePath == null)
            {
                var downloadFilesTask = CreateDownloadTask();
                unzipTasks = downloadFilesTask.Then(unzipTasks);
            }

            return unzipTasks;
        }

        private NPath MoveGitAndLfs(NPath gitExtractPath, NPath gitLfsExtractPath, NPath tempZipExtractPath)
        {
            var targetGitLfsExecPath = installDetails.GetGitLfsExecutablePath(gitExtractPath);
            var extractGitLfsExePath = gitLfsExtractPath.Combine(installDetails.GitLfsExecutable);

            Logger.Trace($"Moving Git LFS Exe:'{extractGitLfsExePath}' to target in tempDirectory:'{targetGitLfsExecPath}'");

            extractGitLfsExePath.Move(targetGitLfsExecPath);

            Logger.Trace($"Moving tempDirectory:'{gitExtractPath}' to extractTarget:'{installDetails.GitInstallationPath}'");

            installDetails.GitInstallationPath.EnsureParentDirectoryExists();
            gitExtractPath.Move(installDetails.GitInstallationPath);

            Logger.Trace($"Deleting targetGitLfsExecPath:'{targetGitLfsExecPath}'");

            targetGitLfsExecPath.DeleteIfExists();

            Logger.Trace($"Deleting tempZipPath:'{tempZipExtractPath}'");
            tempZipExtractPath.DeleteIfExists();
            return installDetails.GitExecutablePath;
        }

        private bool IsGitExtracted()
        {
            if (!installDetails.GitInstallationPath.DirectoryExists())
            {
                Logger.Warning($"{installDetails.GitInstallationPath} does not exist");
                return false;
            }

            var gitExecutableMd5 = installDetails.GitExecutablePath.CalculateMD5();
            var expectedGitExecutableMd5 = environment.IsWindows ? GitInstallDetails.WindowsGitExecutableMD5 : GitInstallDetails.MacGitExecutableMD5;

            if (!expectedGitExecutableMd5.Equals(gitExecutableMd5, StringComparison.InvariantCultureIgnoreCase))
            {
                Logger.Warning($"Path {installDetails.GitExecutablePath} has MD5 {gitExecutableMd5} expected {expectedGitExecutableMd5}");
                return false;
            }

            var gitLfsExecutableMd5 = installDetails.GitLfsExecutablePath.CalculateMD5();
            var expectedGitLfsExecutableMd5 = environment.IsWindows ? GitInstallDetails.WindowsGitLfsExecutableMD5 : GitInstallDetails.MacGitLfsExecutableMD5;

            if (!expectedGitLfsExecutableMd5.Equals(gitLfsExecutableMd5, StringComparison.InvariantCultureIgnoreCase))
            {
                Logger.Warning($"Path {installDetails.GitLfsExecutablePath} has MD5 {gitLfsExecutableMd5} expected {expectedGitLfsExecutableMd5}");
                return false;
            }

            Logger.Trace("Git Present");
            return true;
        }
    }
}
