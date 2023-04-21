using System;
using Scenes.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes
{
	public class ShuffleTypeUI : MonoBehaviour
	{
		[Header("UI Elements")]		
		[SerializeField] private TMP_Text _leftText;
		[SerializeField] private TMP_Text _randomText;
		[SerializeField] private TMP_Text _rightText;
		[SerializeField] private Image _leftBg;
		[SerializeField] private Image _randomBg;
		[SerializeField] private Image _rightBg;

		[SerializeField] private Color _activeColor;
		[SerializeField] private Color _inactiveColor;
		[SerializeField] private Color _activeBackgroundColor;
		[SerializeField] private Color _inactiveBackgroundColor;

		private CellsManager _cellsManager;

		private void Awake() => _cellsManager = FindObjectOfType<CellsManager>();

		private void OnEnable() => _cellsManager.OnShuffleTypeChanged += UpdateText;
		private void OnDisable() => _cellsManager.OnShuffleTypeChanged -= UpdateText;

		private void UpdateText(ShuffleType shuffleType)
		{
			switch (shuffleType)
			{
				case ShuffleType.ShuffleLeft:
					_leftText.color = _activeColor;
					_leftBg.color = _activeBackgroundColor;
					_randomText.color = _inactiveColor;
					_randomBg.color = _inactiveBackgroundColor;
					_rightText.color = _inactiveColor;
					_rightBg.color = _inactiveBackgroundColor;
					break;
				case ShuffleType.ShuffleRandom:
					_leftText.color = _inactiveColor;
					_leftBg.color = _inactiveBackgroundColor;
					_randomText.color = _activeColor;
					_randomBg.color = _activeBackgroundColor;
					_rightText.color = _inactiveColor;
					_rightBg.color = _inactiveBackgroundColor;
					break;
				case ShuffleType.ShuffleRight:
					_leftText.color = _inactiveColor;
					_leftBg.color = _inactiveBackgroundColor;
					_randomText.color = _inactiveColor;
					_randomBg.color = _inactiveBackgroundColor;
					_rightText.color = _activeColor;
					_rightBg.color = _activeBackgroundColor;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(shuffleType), shuffleType, null);
			}
		}
	}
}
