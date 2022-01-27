using System.Collections.Generic;

public class Map
{
	public List<MapElement>          Elements { get; set; } = new List<MapElement>();
	public Dictionary<int, Province> ProvinceMap { get; set; } = new Dictionary<int, Province>();

}