using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility.Extensions
{

	public static class ListExtensions
	{
		public static void SafeDestroyList<T> (this IList<T> list) where T : Component
		{
			if (list == null) return;
		
			foreach (var component in list)
			{
				if (component != null) component.SafeDestroy();
			}
			list.Clear();
		}
	
		public static T GetRandom<T> (this IList<T> list)
		{
			return list.Count == 0 ? default : list[UnityEngine.Random.Range(0, list.Count)];
		}

		public static T GetRandom<T> (this IEnumerable<T> enumerable)
		{
			return GetRandom(enumerable.ToList());
		}
	}

}