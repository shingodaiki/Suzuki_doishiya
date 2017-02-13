﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using GitHub.Helpers;

namespace GitHub.PortableGit.Helpers
{
    public class PortableGitManager : PortablePackageManager, IPortableGitManager
    {
        readonly Lazy<string> gitExecutablePath;
        readonly Lazy<string> gitEtcDirPath;
//        readonly Lazy<IFile> systemConfigFile;

//        readonly IEmbeddedResource embeddedSystemConfigFile;
//        readonly IEmbeddedResource embeddedGitAttributesFile;
//        readonly IProcessStarter processStarter;

//        [ImportingConstructor]
//        public PortableGitManager(
//            IOperatingSystem operatingSystem,
//            IProcessStarter processStarter,
//            IProgram program,
//            IZipArchive zipArchive)
//            : this(operatingSystem,
//                processStarter,
//                program,
//                zipArchive,
//                new EmbeddedResource(typeof(PortableGitManager).Assembly, "GitHub.PortableGit.Resources.gitconfig", "gitconfig", operatingSystem),
//                new EmbeddedResource(typeof(PortableGitManager).Assembly, "GitHub.PortableGit.Resources.gitattributes.suggested", ".gitattributes", operatingSystem))
//        {
//        }

//        public PortableGitManager(
//            IOperatingSystem operatingSystem,
//            IProcessStarter processStarter,
//            IProgram program,
//            IZipArchive zipArchive,
//            IEmbeddedResource embeddedSystemConfigFile,
//            IEmbeddedResource embeddedGitAttributesFile)
//            : base(operatingSystem, program, zipArchive)
//        {
//            Ensure.ArgumentNotNull(embeddedSystemConfigFile, "embeddedSystemConfigFile");
//            Ensure.ArgumentNotNull(embeddedGitAttributesFile, "embeddedGitAttributesFile");
//            Ensure.ArgumentNotNull(processStarter, "processStarter");

//            this.embeddedSystemConfigFile = embeddedSystemConfigFile;
//            this.embeddedGitAttributesFile = embeddedGitAttributesFile;
//            this.processStarter = processStarter;

//            gitExecutablePath = new Lazy<string>(() =>
//            {
//                var path = Path.Combine(GetPortableGitDestinationDirectory(), "cmd", "git.exe");
//                Debug.Assert(operatingSystem != null, "The base class should verify this is not null");
//                if (!operatingSystem.FileExists(path))
//                {
//                    log.Error(CultureInfo.InvariantCulture, "git.exe doesn't exist at '{0}'", path);
//                }
//                return path;
//            });

//            gitEtcDirPath = new Lazy<string>(() => Path.Combine(GetPortableGitDestinationDirectory(), "mingw32", "etc"));
//            systemConfigFile = new Lazy<IFile>(
//                () => operatingSystem.GetFile(Path.Combine(EtcDirectoryPath, "gitconfig")));
//        }

        /// <summary>
        /// The path to git.exe
        /// </summary>
        public string GitExecutablePath
        {
            get { return gitExecutablePath.Value; }
        }

        public string EtcDirectoryPath
        {
            get { return gitEtcDirPath.Value; }
        }

        /// <summary>
        ///   Extracts Portable Git if it has not already been extracted.
        /// </summary>
        /// <returns>An IObservable which will yield progress values from 0-100
        ///   or return the Exception if an error occurs. If PGit is already 
        ///   extracted, this will return 100 and Complete.</returns>
        public void ExtractGitIfNeeded()
        {
            //TODO: Look here next
//            Observable.Defer(() => ExtractPackageIfNeeded("PortableGit.7z", KillAllSSHAgent, ExecuteBashLogin, 5221));
        }

        void ExecuteBashLogin()
        {
            var destinationDirectory = GetPortableGitDestinationDirectory();
            var gitBash = Path.Combine(destinationDirectory, "git-bash.exe");
            var args = string.Format(CultureInfo.InvariantCulture, "--no-needs-console --hide --no-cd --command=usr\\bin\\dash.exe -c 'bash --login -c exit'");

            var info = new ProcessStartInfo(gitBash, args)
            {
                WorkingDirectory = destinationDirectory,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

//            var process = processStarter.StartObservableProcess(info);

//            process.Subscribe(
//                _ => { },
//                ex => { log.Warn("Git Bash login failed", ex); },
//                () => { log.Info("Git Bash completed successfully"); });
        }

        /// <summary>
        /// Indicates if PortableGit has been extracted or not.
        /// </summary>
        /// <returns></returns>
        public bool IsExtracted()
        {
            return IsPackageExtracted();
        }

        public string GetPortableGitDestinationDirectory(bool createIfNeeded = false)
        {
            return GetPackageDestinationDirectory(createIfNeeded);
        }

        public void EnsureSystemConfigFileExtracted()
        {
            //TODO: Look here next
//            var configFile = systemConfigFile.Value;
//            if (configFile.Exists) return Observable.Return(configFile);
//
//            embeddedSystemConfigFile.ExtractToFile(EtcDirectoryPath);
//            configFile.Refresh();
//            Debug.Assert(configFile.Exists, "After extracting the system config file, we expect it to exist.");
//            return Observable.Return(configFile);
        }

        public string ExtractSuggestedGitAttributes(string targetDirectory)
        {
            throw new NotImplementedException();

//            return embeddedGitAttributesFile.ExtractToFile(targetDirectory);
        }

        protected override string GetExpectedVersion()
        {
            throw new NotImplementedException();

//            return AppBuildInfo.PGitVersionSHA1;
        }

        protected override string GetPathToCanary(string rootDir)
        {
            throw new NotImplementedException();

//            return Path.Combine(rootDir ?? @"C:\__NOTHERE", "cmd", "git.exe");
        }

        protected override string GetPackageName()
        {
            return "PortableGit";
        }

        protected void KillAllSSHAgent()
        {
            // NB: SSH Agent is often running when we attempt to extract a new
            // version of PGit and our Rename dir trick is blocked because of
            // it. So, kill off any SSH Agents we find.
//            Process.GetProcesses()
//                .Where(x => x.ProcessName.Contains("ssh-agent", StringComparison.OrdinalIgnoreCase))
//                .ForEach(x =>
//                {
//                    try
//                    {
//                        x.Kill();
//                    }
//                    catch (Exception exception)
//                    {
//                        var win32Exception = exception as Win32Exception;
//                        const int ERROR_ACCESS_DENIED = 5;
//                        if (win32Exception != null && win32Exception.NativeErrorCode == ERROR_ACCESS_DENIED)
//                        {
//                            // This process is probably owned by another user.
//                            return;
//                        }
//                        log.Info(string.Format(CultureInfo.InvariantCulture, "Failed to kill ssh-agent with PID {0}", x.Id), exception);
//                    }
//                });
        }
    }
}
