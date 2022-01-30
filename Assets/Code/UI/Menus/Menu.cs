using UnityEngine;

namespace UI.Menus
{

    public abstract class Menu : MonoBehaviour
    {
        public RectTransform RectTrans => GetComponent<RectTransform>();
    
        public virtual void Show ()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide ()
        {
            gameObject.SetActive(false);
        }
    }

}