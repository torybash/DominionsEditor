using System;

[Serializable]
public struct Nation
{
	public int number;

	public static Nation Invalid => new Nation { number = -1 };
	public static Nation Independents => new Nation { number = 0 };
	public static Nation Arco => new Nation { number = 80 };
	public static Nation Phlegra => new Nation { number = 102 };
	
	public static implicit operator Nation(int value)
	{
		var nation = new Nation
		{
			number = value
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
		return number == other.number;
	}
	public override bool Equals (object obj)
	{
		return obj is Nation other && Equals(other);
	}
	public override int GetHashCode ()
	{
		return number;
	}

	public override string ToString ()
	{
		return $"{nameof(number)}: {number}";
	}
}