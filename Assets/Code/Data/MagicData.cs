using System;
using Core.Entities;
using Dom;
using UnityEngine;

namespace Data
{

	[Serializable]
	public class MagicData : IEntityData
	{
		[SerializeField] public  MagicPath magicPath;
		[SerializeField] private Sprite    sprite;

		public MagicData (MagicPath magicPath, Sprite sprite)
		{
			this.magicPath = magicPath;
			this.sprite    = sprite;
		}

		public string Name   => magicPath.ToString();
		public Sprite Sprite => sprite;
	}

}