using UnityEngine;
public abstract class Gizmo : MonoBehaviour
{
	public RectTransform RectTrans => GetComponent<RectTransform>();
}