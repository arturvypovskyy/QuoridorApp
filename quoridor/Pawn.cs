using System;
namespace quoridor
{
	public class Pawn
	{
		public char Name { get; set; }

		public int Row { get; set; }

		public int Col { get; set; }

		public Pawn(char name, int col, int row)
		{
			Name = name;
			Row = row;
			Col = col;
		}
	}
}

