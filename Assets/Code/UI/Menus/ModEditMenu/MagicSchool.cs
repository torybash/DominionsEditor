using System.Collections.Generic;

namespace UI.Menus
{

	public struct MagicSchool
	{
		public string name;
		public int    schoolId;

		public static List<MagicSchool> All = new List<MagicSchool>
		{
			new MagicSchool { name = "Conjuration", schoolId  = 0 },
			new MagicSchool { name = "Alteration", schoolId   = 1 },
			new MagicSchool { name = "Evocation", schoolId    = 2 },
			new MagicSchool { name = "Construction", schoolId = 3 },
			new MagicSchool { name = "Enchantment", schoolId  = 4 },
			new MagicSchool { name = "Thaumaturgy", schoolId  = 5 },
			new MagicSchool { name = "Blood Magic", schoolId  = 6 },
			new MagicSchool { name = "Divine", schoolId       = 7 },
		};
	}

}