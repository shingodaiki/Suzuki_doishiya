namespace GitHub.Unity
{
    interface IGitClient
    {
        IRepository GetRepository();
        ConfigBranch? ActiveBranch { get; }

        ConfigRemote? GetActiveRemote(string defaultRemote = "origin");
        string RepositoryPath { get; }
        void Start();
        void Stop();
    }
}