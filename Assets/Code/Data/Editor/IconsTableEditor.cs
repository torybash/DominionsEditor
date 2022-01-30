using System;
using System.Collections.Generic;
using System.IO;
using Data.Tables;
using UnityEditor;
using UnityEngine;

namespace Data.Editor
{

	[CustomEditor(typeof(IconsTable))]
	public class IconsTableEditor : UnityEditor.Editor
	{
		private const string NATION_ICONS_PATH = "Assets/Art/nations/";
		private const string UNIT_ICONS_PATH = "Assets/Art/sprites/";

		private IconsTable Target => (IconsTable)target;

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Load Nation Icons"))
			{
				LoadNationIcons();
				EditorUtility.SetDirty(Target);
			}
			
			if (GUILayout.Button("Load Unit Icons"))
			{
				LoadUnitIcons();
				EditorUtility.SetDirty(Target);
			}
		}

		private void LoadNationIcons ()
		{
			Target.nationIcons = new List<NationIcon>();
			foreach (var iconPath in Directory.GetFiles(NATION_ICONS_PATH))
			{
				if (!iconPath.EndsWith(".png")) continue;
			
				int lastSepIdx = iconPath.LastIndexOf('/');
				int extIdx     = iconPath.LastIndexOf('.');
	
				var iconName = iconPath.Substring(lastSepIdx + 1, extIdx - lastSepIdx - 1);
			
				var eraShort   = iconName.Split('_')[0];
				var nationName = iconName.Split('_')[1];

				var eraLong = eraShort switch
				{
					"ea" => "early",
					"ma" => "mid",
					"la" => "late",
					_    => throw new ArgumentOutOfRangeException()
				};
			
				var nationFileName = eraLong + "_" + nationName;
			
				var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
				Target.nationIcons.Add(new NationIcon(nationFileName, sprite));
			}
		}

		private void LoadUnitIcons ()
		{
			Target.unitIcons = new List<UnitIcon>();
			foreach (var iconPath in Directory.GetFiles(UNIT_ICONS_PATH))
			{
				if (!iconPath.EndsWith(".png")) continue;
			
				int lastSepIdx = iconPath.LastIndexOf('/');
				int extIdx     = iconPath.LastIndexOf('.');

				var iconName = iconPath.Substring(lastSepIdx + 1, extIdx - lastSepIdx - 1);

				var idText   = iconName.Split('_')[0];
				int id       = int.Parse(idText);
				var typeText = iconName.Split('_')[1];
				int type     = int.Parse(typeText);
				if (type == 1)
				{
					var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
					Target.unitIcons.Add(new UnitIcon(id, sprite));
				}
			}
		}
	}

}