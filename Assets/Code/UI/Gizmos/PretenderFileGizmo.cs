using System;
using Dom;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gizmos
{

    public class PretenderFileGizmo : Gizmo
    {
        public event Action LoadClicked;
    
        [SerializeField] private TMP_Text fileText;
        [SerializeField] private Button   loadButton;

        private void Awake ()
        {
            loadButton.onClick.AddListener(() => LoadClicked?.Invoke());
        }
        public void Initialize (Pretender pretender)
        {
            fileText.text = pretender.fileName;
        }
    }

}