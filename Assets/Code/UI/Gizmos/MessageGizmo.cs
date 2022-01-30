using TMPro;
using UnityEngine;

namespace UI.Gizmos
{

	public class MessageGizmo : Gizmo
	{
		[SerializeField] private TMP_Text label;

		public void Write (string message)
		{
			label.text = message;
			Invoke(nameof(SelfDestruct), 1f);
		}

		private void SelfDestruct ()
		{
			Destroy(gameObject);
		}
	}

}