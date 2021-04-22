public class Item
{
	public string ItemName { get; set; }
	
	public static Item Create (string itemName)
	{
		return new Item { ItemName = itemName};
	}
}