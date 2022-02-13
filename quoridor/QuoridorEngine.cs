using System;

namespace quoridor
{
	public class QuoridorEngine
	{
		public List<Pawn> PawnsOnBoard = new();

		public List<Wall> WallsOnBoard = new();

		public List<Pawn> possibleMoves = new();

		private static readonly Player playerA = new('A', 10);

		private static readonly Player playerB = new('B', 10);

		public Player currentPlayer = playerA;


		public void GameInitializer()
		{
			PawnsOnBoard.Add(new Pawn(name: 'A', col: 5, row: 1));
			PawnsOnBoard.Add(new Pawn(name: 'B', col: 5, row: 9));
		}


		public void MovePiece(char name, int toCol, int toRow)
		{
			Pawn? pawn = GetPawn(name);
			if (pawn is not null)
			{
				SetPossibleMoves(pawn);
				if (!ContainsPawn(new Pawn(name, toCol, toRow), possibleMoves))
				{
					Console.WriteLine("Forbidden move");
					ShowPossibleMoves(possibleMoves);
				}
				else
				{
					PawnsOnBoard.Add(new Pawn(name: pawn.Name, col: toCol, row: toRow));
					PawnsOnBoard.Remove(pawn);
					ChangePlayer();
				}
				possibleMoves.Clear();
			}
			else
			{
				Console.WriteLine("Unknown pawn");
			}
		}


		private static void ShowPossibleMoves(List<Pawn> pawns)
		{
			Console.WriteLine("Possible moves:");
			foreach (var pawn in pawns)
			{
				Console.WriteLine($"{pawn.Name} col: {pawn.Col}  row: {pawn.Row}");
			}
		}


		private static bool ContainsPawn(Pawn pawn, List<Pawn> pawns)
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
			possibleMoves.Add(new Pawn(pawn.Name, col: pawn.Col,     row: pawn.Row + 1));
			possibleMoves.Add(new Pawn(pawn.Name, col: pawn.Col,     row: pawn.Row - 1));
			possibleMoves.Add(new Pawn(pawn.Name, col: pawn.Col + 1, row: pawn.Row));
			possibleMoves.Add(new Pawn(pawn.Name, col: pawn.Col - 1, row: pawn.Row));

			for(int i = 0; i < possibleMoves.Count; i++)
			{
				Pawn possibleMove = possibleMoves[i];
				if (possibleMove.Col > 9 || possibleMove.Col < 1 || possibleMove.Row > 9 || possibleMove.Row < 1)
				{
					possibleMoves.Remove(possibleMove);
				}
			}
		}


		public void SetWall(char? orientation, int toRow, int toCol)
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

