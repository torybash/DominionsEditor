using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Map
{

	public static class FileLoader
	{
		public static List<FileProperty> ParseProperties (string[] mapLineTexts)
		{
			var mapProperties = new List<FileProperty>();
			for (int i = 0; i < mapLineTexts.Length; i++)
			{
				var mapLine = mapLineTexts[i];
				if (mapLine.Length == 0) continue;
				if (mapLine[0]     != '#') continue;

				int spaceIndex  = mapLine.IndexOf(' ');
				int keyEndIndex = spaceIndex == -1 ? mapLine.Length : spaceIndex;

				var key = mapLine.Substring(1, keyEndIndex - 1);

				var mapProperty = new FileProperty(key);
				if (spaceIndex != -1)
				{
					var args = mapLine.Substring(keyEndIndex + 1, mapLine.Length - keyEndIndex - 1);

					var      separatorChar = ' ';
					Regex    regx          = new Regex(separatorChar + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
					string[] argsSplit     = regx.Split(args);

					foreach (var arg in argsSplit)
					{
						if (arg.Contains("--")) break;
						mapProperty.AddArg(arg);
					}
				}

				mapProperties.Add(mapProperty);
			}
			return mapProperties;
		}
	}

}