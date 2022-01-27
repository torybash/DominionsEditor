using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    public virtual void Show ()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide ()
    {
        gameObject.SetActive(false);
    }
}