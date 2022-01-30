using System.Collections.Generic;
using Core;
using Core.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gizmos
{

	public class CommanderGizmo : MonsterGizmo<Commander>
	{
		public const float DEFAULT_HEIGHT   = 30f;
		public const float EXTRA_ROW_HEIGHT = 30f;
	
		[SerializeField] private HorizontalLayoutGroup itemGroup;
		[SerializeField] private HorizontalLayoutGroup magicGroup;
	
		private readonly List<ItemGizmo>  _itemGizmos  = new List<ItemGizmo>();
		private readonly List<MagicGizmo> _magicGizmos = new List<MagicGizmo>();

		public override void SetData (Monster data)
		{
			base.SetData(data);

			bool hasItems = Data.Items.Count          > 0;
			bool hasMagic = Data.MagicOverrides.Count > 0;

			float height         = DEFAULT_HEIGHT;
			if (hasItems) height += EXTRA_ROW_HEIGHT;
			if (hasMagic) height += EXTRA_ROW_HEIGHT;

			RectTrans.sizeDelta = new Vector2(RectTrans.sizeDelta.x, height);

			UpdateItems();
			UpdateMagic();
		}

		private void UpdateItems ()
		{
			foreach (var gizmo in _itemGizmos) Destroy(gizmo.gameObject);
			_itemGizmos.Clear();

			float totalItemWidth = 0f;
			foreach (var item in Data.Items)
			{
				var itemGizmo = DomEdit.I.Ui.Create<ItemGizmo>(itemGroup.transform);
				itemGizmo.Initialize(this, item);
				totalItemWidth += itemGizmo.RectTrans.sizeDelta.x;

				_itemGizmos.Add(itemGizmo);
			}

			float groupWidth = itemGroup.GetComponent<RectTransform>().sizeDelta.x;
			if (totalItemWidth > groupWidth && Data.Items.Count > 1)
			{
				float widthNeeded   = totalItemWidth - groupWidth;
				float spacingNeeded = widthNeeded/(Data.Items.Count - 1);
				itemGroup.spacing = -spacingNeeded;
			}
		}
	
		private void UpdateMagic ()
		{
			foreach (var gizmo in _magicGizmos) Destroy(gizmo.gameObject);
			_magicGizmos.Clear();

			float totalItemWidth = 0f;
			foreach (var item in Data.MagicOverrides)
			{
				var itemGizmo = DomEdit.I.Ui.Create<MagicGizmo>(magicGroup.transform);
				itemGizmo.Initialize(this, item);
				totalItemWidth += itemGizmo.RectTrans.sizeDelta.x;

				_magicGizmos.Add(itemGizmo);
			}

			float groupWidth = magicGroup.GetComponent<RectTransform>().sizeDelta.x;
			if (totalItemWidth > groupWidth && Data.MagicOverrides.Count > 1)
			{
				float widthNeeded   = totalItemWidth - groupWidth;
				float spacingNeeded = widthNeeded/(Data.MagicOverrides.Count - 1);
				magicGroup.spacing = -spacingNeeded;
			}
		}
	}

}