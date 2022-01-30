using UnityEngine;
using Utility;

namespace Tools
{

	public static class MapImageLoader
	{
		public static Texture2D LoadMapTexture (string mapImagePath)
		{
			Debug.Log($"LoadMapTexture mapImagePath: {mapImagePath}");

			TgaLoader tgaLoader = new TgaLoader();
			tgaLoader.Load(mapImagePath);

			var mapTex = new Texture2D(tgaLoader.Width, tgaLoader.Height, TextureFormat.RGBA32, false);
			var pixels = new Color32[tgaLoader.Width*tgaLoader.Height];
			for (int i = 0; i < tgaLoader.ImageSize; i += 4)
			{
				var color = new Color32(
					tgaLoader.Image[i + 2],
					tgaLoader.Image[i + 1],
					tgaLoader.Image[i + 0],
					byte.MaxValue
					);
				pixels[i/4] = color;
			}
			mapTex.SetPixels32(pixels);
			mapTex.Apply();
			return mapTex;
		}
	}

}