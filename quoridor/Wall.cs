using System;
namespace quoridor
{
	public class Wall
	{
		public string Orientation { get; set; }

		public int Row { get; set; }

		public int Col { get; set; }

		public Wall(string orientation, int row, int col)
		{
			Orientation = orientation;

			Row = row;

			Col = col;
		}
	}
}

