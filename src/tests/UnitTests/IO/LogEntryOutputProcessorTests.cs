using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using TestUtils;
using GitHub.Unity;

namespace UnitTests
{
    [TestFixture]
    class LogEntryOutputProcessorTests : BaseOutputProcessorTests
    {
        [Test]
        public void ShouldParseZeroCommits()
        {
            var output = new[]
            {
                "fatal: your current branch 'master' does not have any commits yet",
            };

            AssertProcessOutput(output, new GitLogEntry[] {});
        }

        [Test]
        public void ShouldParseSingleCommit()
        {
            var output = new[]
            {
                "1cd4b9154a88bc8c7b09cb8cacc79bf1d5bde8cf",
                "865b8d9d6e5e3bd6d7a4dc9c9f3588192314942c",
                "Author Person",
                "author@example.com",
                "2017-01-06T15:36:57+01:00",
                "Author Person",
                "author@example.com",
                "2017-01-06T15:36:57+01:00",
                "Rename RepositoryModelBase to RepositoryModel",
                "---GHUBODYEND---",
                "M       src/GitHub.App/Models/RemoteRepositoryModel.cs",
                null,
            };

            var commitTime = new DateTimeOffset(2017, 1, 6, 15, 36, 57, TimeSpan.FromHours(1));
            var expected = new[]
            {
                new GitLogEntry
                {
                    CommitID = "1cd4b9154a88bc8c7b09cb8cacc79bf1d5bde8cf",
                    AuthorEmail = "author@example.com",
                    AuthorName = "Author Person",
                    CommitEmail = "author@example.com",
                    CommitName = "Author Person",
                    Changes = new List<GitStatusEntry>
                    {
                        new GitStatusEntry("src/GitHub.App/Models/RemoteRepositoryModel.cs",
                            TestRootPath + @"\src/GitHub.App/Models/RemoteRepositoryModel.cs", null,
                            GitFileStatus.Modified),
                    },
                    Summary = "Rename RepositoryModelBase to RepositoryModel",
                    Description = "Rename RepositoryModelBase to RepositoryModel",
                    TimeString = commitTime.ToString(DateTimeFormatInfo.CurrentInfo),
                    CommitTimeString = commitTime.ToString(DateTimeFormatInfo.CurrentInfo),
                },
            };

            AssertProcessOutput(output, expected);
        }

        private void AssertProcessOutput(IEnumerable<string> lines, GitLogEntry[] expected)
        {
            var gitObjectFactory = SubstituteFactory.CreateGitObjectFactory(TestRootPath);

            var results = new List<GitLogEntry>();
            var outputProcessor = new LogEntryOutputProcessor(gitObjectFactory);
            outputProcessor.OnEntry += logEntry => { results.Add(logEntry); };

            foreach (var line in lines)
            {
                outputProcessor.LineReceived(line);
            }

            results.AssertEqual(expected);
        }
    }
}