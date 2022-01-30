using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	[Serializable]
	public class Nation
	{
		public int    id;
		public string name;
		public string epithet;
		public string fileNameBase;
		public int    era;
	
		public Sprite    icon;
		
		public List<int> homerealm           = new List<int>();
		public List<int> pretenders          = new List<int>();
		public List<int> commanders          = new List<int>();
		public List<int> foreigncommanders   = new List<int>();
		public List<int> units               = new List<int>();
		public List<int> foreignunits        = new List<int>();
		public List<int> coastcom            = new List<int>();
		public List<int> coastrec            = new List<int>();
		public List<int> sites               = new List<int>();
		public List<int> futuresites         = new List<int>();
		public List<int> landcom             = new List<int>();
		public List<int> uwcom               = new List<int>();
		public List<int> uwunit              = new List<int>();
		public List<int> forestrec           = new List<int>();
		public List<int> forestcom           = new List<int>();
		public List<int> swamprec            = new List<int>();
		public List<int> swampcom            = new List<int>();
		public List<int> mountainrec         = new List<int>();
		public List<int> mountaincom         = new List<int>();
		public List<int> wasterec            = new List<int>();
		public List<int> wastecom            = new List<int>();
		public List<int> caverec             = new List<int>();
		public List<int> cavecom             = new List<int>();
		public List<int> plainsrec           = new List<int>();
		public List<int> plainscom           = new List<int>();
		public List<int> heroes              = new List<int>();
		public List<int> multiheroes         = new List<int>();
		public List<int> capunits            = new List<int>();
		public List<int> capcommanders       = new List<int>();
		public List<int> futurecapunits      = new List<int>();
		public List<int> futurecapcommanders = new List<int>();
		public List<int> landunit            = new List<int>();

		public static Nation Invalid      => new Nation { id = -1 };
		public static Nation Independents => new Nation { id = 0 };

		public static implicit operator Nation(int value)
		{
			var nation = new Nation
			{
				id = value
			};
			return nation;
		}

		protected bool Equals (Nation other)
		{
			return id == other.id;
		}

		public override bool Equals (object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Nation)obj);
		}

		public override int GetHashCode ()
		{
			return id;
		}

		public static bool operator == (Nation left, Nation right)
		{
			return Equals(left, right);
		}

		public static bool operator != (Nation left, Nation right)
		{
			return !Equals(left, right);
		}

		public override string ToString ()
		{
			return $"{nameof(id)}: {id}";
		}
	}

}