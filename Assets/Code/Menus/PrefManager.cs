using UnityEngine;
public static class PrefManager
{
	public static readonly PrefString ExecutablePath = new PrefString(Constants.PrefKey_ExecutablePath, "[game folder]/Dominions.exe");
	public static readonly PrefString DataFolderPath = new PrefString(Constants.PrefKey_DataFolderPath, "[data folder]/Dominions5/");
}


public abstract class PrefValue<T>
{
	public string Key { get; set; }
	public T DefaultValue { get; set; }

	public PrefValue (string key, T defaultValue = default)
	{
		Key = key;
		DefaultValue = defaultValue;
	}

	public abstract T Get();
	public abstract void Set(T value);
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