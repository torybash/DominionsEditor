using UnityEngine;

[System.Serializable]
public class ItemEntry : SearchableEntry
{
	[SerializeField] private string name;
	[SerializeField] private int id;
	[SerializeField] private Sprite sprite;

	public override string Name => name;
	public int Id => id;
	public override Sprite Sprite => sprite;

	public ItemEntry (int id, string name, Sprite sprite)
	{
		this.id = id;
		this.name = name;
		this.sprite = sprite;
	}
}