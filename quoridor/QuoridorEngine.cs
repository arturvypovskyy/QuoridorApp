using System;
namespace quoridor
{
	public class QuoridorEngine
	{
		public List<Pawn> PawnsOnBoard = new();

		public List<Wall> WallsOnBoard = new();

		public void GameInitializer()
		{
			PawnsOnBoard.Add(new Pawn(name: "W", row: 1, col: 5 ));
			PawnsOnBoard.Add(new Pawn(name: "B", row: 9, col: 5));

		}

		public QuoridorEngine()
		{
		}
	}
}

