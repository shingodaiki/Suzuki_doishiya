﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using GitHub.Unity;
using NSubstitute.Core;
using NUnit.Framework;
using TestUtils;

namespace UnitTests
{
    [TestFixture]
    class FindCommonPathTests
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var substituteFactory = new TestUtils.SubstituteFactory();
            var fileSystem = substituteFactory.CreateFileSystem(new CreateFileSystemOptions() { });

            NPathFileSystemProvider.Current.Should().BeNull();
            NPathFileSystemProvider.Current = fileSystem;
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            NPathFileSystemProvider.Current.Should().NotBeNull();
            NPathFileSystemProvider.Current = null;
        }

        [Test]
        public void ShouldErrorIfEmpty()
        {
            Action item;

            item = () => { FileSystemHelpers.FindCommonPath(new List<string>()); };
            item.ShouldThrow<InvalidOperationException>();

            item = () => { FileSystemHelpers.FindCommonPath(new List<string> { null }); };
            item.ShouldThrow<ArgumentNullException>();

            item = () => { FileSystemHelpers.FindCommonPath(new List<string> { "" }); };
            item.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ShouldFindPaths()
        {
            AssertCommonPathFound(null, ".");
            AssertCommonPathFound(null, ".", "./f1/asdf.txt");
            AssertCommonPathFound(@".\f1", "./f1/asdf2.txt", "./f1/asdf.txt");
            AssertCommonPathFound(@".\f1", "./f1/asdf2.txt", "./f1/c2/asdf.txt");
            AssertCommonPathFound(@".\f1\c2", "./f1/c2/asdf2.txt", "./f1/c2/asdf.txt");
            AssertCommonPathFound(@".\f1\c2", "./f1/c2/asdf2.txt", "./f1/c2/asdf.txt", "./f1/c2/c3/asdf.txt");
            AssertCommonPathFound(@".\f1", "./f1/c2/asdf2.txt", "./f1/c2/asdf.txt", "./f1/asdf.txt");
        }

        private static void AssertCommonPathFound(string result, params string[] paths)
        {
            var findCommonPath = FileSystemHelpers.FindCommonPath(paths);
            if (result == null)
            {
                findCommonPath.Should().BeNull();
            }
            else
            {
                findCommonPath.Should().Be(result);
            }
        }
    }
}
