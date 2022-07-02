using System.Collections.Generic;
using Core;
using Core.Entities;
using UI.Gizmos;
using UnityEngine;
using UnityEngine.UI;
using Utility.Extensions;

namespace UI.Menus
{

	public class MapPicture : Menu
	{
		[SerializeField] public RawImage mapImage;

		readonly List<ProvinceGizmo> _provinceGizmos = new List<ProvinceGizmo>();

		public void LoadMap (Dictionary<int, Province> provinceMap, Texture2D mapTexture)
		{
			_provinceGizmos.SafeDestroyList();
		
			foreach (var province in provinceMap.Values)
			{
				var gizmo = DomEdit.I.Ui.Create<ProvinceGizmo>(transform);
				gizmo.RectTrans.anchoredPosition = province.CenterPos;
				gizmo.Initialize(province);
			
				_provinceGizmos.Add(gizmo);
			}
		
			LoadMapTexture(mapTexture);

			Show();
		}

		void LoadMapTexture (Texture2D mapTex)
		{
			mapImage.texture    = mapTex;
			RectTrans.sizeDelta = new Vector2(mapTex.width, mapTex.height);
		}
		
	}

}