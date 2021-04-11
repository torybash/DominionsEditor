using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
public abstract class MapElement
{
	public MapManager Man { get; set; }
	
	public abstract void ParseArgs (string[] args);
	public abstract string[] SaveArgs ();

}

[MapKeyName("dom2title")]
public class Dom2Title : MapElement
{
	private string _title;
	public override void ParseArgs (string[] args) => _title = args[0];
	public override string[] SaveArgs () => new[] { _title };
}

[MapKeyName("mapsize")]
public class MapSize : MapElement
{
	private int _width;
	private int _height;
	public override void ParseArgs (string[] args)
	{
		_width = int.Parse(args[0]);
		_height = int.Parse(args[1]);
	}
	public override string[] SaveArgs ()
	{
		return new[] { _width.ToString(), _height.ToString() };
	}
}

[MapKeyName("domversion")]
public class DomVersion : MapElement
{
	private int _version;
	public override void ParseArgs (string[] args) => _version = int.Parse(args[0]);
	public override string[] SaveArgs () => new[] { _version.ToString() };
}

[MapKeyName("maptextcol")]
public class MapTextColor : MapElement
{
	private Color _color;
	public override void ParseArgs (string[] args) => _color = new Color(float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2]), float.Parse(args[3]));
	public override string[] SaveArgs () => new[] { _color.r.ToString(CultureInfo.InvariantCulture), _color.g.ToString(CultureInfo.InvariantCulture), _color.b.ToString(CultureInfo.InvariantCulture), _color.a.ToString(CultureInfo.InvariantCulture) };
}

[MapKeyName("description")]
public class Description : MapElement
{
	private string _description;
	public override void ParseArgs (string[] args) => _description = "\"TODO Parse strings\"";
	public override string[] SaveArgs () => new[] { _description };
}

[MapKeyName("scenario")]
public class Scenario : MapElement
{
	public override void ParseArgs (string[] args)
	{
	}
	public override string[] SaveArgs ()
	{
		return new string[0];
	}
}

[MapKeyName("imagefile")]
public class ImageFile : MapElement
{
	private string _mapImageName;

	public Texture2D GetTexture ()
	{
		string mapImagePath = $"{Man.MapFolderPath}{_mapImageName}";
		Debug.Log($"GetTexture mapImagePath: {mapImagePath}");
		var texture2D = TGALoader.LoadTGA(mapImagePath);
		Debug.Log($"GetTexture tex: {texture2D}, (w,h): ({texture2D.width}, {texture2D.height})");
		return texture2D;
		// byte[] mapImageBytes = File.ReadAllBytes(mapImagePath);
		// return mapImageBytes;
	}
	
	public override void ParseArgs (string[] args) => _mapImageName = args[0];
	public override string[] SaveArgs () => new[] { _mapImageName };
}

[MapKeyName("winterimagefile")]
public class WinterImageFile : ImageFile
{
}

[MapKeyName("allowedplayer")]
public class AllowedPlayer : MapElement
{
	public int NationNum { get; private set; }
	
	public override void ParseArgs (string[] args)
	{
		NationNum = int.Parse(args[0]);
	}
	
	public override string[] SaveArgs ()
	{
		return new[] { NationNum.ToString() };
	}
}

[MapKeyName("specstart")]
public class SpecStart : MapElement
{
	public int NationNum { get; private set; }
	public int ProvinceNum { get; private set; }

	public override void ParseArgs (string[] args)
	{
		NationNum = int.Parse(args[0]);
		ProvinceNum = int.Parse(args[1]);
	}
	
	public override string[] SaveArgs ()
	{
		return new[] { NationNum.ToString(), ProvinceNum.ToString() };
	}
}


[MapKeyName("dominionstr")]
public class DominionStrength : MapElement
{
	public int NationNum { get; private set; }
	public int Strength { get; private set; }

	public override void ParseArgs (string[] args)
	{
		NationNum = int.Parse(args[0]);
		Strength = int.Parse(args[1]);
	}
	
	public override string[] SaveArgs ()
	{
		return new[] { NationNum.ToString(), Strength.ToString() };
	}
}

[MapKeyName("terrain")]
public class Terrain : MapElement
{
	public int ProvinceNum { get; private set; }
	public int TerrainMask { get; private set; }
	
	public override void ParseArgs (string[] args)
	{
		ProvinceNum = int.Parse(args[0]);
		TerrainMask = int.Parse(args[1]);
	}
	
	public override string[] SaveArgs ()
	{
		return new[] { ProvinceNum.ToString(), TerrainMask.ToString() };
	}
}


[MapKeyName("landname")]
public class LandName : MapElement
{
	public int ProvinceNum { get; private set; }
	public string Name { get; private set; }

	public override void ParseArgs (string[] args)
	{
		ProvinceNum = int.Parse(args[0]);
		Name = args[1];
	}

	public override string[] SaveArgs ()
	{
		return new[] { ProvinceNum.ToString(), Name };
	}
}



[MapKeyName("neighbour")]
public class Neighbour : MapElement
{
	public int FromProvinceNum { get; private set; }
	public int ToProvinceNum { get; private set; }

	public override void ParseArgs (string[] args)
	{
		FromProvinceNum = int.Parse(args[0]);
		ToProvinceNum = int.Parse(args[1]);
	}
	
	public override string[] SaveArgs ()
	{
		return new[] { FromProvinceNum.ToString(), ToProvinceNum.ToString() };
	}
}

[MapKeyName("pb")]
public class ProvinceBorders : MapElement
{
	public int X { get; private set; }
	public int Y { get; private set; }
	public int Len { get; private set; }
	public int ProvinceNum { get; private set; }

	public override void ParseArgs (string[] args)
	{
		X = int.Parse(args[0]);
		Y = int.Parse(args[1]);
		Len = int.Parse(args[2]);
		ProvinceNum = int.Parse(args[3]);
	}
	
	public override string[] SaveArgs ()
	{
		return new[] { X.ToString(), Y.ToString(), Len.ToString(), ProvinceNum.ToString() };
	}
}

	
[MapKeyName("setland")]
public class SetLand : Land
{
	
}
	
[MapKeyName("land")]
public class Land : MapElement
{
	public int ProvinceNum { get; set; }

	public override void ParseArgs (string[] args)
	{
		ProvinceNum = int.Parse(args[0]);
	}
	public override string[] SaveArgs ()
	{
		return new[] { ProvinceNum.ToString() };
	}
}

public abstract class ProvinceDataElement : MapElement, IOwnedByProvince
{
	public int ProvinceNum { get; set; }
}


public abstract class MonsterElement : ProvinceDataElement
{
	public int UnitId { get; set; }
	public string UnitName { get; protected set; }
	
	public override string[] SaveArgs ()
	{
		return new[] { UnitId.ToString() };
	}
}


[MapKeyName("poptype")]
public class PopType : ProvinceDataElement
{
	public int PopulationId { get; set; }

	public override void ParseArgs (string[] args)
	{
		PopulationId = int.Parse(args[0]);
	}
	
	public override string[] SaveArgs ()
	{
		return new[] { PopulationId.ToString() };
	}
}
	
[MapKeyName("commander")]
public class Commander : MonsterElement
{
	public override void ParseArgs (string[] args)
	{
		if (int.TryParse(args[0], out int unitId))
		{
			UnitId = unitId;
		} else
		{
			UnitName = args[0];
		}
	}
}

[MapKeyName("units")]
public class Units : MonsterElement, IOwnedByCommander
{
	public int Amount { get; set; }
	public Commander Commander { get; set; }

	public override void ParseArgs (string[] args)
	{
		Amount = int.Parse(args[0]);
		if (int.TryParse(args[1], out int unitId))
		{
			UnitId = unitId;
		} else
		{
			UnitName = args[1];
		}
	}

	public override string[] SaveArgs ()
	{
		return new[] { Amount.ToString(), UnitId.ToString() };
	}
}

[MapKeyName("bodyguards")]
public class Bodyguards : MonsterElement, IOwnedByCommander
{
	public int Amount { get; private set; }
	public Commander Commander { get; set; }

	public override void ParseArgs (string[] args)
	{
		Amount = int.Parse(args[0]);
		if (int.TryParse(args[1], out int unitId))
		{
			UnitId = unitId;
		} else
		{
			UnitName = args[1];
		}
	}
	
	public override string[] SaveArgs ()
	{
		return new[] { Amount.ToString(), UnitId.ToString() };
	}
}


public interface IOwnedByCommander
{
	Commander Commander { get; set; }
}

public interface IOwnedByProvince
{
	int ProvinceNum { get; set; }
}