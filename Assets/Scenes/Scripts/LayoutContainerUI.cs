using System;
using System.Collections;
using System.Linq;
using Scenes.Enums;
using Scenes.Helpers;
using Scenes.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes
{
	public class LayoutContainerUI : MonoBehaviour, ILayoutContainer
	{
		public float WaitTime { get; set; } = 1f;

		private RectTransform _self;
		private RectTransform[] _children;
		private Coroutine _coroutine;
		private ShuffleType _shuffleType;

		private ShuffleController _shuffleController;
		private GridLayoutGroup _gridLayout;

		private void Awake()
		{
			_shuffleController ??= FindObjectOfType<ShuffleController>();
			_gridLayout = GetComponent<GridLayoutGroup>();
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

		public IEnumerator ReorderCoroutine()
		{
			while (true)
			{
				yield return new WaitForSeconds(WaitTime);
				_children = _shuffleType switch
				{
					ShuffleType.ShuffleLeft => _children.ShiftLeft().ToArray(),
					ShuffleType.ShuffleRight => _children.ShiftRight().ToArray(),
					ShuffleType.ShuffleRandom => _children.Shuffle().ToArray(),
					_ => throw new ArgumentOutOfRangeException()
				};

				for (var i = 0; i < _children.Length; i++)
					_children.ElementAt(i).SetSiblingIndex(i);

				yield return new WaitForEndOfFrame();
			}
		}

		private void UpdateLayout(BoxLayout layout)
		{
			_self.sizeDelta = layout switch
			{
				BoxLayout.Block => new Vector2(210, 210),
				BoxLayout.Horizontal => new Vector2(430, 100),
				BoxLayout.Vertical => new Vector2(100, 430),
				_ => throw new ArgumentOutOfRangeException()
			};

			// Calculate the layout input for the horizontal and vertical axes
			_gridLayout.CalculateLayoutInputHorizontal();
			_gridLayout.CalculateLayoutInputVertical();

			// Force the layout to update
			LayoutRebuilder.ForceRebuildLayoutImmediate(_self);
		}
	}
}
