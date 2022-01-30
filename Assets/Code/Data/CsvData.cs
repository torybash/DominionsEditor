using System.Collections.Generic;
using System.IO;

namespace Data
{

	public class CsvData
	{
		public List<Dictionary<string, string>> data;
		public CsvData (List<Dictionary<string, string>> data)
		{
			this.data = data;
		}

		public static CsvData LoadCsv (string path)
		{
			List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

			var lines          = File.ReadAllLines(path);
			var attributeNames = lines[0].Split('\t');

			for (var i = 1; i < lines.Length; i++)
			{
				var dataMap       = new Dictionary<string, string>();
				var monsterValues = lines[i].Split('\t');
				for (int j = 0; j < attributeNames.Length; j++)
				{
					var attr = attributeNames[j];
					var val  = monsterValues[j];

					if (!dataMap.ContainsKey(attr)) dataMap.Add(attr, val);
				}

				data.Add(dataMap);
			}

			var csvData = new CsvData(data);
			return csvData;
		}
	}

}