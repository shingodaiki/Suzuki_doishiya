﻿using System.Collections.Generic;
using System.Linq;
using GitHub.Unity;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Rackspace.Threading;
using TestUtils;

namespace IntegrationTests
{
    [TestFixture]
    class LoginIntegrationTests : BaseGitEnvironmentTest
    {
        [Test]
        public void CommonParentTest()
        {
            var environment = new DefaultEnvironment();
            environment.FileSystem = new FileSystem(TestRepoMasterDirtyUnsynchronized);

            var ret = FileSystemHelpers.FindCommonPath(new string[]
            {
                "Assets/Test/Path/file",
                "Assets/Test/something",
                "Assets/Test/Path/another",
                "Assets/alkshdsd",
                "Assets/Test/sometkjh",
            });

            Assert.AreEqual("Assets", ret);
        }
    }
}
