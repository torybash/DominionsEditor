using UnityEngine;
using UnityEngine.UI;

public class EntryGizmo : MonoBehaviour
{
	[SerializeField] private Image image;

	public SearchableEntry Entry { get; set; }
	
	public void SetEntry (SearchableEntry entry)
	{
		Entry = entry;

		image.sprite = entry.Sprite;
	}
}
