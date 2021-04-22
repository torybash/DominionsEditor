using System.Collections.Generic;
using UnityEngine;

public class CommanderGizmo : MonsterGizmo<Commander>
{
	[SerializeField] private RectTransform itemGroup;
	[SerializeField] private RectTransform magicGroup;
	
	private readonly List<ItemGizmo> _itemGizmos = new List<ItemGizmo>();

	public override void SetData (Monster data)
	{
		base.SetData(data);

		if (Data.Items.Count > 0)
		{
			RectTrans.sizeDelta = new Vector2(RectTrans.sizeDelta.x, EXPANDED_HEIGHT);
		} 
		else
		{
			RectTrans.sizeDelta = new Vector2(RectTrans.sizeDelta.x, DEFAULT_HEIGHT);
		}

		foreach (var itemGizmo in _itemGizmos) Destroy(itemGizmo.gameObject);
		_itemGizmos.Clear();
		
		foreach (var item in Data.Items)
		{
			Debug.Log($"Adding item {item.ItemName}");

			var itemGizmo = Ui.Create<ItemGizmo>(itemGroup);
			itemGizmo.Initialize(item);

			_itemGizmos.Add(itemGizmo);
		}
	}
}