using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class NationsTable : ScriptableObject
{

	[SerializeField] private List<NationEntry> nations;
	public List<NationEntry> Nations => nations;

	public NationEntry GetNationEntry (Nation nation)
	{
		return nations.SingleOrDefault(x => x.NationNum == nation.Number);
	}
	
	public Nation GetNationByName (string nationName)
	{
		var entry = nations.SingleOrDefault(x => x.Name.Equals(nationName, StringComparison.OrdinalIgnoreCase));
		if (entry == null) return Nation.Invalid;
		return entry.NationNum;
	}
}