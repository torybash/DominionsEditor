using UnityEngine;
using UnityEngine.UI;
public class ItemGizmo : Gizmo
{
	[SerializeField] private Image icon;
	public Item Item { get; private set; }

	public void Initialize (Item item)
	{
		Item = item;
		
		var itemEntry = Map.Items.GetItem(item.ItemName);
		icon.sprite = itemEntry.Sprite;
		// icon.SetNativeSize();
	}
}