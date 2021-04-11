using System.Collections.Generic;
public class MapLineData
{
	public string Key { get; set; }
	public List<string> Args { get; set; } = new List<string>();

	public MapLineData (string key)
	{
		Key = key;
	}
		
	public void AddArg (string arg)
	{
		Args.Add(arg);
	}

	public override string ToString ()
	{
		return $"{Key}: {string.Join(", ", Args)}";
	}
}