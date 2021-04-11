using UnityEngine;
public abstract class Gizmo : MonoBehaviour
{
	protected RectTransform RectTrans => GetComponent<RectTransform>();
	public MapManager Man { get; private set; }
	public UiManager Ui { get; private set;}
	public GameSetup Game { get; private set;}

	public void Init (MapManager mapMan, UiManager uiManager, GameSetup gameSetup)
	{
		Man = mapMan;
		Ui = uiManager;
		Game = gameSetup;
	}
}