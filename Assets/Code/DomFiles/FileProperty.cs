using System.Collections.Generic;

namespace Map
{

	public class FileProperty
	{
		public string       Key  { get; set; }
		public List<string> Args { get; set; } = new List<string>();

		public FileProperty (string key)
		{
			Key = key;
		}
		
		public void AddArg (string arg)
		{
			arg = arg.Trim('"');

			Args.Add(arg);
		}

		public override string ToString ()
		{
			return $"{Key}: {string.Join(", ", Args)}";
		}
	}

}