using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class MagicPathTable : ScriptableObject
{
	public List<MagicEntry> magics;

	public MagicEntry GetEntry (MagicPath path)
	{
		return magics.SingleOrDefault(x => x.magicPath == path);
	}
}