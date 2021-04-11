using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NationGizmo : Gizmo
{
	[SerializeField] private Image flagImage;
	[SerializeField] private TMP_Text nameLabel;

	public int NationNum { get; private set; }
	
	public void SetPlayerNumber (int nationNum)
	{
		NationNum = nationNum;
		
		var entry = Man.Nations.GetNationEntry(nationNum);
		flagImage.sprite = entry.Sprite;
		nameLabel.text = entry.Name;
	}
}