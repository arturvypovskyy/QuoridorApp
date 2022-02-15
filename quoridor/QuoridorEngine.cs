using System;

namespace quoridor
{
	public class QuoridorEngine
	{
		public List<Pawn> PawnsOnBoard = new();

		public List<Wall> WallsOnBoard = new();

		public List<Pawn> possibleMoves = new();

		public List<Wall> possibleWalls = new();

		private static readonly Player playerA = new('A', 10);

		private static readonly Player playerB = new('B', 10);

		public Player currentPlayer = playerA;


		public void GameInitializer()
		{
			PawnsOnBoard.Add(new Pawn(name: 'A', col: 5, row: 1));
			PawnsOnBoard.Add(new Pawn(name: 'B', col: 5, row: 9));
			GetAllPossibleWalls();
		}


		public void MovePiece(char name, int toCol, int toRow)
		{
			Pawn? pawn = GetPawn(name);
			if (pawn is not null)
			{
				GetPossibleMoves(pawn);
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
				if (pawn.Name == possiblePawn.Name &&
					pawn.Row == possiblePawn.Row &&
					pawn.Col == possiblePawn.Col)
				{
					return true;
				}
			}
			return false;
		}


		public void GetPossibleMoves(Pawn pawn)
		{
			//adding possible moves from the start
			possibleMoves.Add(new Pawn(pawn.Name, col: pawn.Col,     row: pawn.Row + 1));
			possibleMoves.Add(new Pawn(pawn.Name, col: pawn.Col,     row: pawn.Row - 1));
			possibleMoves.Add(new Pawn(pawn.Name, col: pawn.Col + 1, row: pawn.Row));
			possibleMoves.Add(new Pawn(pawn.Name, col: pawn.Col - 1, row: pawn.Row));
			//adding possible moves to jump and removing pawn layering
			//forward
			var pawnsToJump = PawnsOnBoard
				.Select(x => x)
				.Where(x => x.Col == pawn.Col && x.Row == pawn.Row - 1);
			if (pawnsToJump.Any())
			{
				possibleMoves.Add(new Pawn(name: currentPlayer.PawnName, col: pawn.Col, row: pawn.Row - 2));
				possibleMoves.RemoveAll(x => x.Col == pawn.Col && x.Row == pawn.Row - 1);
			}
			//backward
			pawnsToJump = PawnsOnBoard
				.Select(x => x)
				.Where(x => x.Col == pawn.Col && x.Row == pawn.Row + 1);
			if (pawnsToJump.Any())
			{
				possibleMoves.Add(new Pawn(name: currentPlayer.PawnName, col: pawn.Col, row: pawn.Row + 2));
				possibleMoves.RemoveAll(x => x.Col == pawn.Col && x.Row == pawn.Row + 1);
			}
			//to the left
			pawnsToJump = PawnsOnBoard
				.Select(x => x)
				.Where(x => x.Col == pawn.Col - 1 && x.Row == pawn.Row);
			if (pawnsToJump.Any())
			{
				possibleMoves.Add(new Pawn(name: currentPlayer.PawnName, col: pawn.Col - 2, row: pawn.Row));
				possibleMoves.RemoveAll(x => x.Col == pawn.Col - 1 && x.Row == pawn.Row);
			}
			//to the right
			pawnsToJump = PawnsOnBoard
				.Select(x => x)
				.Where(x => x.Col == pawn.Col + 1 && x.Row == pawn.Row);
			if (pawnsToJump.Any())
			{
				possibleMoves.Add(new Pawn(name: currentPlayer.PawnName, col: pawn.Col + 2, row: pawn.Row));
				possibleMoves.RemoveAll(x => x.Col == pawn.Col + 1 && x.Row == pawn.Row);
			}

			//removing possible moves according to walls
			//wall forward
			if (ContainsWall(new Wall(orientation: 'h', col: pawn.Col, row: pawn.Row - 1), WallsOnBoard) ||
				ContainsWall(new Wall(orientation: 'h', col: pawn.Col - 1, row: pawn.Row - 1), WallsOnBoard))
			{
				possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col && x.Row == pawn.Row - 1);
				possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col && x.Row == pawn.Row - 2);
			}
			//wall backward
			if (ContainsWall(new Wall(orientation: 'h', col: pawn.Col, row: pawn.Row), WallsOnBoard) ||
				ContainsWall(new Wall(orientation: 'h', col: pawn.Col - 1, row: pawn.Row), WallsOnBoard))
			{
				possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col && x.Row == pawn.Row + 1);
				possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col && x.Row == pawn.Row + 2);
			}
			//wall to the left
			if (ContainsWall(new Wall(orientation: 'v', col: pawn.Col - 1, row: pawn.Row), WallsOnBoard) ||
				ContainsWall(new Wall(orientation: 'v', col: pawn.Col - 1, row: pawn.Row - 1), WallsOnBoard))
			{
				possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col - 1 && x.Row == pawn.Row);
				possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col - 2 && x.Row == pawn.Row);
			}
			//wall to the right
			if (ContainsWall(new Wall(orientation: 'v', col: pawn.Col, row: pawn.Row), WallsOnBoard) ||
				ContainsWall(new Wall(orientation: 'v', col: pawn.Col, row: pawn.Row - 1), WallsOnBoard))
			{
				possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col + 1 && x.Row == pawn.Row);
				possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col + 2 && x.Row == pawn.Row);
			}

			//removing impossible moves on board
			for (int i = 0; i < possibleMoves.Count; i++)
			{
				Pawn possibleMove = possibleMoves[i];
				if (possibleMove.Col > 9 ||
					possibleMove.Col < 1 ||
					possibleMove.Row > 9 ||
					possibleMove.Row < 1)
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
			else if (!ContainsWall(new Wall(orientation: orientation, row: toRow, col: toCol), possibleWalls))
			{
				Console.WriteLine("Forbidden wall");
			}
			else
			{
				WallsOnBoard.Add(new Wall(orientation, toRow, toCol));
				currentPlayer.WallsLeft -= 1;

				possibleWalls.RemoveAll(x => x.Orientation == 'h' && x.Col == toCol && x.Row == toRow);
				possibleWalls.RemoveAll(x => x.Orientation == 'v' && x.Col == toCol && x.Row == toRow);

				if (orientation == 'h')
				{
					possibleWalls.RemoveAll(x => x.Orientation == 'h' && x.Col == toCol + 1 && x.Row == toRow);
					possibleWalls.RemoveAll(x => x.Orientation == 'h' && x.Col == toCol - 1 && x.Row == toRow);
				}
				else if(orientation == 'v')
				{
					possibleWalls.RemoveAll(x => x.Orientation == 'v' && x.Col == toCol && x.Row == toRow + 1);
					possibleWalls.RemoveAll(x => x.Orientation == 'v' && x.Col == toCol && x.Row == toRow - 1);
				}
				ChangePlayer();
			}
		}


		private void GetAllPossibleWalls()
		{
			for (int i = 1; i < 9; i++)
			{
				for (int j = 1; j < 9; j++)
				{
					possibleWalls.Add(new Wall(orientation: 'h', col: i, row: j));
					possibleWalls.Add(new Wall(orientation: 'v', col: i, row: j));
				}
			}
		}


		private static bool ContainsWall(Wall wall, List<Wall> possibleWalls)
		{
			foreach (var possibleWall in possibleWalls)
			{
				if (possibleWall.Col == wall.Col &&
					possibleWall.Row == wall.Row &&
					possibleWall.Orientation == wall.Orientation)
				{
					return true;
				}
			}
			return false;
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

