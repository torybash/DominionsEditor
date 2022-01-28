using UnityEngine;

[System.Serializable]
public class MagicEntry : SearchableEntry
{
	[SerializeField] public MagicPath magicPath;
	[SerializeField] private Sprite    sprite;

	public override string Name   => magicPath.ToString();
	public override Sprite Sprite => sprite;
}