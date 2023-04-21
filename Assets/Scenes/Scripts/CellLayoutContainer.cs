using System;
using System.Collections;
using System.Linq;
using Scenes.Enums;
using Scenes.Helpers;
using Scenes.Interfaces;
using UnityEngine;

namespace Scenes
{
	public class CellLayoutContainer : MonoBehaviour, ICellLayoutContainer<Transform>
	{
		public Transform Self { get; private set; }
		public IColorCell<Transform>[] Children { get; private set; }
		
		private ShuffleType _shuffleType;
		private CellsManager _cellsManager;
		private Vector2[] _targetPositions;

		private void Awake()
		{
			_cellsManager ??= FindObjectOfType<CellsManager>();
			Self = GetComponent<Transform>();
			Children = GetComponentsInChildren<IColorCell<Transform>>().ToArray();
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
			_targetPositions = layoutStyle switch
			{
				LayoutStyle.Block => new[] { new Vector2(-.5f, .5f), new Vector2(.5f, .5f), new Vector2(.5f, -.5f), new Vector2(-.5f, -.5f) },
				LayoutStyle.Horizontal => new[] { new Vector2(-1.5f, 0), new Vector2(-.5f, 0), new Vector2(.5f, 0), new Vector2(1.5f, 0) },
				LayoutStyle.Vertical => new[] { new Vector2(0, 1.5f), new Vector2(0, .5f), new Vector2(0, -.5f), new Vector2(0, -1.5f) },
				_ => throw new ArgumentOutOfRangeException(nameof(layoutStyle), layoutStyle, null)
			};
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
				{
					var child = Children.ElementAt(i);
					child.SpriteRenderer.sortingOrder = i;
					child.Transform.SetSiblingIndex(i);
					var targetPosition = _targetPositions[i];

					StartCoroutine(EaseToPosition(child.Transform, targetPosition, _cellsManager.TransitionTime));
				}
				
				yield return null;
			}
		}

		private static IEnumerator EaseToPosition(Transform child, Vector2 targetPosition, float duration)
		{
			Vector2 startPosition = child.localPosition;
			var startTime = Time.time;

			while (Time.time - startTime < duration)
			{
				var t = (Time.time - startTime) / duration;
				var easeInOutT = Mathf.SmoothStep(0, 1, t);
				child.localPosition = Vector2.Lerp(startPosition, targetPosition, easeInOutT);
				yield return null;
			}

			child.localPosition = targetPosition;
		}

		private static IEnumerator LerpToPosition(Transform child, Vector2 targetPosition, float duration)
		{
			Vector2 startPosition = child.localPosition;
			var startTime = Time.time;

			while (Time.time - startTime < duration)
			{
				var t = (Time.time - startTime) / duration;
				child.localPosition = Vector2.Lerp(startPosition, targetPosition, t);
				yield return null;
			}

			child.localPosition = targetPosition;
		}

	}
}
