using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFC_Monitor
{
	class Tile
	{
		public static int size = 5;

		public Bitmap image;

		public Dictionary<Side, List<Color>> sides;

		public Tile(Bitmap image)
		{
			this.image = image;

			sides = new Dictionary<Side, List<Color>>();

			{
				List<Color> top = new List<Color>();
				List<Color> bottom = new List<Color>();

				for (int x = 0; x < image.Width; x++)
				{
					top.Add(image.GetPixel(x, 0));
					bottom.Add(image.GetPixel(x, image.Height - 1));
				}

				sides.Add(Side.Top, top);
				sides.Add(Side.Bottom, bottom);
			}

			{
				List<Color> left = new List<Color>();
				List<Color> right = new List<Color>();

				for (int y = 0; y < image.Width; y++)
				{
					left.Add(image.GetPixel(0, y));
					right.Add(image.GetPixel(image.Width - 1, y));
				}

				sides.Add(Side.Left, left);
				sides.Add(Side.Right, right);
			}
		}

		public enum Side
		{
			Top = 0,
			Right = 1,
			Bottom = 2,
			Left = 3,
		}

		public static bool Compare(Tile me, Tile other, Side side)
		{
			switch (side)
			{
				case Side.Top:
					return Enumerable.SequenceEqual(me.sides[Side.Top], other.sides[Side.Bottom]);
				case Side.Right:
					return Enumerable.SequenceEqual(me.sides[Side.Right], other.sides[Side.Left]);
				case Side.Bottom:
					return Enumerable.SequenceEqual(me.sides[Side.Bottom], other.sides[Side.Top]);
				case Side.Left:
					return Enumerable.SequenceEqual(me.sides[Side.Left], other.sides[Side.Right]);
				default:
					throw new Exception();
			}
		}
	}
}
