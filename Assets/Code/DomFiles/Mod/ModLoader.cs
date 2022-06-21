using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Map;
using Mod.ModElements;
using UI.Menus;

namespace Mod
{

	public class ModLoader
	{
		public static Mod Load (DomFile modFile)
		{
			var allElements = LoadModElements(modFile.path);

			var mod = new Mod();
			// mod.ModFile = modFile;
			//
			// mod.ModTexture  = LoadModTexture(mod);
			// mod.ProvinceMod = LoadProvinces(mod);
			// mod.Players     = LoadPlayers(mod);

			return mod;
		}
		
		public static List<ModElement> LoadModElements (string modPath)
		{
			var texts      = File.ReadAllLines(modPath);
			var properties = FileLoader.ParseProperties(texts);
			var elements   = ParseElements(properties);

			return elements;
		}

		static List<ModElement> ParseElements (List<FileProperty> modLineDatas)
		{
			var modElements = new List<ModElement>();
			for (var i = 0; i < modLineDatas.Count; i++)
			{
				var modLineData = modLineDatas[i];
				var modElemType = GetModElementType(modLineData.Key);
				if (modElemType == null) continue;

				var modElem = (ModElement)Activator.CreateInstance(modElemType);
				modElem.ParseArgs(modLineData.Args.ToArray());
				modElements.Add(modElem);
			}

			return modElements;
		}

		static Type GetModElementType (string nameKey)
		{

			var ass = typeof(ModElement).Assembly;
			foreach (Type type in ass.GetTypes())
			{
				if (!typeof(ModElement).IsAssignableFrom(type)) continue;
				if (typeof(ModElement) == type) continue;
				if (type.IsAbstract) continue;

				var modKeyName = ((ModKeyNameAttribute[])type.GetCustomAttributes(typeof(ModKeyNameAttribute), false)).Single();

				if (modKeyName.Name == nameKey) return type;
			}
			return null;
		}
		
	}
	
	[AttributeUsage(AttributeTargets.Class)]
	public class ModKeyNameAttribute : Attribute
	{
		string _name;

		public ModKeyNameAttribute (string name)
		{
			_name = name;
		}
		public string Name => _name;
	}

	

	public class Mod
	{
		
	}

}