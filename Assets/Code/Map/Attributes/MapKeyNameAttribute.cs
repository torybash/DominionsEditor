using System;

[AttributeUsage(AttributeTargets.Class)]
public class MapKeyNameAttribute : Attribute
{
	private string _name;

	public MapKeyNameAttribute (string name)
	{
		_name = name;
	}
	public string Name => _name;
}