﻿using System;
using System.Threading.Tasks;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using Rackspace.Threading;

namespace GitHub.Unity
{
    class KeychainAdapter : ICredentialStore
    {
        public Credentials OctokitCredentials { get; private set; } = Credentials.Anonymous;
        public ICredential Credential { get; private set; }

        public void Set(ICredential credential)
        {
            Credential = credential;
            OctokitCredentials = new Credentials(credential.Username, credential.Token);
        }

        public void UpdateToken(string token)
        {
            Credential.UpdateToken(token);
        }

        /// <summary>
        /// Implementation for Octokit
        /// </summary>
        /// <returns>Octokit credentials</returns>
        Task<Credentials> ICredentialStore.GetCredentials()
        {
            return TaskEx.FromResult(OctokitCredentials);
        }
    }

    struct Connection
    {
        public UriString Host;
        public string Username;
    }
    class ConnectionCacheItem
    {
        public string Host { get; set; }
        public string Username { get; set; }
    }

    /// <summary>
    /// {
    ///     connections: [
    ///         connection: { host: "github.com", user: "shana" },
    ///         connection: { host: "server.com", user: "auser" },
    ///     ]
    /// }
    /// </summary>
    class Keychain : IKeychain
    {
        private readonly ILogging logger = Logging.GetLogger<Keychain>();

        private const string ConnectionCacheSettingsKey = "connectionCache";

        private readonly ICredentialManager credentialManager;
        private readonly ISettings settings;

        // loaded at the start of application from cached/serialized data
        private Dictionary<UriString, Connection> connectionCache = new Dictionary<UriString, Connection>();

        // cached credentials loaded from git to pass to GitHub/ApiClient
        private Dictionary<UriString, KeychainAdapter> keychainAdapters =
            new Dictionary<UriString, KeychainAdapter>();

        public Keychain(ICredentialManager credentialManager, ISettings settings)
        {
            this.credentialManager = credentialManager;
            this.settings = settings;
        }

        public KeychainAdapter Connect(UriString host)
        {
            return FindOrCreateAdapter(host);
        }

        public async Task<KeychainAdapter> Load(UriString host)
        {
            logger.Trace("Load: {0}", host);

            var keychainAdapter = FindOrCreateAdapter(host);

            var keychainItem = await credentialManager.Load(host);
            keychainAdapter.Set(keychainItem);

            return keychainAdapter;
        }

        private KeychainAdapter FindOrCreateAdapter(UriString host)
        {
            KeychainAdapter value;
            if (!keychainAdapters.TryGetValue(host, out value))
            {
                value = new KeychainAdapter();
                keychainAdapters.Add(host, value);
            }

            return value;
        }

        public void Initialize()
        {
            logger.Trace("Initialize");

            var connections = settings.Get<ConnectionCacheItem[]>(ConnectionCacheSettingsKey);
            if (connections != null)
            {
                connectionCache =
                    connections.Select(item => new Connection { Host = new UriString(item.Host), Username = item.Username })
                               .ToDictionary(connection => connection.Host);
            }
            else
            {
                connectionCache = new Dictionary<UriString, Connection>();
            }
        }

        public async Task Clear(UriString host)
        {
            logger.Trace("Clear: {0}", host);
       
            // delete connection in the connection list
            connectionCache.Remove(host);

            // delete credential in octokit store
            keychainAdapters.Remove(host);

            //TODO: Confirm that we should delete credentials here
            // delete credential from credential manager
            await credentialManager.Delete(host);
        }

        public async Task Flush(UriString host)
        {
            logger.Trace("Flush: {0}", host);

            KeychainAdapter credentialAdapter;
            if (!keychainAdapters.TryGetValue(host, out credentialAdapter))
            {
                throw new ArgumentException($"Host: {host} is not found");
            }

            if (credentialAdapter.OctokitCredentials == Credentials.Anonymous)
            {
                throw new InvalidOperationException("Anonymous credentials cannot be stored");
            }

            // create new connection in the connection cache for this host
            connectionCache.Add(host, new Connection { Host = host, Username = credentialAdapter.OctokitCredentials.Login });

            // flushes credential cache to disk (host and username only)
            var connectionCacheItems =
                connectionCache.Select(
                    pair =>
                        new ConnectionCacheItem() {
                            Host = pair.Value.Host.ToString(),
                            Username = pair.Value.Username
                        }).ToArray();

            settings.Set(ConnectionCacheSettingsKey, connectionCacheItems);

            // saves credential in git credential manager (host, username, token)
            await credentialManager.Delete(host);
            await credentialManager.Save(credentialAdapter.Credential);
        }

        public void Save(ICredential credential)
        {
            logger.Trace("Save: {0}", credential.Host);

            var credentialAdapter = FindOrCreateAdapter(credential.Host);
            credentialAdapter.Set(credential);
        }

        public void UpdateToken(UriString host, string token)
        {
            logger.Trace("UpdateToken: {0}", host);

            KeychainAdapter keychainAdapter;
            if (!keychainAdapters.TryGetValue(host, out keychainAdapter))
            {
                throw new ArgumentException($"Host: {host} is not found");
            }

            var keychainItem = keychainAdapter.Credential;
            keychainItem.UpdateToken(token);
        }

        public bool HasKeys => connectionCache.Any();
    }

    internal interface IKeychain
    {
        KeychainAdapter Connect(UriString host);
        Task<KeychainAdapter> Load(UriString host);
        Task Clear(UriString host);
        Task Flush(UriString host);
        void UpdateToken(UriString host, string token);
        void Save(ICredential credential);
        void Initialize();
        bool HasKeys { get; }
    }
}
