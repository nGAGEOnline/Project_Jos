using Scenes.Enums;
using TMPro;
using UnityEngine;

namespace Scenes
{

	public class LayoutUI : MonoBehaviour
	{
		[Header("UI Elements")]		
		[SerializeField] private TMP_Text _layoutText;

		private CellsManager _cellsManager;

		private void Awake() => _cellsManager = FindObjectOfType<CellsManager>();

		private void OnEnable() => _cellsManager.OnLayoutChanged += UpdateLayoutText;
		private void OnDisable() => _cellsManager.OnLayoutChanged -= UpdateLayoutText;

		private void UpdateLayoutText(LayoutStyle layoutStyle) => _layoutText.text = layoutStyle.ToString();
	}
}
