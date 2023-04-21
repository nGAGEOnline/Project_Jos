using Scenes.Enums;
using Scenes.Helpers;
using Scenes.Interfaces;
using UnityEngine;

namespace Scenes
{
	public class ColorCell : MonoBehaviour, IColorCell<Transform>
	{
		public Transform Transform => transform;
		public CellColorType CellColorType => _cellColorType;
		public Color CellColor => _cellColorType.GetColor();
		public SpriteRenderer SpriteRenderer { get; private set; }

		[SerializeField] private CellColorType _cellColorType;
		
		private void Awake()
		{
			SpriteRenderer = GetComponent<SpriteRenderer>();
		}
	}
}