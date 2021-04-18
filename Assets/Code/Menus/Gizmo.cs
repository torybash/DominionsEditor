using UnityEngine;
public abstract class Gizmo : MonoBehaviour
{
	protected RectTransform RectTrans => GetComponent<RectTransform>();
	public MapManager Map { get; private set; }
	public UiManager Ui { get; private set;}
	public GameSetup Game { get; private set;}

	public void Init (MapManager mapMan, UiManager uiManager, GameSetup gameSetup)
	{
		Map = mapMan;
		Ui = uiManager;
		Game = gameSetup;
	}
}