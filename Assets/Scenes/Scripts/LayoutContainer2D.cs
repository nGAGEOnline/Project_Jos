using System.Collections;
using Scenes.Interfaces;
using UnityEngine;

namespace Scenes
{
	public class LayoutContainer2D : MonoBehaviour, ILayoutContainer
	{
		public float WaitTime { get; set; } = 1f;
		
		public IEnumerator ReorderCoroutine()
		{
			while (true)
			{
				yield return new WaitForSeconds(WaitTime);
				// TODO: Implement 2D layout instead of a Canvas/UI layout
				yield return null;
			}
		}
	}
}
