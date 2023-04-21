using System;
using Scenes.Enums;
using Scenes.Interfaces;
using UnityEngine;

namespace Scenes
{
	public class ShuffleController : MonoBehaviour
	{
		public event Action<BoxLayout> OnLayoutChanged;
		public event Action<ShuffleType> OnShuffleTypeChanged;
		
		[Header("Settings")]
		[SerializeField] private BoxLayout _boxLayout = BoxLayout.Block;
		[SerializeField] private ShuffleType _shuffleType = ShuffleType.ShuffleRight;
		[SerializeField][Range(0.1f, 2f)] private float _waitTime = 0.5f;
		private float _lastWaitTime;
		
		private ILayoutContainer _layoutContainer;
		
		private Coroutine _coroutine;

		private void Awake()
		{
			_layoutContainer = GetComponentInChildren<ILayoutContainer>();
		}

		private void Start()
		{
			OnLayoutChanged?.Invoke(_boxLayout);	
			OnShuffleTypeChanged?.Invoke(_shuffleType);
			
			_coroutine = StartCoroutine(_layoutContainer.ReorderCoroutine());
		}

		private void OnDisable()
		{
			StopCoroutine(_coroutine);
			_coroutine = null;
		}

		private void Update()
		{
			if (Math.Abs(_lastWaitTime - _waitTime) > 0.01f)
			{
				_layoutContainer.WaitTime = _waitTime;
				_lastWaitTime = _waitTime;
			}
			
			if (Input.GetKeyDown(KeyCode.Space))
				ToggleCoroutine();
			
			if (Input.GetKeyDown(KeyCode.UpArrow))
				NextLayout();
			if (Input.GetKeyDown(KeyCode.DownArrow))
				PreviousLayout();
			if (Input.GetKeyDown(KeyCode.LeftArrow))
				PreviousShuffleType();
			if (Input.GetKeyDown(KeyCode.RightArrow))
				NextShuffleType();
		}

		private void ToggleCoroutine()
		{
			if (_coroutine != null)
			{
				StopCoroutine(_coroutine);
				_coroutine = null;
			}
			else
			{
				_coroutine = StartCoroutine(_layoutContainer.ReorderCoroutine());
			}
		}

		public void PreviousLayout()
		{
			_boxLayout = _boxLayout switch
			{
				BoxLayout.Block => BoxLayout.Vertical,
				BoxLayout.Horizontal => BoxLayout.Block,
				BoxLayout.Vertical => BoxLayout.Horizontal,
				_ => throw new ArgumentOutOfRangeException()
			};
			OnLayoutChanged?.Invoke(_boxLayout);
		}
		public void NextLayout()
		{
			_boxLayout = _boxLayout switch
			{
				BoxLayout.Block => BoxLayout.Horizontal,
				BoxLayout.Horizontal => BoxLayout.Vertical,
				BoxLayout.Vertical => BoxLayout.Block,
				_ => throw new ArgumentOutOfRangeException()
			};
			OnLayoutChanged?.Invoke(_boxLayout);
		}

		public void PreviousShuffleType()
		{
			_shuffleType = _shuffleType switch
			{
				ShuffleType.ShuffleLeft => ShuffleType.ShuffleRandom,
				ShuffleType.ShuffleRight => ShuffleType.ShuffleLeft,
				ShuffleType.ShuffleRandom => ShuffleType.ShuffleRight,
				_ => throw new ArgumentOutOfRangeException()
			};
			OnShuffleTypeChanged?.Invoke(_shuffleType);
		}
		public void NextShuffleType()
		{
			_shuffleType = _shuffleType switch
			{
				ShuffleType.ShuffleLeft => ShuffleType.ShuffleRight,
				ShuffleType.ShuffleRight => ShuffleType.ShuffleRandom,
				ShuffleType.ShuffleRandom => ShuffleType.ShuffleLeft,
				_ => throw new ArgumentOutOfRangeException()
			};
			OnShuffleTypeChanged?.Invoke(_shuffleType);
		}
		public void SetShuffleType(string shuffleType)
		{
			
			_shuffleType =  shuffleType.ToLower() switch
			{
				"left" => ShuffleType.ShuffleLeft,
				"right" => ShuffleType.ShuffleRight,
				"random" => ShuffleType.ShuffleRandom,
				_ => _shuffleType
			};
			OnShuffleTypeChanged?.Invoke(_shuffleType);
		}
	}
}
