using Scenes.Enums;
using TMPro;
using UnityEngine;

namespace Scenes
{
	public class ShuffleControllerUI : MonoBehaviour
	{
		[Header("UI Elements")]		
		[SerializeField] private TMP_Text _shuffleTypeText;
		[SerializeField] private TMP_Text _layoutText;

		private ShuffleController _shuffleController;

		private void Awake() => _shuffleController = FindObjectOfType<ShuffleController>();

		private void OnEnable()
		{
			_shuffleController.OnLayoutChanged += UpdateLayoutText;
			_shuffleController.OnShuffleTypeChanged += UpdateShuffleTypeText;
		}

		private void OnDisable()
		{
			_shuffleController.OnLayoutChanged -= UpdateLayoutText;
			_shuffleController.OnShuffleTypeChanged -= UpdateShuffleTypeText;
		}

		private void UpdateLayoutText(BoxLayout layout) => _layoutText.text = layout.ToString();
		private void UpdateShuffleTypeText(ShuffleType type) => _shuffleTypeText.text = type.ToString();
	}
}
