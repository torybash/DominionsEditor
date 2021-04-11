using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    protected MapManager Man { get; private set; }

    public virtual void Initialize (MapManager manager)
    {
        Man = manager;
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