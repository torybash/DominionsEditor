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
		[SerializeField] private string name2;
		[SerializeField] private int nationNum;
		[SerializeField] private string pretenderFileName;
		[SerializeField] private Sprite sprite;

		public string Name => name;
		public string Name2 => name2;
		public int NationNum => nationNum;
		public string PretenderFileName => pretenderFileName;
		public Sprite Sprite => sprite;
	}
	
	[SerializeField] private List<Entry> nations;

	public Entry GetNationEntry (int specStartNationNum)
	{
		return nations.SingleOrDefault(x => x.NationNum == specStartNationNum);
	}
}