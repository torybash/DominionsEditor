using System.Collections.Generic;
using Core;

namespace Data
{

	public class Items
	{
		private List<ItemData> _items;

		public List<ItemData> GetAll ()
		{
			return _items;
		}

		public void ParseData ()
		{
			_items = new List<ItemData>();
			foreach (var unitData in DomEdit.I.GameData.itemsTable)
			{
				var unit = new ItemData();
				unit.id   = int.Parse(unitData["id"]);
				unit.name = unitData["name"];
				unit.icon = DomEdit.I.icons.GetItemIcon(unit.id);

				_items.Add(unit);
			}
		}
	}

}