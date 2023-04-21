using Scenes.Enums;
using TMPro;
using UnityEngine;

namespace Scenes
{

	public class LayoutUI : MonoBehaviour
	{
		[Header("UI Elements")]		
		[SerializeField] private TMP_Text _layoutText;

		private ShuffleController _shuffleController;

		private void Awake() => _shuffleController = FindObjectOfType<ShuffleController>();

		private void OnEnable() => _shuffleController.OnLayoutChanged += UpdateLayoutText;
		private void OnDisable() => _shuffleController.OnLayoutChanged -= UpdateLayoutText;

		private void UpdateLayoutText(BoxLayout layout) => _layoutText.text = layout.ToString();
	}
}
