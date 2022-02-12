using System;
namespace quoridor
{
	public class QuoridorEngine
	{
		public List<Pawn> PawnsOnBoard = new();

		public List<Wall> WallsOnBoard = new();

		public Player currentPlayer = new('A', 10);

		public Player playerA = new Player('A', 10);

		public Player playerB = new Player('B', 10);

		public void GameInitializer()
		{
			PawnsOnBoard.Add(new Pawn(name: 'A', row: 1, col: 5 ));
			PawnsOnBoard.Add(new Pawn(name: 'B', row: 9, col: 5));
		}


		public void MovePiece(char name, int toRow, int toCol)
		{
			Pawn? pawn = GetPawn(name);
			if (pawn is not null)
			{
				if (toRow <= 0 || toRow > 9)
				{
					Console.WriteLine("Impossible \'toRow\' parametr");
				}
				else if (toCol <= 0 || toCol > 9)
				{
					Console.WriteLine("Impossible \'toCol\' parametr");
				}
				else
				{
					PawnsOnBoard.Add(new Pawn(pawn.Name, toRow, toCol));
					PawnsOnBoard.Remove(pawn);
					ChangePlayer();
				}
			}
			else
			{
				Console.WriteLine("Unknown pawn");
			}
		}


		public void SetWall(char? orientation,int toRow, int toCol)
		{
			if (orientation is null)
			{
				Console.WriteLine("Incorrect orientation");
			}
			else
			{
				WallsOnBoard.Add(new Wall(orientation, toRow, toCol));
			}
		}


		private Pawn? GetPawn(char name)
		{
			foreach (var pawn in PawnsOnBoard)
			{
				if (pawn.Name == name)
				{
					return pawn;
				}
			}
			return null;
		}

		public void ChangePlayer()
		{
			if (currentPlayer.PawnName == 'A')
			{
				currentPlayer = playerB;
			}
			else
			{
				currentPlayer = playerA;
			}
		}


		public QuoridorEngine(){}
	}
}

