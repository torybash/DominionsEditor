using System.IO;

namespace UI.Menus
{

	public struct DomFile
	{
		public string path;
		public string name;
		public string folder;

		public static DomFile LoadPath (string mapPath)
		{
			DomFile domFile = new DomFile();
			domFile.path   = mapPath;
			domFile.name   = Path.GetFileName(mapPath);
			domFile.folder = Path.GetDirectoryName(mapPath);

			return domFile;
		}

		public override string ToString ()
		{
			return $"{nameof(path)}: {path}, {nameof(name)}: {name}, {nameof(folder)}: {folder}";
		}
	}

}