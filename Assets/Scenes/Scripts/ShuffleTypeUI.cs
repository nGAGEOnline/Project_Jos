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

		private ShuffleController _shuffleController;

		private void Awake() => _shuffleController = FindObjectOfType<ShuffleController>();

		private void OnEnable() => _shuffleController.OnShuffleTypeChanged += UpdateText;
		private void OnDisable() => _shuffleController.OnShuffleTypeChanged -= UpdateText;

		private void UpdateText(ShuffleType shuffleType)
		{
			switch (shuffleType)
			{
				case ShuffleType.ShuffleLeft:
					_leftBg.color = _activeColor;
					_randomBg.color = _inactiveColor;
					_rightBg.color = _inactiveColor;
					_leftText.color = Color.white;
					_randomText.color = Color.gray;
					_rightText.color = Color.gray;
					break;
				case ShuffleType.ShuffleRight:
					_leftBg.color = _inactiveColor;
					_randomBg.color = _inactiveColor;
					_rightBg.color = _activeColor;
					_leftText.color = Color.gray;
					_randomText.color = Color.gray;
					_rightText.color = Color.white;
					break;
				case ShuffleType.ShuffleRandom:
					_leftBg.color = _inactiveColor;
					_randomBg.color = _activeColor;
					_rightBg.color = _inactiveColor;
					_leftText.color = Color.gray;
					_randomText.color = Color.white;
					_rightText.color = Color.gray;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(shuffleType), shuffleType, null);
			}
		}
	}
}
