using System.Collections.Generic;
using Core;
using UI.Gizmos;
using UnityEngine;
using UnityEngine.UI;
using Utility.Extensions;

namespace UI.Menus
{

	public class MapPicture : Menu
	{
		[SerializeField] public RawImage mapImage;
	
		private readonly List<ProvinceGizmo> _provinceGizmos = new List<ProvinceGizmo>();

		public void LoadMap (Map.Map map)
		{
			_provinceGizmos.SafeDestroy();
		
			foreach (var province in map.ProvinceMap.Values)
			{
				var gizmo = DomEdit.I.Ui.Create<ProvinceGizmo>(transform);
				gizmo.RectTrans.anchoredPosition = province.CenterPos;
				gizmo.Initialize(province);
			
				_provinceGizmos.Add(gizmo);
			}
		
			LoadMapTexture(map.MapTexture);

			Show();
		}

		private void LoadMapTexture (Texture2D mapTex)
		{
			mapImage.texture    = mapTex;
			RectTrans.sizeDelta = new Vector2(mapTex.width, mapTex.height);
		}

	}

}