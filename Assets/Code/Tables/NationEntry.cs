using UnityEngine;
[System.Serializable]
public class NationEntry
{
	[SerializeField] private string name;
	[SerializeField] private string epithet;
	[SerializeField] private int nationNum;
	[SerializeField] private string pretenderFileName;
	[SerializeField] private int era;
	[SerializeField] private Sprite sprite;
	[SerializeField] private Color tintColor;

	public string Name => name;
	public string Epithet => epithet;
	public int NationNum => nationNum;
	public string PretenderFileName => pretenderFileName;
	public Sprite Sprite => sprite;
	public Color TintColor => tintColor;
	public int Era => era;

	public NationEntry (string name, string epithet, int nationNum, string pretenderFileName, int era, Sprite sprite, Color tintColor)
	{
		this.name = name;
		this.epithet = epithet;
		this.nationNum = nationNum;
		this.pretenderFileName = pretenderFileName;
		this.era = era;
		this.sprite = sprite;
		this.tintColor = tintColor;
	}
}