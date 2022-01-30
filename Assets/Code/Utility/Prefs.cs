using UnityEngine;

namespace Utility
{

	public static class Prefs
	{
		public static readonly PrefString ExecutablePath    = new PrefString(nameof(ExecutablePath), "[game folder]/Dominions5.exe");
		public static readonly PrefString DataFolderPath    = new PrefString(nameof(DataFolderPath), "[data folder]/Dominions5/");
		public static readonly PrefString DefaultPretenderA = new PrefString(nameof(DefaultPretenderA));
		public static readonly PrefString DefaultPretenderB = new PrefString(nameof(DefaultPretenderB));
		public static readonly PrefString PreviousMapPath   = new PrefString(nameof(PreviousMapPath));
		public static readonly PrefInt    Era               = new PrefInt(nameof(Era), 2);
	}


	public abstract class PrefValue<T>
	{
		public string Key          { get; set; }
		public T      DefaultValue { get; set; }

		public PrefValue (string key, T defaultValue = default)
		{
			Key          = key;
			DefaultValue = defaultValue;
		}

		public abstract T    Get();
		public abstract void Set(T value);
	
		public void Clear ()
		{
			PlayerPrefs.DeleteKey(Key);
			PlayerPrefs.Save();
		}
	}

	public class PrefString : PrefValue<string>
	{
		public PrefString (string key, string defaultValue = default) : base(key, defaultValue) {}
	
		public override string Get ()
		{
			return PlayerPrefs.GetString(Key, DefaultValue);
		}
	
		public override void Set (string value)
		{
			PlayerPrefs.SetString(Key, value);
			PlayerPrefs.Save();
		}
	
		public static implicit operator string(PrefString value)
		{
			return value.Get();
		}
	}

	public class PrefInt : PrefValue<int>
	{
		public PrefInt (string key, int defaultValue = default) : base(key, defaultValue) {}
	
		public override int Get ()
		{
			return PlayerPrefs.GetInt(Key, DefaultValue);
		}
	
		public override void Set (int value)
		{
			PlayerPrefs.SetInt(Key, value);
			PlayerPrefs.Save();
		}
	}

	public class PrefFloat : PrefValue<float>
	{
		public PrefFloat (string key, float defaultValue = default) : base(key, defaultValue) {}
	
		public override float Get ()
		{
			return PlayerPrefs.GetFloat(Key, DefaultValue);
		}
	
		public override void Set (float value)
		{
			PlayerPrefs.SetFloat(Key, value);
			PlayerPrefs.Save();
		}
	}

}