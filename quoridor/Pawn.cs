using System;
namespace quoridor
{
	public class Pawn
	{
		public string Name { get; set; }

		public int Row { get; set; }

		public int Col { get; set; }

		public Pawn(string name, int row, int col)
		{
			Name = name;
			Row = row;
			Col = col;
		}

	}
}

