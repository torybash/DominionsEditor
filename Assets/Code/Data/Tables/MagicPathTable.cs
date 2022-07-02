using System.Collections.Generic;
using System.Linq;
using Data.Entries;
using Dom;
using UnityEngine;

namespace Data.Tables
{

	[CreateAssetMenu]
	public class MagicPathTable : Table
	{
		public List<MagicEntry> magics;

		public MagicEntry GetEntry (MagicPath path)
		{
			return magics.SingleOrDefault(x => x.magicPath == path);
		}
	}

}