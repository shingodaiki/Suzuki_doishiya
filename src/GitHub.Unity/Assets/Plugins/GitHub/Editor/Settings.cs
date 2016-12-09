using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace GitHub.Unity
{
	public class Settings : ScriptableObject
	{
		const string
			SettingsParseError = "Failed to parse settings file at '{0}'",
			LocalSettingsName = "GitHub.local.json";


		[SerializeField] List<string>
			keys = new List<string>(),
			values = new List<string>();


		static Settings asset;


		static string GetPath()
		{
			return string.Format("{0}/ProjectSettings/{1}", Utility.UnityProjectPath, LocalSettingsName);
		}


		public static bool Reload()
		{
			string path = GetPath();

			Settings newAsset = CreateInstance<Settings>();

			if (!File.Exists(path))
			{
				asset = newAsset;
				return true;
			}

			object parseResult;
			IDictionary<string, object> settings;

			if(!SimpleJson.TryDeserializeObject(File.ReadAllText(path), out parseResult) || (settings = parseResult as IDictionary<string, object>) == null)
			{
				Debug.LogErrorFormat(SettingsParseError, path);
				return false;
			}

			newAsset.keys.Clear();
			newAsset.keys.AddRange(settings.Keys);
			newAsset.values.Clear();
			newAsset.values.AddRange(settings.Values.Select(v => v as string));

			asset = newAsset;

			return true;
		}


		public static bool Save()
		{
			if (asset == null)
			{
				return false;
			}

			string path = GetPath();

			StreamWriter settings = File.CreateText(path);
			settings.Write("{\n");

			for (int index = 0; index < asset.keys.Count; ++index)
			{
				settings.Write("\t\"{0}\": \"{1}\"\n", asset.keys[index], asset.values[index]);
			}

			settings.Write("}\n");
			settings.Close();

			return true;
		}


		static Settings GetAsset()
		{
			if (asset == null)
			{
				Reload();
			}

			return asset;
		}


		public static string Get(string key, string fallback = "")
		{
			Settings asset = GetAsset();

			if (asset == null)
			{
				return null;
			}

			int index = asset.keys.IndexOf(key);

			if (index >= 0)
			{
				return asset.values[index];
			}

			return fallback;
		}


		public static bool Set(string key, string value)
		{
			Settings asset = GetAsset();

			if (asset == null)
			{
				return false;
			}

			int index = asset.keys.IndexOf(key);

			if (index >= 0)
			{
				if (!asset.values[index].Equals(value))
				{
					asset.values[index] = value;

					Save();
				}

				return true;
			}

			asset.keys.Add(key);
			asset.values.Add(value);

			Save();

			return true;
		}
	}
}
