using UnityEngine;
using UnityEngine.UI;
public class ItemGizmo : Gizmo
{
	[SerializeField] private Image icon;

	public void Initialize (Item item)
	{
		var itemEntry = Map.Items.GetItem(item.ItemName);
		icon.sprite = itemEntry.Sprite;
		// icon.SetNativeSize();
	}
}