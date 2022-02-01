using Core.Entities;

namespace Data
{

	public class ExperienceData : IEntityData
	{
		public int amount;

		public ExperienceData (int amount)
		{
			this.amount = amount;
		}
	}

}