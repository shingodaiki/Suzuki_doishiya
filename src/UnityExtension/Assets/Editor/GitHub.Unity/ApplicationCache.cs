using System;
using System.IO;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GitHub.Unity
{
    sealed class ApplicationCache : ScriptableObject, ISerializationCallbackReceiver, IApplicationCache
    {
        private static ApplicationCache instance;
        private static string cachePath;

        [SerializeField] private bool firstRun = true;
        public bool FirstRun
        {
            get { return firstRun; }
            private set
            {
                firstRun = value;
                Flush();
            }
        }

        [SerializeField] private string createdDate;
        public string CreatedDate
        {
            get { return createdDate; }
        }

        [SerializeField] private bool usageIncremented;
        public bool UsageIncremented
        {
            get { return usageIncremented; }
            set
            {
                usageIncremented = value;
                Flush();
            }
        }

        public static ApplicationCache Instance
        {
            get { return instance ?? CreateApplicationCache(EntryPoint.Environment); }
        }

        private static ApplicationCache CreateApplicationCache(IEnvironment environment)
        {
            cachePath = environment.UnityProjectPath + "/Temp/github_cache.yaml";

            if (File.Exists(cachePath))
            {
                var objects = InternalEditorUtility.LoadSerializedFileAndForget(cachePath);
                if (objects.Any())
                {
                    instance = objects[0] as ApplicationCache;
                    if (instance != null)
                    {
                        if (instance.FirstRun)
                        {
                            instance.FirstRun = false;
                        }

                        return instance;
                    }
                }
            }

            instance = CreateInstance<ApplicationCache>();
            instance.createdDate = DateTime.Now.ToLongTimeString();
            instance.Flush();

            return instance;
        }

        private void Flush()
        {
            InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] { this }, cachePath, true);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
        }
    }
}
