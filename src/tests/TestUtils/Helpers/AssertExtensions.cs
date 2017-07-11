using System;
using System.Collections.Generic;
using FluentAssertions;
using GitHub.Unity;
using NUnit.Framework;

namespace TestUtils
{
    static class AssertExtensions
    {
        public static void AssertEqual(this GitLogEntry gitLogEntry, GitLogEntry other)
        {
            gitLogEntry.AuthorName.Should().Be(other.AuthorName);
            gitLogEntry.CommitName.Should().Be(other.CommitName);
            gitLogEntry.AuthorEmail.Should().Be(other.AuthorEmail);
            gitLogEntry.CommitEmail.Should().Be(other.CommitEmail);
            gitLogEntry.MergeA.Should().Be(other.MergeA);
            gitLogEntry.MergeB.Should().Be(other.MergeB);
            gitLogEntry.Changes.AssertEqual(other.Changes);
            gitLogEntry.CommitID.Should().Be(other.CommitID);
            gitLogEntry.Summary.Should().Be(other.Summary);
            gitLogEntry.Description.Should().Be(other.Description);
            gitLogEntry.TimeString.Should().Be(other.TimeString);
            gitLogEntry.CommitTimeString.Should().Be(other.CommitTimeString);
            gitLogEntry.Time.Should().Be(other.Time);
            gitLogEntry.CommitTime.Should().Be(other.CommitTime);
        }

        public static void AssertNotEqual(this GitLogEntry gitLogEntry, GitLogEntry other)
        {
            Action action = () => gitLogEntry.AssertEqual(other);
            action.ShouldThrow<AssertionException>();
        }

        public static void AssertEqual(this IList<GitLogEntry> gitLogEntries, IList<GitLogEntry> others)
        {
            if (gitLogEntries == null)
            {
                others.Should().BeNull();
                return;
            }

            others.Should().NotBeNull();
            gitLogEntries.Count.Should().Be(others.Count);

            for (var i = 0; i < gitLogEntries.Count; i++)
            {
                var gitLogEntry = gitLogEntries[i];
                var other = others[i];

                gitLogEntry.AssertEqual(other);
            }
        }

        public static void AssertNotEqual(this IList<GitLogEntry> gitLogEntries, IList<GitLogEntry> others)
        {
            Action action = () => gitLogEntries.AssertEqual(others);
            action.ShouldThrow<AssertionException>();
        }

        public static void AssertEqual(this GitStatusEntry gitStatusEntry, GitStatusEntry other)
        {
            gitStatusEntry.Path.Should().Be(other.Path);
            gitStatusEntry.FullPath.Should().Be(other.FullPath);
            gitStatusEntry.OriginalPath.Should().Be(other.OriginalPath);
            gitStatusEntry.ProjectPath.Should().Be(other.ProjectPath);
            gitStatusEntry.Status.Should().Be(other.Status);
            gitStatusEntry.Staged.Should().Be(other.Staged);
        }

        public static void AssertNotEqual(this GitStatusEntry gitStatusEntry, GitStatusEntry other)
        {
            Action action = () => gitStatusEntry.AssertEqual(other);
            action.ShouldThrow<AssertionException>();
        }

        public static void AssertEqual(this IList<GitStatusEntry> gitLogEntries, IList<GitStatusEntry> others)
        {
            if (gitLogEntries == null)
            {
                others.Should().BeNull();
                return;
            }

            others.Should().NotBeNull();
            gitLogEntries.Count.Should().Be(others.Count);

            for (var i = 0; i < gitLogEntries.Count; i++)
            {
                var gitLogEntry = gitLogEntries[i];
                var other = others[i];

                gitLogEntry.AssertEqual(other);
            }
        }

        public static void AssertNotEqual(this IList<GitStatusEntry> gitLogEntries, IList<GitStatusEntry> others)
        {
            Action action = () => gitLogEntries.AssertEqual(others);
            action.ShouldThrow<AssertionException>();
        }

        public static void AssertEqual(this GitStatus gitStatus, GitStatus other)
        {
            gitStatus.Ahead.Should().Be(other.Ahead);
            gitStatus.Behind.Should().Be(other.Behind);
            gitStatus.LocalBranch.Should().Be(other.LocalBranch);
            gitStatus.RemoteBranch.Should().Be(other.RemoteBranch);
            gitStatus.Entries.AssertEqual(other.Entries);
        }

        public static void AssertNotEqual(this GitStatus gitStatus, GitStatus other)
        {
            Action action = () => gitStatus.AssertEqual(other);
            action.ShouldThrow<AssertionException>();
        }
   
        public static void AssertEqual(this GitLock gitLock, GitLock other)
        {
            gitLock.Path.Should().Be(other.Path);
            gitLock.FullPath.Should().Be(other.FullPath);
            gitLock.User.Should().Be(other.User);
        }

        public static void AssertNotEqual(this GitLock gitLock, GitLock other)
        {
            Action action = () => gitLock.AssertEqual(other);
            action.ShouldThrow<AssertionException>();
        }

        public static void AssertEqual(this IList<GitLock> gitLocks, IList<GitLock> others)
        {
            if (gitLocks == null)
            {
                others.Should().BeNull();
                return;
            }

            others.Should().NotBeNull();
            gitLocks.Count.Should().Be(others.Count);

            for (var i = 0; i < gitLocks.Count; i++)
            {
                var gitLock = gitLocks[i];
                var other = others[i];

                gitLock.AssertEqual(other);
            }
        }

        public static void AssertNotEqual(this IList<GitLock> gitLocks, IList<GitLock> others)
        {
            Action action = () => gitLocks.AssertEqual(others);
            action.ShouldThrow<AssertionException>();
        }
    }
}
