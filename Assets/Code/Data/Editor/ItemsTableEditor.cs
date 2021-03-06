using System.Collections.Generic;
using System.IO;
using Data.Entries;
using Data.Tables;
using Dom;
using UnityEditor;
using UnityEngine;

namespace Data.Editor
{

	[CustomEditor(typeof(ItemsTable))]
	public class ItemsTableEditor : UnityEditor.Editor
	{
		private const string DATA_PATH         = "Assets/Data/gamedata/BaseI.csv";
		private const string ICONS_FOLDER_PATH = "Assets/Art/items/";

		private ItemsTable              Target => (ItemsTable)target;
		private Dictionary<int, Sprite> _spriteMap;

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Load"))
			{
				LoadItems();
			
				EditorUtility.SetDirty(Target);
			}
		}

		private void LoadItems ()
		{
			Target.Entries.Clear();

			var lines          = File.ReadAllLines(DATA_PATH);
			var attributeNames = lines[0].Split('\t');
		
			
			for (var i = 1; i < lines.Length; i++)
			{
				var itemValues = lines[i].Split('\t');
			
				const int ID_IDX   = 0;
				const int NAME_IDX = 1;
				const int TYPE_IDX = 1;
				int       id       = int.Parse(itemValues[ID_IDX]);
				var       itemName = itemValues[NAME_IDX];
				var       itemType = ItemTypeUtil.GetItemType(itemValues[TYPE_IDX]);

				// var attributes = new Dictionary<string, int>();
			
				var sprite    = GetSprite(id);
				var itemEntry = new ItemEntry(id, itemName, itemType, sprite );
				Target.Entries.Add(itemEntry);
			}
		}
	
		private Sprite GetSprite (int monsterId)
		{
			if (_spriteMap == null) CreateSpriteMap();
			if (!_spriteMap.TryGetValue(monsterId, out var foundSprite))
			{
				Debug.LogError($"No sprite found for monster with id {monsterId}");
			}
			return foundSprite;
		}
	
		private void CreateSpriteMap ()
		{
			_spriteMap = new Dictionary<int, Sprite>();
			foreach (var iconPath in Directory.GetFiles(ICONS_FOLDER_PATH))
			{
				if (!iconPath.EndsWith(".png")) continue;
			
				int lastSepIdx = iconPath.LastIndexOf('/');
				int extIdx     = iconPath.LastIndexOf('.');

				var iconName = iconPath.Substring(lastSepIdx + 1, extIdx - lastSepIdx - 1);

				var iconIdText = iconName.Substring(4);
				int iconId     = int.Parse(iconIdText);
			
				var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
				_spriteMap.Add(iconId, sprite);
			}
		}
	}

}