using TMPro;
using UnityEngine;
using UnityEngine.UI;
public abstract class MonsterGizmo : Gizmo
{
	public abstract Monster MonsterData { get; }
}

public abstract class MonsterGizmo<T> : MonsterGizmo where T : Monster
{
	public const float DEFAULT_HEIGHT = 30f;
	public const float EXPANDED_HEIGHT = 55f;
	
	[SerializeField] protected TMP_Text nameLabel;
	[SerializeField] protected Image spritePicture;
	[SerializeField] protected Image background;
	
	private Color? defaultColor;

	public T Data { get; private set; }

	public override Monster MonsterData => Data;

	public virtual void SetData (Monster data)
	{
		Data = (T)data;
		
		var monster = Map.GetMonster(data.MonsterId);
		spritePicture.sprite = monster.Sprite;
		nameLabel.text = monster.Name;
		
		if (data.Nationality != Nation.Independents)
		{
			if (defaultColor == null) defaultColor = background.color;
			
			var nationEntry = Map.Nations.GetNationEntry(data.Nationality);
			background.color = defaultColor.Value * nationEntry.TintColor;
		}
	}
}