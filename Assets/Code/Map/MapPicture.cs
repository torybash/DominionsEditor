using Tga;
using UnityEngine;
using UnityEngine.UI;

public class MapPicture : Menu
{
	[SerializeField] private RawImage mapImage;
	
	public RawImage MapImage => mapImage;

	public void SetMapImage (string mapImagePath)
	{
		// Texture2D mapTex = null;

		Debug.Log($"GetTexture mapImagePath: {mapImagePath}");
		// var texture2D = TGALoader.LoadTGA(mapImagePath);
		TgaLoader tgaLoader = new TgaLoader();
		tgaLoader.Load(mapImagePath);

		var mapTex = new Texture2D(tgaLoader.Width, tgaLoader.Height, TextureFormat.RGBA32, false);
		var pixels = new Color32[tgaLoader.Width * tgaLoader.Height];
		for (int i = 0; i < tgaLoader.ImageSize; i+=4)
		{
			var color = new Color32(
				tgaLoader.Image[i + 2],
				tgaLoader.Image[i + 1],
				tgaLoader.Image[i + 0],
				byte.MaxValue
				// tgaLoader.Image[i + 1],
				// tgaLoader.Image[i + 2],
				// tgaLoader.Image[i + 3],
				// tgaLoader.Image[i]
				);
			pixels[i/4] = color;
		}
		mapTex.SetPixels32(pixels);
		mapTex.Apply();
		
		// var texture2D = Tga.TgaLoader.LoadTGA(mapImagePath);
		// Debug.Log($"GetTexture tex: {texture2D}, (w,h): ({texture2D.width}, {texture2D.height})");
		// mapTex = texture2D;


		MapImage.texture = mapTex;
		// byte[] mapImageBytes = File.ReadAllBytes(mapImagePath);
		// return mapImageBytes;
	}
	
}