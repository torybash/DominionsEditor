using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommanderGizmo : MonsterGizmo<Commander>
{
	[SerializeField] private HorizontalLayoutGroup itemGroup;
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

		float totalItemWidth = 0f;
		foreach (var item in Data.Items)
		{
			var itemGizmo = Ui.Create<ItemGizmo>(itemGroup.transform);
			itemGizmo.Initialize(item);
			totalItemWidth += itemGizmo.RectTrans.sizeDelta.x;
			
			_itemGizmos.Add(itemGizmo);
		}

		float groupWidth = itemGroup.GetComponent<RectTransform>().sizeDelta.x;
		if (totalItemWidth > groupWidth && Data.Items.Count > 1)
		{
			float widthNeeded = totalItemWidth - groupWidth;
			float spacingNeeded = widthNeeded / (Data.Items.Count - 1);
			itemGroup.spacing = -spacingNeeded;
		}
	}
}