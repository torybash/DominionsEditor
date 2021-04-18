using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class NationsTable : ScriptableObject
{
	[System.Serializable]
	public class Entry
	{
		[SerializeField] private string name;
		[SerializeField] private string epithet;
		[SerializeField] private int nationNum;
		[SerializeField] private string pretenderFileName;
		[SerializeField] private Sprite sprite;
		[SerializeField] private string samplePretenderFilePath;

		public string Name => name;
		public string Epithet => epithet;
		public int NationNum => nationNum;
		public string PretenderFileName => pretenderFileName;
		public Sprite Sprite => sprite;
		public string SamplePretenderFilePath => samplePretenderFilePath;
	}
	
	[SerializeField] private List<Entry> nations;

	public Entry GetNationEntry (Nation nation)
	{
		return nations.Single(x => x.NationNum == nation.Number);
	}
}