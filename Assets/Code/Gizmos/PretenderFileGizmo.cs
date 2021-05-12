using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PretenderFileGizmo : Gizmo
{
    public event Action LoadClicked;
    
    [SerializeField] private TMP_Text fileText;
    [SerializeField] private Button loadButton;

    private void Awake ()
    {
        loadButton.onClick.AddListener(() => LoadClicked?.Invoke());
    }
    public void Initialize (Pretender pretender)
    {
        fileText.text = pretender.FileName;
    }
}
