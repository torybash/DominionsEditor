using System.Collections.Generic;
using Core;
using Data.Tables;
using QuickCombat;

namespace Data
{

	public class Items
	{
		List<ItemData> _items;

		readonly GameData _gameData;

		Items (GameData gameData)
		{
			_gameData = gameData;
		}

		public static Items Load (GameData gameData)
		{
			return new Items(gameData);
		}

		public List<ItemData> GetAll ()
		{
			return _items;
		}

		public void ParseData ()
		{
			_items = new List<ItemData>();
			foreach (var unitData in _gameData.itemsTable)
			{
				var unit = new ItemData();
				unit.id   = int.Parse(unitData["id"]);
				unit.name = unitData["name"];
				unit.icon = Tbl.Get<IconsTable>().GetItemIcon(unit.id);

				_items.Add(unit);
			}
		}
	}

}