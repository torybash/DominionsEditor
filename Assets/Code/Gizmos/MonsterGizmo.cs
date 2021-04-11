using TMPro;
using UnityEngine;
using UnityEngine.UI;
public abstract class MonsterGizmo : Gizmo
{
	[SerializeField] protected TMP_Text nameLabel;
	[SerializeField] protected Image spritePicture;

	public ProvinceDataElement Element { get; private set; }
	
	public virtual void Initialize (MonsterElement monsterElement)
	{
		Element = monsterElement;
		
		MonstersTable.Entry monster;
		if (monsterElement.UnitId > 0)
		{
			monster = Man.GetMonster(monsterElement.UnitId);
		} else
		{
			monster = Man.GetMonster(monsterElement.UnitName);
		}
		
		spritePicture.sprite = monster.Sprite;
		nameLabel.text = monster.Name;
	}
}