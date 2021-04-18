using TMPro;
using UnityEngine;
using UnityEngine.UI;
public abstract class MonsterGizmo : Gizmo
{
	public abstract Monster MonsterData { get; }
}

public abstract class MonsterGizmo<T> : MonsterGizmo where T : Monster
{
	[SerializeField] protected TMP_Text nameLabel;
	[SerializeField] protected Image spritePicture;

	public T Data { get; private set; }

	public override Monster MonsterData => Data;

	public virtual void Initialize (Monster data)
	{
		Data = (T)data;
		
		var monster = Map.GetMonster(data.MonsterId);
		spritePicture.sprite = monster.Sprite;
		nameLabel.text = monster.Name;
	}
}