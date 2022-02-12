﻿using System;
namespace quoridor
{
	public class BoardView
	{
		public List<Pawn> shadownPawns = new();

		public List<Wall> shadowWalls = new();

		public char[,] boardMatrix = new char[17, 17];

		public void SetEmptyMatrix()
		{
			for (int i = 0; i<= 16; i++)
			{
				for (int j = 0; j <= 16; j++)
				{
					boardMatrix[i,j] = ' ';
				}
			}
		}


		public void DrawBoard()
		{
			for(int i = 0; i <= 16; i++)
			{
				var row = new char[17];
				for (int j = 0; j <= 16; j++)
				{
					row[j] = boardMatrix[i,j];
				}

				string rowString;

				if (i % 2 == 0)
				{
					rowString = String.Format(" _   _   _   _   _   _   _   _   _ \n" +
											"|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|\n" +
											" ‾   ‾   ‾   ‾   ‾   ‾   ‾   ‾   ‾ \n",
										row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], row[10], row[11], row[12], row[13], row[14], row[15], row[16]);
				}
				else
				{
					rowString = String.Format( " {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14} {15} {16}",
										row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], row[10], row[11], row[12], row[13], row[14], row[15], row[16]);
				}
				Console.WriteLine(rowString);
			}
		}


		public void DrawPawns()
		{
			foreach (var pawn in shadownPawns)
			{
				DrawPawn(pawn);
			}
		}


		public void DrawPawn(Pawn pawn)
		{
			int row = pawn.Row * 2 - 2;
			int col = pawn.Col * 2 - 2;
			boardMatrix[row, col] = pawn.Name;
		}


		public void DrawWalls()
		{
			foreach (var wall in shadowWalls)
			{
				DrawWall(wall);
			}
		}


		public void DrawWall(Wall wall)
		{
			if (wall.Orientation == 'h')
			{
				boardMatrix[wall.Row, wall.Col - 1] = '█';
				boardMatrix[wall.Row, wall.Col] = '█';
				boardMatrix[wall.Row, wall.Col + 1] = '█';
			}
			else
			{
				boardMatrix[wall.Row + 1, wall.Col] = '█';
				boardMatrix[wall.Row, wall.Col] = '█';
				boardMatrix[wall.Row - 1, wall.Col] = '█';
			}
		}


		public void ViewDisplay()
		{
			SetEmptyMatrix();
			DrawPawns();
			DrawWalls();
			DrawBoard();
		}


		public Command Read()
		{
			while (true)
			{
				Console.WriteLine("Your move:");
				string? args = Console.ReadLine();

				if (args is null)
					throw new ArgumentNullException(nameof(args));

				if (TryParse(args, out Command command))
				{
					return command;
				}
				else
				{
					Console.WriteLine("Incorrect input");
				}
			}
		}


		public bool TryParse(string args, out Command command)
		{
			var input = args.Split();

			var possibleCommands = new string[] {"move", "jump", "wall"};

			if (possibleCommands.Contains(input[0]))
			{
				if (int.TryParse(input[1], out int toRow) && int.TryParse(input[2], out int toCol))
				{
					if (input.Length > 3 && char.TryParse(input[3], out char orientation))
					{
						command = new Command(input[0], toRow, toCol, orientation);
					}
					else
					{
						command = new Command(input[0], toRow, toCol);
					}
					return true;
				}
			}
			command = new();
			return false;
		}
	}
}
