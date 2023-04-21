using Scenes.Enums;
using UnityEngine;

namespace Scenes.Interfaces
{
	public interface IColorCell<out T> where T : Component
	{
		T Transform { get; }
		CellColorType CellColorType { get; }
		Color CellColor { get; }
		SpriteRenderer SpriteRenderer { get; }
	}
}
