using System;
[AttributeUsage(AttributeTargets.Class)]
public class MapKeyName : Attribute
{
	private string _name;

	public MapKeyName (string name)
	{
		_name = name;
	}
	public string Name => _name;
}