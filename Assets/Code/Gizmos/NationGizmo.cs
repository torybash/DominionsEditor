using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class NationGizmo : Gizmo
{
	[SerializeField] private Image flagImage;
	[SerializeField] private TMP_Text nameLabel;
		
		
	public int NationNum { get; private set; }
	
	public void Initialize (int specStartNationNum)
	{
		NationNum = specStartNationNum;
		
		var entry = Man.Nations.GetNationEntry(specStartNationNum);
		flagImage.sprite = entry.Sprite;
		nameLabel.text = entry.Name;
	}
}