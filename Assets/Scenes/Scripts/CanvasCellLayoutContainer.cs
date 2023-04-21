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
	public class CanvasCellLayoutContainer : MonoBehaviour, ICellLayoutContainer<RectTransform>
	{
		public RectTransform Self { get; private set; }
		public IColorCell<RectTransform>[] Children { get; private set; }
		
		private ShuffleType _shuffleType;

		private CellsManager _cellsManager;
		private GridLayoutGroup _gridLayout;

		private void Awake()
		{
			_cellsManager ??= FindObjectOfType<CellsManager>();
			_gridLayout = GetComponent<GridLayoutGroup>();
			Self = GetComponent<RectTransform>();
			Children = GetComponentsInChildren<IColorCell<RectTransform>>().ToArray();
		}

		private void OnEnable()
		{
			_cellsManager ??= FindObjectOfType<CellsManager>();
			_cellsManager.OnLayoutChanged += UpdateLayout;
			_cellsManager.OnShuffleTypeChanged += SetShuffleType;
		}
		private void OnDisable()
		{
			_cellsManager.OnLayoutChanged -= UpdateLayout;
			_cellsManager.OnShuffleTypeChanged -= SetShuffleType;
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
				yield return new WaitForSeconds(_cellsManager.WaitTime);
				Children = _shuffleType switch
				{
					ShuffleType.ShuffleLeft => Children.ShiftLeft().ToArray(),
					ShuffleType.ShuffleRight => Children.ShiftRight().ToArray(),
					ShuffleType.ShuffleRandom => Children.Shuffle().ToArray(),
					_ => throw new ArgumentOutOfRangeException()
				};

				for (var i = 0; i < Children.Length; i++)
					Children.ElementAt(i).Transform.SetSiblingIndex(i);

				yield return new WaitForEndOfFrame();
			}
		}

	}
}
