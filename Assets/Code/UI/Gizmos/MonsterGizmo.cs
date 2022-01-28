using TMPro;
using UnityEngine;
using UnityEngine.UI;
public abstract class MonsterGizmo : Gizmo
{
	[SerializeField] protected TMP_Text      nameLabel;
	[SerializeField] protected Image         spritePicture;
	[SerializeField] protected Image         background;
	[SerializeField] protected RectTransform nationContainer;

	public abstract Monster       MonsterData   { get; }
	public          ProvinceGizmo OwnerProvince { get; private set; }

	public void Initialize (ProvinceGizmo ownerProvince)
	{
		OwnerProvince = ownerProvince;
	}
}

public abstract class MonsterGizmo<T> : MonsterGizmo where T : Monster
{
	private Color? defaultColor;

	public T Data { get; private set; }

	public override Monster MonsterData => Data;


	public virtual void SetData (Monster data)
	{
		Data = (T)data;

		var monster = DomEdit.I.monsters.GetMonster(data.MonsterId);
		spritePicture.sprite = monster.Sprite;
		nameLabel.text       = monster.Name;

		if (data.Nationality != Nation.Independents)
		{
			if (defaultColor == null) defaultColor = background.color;

			var nationEntry = DomEdit.I.nations.GetNationEntry(data.Nationality);
			background.color = defaultColor.Value*nationEntry.TintColor;

			var nationGizmo = DomEdit.I.Ui.Create<NationGizmo>(nationContainer);
			nationGizmo.SetNation(data.Nationality, false);
		}
	}
}