using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFC_Monitor
{
	public partial class Form1 : Form
	{
		List<Tile> originalTiles = new List<Tile>();

		int drawSize = 20;

		Map map;

		Random random;

		public Form1()
		{
			InitializeComponent();

			//Bitmap tilemap = new Bitmap(@"C:\Users\TadjihojaevAR\source\repos\WFC_Monitor\WFC_Monitor\Tilemap.png");

			//for (int y = 0; y < 5; y++)
			//{
			//	for (int x = 0; x < 4; x++)
			//	{
			//		var image = tilemap.Clone(new Rectangle(x * Tile.size + x, y * Tile.size + y, Tile.size, Tile.size), System.Drawing.Imaging.PixelFormat.DontCare);

			//		image.Save($@"C:\Users\TadjihojaevAR\source\repos\WFC_Monitor\WFC_Monitor\Tiles\Tile {y}-{x}.png");

			//		originalTiles.Add(new Tile(image));
			//	}
			//}
			var files = Directory.GetFiles(@"C:\Users\TadjihojaevAR\source\repos\WFC_Monitor\WFC_Monitor\Tiles");

			foreach (var file in files)
			{
				var bitmap = new Bitmap(file);
				originalTiles.Add(new Tile(bitmap));
			}

			random = new Random(DateTime.Now.Millisecond);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			map = new Map(trackBar1.Value, originalTiles);

			//FullGeneration();

			Point start = new Point(8, 8);

			map.cells[start.Y, start.X].StayOne(random);
			map.cells[start.Y, start.X].state = Cell.State.Changed;

			Iteration(start.Y, start.X);

			Propagation();

			pictureBox1.Image = map.GetImage(drawSize);

			timer1.Enabled = true;
		}

		void FullGeneration()
		{
			Point start = new Point(8, 8);

			map.cells[start.Y, start.X].StayOne(random);
			map.cells[start.Y, start.X].state = Cell.State.Changed;

			Iteration(start.Y, start.X);

			Propagation();

			Cell cell = null;

			do
			{
				cell = map.GetMinimum();

				if (cell == null)
					continue;

				cell.StayOne(random);
				cell.state = Cell.State.Changed;

				Propagation();
			}
			while (cell != null);
		}

		void Propagation()
		{
			while (map.HaveChanged())
			{
				var changed = map.GetAllChanged();

				foreach (var cell in changed)
				{
					Iteration(cell.y, cell.x);
				}
			}
		}

		void Iteration(int y, int x)
		{
			List<Cell> needCheck = map.GetNeedCheck();

			map.MarkChangedAsDisable();

			if (y - 1 >= 0)
				if (map.cells[y - 1, x].RemoveUnsuitable(map.GetCell(y - 2, x)?.tiles, map.GetCell(y, x)?.tiles, map.GetCell(y - 1, x - 1)?.tiles, map.GetCell(y - 1, x + 1)?.tiles))
					map.cells[y - 1, x].state = Cell.State.Changed;

			if (y + 1 < map.size)
				if (map.cells[y + 1, x].RemoveUnsuitable(map.GetCell(y, x)?.tiles, map.GetCell(y + 2, x)?.tiles, map.GetCell(y + 1, x - 1)?.tiles, map.GetCell(y + 1, x + 1)?.tiles))
					map.cells[y + 1, x].state = Cell.State.Changed;

			if (x - 1 >= 0)
				if (map.cells[y, x - 1].RemoveUnsuitable(map.GetCell(y - 1, x - 1)?.tiles, map.GetCell(y + 1, x - 1)?.tiles, map.GetCell(y, x - 2)?.tiles, map.GetCell(y, x)?.tiles))
					map.cells[y, x - 1].state = Cell.State.Changed;

			if (x + 1 < map.size)
				if (map.cells[y, x + 1].RemoveUnsuitable(map.GetCell(y - 1, x + 1)?.tiles, map.GetCell(y + 1, x + 1)?.tiles, map.GetCell(y, x)?.tiles, map.GetCell(y, x + 2)?.tiles))
					map.cells[y, x + 1].state = Cell.State.Changed;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			Cell cell = map.GetMinimum();

			if (cell == null)
			{
				timer1.Enabled = false;
				return;
			}

			cell.StayOne(random);
			cell.state = Cell.State.Changed;

			Propagation();

			pictureBox1.Image = map.GetImage(drawSize);
		}
	}
}
