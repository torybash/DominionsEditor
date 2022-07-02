using System;
using System.Collections.Generic;
using Data.Tables;
using UnityEngine;

namespace QuickCombat
{
	public class TableControl : MonoBehaviour
	{
		public List<Table> tables;

		public void Init ()
		{
			Tbl.SetInstance(this);
		}
	}

	public static class Tbl
	{
		static TableControl instance;

		public static void SetInstance (TableControl tableControl)
		{
			Debug.Assert(tableControl == null);
			instance = tableControl;
		}
		
		public static T Get<T> () where T : Table
		{
			foreach (Table table in instance.tables)
			{
				if (table is T t) return t;
			}
			throw new Exception($"Table {typeof(T)} not found");
		}
	}
}