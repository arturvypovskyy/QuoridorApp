using System;
namespace quoridor
{
	public class QuoridorEngine
	{
		public List<Pawn> PawnsOnBoard = new();

		public List<Wall> WallsOnBoard = new();

		public List<Pawn> possibleMoves = new();

		public static Player playerA = new('A', 10);

		public static Player playerB = new('B', 10);

		public Player currentPlayer = playerA;


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
				SetPossibleMoves(pawn);
				OutputPossibleMoves(possibleMoves);
				if (toRow <= 0 || toRow > 9)
				{
					Console.WriteLine("Impossible \'toRow\' parametr");
				}
				else if (toCol <= 0 || toCol > 9)
				{
					Console.WriteLine("Impossible \'toCol\' parametr");
				}
				else if (!ContainsPawn(new Pawn(name, toRow, toCol), possibleMoves))
				{
					Console.WriteLine("Forbidden move");
				}
				else
				{
					PawnsOnBoard.Add(new Pawn(pawn.Name, toRow, toCol));
					PawnsOnBoard.Remove(pawn);
					possibleMoves.Clear();
					ChangePlayer();
				}
			}
			else
			{
				Console.WriteLine("Unknown pawn");
			}
		}


		private void OutputPossibleMoves(List<Pawn> pawns)
		{
			foreach (var pawn in pawns)
			{
				Console.WriteLine($"{pawn.Name}  {pawn.Row}  {pawn.Col}");
			}
		}


		private bool ContainsPawn(Pawn pawn, List<Pawn> pawns)
		{
			foreach (var possiblePawn in pawns)
			{
				if (pawn.Name == possiblePawn.Name && pawn.Row == possiblePawn.Row && pawn.Col == possiblePawn.Col)
				{
					return true;
				}
			}
			return false;
		}


		public void SetPossibleMoves(Pawn pawn)
		{
			possibleMoves.Add(new Pawn(pawn.Name, row: pawn.Row + 1, col: pawn.Col));
			possibleMoves.Add(new Pawn(pawn.Name, row: pawn.Row - 1, col: pawn.Col));
			possibleMoves.Add(new Pawn(pawn.Name, row: pawn.Row, col: pawn.Col + 1));
			possibleMoves.Add(new Pawn(pawn.Name, row: pawn.Row, col: pawn.Col - 1));
		}


		public void SetWall(char? orientation,int toRow, int toCol)
		{
				if (orientation is null)
				{
					Console.WriteLine("Incorrect orientation");
				}
				else if (currentPlayer.WallsLeft == 0)
				{
					Console.WriteLine("No walls left");
				}
				else
				{
					WallsOnBoard.Add(new Wall(orientation, toRow, toCol));
					currentPlayer.WallsLeft -= 1;
					ChangePlayer();
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

