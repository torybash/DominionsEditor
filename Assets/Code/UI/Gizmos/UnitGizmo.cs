using Map.MapData;

namespace UI.Gizmos
{

	public class UnitGizmo : MonsterGizmo<Unit>
	{
		public override void SetData (Monster data)
		{
			base.SetData(data);
		
			nameLabel.text += $" ({Data.Amount})";
		}

	}

}