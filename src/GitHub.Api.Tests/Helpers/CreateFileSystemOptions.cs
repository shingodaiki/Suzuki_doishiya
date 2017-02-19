using System.Collections.Generic;

namespace GitHub.Unity.Tests
{
    class CreateFileSystemOptions
    {
        public const string DefaultTemporaryPath = @"c:\tmp";

        public IList<string> FilesThatExist { get; set; }
        public IDictionary<string, IList<string>> FileContents { get; set; }
        public IList<string> RandomFileNames { get; set; }
        public string TemporaryPath { get; set; } = DefaultTemporaryPath;
        public IList<string> DirectoriesThatExist { get; set; }
        public IDictionary<SubstituteFactory.ContentsKey, IList<string>> ChildFiles { get; set; }
        public IDictionary<SubstituteFactory.ContentsKey, IList<string>> ChildDirectories { get; set; }
    }
}