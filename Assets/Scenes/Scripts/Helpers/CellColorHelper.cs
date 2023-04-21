using System;
using Scenes.Enums;
using UnityEngine;

namespace Scenes.Helpers
{
	public static class CellColorHelper
	{
		private static readonly Color Red;
		private static readonly Color Green;
		private static readonly Color Yellow;
		private static readonly Color Blue;


		static CellColorHelper()
		{
			ColorUtility.TryParseHtmlString("#E94027", out var red);
			ColorUtility.TryParseHtmlString("#1DD62A", out var green);
			ColorUtility.TryParseHtmlString("#F3E14A", out var yellow);
			ColorUtility.TryParseHtmlString("#1994E7", out var blue);

			Red = red;
			Green = green;
			Yellow = yellow;
			Blue = blue;
		}
		public static Color GetColor(this CellColorType cellColorType)
		{
			return cellColorType switch
			{
				CellColorType.Red => Red,
				CellColorType.Green => Green,
				CellColorType.Blue => Blue,
				CellColorType.Yellow => Yellow,
				_ => throw new ArgumentOutOfRangeException(nameof(cellColorType), cellColorType, null)
			};
		}
	}
}
