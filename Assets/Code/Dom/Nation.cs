using System;using System.Collections.Generic;

[Serializable]
public struct Nation
{
	public int Number;

	public static Nation Independents => new Nation { Number = 0 };
	public static Nation Arco => new Nation { Number = 80 };
	public static Nation Phlegra => new Nation { Number = 102 };
	
	public static implicit operator Nation(int value)
	{
		var nation = new Nation
		{
			Number = value
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
		return Number == other.Number;
	}
	public override bool Equals (object obj)
	{
		return obj is Nation other && Equals(other);
	}
}