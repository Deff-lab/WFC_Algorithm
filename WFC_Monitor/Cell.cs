using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFC_Monitor
{
	class Cell
	{
		public readonly int y;
		public readonly int x;

		public List<Tile> tiles;

		/// <summary>
		/// Корректироваллось
		/// </summary>
		public State state = State.None;// changed = false;

		public Cell(int y, int x, List<Tile> tiles)
		{
			this.y = y;
			this.x = x;

			this.tiles = new List<Tile>( tiles );
		}

		/// <summary>
		/// Оставляем отлько один случайный tile
		/// </summary>
		public void StayOne(Random random)
		{
			tiles = new List<Tile> { tiles[random.Next(0, tiles.Count)] };
		}

		public Bitmap GetFullImage()
		{
			if (tiles.Count == 1)
				return tiles[0].image;

			if (tiles.Count <= 4)
				return GetFullImage(2);

			if (tiles.Count <= 9)
				return GetFullImage(3);

			return GetFullImage(4);
		}

		/// <summary>
		/// Получение изображение из всех оставшихся tile
		/// </summary>
		Bitmap GetFullImage(int size)
		{
			Bitmap bitmap = new Bitmap(size * Tile.size, size * Tile.size);

			int y = 0;
			int x = 0;

			Graphics g = Graphics.FromImage(bitmap);

			for (int i = 0; i < tiles.Count; i++)
			{
				g.DrawImage(tiles[i].image, x * Tile.size, y * Tile.size, Tile.size, Tile.size);
				x++;
				if (x > size - 1)
				{
					y++;
					x = 0;
				}
			}

			return bitmap;
		}

		public enum State
		{
			None,
			Changed,
			Disable,
		}

		/// <summary>
		/// Удаление неподходящих
		/// </summary>
		public bool RemoveUnsuitable(List<Tile> top, List<Tile> bottom, List<Tile> left, List<Tile> right)
		{
			bool result = false;

			if (top != null)
				if (tiles.RemoveAll(e => !top.Any(o => Tile.Compare(e, o, Tile.Side.Top))) > 0)
					result = true;

			if (bottom != null)
				if (tiles.RemoveAll(e => !bottom.Any(o => Tile.Compare(e, o, Tile.Side.Bottom))) > 0)
					result = true;

			if (left != null)
				if (tiles.RemoveAll(e => !left.Any(o => Tile.Compare(e, o, Tile.Side.Left))) > 0)
					result = true;

			if (right != null)
				if (tiles.RemoveAll(e => !right.Any(o => Tile.Compare(e, o, Tile.Side.Right))) > 0)
					result = true;

			return result;
		}
	}
}
