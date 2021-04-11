using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    protected MapManager Map { get; private set; }
    protected UiManager Ui { get; private set; }
    protected GameSetup Game { get; set; }

    public virtual void Initialize (MapManager manager, UiManager uiManager, GameSetup setup)
    {
        Map = manager;
        Ui = uiManager;
        Game = setup;
    }

    public virtual void Show ()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide ()
    {
        gameObject.SetActive(false);
    }
    

}