using System;
using System.Collections;
using System.Linq;
using Scenes.Enums;
using Scenes.Helpers;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Scenes
{
	public class BoxContainer : MonoBehaviour
	{
		private RectTransform _self;
		private RectTransform[] _children;
		private Coroutine _coroutine;
		private ShuffleType _shuffleType;
		
		private ShuffleController _shuffleController;

		private void Awake()
		{
			_shuffleController ??= FindObjectOfType<ShuffleController>();
			_self = GetComponent<RectTransform>();
			_children = GetComponentsInChildren<RectTransform>()
				.Where(x => x != transform)
				.ToArray();
		}

		private void OnEnable()
		{
			_shuffleController ??= FindObjectOfType<ShuffleController>();
			_shuffleController.OnLayoutChanged += UpdateLayout;
			_shuffleController.OnShuffleTypeChanged += SetShuffleType;
		}
		private void OnDisable()
		{
			_shuffleController.OnLayoutChanged -= UpdateLayout;
			_shuffleController.OnShuffleTypeChanged -= SetShuffleType;
		}

		private void SetShuffleType(ShuffleType shuffleType) => _shuffleType = shuffleType;

		public IEnumerator ReorderCoroutine(float waitTime)
		{
			while (true)
			{
				yield return new WaitForSeconds(waitTime);
				_children = _shuffleType switch
				{
					ShuffleType.ShuffleLeft => _children.ShiftLeft().ToArray(),
					ShuffleType.ShuffleRight => _children.ShiftRight().ToArray(),
					ShuffleType.ShuffleRandom => _children.OrderBy(x => Random.value).ToArray(),
					_ => throw new ArgumentOutOfRangeException()
				};
				
				for (var i = 0; i < _children.Length; i++)
					_children.ElementAt(i).SetSiblingIndex(i);
				
				yield return new WaitForEndOfFrame();
			}
		}

		public void UpdateLayout(BoxLayout layout)
		{
			_self.sizeDelta = layout switch
			{
				BoxLayout.Block => new Vector2(210, 210),
				BoxLayout.Horizontal => new Vector2(430, 100),
				BoxLayout.Vertical => new Vector2(100, 430),
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
	}
}
