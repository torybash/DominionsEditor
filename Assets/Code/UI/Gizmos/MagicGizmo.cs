using Core;
using Map.MapData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gizmos
{

	public class MagicGizmo : Gizmo
	{
		[SerializeField] private Image    icon;
		[SerializeField] private TMP_Text valueLabel;

		public MagicOverride  Magic      { get; private set; }
		public CommanderGizmo OwnerGizmo { get; set; }

		public void Initialize (CommanderGizmo owner, MagicOverride magic)
		{
			OwnerGizmo = owner;
			Magic      = magic;
		
			var itemEntry = DomEdit.I.magicPaths.GetEntry(Magic.Path);
			icon.sprite     = itemEntry.Sprite;
			valueLabel.text = magic.MagicValue.ToString();
		}
	}

}