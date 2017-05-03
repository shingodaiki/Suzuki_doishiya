using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GitHub.Unity
{
    abstract class BaseSettings : ISettings
    {
        public abstract bool Exists(string key);
        public abstract string Get(string key, string fallback = "");
        public abstract T Get<T>(string key, T fallback = default(T));
        public abstract void Initialize();
        public abstract void Rename(string oldKey, string newKey);
        public abstract void Set<T>(string key, T value);
        public abstract void Unset(string key);

        protected virtual string SettingsFileName { get; set; }
        protected virtual string SettingsPath { get; set; }
    }

    class JsonBackedSettings : BaseSettings
    {

        private const string SettingsParseError = "Failed to parse settings file at '{0}'";

        private string cachePath;
        private CacheData cacheData = new CacheData();
        private Action<string> dirCreate;
        private Func<string, bool> dirExists;
        private Action<string> fileDelete;
        private Func<string, bool> fileExists;
        private Func<string, Encoding, string> readAllText;
        private Action<string, string> writeAllText;
        private readonly ILogging logger;

        public JsonBackedSettings()
        {
            logger = Logging.GetLogger(GetType());
            fileExists = (path) => File.Exists(path);
            readAllText = (path, encoding) => File.ReadAllText(path, encoding);
            writeAllText = (path, content) => File.WriteAllText(path, content);
            fileDelete = (path) => File.Delete(path);
            dirExists = (path) => Directory.Exists(path);
            dirCreate = (path) => Directory.CreateDirectory(path);
        }

        public override void Initialize()
        {
            cachePath = Path.Combine(SettingsPath, SettingsFileName);
            LoadFromCache(cachePath);
        }

        public override bool Exists(string key)
        {
            return cacheData.GitHubUnity.ContainsKey(key);
        }

        public override string Get(string key, string fallback = "")
        {
            return Get<string>(key, fallback);
        }

        public override T Get<T>(string key, T fallback = default(T))
        {
            object value = null;
            if (cacheData.GitHubUnity.TryGetValue(key, out value))
            {
                return (T)value;
            }

            return fallback;
        }

        public override void Set<T>(string key, T value)
        {
            if (!cacheData.GitHubUnity.ContainsKey(key))
                cacheData.GitHubUnity.Add(key, value);
            else
                cacheData.GitHubUnity[key] = value;
            SaveToCache(cachePath);
        }

        public override void Unset(string key)
        {
            if (cacheData.GitHubUnity.ContainsKey(key))
                cacheData.GitHubUnity.Remove(key);
            SaveToCache(cachePath);
        }

        public override void Rename(string oldKey, string newKey)
        {
            object value = null;
            if (cacheData.GitHubUnity.TryGetValue(oldKey, out value))
            {
                cacheData.GitHubUnity.Remove(oldKey);
                Set(newKey, value);
            }
            SaveToCache(cachePath);
        }

        private void LoadFromCache(string cachePath)
        {
            EnsureCachePath(cachePath);

            if (!fileExists(cachePath))
                return;

            var data = readAllText(cachePath, Encoding.UTF8);

            try
            {
                cacheData = SimpleJson.DeserializeObject<CacheData>(data);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Error Deserializing");
                cacheData = null;
            }

            if (cacheData == null)
            {
                // cache is corrupt, remove
                fileDelete(cachePath);
                return;
            }
        }

        private bool SaveToCache(string cachePath)
        {
            EnsureCachePath(cachePath);

            try
            {
                var data = SimpleJson.SerializeObject(cacheData);
                writeAllText(cachePath, data);
            }
            catch (Exception ex)
            {
                logger.Error(SettingsParseError, cachePath);
                logger.Error(ex);
                return false;
            }

            return true;
        }

        private void EnsureCachePath(string cachePath)
        {
            logger.Trace("EnsureCachePath: {0}", cachePath);

            if (fileExists(cachePath))
                return;

            var di = Path.GetDirectoryName(cachePath);
            if (!dirExists(di))
                dirCreate(di);
        }

        private class CacheData
        {
            public Dictionary<string, object> GitHubUnity = new Dictionary<string, object>();
        }

    }

    class LocalSettings : JsonBackedSettings
    {
        private const string RelativeSettingsPath = "ProjectSettings";
        private const string settingsFileName = "GitHub.local.json";

        public LocalSettings(IEnvironment environment)
        {
            SettingsPath = environment.UnityProjectPath.ToNPath().Combine(RelativeSettingsPath);
        }

        protected override string SettingsFileName { get { return settingsFileName; } }
    }

    class UserSettings : JsonBackedSettings
    {
        private const string settingsFileName = "settings.json";

        public UserSettings(IEnvironment environment, string path)
        {
            SettingsPath = environment.GetSpecialFolder(Environment.SpecialFolder.LocalApplicationData).ToNPath().Combine(path);
        }

        protected override string SettingsFileName { get { return settingsFileName; } }
    }

    class SystemSettings : JsonBackedSettings
    {
        private const string settingsFileName = "settings.json";

        public SystemSettings(IEnvironment environment, string path)
        {
            SettingsPath = environment.GetSpecialFolder(Environment.SpecialFolder.ApplicationData).ToNPath().Combine(path);
        }

        protected override string SettingsFileName { get { return settingsFileName; } }
    }
}
