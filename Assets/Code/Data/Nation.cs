using System;
using UnityEngine;

[Serializable]
public struct Nation
{
	public int id;
	public string name;
	public string epithet;
	public string file_name_base;
	public int era;
	
	public Sprite icon;

	public static Nation Invalid => new Nation { id = -1 };
	public static Nation Independents => new Nation { id = 0 };

	public static implicit operator Nation(int value)
	{
		var nation = new Nation
		{
			id = value
		};
		return nation;
	}

	public static bool operator== (Nation first, Nation second)
	{
		return first.Equals(second);
	}
	public static bool operator != (Nation first, Nation second)
	{
		return !first.Equals(second);
	}

	public bool Equals (Nation other)
	{
		return id == other.id;
	}
	public override bool Equals (object obj)
	{
		return obj is Nation other && Equals(other);
	}
	public override int GetHashCode ()
	{
		return id;
	}

	public override string ToString ()
	{
		return $"{nameof(id)}: {id}";
	}
}