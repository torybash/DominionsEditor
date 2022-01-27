using System.ComponentModel;
public enum ItemType
{
	OneHandedWeapon,
	TwoHandedWeapon, 
	Shield,
	Helmet,
	Crown,
	Armor,
	Boots,
	Misc,
}

public static class ItemTypeUtil
{
	public static ItemType GetItemType (string key)
	{
		return key switch
		{
			"1-h wpn" => ItemType.OneHandedWeapon,
			"2-h wpn" => ItemType.TwoHandedWeapon, 
			"shield" => ItemType.Shield,
			"helm" => ItemType.Helmet,
			"crown" => ItemType.Crown,
			"armor" => ItemType.Armor,
			"boots" => ItemType.Boots,
			"misc" => ItemType.Misc,
			_ => throw new InvalidEnumArgumentException()
		};
	}
}