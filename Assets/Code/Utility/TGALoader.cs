using System;
using System.IO;
using UnityEngine;
public static class TGALoader
{

	// Loads 32-bit (RGBA) uncompressed TGA. Actually, due to TARGA file structure, BGRA32 is good option...
	// Disabled mipmaps. Disabled read/write option, to release texture memory copy.
	// public static Texture2D LoadTGA(string fileName)
	// {
	// 	try
	// 	{
	// 		BinaryReader reader = new BinaryReader(File.OpenRead(fileName));
	// 		reader.BaseStream.Seek(12, SeekOrigin.Begin);    
	// 		short width = reader.ReadInt16();
	// 		short height = reader.ReadInt16();
	// 		reader.BaseStream.Seek(2, SeekOrigin.Current);
	// 		byte[] source = reader.ReadBytes(width * height * 4);
	// 		reader.Close();
	// 		Texture2D texture = new Texture2D(width, height, TextureFormat.BGRA32, false);
	// 		texture.LoadRawTextureData(source);
	// 		texture.name = Path.GetFileName(fileName);
	// 		texture.Apply(false, true);
	// 		return texture;
	// 	}
	// 	catch (Exception)
	// 	{
	// 		return Texture2D.blackTexture;
	// 	}
	// }
	//
	

	public static Texture2D LoadTGA (string fileName)
	{
		using (var imageFile = File.OpenRead(fileName))
		{
			return LoadTGA(imageFile);
		}
	}

	public static Texture2D LoadTGA (Stream TGAStream)
	{
	
		Debug.Log($"TGA Stream length {TGAStream.Length}");
		
		using (BinaryReader r = new BinaryReader(TGAStream))
		{
			// Skip some header info we don't care about.
			// Even if we did care, we have to move the stream seek point to the beginning,
			// as the previous method in the workflow left it at the end.
			r.BaseStream.Seek(12, SeekOrigin.Begin);
			
			short width = r.ReadInt16();
			short height = r.ReadInt16();
			int bitDepth = r.ReadByte();
	
			Debug.Log("width: "+ width + ", height: " + height + ", bitDepth: "+ bitDepth);
			// Skip a byte of header information we don't care about.
			r.BaseStream.Seek(2, SeekOrigin.Current);
	
			Texture2D tex = new Texture2D(width, height);
			Color32[] pulledColors = new Color32[width*height];
	
			bool hadExcep = false;
			if (bitDepth == 32)
			{
				for (int i = 0; i < width*height; i++)
				{
					byte huh = 0;
					byte red = 0;
					byte green = 0;
					byte blue  = 0;
					byte alpha = 0;
						
					try
					{
						// huh = r.ReadByte();
						blue = r.ReadByte();
						green = r.ReadByte();
						red = r.ReadByte();
						alpha = r.ReadByte();
	
					} catch (Exception e)
					{
	
						// throw;
					}
					
					pulledColors[i] = new Color32(blue, green, red, alpha);
	
				}
			} else if (bitDepth == 24)
			{
				for (int i = 0; i < width*height; i++)
				{
					byte red = r.ReadByte();
					byte green = r.ReadByte();
					byte blue = r.ReadByte();
	
					pulledColors[i] = new Color32(blue, green, red, 1);
				}
			} else
			{
				throw new Exception("TGA texture had non 32/24 bit depth.");
			}
	
			tex.SetPixels32(pulledColors);
			tex.Apply();
			return tex;
	
		}
	}
}