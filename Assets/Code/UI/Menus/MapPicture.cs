using UnityEngine;
using UnityEngine.UI;

public class MapPicture : Menu
{
	[SerializeField] public RawImage mapImage;
	
	public void LoadMapTexture (Texture2D mapTex)
	{
		mapImage.texture    = mapTex;
		RectTrans.sizeDelta = new Vector2(mapTex.width, mapTex.height);

		Show();
	}

}