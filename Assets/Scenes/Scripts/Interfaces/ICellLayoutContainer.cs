using System.Collections;
using UnityEngine;

namespace Scenes.Interfaces
{
	public interface ILayoutContainer
	{
		IEnumerator ReorderCoroutine();
	}
	public interface ICellLayoutContainer<T> : ILayoutContainer where T : Component
	{
		T Self { get; }
		IColorCell<T>[] Children { get; }
	}
}
