using System;
namespace quoridor
{
	public class Player
	{
		public string PawnName { get; set; }

		public int WallsLeft { get; set; }

		public Player(string pawnName, int wallsLeft)
		{
			PawnName = pawnName;
			WallsLeft = wallsLeft;
		}
	}
}

