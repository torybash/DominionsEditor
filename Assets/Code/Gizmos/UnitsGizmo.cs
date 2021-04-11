using System.Collections;
using System.Collections.Generic;

public class UnitsGizmo : MonsterGizmo
{
	public override void Initialize (MonsterElement monsterElement)
	{
		base.Initialize(monsterElement);

		UnitsElement = (Units) monsterElement;
		
		nameLabel.text += $" ({UnitsElement.Amount})";
	}
	public Units UnitsElement { get; set; }

}