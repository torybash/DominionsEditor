using UnityEngine;

namespace UI.Gizmos
{

	public abstract class Gizmo : MonoBehaviour
	{
		public RectTransform RectTrans => GetComponent<RectTransform>();
	}

}