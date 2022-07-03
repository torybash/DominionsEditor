namespace QuickCombat
{
	public class ItemPanelHandler
	{
		readonly Session    _session;
		readonly ItemsPanel _menuItemPanel;

		public ItemPanelHandler (Session session, ItemsPanel menuItemPanel)
		{
			_session       = session;
			_menuItemPanel = menuItemPanel;
		}
	}
}