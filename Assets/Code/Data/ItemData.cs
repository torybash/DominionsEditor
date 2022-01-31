using Core.Entities;
using Dom;
using UnityEngine;

namespace Data
{

	public class ItemData : IEntityData
	{
		public int      id;
		public string   name;
		public ItemType unitType;
		public Sprite   icon;
	}

}