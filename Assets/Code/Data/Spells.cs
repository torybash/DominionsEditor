using System.Collections.Generic;
using Core;

namespace Data
{

	public class Spells
	{
		List<SpellData> _spells;

		public List<SpellData> GetAll ()
		{
			return _spells;
		}

		public void ParseData ()
		{
			_spells = new List<SpellData>();
			foreach (var spell in DomEdit.I.GameData.spellsTable)
			{
				var unit = new SpellData();
				unit.id            = int.Parse(spell["id"]);
				unit.name          = spell["name"];
				unit.schoolId      = int.Parse(spell["school"]);
				unit.researchLevel = int.Parse(spell["researchlevel"]);

				_spells.Add(unit);
			}
		}
	}
	public class SpellData
	{
		public int    id;
		public string name;
		public int    schoolId;
		public int    researchLevel;

		public override string ToString ()
		{
			return $"{nameof(id)}: {id}, {nameof(name)}: {name}";
		}
	}

}