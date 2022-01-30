using System.Collections.Generic;
using Core;
using Map.MapData;
using Map.MapElements;
using UI.Menus;
using UnityEngine;

namespace Map
{

	public class Map
	{
		public Texture2D                 MapTexture        { get; set; }
		public List<MapElement>          AllElements       { get; set; } = new List<MapElement>();
		public List<MapElement>          UnchangedElements { get; set; } = new List<MapElement>();
		public Dictionary<int, Province> ProvinceMap       { get; set; } = new Dictionary<int, Province>();
		public List<GamePlayer>          Players           { get; set; } = new List<GamePlayer>();
		public MapFile                   MapFile           { get; set; }

	}

}