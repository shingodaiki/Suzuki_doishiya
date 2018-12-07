﻿using System;
using System.Threading.Tasks;

namespace GitHub.Unity
{
    public interface ICredential : IDisposable
    {
        UriString Host { get; }
        string Username { get; }
        string Token { get; }
        void Update(string token, string username);
    }

    public interface ICredentialManager
    {
        ICredential Load(UriString host);
        void Save(ICredential cred);
        void Delete(UriString host);
        bool HasCredentials();
    }
}
