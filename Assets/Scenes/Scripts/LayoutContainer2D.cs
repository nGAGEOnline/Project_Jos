using System;
using System.Collections;
using System.Linq;
using Scenes.Enums;
using Scenes.Helpers;
using Scenes.Interfaces;
using UnityEngine;

namespace Scenes
{
	public class LayoutContainer2D : MonoBehaviour, ILayoutContainer<Transform>
	{
		public Transform Self { get; private set; }
		public Transform[] Children { get; private set; }
		public Coroutine Coroutine { get; private set; }
		
		private ShuffleType _shuffleType;
		private ShuffleController _shuffleController;
		private Vector2[] _targetPositions;

		private void Awake()
		{
			_shuffleController ??= FindObjectOfType<ShuffleController>();
			Self = GetComponent<Transform>();
			Children = GetComponentsInChildren<Transform>()
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
				yield return new WaitForSeconds(_shuffleController.WaitTime);
				
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
					child.SetSiblingIndex(i);
					var targetPosition = _targetPositions[i];

					StartCoroutine(EaseToPosition(child, targetPosition, _shuffleController.TransitionTime));
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
