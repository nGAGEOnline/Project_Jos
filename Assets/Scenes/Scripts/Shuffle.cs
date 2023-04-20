using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scenes.Scripts
{
	public class Shuffle : MonoBehaviour
	{
		[SerializeField] private bool _shiftRight = true;
		[SerializeField] private bool _useRandom = false;

		[SerializeField][Range(0.1f, 2f)] private float _waitTime = 0.5f;
		[SerializeField] private bool _looping = true;
		
		private RectTransform[] _children;

		// Start is called before the first frame update
		void Start()
		{
			_children = GetComponentsInChildren<RectTransform>();
			// remove self from _children
			_children = _children.Where(x => x != GetComponent<RectTransform>()).ToArray();

			StartCoroutine(UpdatePositionsCoroutine());
		}



		private IEnumerator UpdatePositionsCoroutine()
		{
			while (_looping)
			{
				yield return new WaitForSeconds(_waitTime);
				_children = (_useRandom
						? ShuffleRandom(_children)
						: ShiftArray(_children, _shiftRight))
					.ToArray();

				UpdatePositions(_children);
				yield return new WaitForEndOfFrame();
			}
		}

		private void UpdatePositions(IEnumerable<RectTransform> result)
		{

			for (int i = 0; i < result.Count(); i++)
			{
				result.ElementAt(i).gameObject.SetActive(true);
				// result.ElementAt(i).position = new Vector3(i, 0, 0);
				Vector2 newPosition = Vector2.zero;
				switch (i)
				{
					case 0:
						newPosition = new Vector2(50, -50);
						break;
					case 1:
						newPosition = new Vector2(160, -50);
						break;
					case 2:
						newPosition = new Vector2(50, -160);
						break;
					case 3:
						newPosition = new Vector2(160, -160);
						break;
				}
				result.ElementAt(i).anchoredPosition = newPosition;
			}

			_children = result.ToArray();
		}

		public static IEnumerable<T> ShiftArray<T>(IReadOnlyList<T> arr, bool shiftRight)
		{
			int length = arr.Count;
			T[] temp = new T[length];

			for (int i = 0; i < length; i++)
			{
				int newPos = i;
				if (shiftRight)
				{
					newPos = (i + 1) % length;
				}
				else
				{
					newPos = (i - 1 + length) % length;
				}
				temp[newPos] = arr.ElementAt(i);
			}
			return temp;
		}

		public static IEnumerable<T> ShuffleRandom<T>(IReadOnlyList<T> arr)
		{
			// Create a new list from the input array
			List<T> tempList = new List<T>(arr);

			// Shuffle the list using Fisher-Yates algorithm
			for (var i = tempList.Count - 1; i >= 1; i--)
			{
				var j = Random.Range(0, i + 1);
				(tempList[i], tempList[j]) = (tempList[j], tempList[i]);
			}

			return tempList;
		}
	}
}
