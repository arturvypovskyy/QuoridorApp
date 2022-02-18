using System;
namespace quoridor
{
	public class Player
	{
		public string PlayerName{ get; set; }

		public char PawnName { get; set; }

		public int WallsLeft { get; set; }

		public Player(string playerName, char pawnName, int wallsLeft)
		{
			PlayerName = playerName;
			PawnName = pawnName;
			WallsLeft = wallsLeft;
		}
	}
}

