using UnityEngine;
public abstract class Gizmo : MonoBehaviour
{
	public MapManager Man { get; set; }

	protected RectTransform RectTrans => GetComponent<RectTransform>();
	
	public void InitializeTemplate (MapManager mapMan)
	{
		gameObject.SetActive(false);
	}
}