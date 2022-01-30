using System.Globalization;
using UnityEngine;

namespace Map.MapElements
{

	public abstract class MapElement
	{

		public abstract void     ParseArgs (string[] args);
		public abstract string[] SaveArgs ();

	}

	[MapKeyName("dom2title")]
	public class Dom2Title : MapElement
	{
		private         string   _title;
		public override void     ParseArgs (string[] args) => _title = args[0];
		public override string[] SaveArgs ()               => new[] { _title };
	}

	[MapKeyName("mapsize")]
	public class MapSize : MapElement
	{
		private int _width;
		private int _height;

		public override void ParseArgs (string[] args)
		{
			_width  = int.Parse(args[0]);
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
		private         int      _version;
		public override void     ParseArgs (string[] args) => _version = int.Parse(args[0]);
		public override string[] SaveArgs ()               => new[] { _version.ToString() };
	}

	[MapKeyName("maptextcol")]
	public class MapTextColor : MapElement
	{
		private         Color    _color;
		public override void     ParseArgs (string[] args) => _color = new Color(float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2]), float.Parse(args[3]));
		public override string[] SaveArgs ()               => new[] { _color.r.ToString(CultureInfo.InvariantCulture), _color.g.ToString(CultureInfo.InvariantCulture), _color.b.ToString(CultureInfo.InvariantCulture), _color.a.ToString(CultureInfo.InvariantCulture) };
	}

	[MapKeyName("description")]
	public class Description : MapElement
	{
		private         string   _description;
		public override void     ParseArgs (string[] args) => _description = "\"TODO Parse strings\"";
		public override string[] SaveArgs ()               => new[] { _description };
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
		public string MapImageName { get; private set; }

		public override void     ParseArgs (string[] args) => MapImageName = args[0];
		public override string[] SaveArgs ()               => new[] { MapImageName };
	}

	[MapKeyName("winterimagefile")]
	public class WinterImageFile : ImageFile
	{
	}

	[MapKeyName("allowedplayer")]
	public class AllowedPlayer : MapElement
	{
		public int NationNum { get; set; }

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
	public class StartLocation : MapElement
	{
		public int NationNum   { get; set; }
		public int ProvinceNum { get; set; }

		public override void ParseArgs (string[] args)
		{
			NationNum   = int.Parse(args[0]);
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
		public int Strength  { get; private set; }

		public override void ParseArgs (string[] args)
		{
			NationNum = int.Parse(args[0]);
			Strength  = int.Parse(args[1]);
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
		public int    ProvinceNum { get; private set; }
		public string Name        { get; private set; }

		public override void ParseArgs (string[] args)
		{
			ProvinceNum = int.Parse(args[0]);
			Name        = args[1];
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
		public int ToProvinceNum   { get; private set; }

		public override void ParseArgs (string[] args)
		{
			FromProvinceNum = int.Parse(args[0]);
			ToProvinceNum   = int.Parse(args[1]);
		}

		public override string[] SaveArgs ()
		{
			return new[] { FromProvinceNum.ToString(), ToProvinceNum.ToString() };
		}
	}

	[MapKeyName("pb")]
	public class ProvinceBorders : MapElement
	{
		public int X           { get; private set; }
		public int Y           { get; private set; }
		public int Len         { get; private set; }
		public int ProvinceNum { get; private set; }

		public override void ParseArgs (string[] args)
		{
			X           = int.Parse(args[0]);
			Y           = int.Parse(args[1]);
			Len         = int.Parse(args[2]);
			ProvinceNum = int.Parse(args[3]);
		}

		public override string[] SaveArgs ()
		{
			return new[] { X.ToString(), Y.ToString(), Len.ToString(), ProvinceNum.ToString() };
		}

		public override string ToString ()
		{
			return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Len)}: {Len}";
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

	[MapKeyName("lab")]
	public class Laboratory : ProvinceDataElement
	{
		public override void ParseArgs (string[] args)
		{
		}

		public override string[] SaveArgs ()
		{
			return new string[0];
		}
	}

	[MapKeyName("temple")]
	public class Temple : ProvinceDataElement
	{
		public override void ParseArgs (string[] args)
		{
		}

		public override string[] SaveArgs ()
		{
			return new string[0];
		}
	}

	[MapKeyName("fort")]
	public class Fort : ProvinceDataElement
	{
		public int FortId { get; set; }

		public override void ParseArgs (string[] args)
		{
			FortId = int.Parse(args[0]);
		}

		public override string[] SaveArgs ()
		{
			return new[] { FortId.ToString() };
		}
	}

	[MapKeyName("defence")]
	public class ProvinceDefence : ProvinceDataElement
	{
		public int Amount { get; set; }

		public override void ParseArgs (string[] args)
		{
			Amount = int.Parse(args[0]);
		}

		public override string[] SaveArgs ()
		{
			return new[] { Amount.ToString() };
		}
	}

	[MapKeyName("killfeatures")]
	public class KillFeatures : ProvinceDataElement
	{
		public override void ParseArgs (string[] args) {}

		public override string[] SaveArgs ()
		{
			return new string[0];
		}
	}

	[MapKeyName("feature")]
	public class HiddenMagicSite : ProvinceDataElement
	{
		public int    SiteId   { get; set; }
		public string SiteName { get; set; }

		public override void ParseArgs (string[] args)
		{
			if (int.TryParse(args[0], out int unitId))
			{
				SiteId = unitId;
			} else
			{
				SiteName = args[1];
			}
		}

		public override string[] SaveArgs ()
		{
			return new[] { SiteId.ToString() };
		}
	}

	[MapKeyName("knownfeature")]
	public class KnownMagicSite : ProvinceDataElement
	{
		public int    SiteId   { get; set; }
		public string SiteName { get; set; }

		public override void ParseArgs (string[] args)
		{
			if (int.TryParse(args[0], out int unitId))
			{
				SiteId = unitId;
			} else
			{
				SiteName = args[1];
			}
		}

		public override string[] SaveArgs ()
		{
			return new[] { SiteId.ToString() };
		}
	}

	[MapKeyName("owner")]
	public class ProvinceOwner : ProvinceDataElement
	{
		public int NationNum { get; set; }

		public override void ParseArgs (string[] args)
		{
			NationNum = int.Parse(args[0]);
		}

		public override string[] SaveArgs ()
		{
			return new[] { NationNum.ToString() };
		}
	}

	public abstract class MonsterElement : ProvinceDataElement
	{
		public int    MonsterId   { get; set; }
		public string MonsterName { get; protected set; }

		public override string[] SaveArgs ()
		{
			return new[] { MonsterId.ToString() };
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
	public class CommanderElement : MonsterElement
	{
		public override void ParseArgs (string[] args)
		{
			if (int.TryParse(args[0], out int unitId))
			{
				MonsterId = unitId;
			} else
			{
				MonsterName = args[0];
			}
		}
	}

	[MapKeyName("units")]
	public class UnitsElement : MonsterElement, IOwnedByCommander
	{
		public int              Amount    { get; set; }
		public CommanderElement Commander { get; set; }

		public override void ParseArgs (string[] args)
		{
			Amount = int.Parse(args[0]);
			if (int.TryParse(args[1], out int unitId))
			{
				MonsterId = unitId;
			} else
			{
				MonsterName = args[1];
			}
		}

		public override string[] SaveArgs ()
		{
			return new[] { Amount.ToString(), MonsterId.ToString() };
		}
	}

	[MapKeyName("bodyguards")]
	public class BodyguardsElement : MonsterElement, IOwnedByCommander
	{
		public int              Amount    { get; private set; }
		public CommanderElement Commander { get; set; }

		public override void ParseArgs (string[] args)
		{
			Amount = int.Parse(args[0]);
			if (int.TryParse(args[1], out int unitId))
			{
				MonsterId = unitId;
			} else
			{
				MonsterName = args[1];
			}
		}

		public override string[] SaveArgs ()
		{
			return new[] { Amount.ToString(), MonsterId.ToString() };
		}
	}

	[MapKeyName("additem")]
	public class ItemElement : MapElement, IOwnedByCommander
	{
		public string           ItemName  { get; set; }
		public CommanderElement Commander { get; set; }

		public override void     ParseArgs (string[] args) => ItemName = args[0];
		public override string[] SaveArgs ()               => new[] { $"\"{ItemName}\"" };
	}

	public abstract class Magic : MapElement, IOwnedByCommander
	{
		public          CommanderElement Commander                 { get; set; }
		public          int              MagicLevel                { get; set; }
		public override void             ParseArgs (string[] args) => MagicLevel = int.Parse(args[0]);
		public override string[]         SaveArgs ()               => new[] { MagicLevel.ToString() };
	}

	[MapKeyName("xp")]
	public abstract class Experience : MapElement, IOwnedByCommander
	{
		public          CommanderElement Commander                 { get; set; }
		public          int              Amount                    { get; set; }
		public override void             ParseArgs (string[] args) => Amount = int.Parse(args[0]);
		public override string[]         SaveArgs ()               => new[] { Amount.ToString() };
	}

	[MapKeyName("clearmagic")]
	public abstract class ClearMagic : MapElement, IOwnedByCommander
	{
		public          CommanderElement Commander                 { get; set; }
		public override void             ParseArgs (string[] args) {}
		public override string[]         SaveArgs ()               => new string[0];
	}

	[MapKeyName("mag_fire ")] public class FireMagic : Magic
	{
	}

	[MapKeyName("mag_air")] public class AirMagic : Magic
	{
	}

	[MapKeyName("mag_water")] public class WaterMagic : Magic
	{
	}

	[MapKeyName("mag_earth")] public class EarthMagic : Magic
	{
	}

	[MapKeyName("mag_astral")] public class AstralMagic : Magic
	{
	}

	[MapKeyName("mag_death")] public class DeathMagic : Magic
	{
	}

	[MapKeyName("mag_nature")] public class NatureMagic : Magic
	{
	}

	[MapKeyName("mag_blood")] public class BloodMagic : Magic
	{
	}

	[MapKeyName("mag_priest")] public class HolyMagic : Magic
	{
	}

	public interface IOwnedByCommander
	{
		CommanderElement Commander { get; set; }
	}

	public interface IOwnedByProvince
	{
		int ProvinceNum { get; set; }
	}

}