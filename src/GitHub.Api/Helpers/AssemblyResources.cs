﻿using System;
using System.Reflection;

namespace GitHub.Unity
{
    enum ResourceType
    {
        Icon,
        Platform,
        Generic
    }

    class AssemblyResources
    {
        public static NPath ToFile(ResourceType resourceType, string resource, NPath destinationPath, IEnvironment environment)
        {
            /*
                This function attempts to get files embedded in the callers assembly.
                GitHub.Unity which tends to contain logos
                GitHub.Api which tends to contain application resources

                Each file's name is their physical path in the project.

                When running tests, we assume the tests are looking for application resources, and default to returning GitHub.Api 

                First check for the resource in the calling assembly.
                If the resource cannot be found, fallback to looking in GitHub.Api's assembly.
                If the resource is still not found, it attempts to find it in the file system
             */

            var target = destinationPath.Combine(resource);
            var os = "";
            if (resourceType == ResourceType.Platform)
            {
                os = DefaultEnvironment.OnWindows ? "windows"
                    : DefaultEnvironment.OnLinux ? "linux"
                        : "mac";
            }
            var type = resourceType == ResourceType.Icon ? "IconsAndLogos"
                : resourceType == ResourceType.Platform ? "PlatformResources"
                : "Resources";

            // all the resources are embedded in GitHub.Api
            var asm = Assembly.GetCallingAssembly();
            if (resourceType != ResourceType.Icon)
                asm = typeof(AssemblyResources).Assembly;
            var stream = asm.GetManifestResourceStream(
                                     String.Format("GitHub.Unity.{0}{1}.{2}", type, !string.IsNullOrEmpty(os) ? "." + os : os, resource));
            if (stream != null)
            {
                target.DeleteIfExists();
                return target.WriteAllBytes(stream.ToByteArray());
            }

            // if we're not in the test runner, we might be running in a Unity-compiled GitHub.Unity assembly, which doesn't
            // embed the resources in the assembly
            // check the filesystem
            NPath possiblePath = environment.ExtensionInstallPath.Combine(type, os, resource);
            if (possiblePath.FileExists())
            {
                target.DeleteIfExists();
                return possiblePath.Copy(target);
            }
            return NPath.Default;
        }
    }
}
