using System;
namespace quoridor
{
	public class Player
	{
		public char PawnName { get; set; }

		public int WallsLeft { get; set; }

		public Player(char pawnName, int wallsLeft)
		{
			PawnName = pawnName;
			WallsLeft = wallsLeft;
		}
	}
}

