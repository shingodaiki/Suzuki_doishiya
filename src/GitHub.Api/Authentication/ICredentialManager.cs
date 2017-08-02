﻿using System;
using System.Threading.Tasks;

namespace GitHub.Unity
{
    interface ICredential : IDisposable
    {
        UriString Host { get; }
        string Username { get; }
        string Token { get; }
        void UpdateToken(string token);
    }

    interface ICredentialManager
    {
        Task<ICredential> Load(UriString host);
        Task Save(ICredential cred);
        Task Delete(UriString host);
        bool HasCredentials();
        ICredential CachedCredentials { get; }
    }
}