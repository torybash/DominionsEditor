using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Data
{

	public class CsvTable : List<Dictionary<string, string>>
	{
		public CsvTable (List<Dictionary<string, string>> table)
		{
			AddRange(table);
		}

		public static CsvTable LoadCsv (string path)
		{
			Debug.Log($"Loading csv at {path}");
			var table = new List<Dictionary<string, string>>();

			var lines    = File.ReadAllLines(path);
			var keyNames = lines[0].Split('\t');

			for (var i = 1; i < lines.Length; i++)
			{
				var values  = lines[i].Split('\t');
				
				if (values[0] == "") continue;
				
				var dataMap = new Dictionary<string, string>();
				for (int j = 0; j < keyNames.Length; j++)
				{
					if (j >= values.Length) continue;
					
					var key = keyNames[j];
					var val  = values[j];

					if (/*val != "" &&*/ val != "\r" && !dataMap.ContainsKey(key))
					{
						dataMap.Add(key, val);
					}
				}

				table.Add(dataMap);
			}

			var csvData = new CsvTable(table);
			return csvData;
		}

		public Dictionary<string, string> GetData (string attr, string value)
		{
			return this.SingleOrDefault(x => x[attr] == value);
		}
	}

}