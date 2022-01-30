using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IconsTable))]
public class IconsTableEditor : Editor
{
	private const string NATION_ICONS_PATH = "Assets/Art/nations/";

	private IconsTable Target => (IconsTable)target;

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Load"))
		{
			LoadNationIcons();
			
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
}