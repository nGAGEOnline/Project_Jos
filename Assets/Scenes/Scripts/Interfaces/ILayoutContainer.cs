using System.Collections;
using UnityEngine;

namespace Scenes.Interfaces
{
	public interface ILayoutContainer
	{
		IEnumerator ReorderCoroutine();
	}
	public interface ILayoutContainer<out T> : ILayoutContainer where T : Component
	{
		T Self { get; }
		T[] Children { get; }
		Coroutine Coroutine { get; }
	}
}
