using System.IO;

public struct MapFile
{
	public string path;
	public string name;
	public string folder;

	public static MapFile LoadPath (string mapPath)
	{
		MapFile mapFile = new MapFile();
		mapFile.path   = mapPath;
		mapFile.name   = Path.GetFileName(mapPath);
		mapFile.folder = Path.GetDirectoryName(mapPath);

		return mapFile;
	}

	public override string ToString ()
	{
		return $"{nameof(path)}: {path}, {nameof(name)}: {name}, {nameof(folder)}: {folder}";
	}
}