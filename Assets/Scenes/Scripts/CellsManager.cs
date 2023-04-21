using System;
using Scenes.Enums;
using Scenes.Interfaces;
using UnityEngine;

namespace Scenes
{
	public class CellsManager : MonoBehaviour
	{
		public event Action<LayoutStyle> OnLayoutChanged;
		public event Action<ShuffleType> OnShuffleTypeChanged;
		public float WaitTime => _waitTime;
		public float TransitionTime => _transitionTime;

		[Header("Settings")]
		[SerializeField] private LayoutStyle _layoutStyle = LayoutStyle.Block;
		[SerializeField] private ShuffleType _shuffleType = ShuffleType.ShuffleRight;
		[SerializeField][Range(0.1f, 2f)] private float _waitTime = 0.5f;
		[SerializeField][Range(0.1f, 2f)] private float _transitionTime = 0.5f;
		private float _lastWaitTime;
		
		private ILayoutContainer _layoutContainer;
		
		private Coroutine _coroutine;

		private void Awake()
		{
			_layoutContainer = GetComponentInChildren<ILayoutContainer>();
		}

		private void Start()
		{
			OnLayoutChanged?.Invoke(_layoutStyle);	
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
			_layoutStyle = _layoutStyle switch
			{
				LayoutStyle.Block => LayoutStyle.Vertical,
				LayoutStyle.Horizontal => LayoutStyle.Block,
				LayoutStyle.Vertical => LayoutStyle.Horizontal,
				_ => throw new ArgumentOutOfRangeException()
			};
			OnLayoutChanged?.Invoke(_layoutStyle);
		}
		public void NextLayout()
		{
			_layoutStyle = _layoutStyle switch
			{
				LayoutStyle.Block => LayoutStyle.Horizontal,
				LayoutStyle.Horizontal => LayoutStyle.Vertical,
				LayoutStyle.Vertical => LayoutStyle.Block,
				_ => throw new ArgumentOutOfRangeException()
			};
			OnLayoutChanged?.Invoke(_layoutStyle);
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
