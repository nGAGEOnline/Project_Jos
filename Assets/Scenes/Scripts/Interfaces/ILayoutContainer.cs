using System.Collections;
using UnityEngine;

namespace Scenes.Interfaces
{
	public interface ILayoutContainer
	{
		float WaitTime { get; set; }
		IEnumerator ReorderCoroutine();
	}
}
