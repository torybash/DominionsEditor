namespace Mod.ModElements
{

	public abstract class ModElement
	{

		public virtual void ParseArgs (string[] args)
		{
		}

		public virtual string[] SaveArgs ()
		{
			return new string[0];
		}

	}
	
	[ModKeyName("selectspell")]
	public class SelectSpell : ModElement
	{
		int      _spellId;
		public override void     ParseArgs (string[] args) => _spellId = int.Parse(args[0]);
		public override string[] SaveArgs ()               => new[] { _spellId.ToString() };
	}
	
	[ModKeyName("researchlevel")]
	public class ResearchLevel : ModElement
	{
		int      _level;
		public override void     ParseArgs (string[] args) => _level = int.Parse(args[0]);
		public override string[] SaveArgs ()               => new[] { _level.ToString() };
	}
	
	[ModKeyName("end")]
	public class End : ModElement {}
}