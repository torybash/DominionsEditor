using System;
using System.IO;
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
            var fileName = Path.GetFileName(pretender.filePath);
            fileText.text = fileName;
        }
    }

}