﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scenes.Helpers
{
	public static class CollectionHelpers
	{
		public static IEnumerable<T> ShiftRight<T>(this IReadOnlyCollection<T> sourceList) 
			=> ShiftArray(sourceList, true);
		public static IEnumerable<T> ShiftLeft<T>(this IReadOnlyCollection<T> sourceList) 
			=> ShiftArray(sourceList, false);

		private static IEnumerable<T> ShiftArray<T>(IReadOnlyCollection<T> sourceList, bool shiftRight)
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

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			var enumerable = source as T[] ?? source.ToArray();
			if (!enumerable.Any())
				return enumerable;

			var originalList = new List<T>(enumerable);
			List<T> shuffledList;

			do
			{
				shuffledList = new List<T>(originalList.OrderBy(x => Random.value));
			} while (IsEqualToOriginalList(originalList, shuffledList));

			return shuffledList;
		}
		private static bool IsEqualToOriginalList<T>(IEnumerable<T> originalList, IEnumerable<T> shuffledList) 
			=> originalList.SequenceEqual(shuffledList);

		public static IEnumerable<T> ShuffleOld<T>(this IEnumerable<T> source)
		{
			var result = new List<T>(source.OrderBy(x => Random.value));
			
			for (var i = result.Count - 1; i >= 1; i--)
			{
				var j = Random.Range(0, i + 1);
				(result[i], result[j]) = (result[j], result[i]);
			}

			return result;
		}
	}
}
