namespace QuickCombat
{
	public class NationPanelsHandler
	{
		public NationPanelsHandler ()
		{
			for (int i = 0; i < M.NationPanels.Length; i++)
			{
				NationPanel panel = M.NationPanels[i];
				panel.Init();
			}
		}
	}

}