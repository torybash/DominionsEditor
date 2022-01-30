using Dom;

namespace Core.Entities
{

	public class MagicOverride
	{
		public MagicPath Path       { get; set; }
		public int       MagicValue { get; set; }
	
		public MagicOverride (MagicPath path, int value)
		{
			Path       = path;
			MagicValue = value;
		}
	}

}