using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFC_Monitor
{
	class Map
	{
		public Cell[,] cells;

		public readonly int size;

		public Map(int size, List<Tile> tiles)
		{
			this.size = size;

			cells = new Cell[size, size];

			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					cells[y, x] = new Cell(y, x, tiles);
				}
			}
		}

		public Cell GetMinimum()
		{
			Cell cell = null;

			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					if (cells[y, x].tiles.Count > 1 && (cell == null || cell?.tiles.Count > cells[y, x].tiles.Count))
						cell = cells[y, x];
				}
			}

			return cell;
		}

		/// <summary>
		/// Получение всех ячеек качающихся изменёных
		/// </summary>
		public List<Cell> GetNeedCheck()
		{
			List<Cell> list = new List<Cell>();

			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					if (HaveChanged(y, x))
						list.Add(cells[y, x]);
				}
			}

			return list;

			bool HaveChanged(int y, int x)
			{
				if (y - 1 >= 0 && cells[y - 1, x].state == Cell.State.Changed)
					return true;

				if (y + 1 < size && cells[y + 1, x].state == Cell.State.Changed)
					return true;

				if (x - 1 >= 0 && cells[y, x - 1].state == Cell.State.Changed)
					return true;

				if (x + 1 < size && cells[y, x + 1].state == Cell.State.Changed)
					return true;

				return false;
			}
		}

		/// <summary>
		/// Все изменёные ячейки
		/// </summary>
		public List<Cell> GetAllChanged()
		{
			List<Cell> list = new List<Cell>();

			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					if (cells[y, x].state == Cell.State.Changed)
						list.Add(cells[y, x]);
				}
			}

			return list;
		}

		/// <summary>
		/// Получение изменёной ячейк или null если координаты не корректны
		/// </summary>
		public Cell GetChangedOrNull(int y, int x)
		{
			var cell = GetCell(y, x);

			if (cell?.state != Cell.State.Changed)
				return null;

			return cell;
		}

		/// <summary>
		/// Получение ячейки или null если координаты не корректны
		/// </summary>
		public Cell GetCell(int y, int x)
		{
			if (y < 0 || y >= size || x < 0 || x >= size)
				return null;

			return cells[y, x];
		}

		/// <summary>
		/// Отметка изменёных ячеек что бы их не изменили повторно
		/// </summary>
		public void MarkChangedAsDisable()
		{
			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					if (cells[y, x].state == Cell.State.Changed)
						cells[y, x].state = Cell.State.Disable;
				}
			}
		}

		/// <summary>
		/// Есть изменёные ячейки
		/// </summary>
		public bool HaveChanged()
		{
			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					if (cells[y, x].state == Cell.State.Changed)
						return true;
				}
			}

			return false;
		}

		public Bitmap GetImage(int drawSize)
		{
			Bitmap bitmap = new Bitmap(size * drawSize, size * drawSize);

			Graphics g = Graphics.FromImage(bitmap);

			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					var image = cells[y, x].GetFullImage();

					//image.Save($@"C:\Users\TadjihojaevAR\source\repos\WFC_Monitor\WFC_Monitor\Tiles\image [{y}-{x}].png");

					g.DrawImage(image, x * drawSize, y * drawSize, drawSize, drawSize);
				}
			}

			//bitmap.Save(@"C:\Users\TadjihojaevAR\source\repos\WFC_Monitor\WFC_Monitor\out.png");

			return bitmap;
		}
	}
}
