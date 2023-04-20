using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Scenes.Scripts
{
	public enum ShuffleType
	{
		ShuffleLeft,
		ShuffleRight,
		ShuffleRandom
	}
	public enum ShuffleLayout
	{
		Block,
		Horizontal,
		Vertical
	}
	
	public class Shuffle : MonoBehaviour
	{
		private ShuffleLayout ShuffleLayout
		{
			get => _shuffleLayout;
			set
			{
				_shuffleLayout = value;
				UpdateLayoutText();
				UpdateLayout();
			}
		}
		private ShuffleType ShuffleType
		{
			get => _shuffleType;
			set
			{
				_shuffleType = value;
				UpdateShuffleStyleText();
			}
		}

		[SerializeField] private ShuffleLayout _shuffleLayout = ShuffleLayout.Block;
		[SerializeField] private TMP_Text _shuffleLayoutText;
		[SerializeField] private ShuffleType _shuffleType = ShuffleType.ShuffleRight;
		[SerializeField] private TMP_Text _shuffleTypeText;

		[SerializeField][Range(0.1f, 2f)] private float _waitTime = 0.5f;
		
		private bool _looping = true;
		
		private Vector2 _blockDimensions = new Vector2(210, 210);
		private Vector2 _horizontalDimensions = new Vector2(430, 100);
		private Vector2 _verticalDimensions = new Vector2(100, 430);

		private RectTransform _self;
		private RectTransform[] _children;
		private Coroutine _coroutine;

		private void Start()
		{
			_self = GetComponent<RectTransform>();
			_children = GetComponentsInChildren<RectTransform>()
				.Where(x => x != transform)
				.ToArray();
			
			UpdateLayoutText();
			UpdateLayout();
			UpdateShuffleStyleText();
			_coroutine = StartCoroutine(ReorderCoroutine());
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				ToggleCoroutine();
		}
		
		private void OnDisable()
		{
			StopCoroutine(_coroutine);
			_coroutine = null;
		}
		
		public void NextShuffle()
		{
			ShuffleType = ShuffleType switch
			{
				ShuffleType.ShuffleLeft => ShuffleType.ShuffleRight,
				ShuffleType.ShuffleRight => ShuffleType.ShuffleRandom,
				ShuffleType.ShuffleRandom => ShuffleType.ShuffleLeft,
				_ => throw new ArgumentOutOfRangeException()
			};
		}
		public void PreviousShuffle()
		{
			ShuffleType = ShuffleType switch
			{
				ShuffleType.ShuffleLeft => ShuffleType.ShuffleRandom,
				ShuffleType.ShuffleRight => ShuffleType.ShuffleLeft,
				ShuffleType.ShuffleRandom => ShuffleType.ShuffleRight,
				_ => throw new ArgumentOutOfRangeException()
			};
		}
		private void UpdateShuffleStyleText()
			=> _shuffleTypeText.text = ShuffleType.ToString();
		
		public void NextLayout()
		{
			ShuffleLayout = ShuffleLayout switch
			{
				ShuffleLayout.Block => ShuffleLayout.Horizontal,
				ShuffleLayout.Horizontal => ShuffleLayout.Vertical,
				ShuffleLayout.Vertical => ShuffleLayout.Block,
				_ => throw new ArgumentOutOfRangeException()
			};
		}
		public void PreviousLayout()
		{
			ShuffleLayout = ShuffleLayout switch
			{
				ShuffleLayout.Block => ShuffleLayout.Vertical,
				ShuffleLayout.Horizontal => ShuffleLayout.Block,
				ShuffleLayout.Vertical => ShuffleLayout.Horizontal,
				_ => throw new ArgumentOutOfRangeException()
			};
		}
		private void UpdateLayoutText() 
			=> _shuffleLayoutText.text = _shuffleLayout.ToString();

		private void UpdateLayout()
		{
			_self.sizeDelta = _shuffleLayout switch
			{
				ShuffleLayout.Block => _blockDimensions,
				ShuffleLayout.Horizontal => _horizontalDimensions,
				ShuffleLayout.Vertical => _verticalDimensions,
				_ => throw new ArgumentOutOfRangeException()
			};
			
			// Get the GridLayoutGroup component
			var gridLayout = GetComponent<GridLayoutGroup>();
    
			// Calculate the layout input for the horizontal and vertical axes
			gridLayout.CalculateLayoutInputHorizontal();
			gridLayout.CalculateLayoutInputVertical();
    
			// Force the layout to update
			LayoutRebuilder.ForceRebuildLayoutImmediate(_self);
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
				_coroutine = StartCoroutine(ReorderCoroutine());
			}
		}
		
		private IEnumerator ReorderCoroutine()
		{
			while (_looping)
			{
				yield return new WaitForSeconds(_waitTime);
				_children = _shuffleType switch
				{
					ShuffleType.ShuffleLeft => ShiftArray(_children).ToArray(),
					ShuffleType.ShuffleRight => ShiftArray(_children, true).ToArray(),
					ShuffleType.ShuffleRandom => ShuffleRandom(_children).ToArray(),
					_ => throw new ArgumentOutOfRangeException()
				};
				UpdatePositions(ref _children);
				
				yield return new WaitForEndOfFrame();
			}
		}

		private static void UpdatePositions(ref RectTransform[] result)
		{
			for (var i = 0; i < result.Length; i++)
				result.ElementAt(i).SetSiblingIndex(i);
		}

		private static IEnumerable<T> ShiftArray<T>(IReadOnlyCollection<T> sourceList, bool shiftRight = false)
		{
			var length = sourceList.Count;
			var result = new T[length];

			for (var i = 0; i < length; i++)
			{
				var newIndex = shiftRight ? 
					(i + 1) % length 
					: (i - 1 + length) % length;
				result[newIndex] = sourceList.ElementAt(i);
			}
			
			return result;
		}

		private static IEnumerable<T> ShuffleRandom<T>(IEnumerable<T> arr)
		{
			var tempList = new List<T>(arr);
			
			for (var i = tempList.Count - 1; i >= 1; i--)
			{
				var j = Random.Range(0, i + 1);
				(tempList[i], tempList[j]) = (tempList[j], tempList[i]);
			}

			return tempList;
		}
	}
}
