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
	public class LayoutContainer : MonoBehaviour, ILayoutContainer<RectTransform>
	{
		public float WaitTime { get; set; } = 1f;

		public RectTransform Self { get; private set; }
		public RectTransform[] Children { get; private set; }
		public Coroutine Coroutine { get; private set; }
		
		private ShuffleType _shuffleType;

		private ShuffleController _shuffleController;
		private GridLayoutGroup _gridLayout;

		private void Awake()
		{
			_shuffleController ??= FindObjectOfType<ShuffleController>();
			_gridLayout = GetComponent<GridLayoutGroup>();
			Self = GetComponent<RectTransform>();
			Children = GetComponentsInChildren<RectTransform>()
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

		private void UpdateLayout(LayoutStyle layoutStyle)
		{
			Self.sizeDelta = layoutStyle switch
			{
				LayoutStyle.Block => new Vector2(210, 210),
				LayoutStyle.Horizontal => new Vector2(430, 100),
				LayoutStyle.Vertical => new Vector2(100, 430),
				_ => throw new ArgumentOutOfRangeException()
			};

			// Calculate the layout input for the horizontal and vertical axes
			_gridLayout.CalculateLayoutInputHorizontal();
			_gridLayout.CalculateLayoutInputVertical();

			// Force the layout to update
			LayoutRebuilder.ForceRebuildLayoutImmediate(Self);
		}

		private void SetShuffleType(ShuffleType shuffleType) => _shuffleType = shuffleType;

		public IEnumerator ReorderCoroutine()
		{
			while (true)
			{
				yield return new WaitForSeconds(WaitTime);
				Children = _shuffleType switch
				{
					ShuffleType.ShuffleLeft => Children.ShiftLeft().ToArray(),
					ShuffleType.ShuffleRight => Children.ShiftRight().ToArray(),
					ShuffleType.ShuffleRandom => Children.Shuffle().ToArray(),
					_ => throw new ArgumentOutOfRangeException()
				};

				for (var i = 0; i < Children.Length; i++)
					Children.ElementAt(i).SetSiblingIndex(i);

				yield return new WaitForEndOfFrame();
			}
		}

	}
}
