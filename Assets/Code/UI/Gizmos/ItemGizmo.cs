using Core;
using Map.MapData;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gizmos
{

	public class ItemGizmo : Gizmo
	{
		[SerializeField] private Image          icon;
		public                   Item           Item       { get; private set; }
		public                   CommanderGizmo OwnerGizmo { get; set; }

		public void Initialize (CommanderGizmo commanderGizmo, Item item)
		{
			OwnerGizmo = commanderGizmo;
			Item       = item;
		
			var itemEntry = DomEdit.I.items.GetItem(item.ItemName);
			icon.sprite = itemEntry.Sprite;
		}
	}

}