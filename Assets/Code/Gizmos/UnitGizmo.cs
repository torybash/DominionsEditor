public class UnitGizmo : MonsterGizmo<Unit>
{
	public override void Initialize (Monster data)
	{
		base.Initialize(data);
		
		nameLabel.text += $" ({Data.Amount})";
	}

}