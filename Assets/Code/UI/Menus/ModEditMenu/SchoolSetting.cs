using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{

	public class SchoolSetting : MonoBehaviour
	{
		[SerializeField] private TMP_Text schoolLabel;
		[SerializeField] private TMP_Text schoolLevel;
		[SerializeField] private Button   increaseButton;
		[SerializeField] private Button   decreaseButton;

		private int level;

		public MagicSchool School { get; set; }
		public int Level
		{
			get => level;
			set
			{
				level            = value;
				schoolLevel.text = level.ToString();
			}
		}

		private void Awake ()
		{
			increaseButton.onClick.AddListener(() => Level = Mathf.Clamp(Level + 1, 0, 8));
			decreaseButton.onClick.AddListener(() => Level = Mathf.Clamp(Level - 1, 0, 8));
		}

		public void Initialize (MagicSchool magicSchool)
		{
			School           = magicSchool;
			schoolLabel.text = magicSchool.name;
		}

	}

}