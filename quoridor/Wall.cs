using System;
namespace quoridor
{
	public class Wall
	{
		public char? Orientation { get; set; }

		public int Row { get; set; }

		public int Col { get; set; }

		public Wall(char? orientation, int row, int col)
		{
			Orientation = orientation;

			Row = row;

			Col = col;
		}
	}
}

