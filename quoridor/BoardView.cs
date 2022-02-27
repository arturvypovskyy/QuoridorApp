using System;
namespace quoridor
{
	public class BoardView
	{
		public List<Pawn> shadownPawns = new();

		public List<Wall> shadowWalls = new();

		public char CurrentPlayerName { get; set; }

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
			int counter = 0;
			Console.WriteLine("   A   B   C   D   E   F   G   H   I");
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
					counter ++;
					rowString = string.Format("   _   _   _   _   _   _   _   _   _ \n" +
											$"{counter}" + " |{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|\n" +
											 "   ‾   ‾   ‾   ‾   ‾   ‾   ‾   ‾   ‾ \n",
										row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8],
										row[9], row[10], row[11], row[12], row[13], row[14], row[15], row[16]);
				}
				else
				{
					rowString = string.Format( "   {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14} {15} {16}  {17}",
										row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8],
										row[9], row[10], row[11], row[12], row[13], row[14], row[15], row[16], counter);
				}
				Console.WriteLine(rowString);
			}
			Console.WriteLine("     S   T   U   V   W   X   Y   Z");
			Console.WriteLine("");
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
			int row = wall.Row * 2 - 1;
			int col = wall.Col * 2 - 1;
			if (wall.Orientation == 'h')
			{
				boardMatrix[row, col + 1] = '█';
				boardMatrix[row, col] = '█';
				boardMatrix[row, col - 1] = '█';
			}
			else if (wall.Orientation == 'v')
			{
				boardMatrix[row + 1, col] = '█';
				boardMatrix[row, col] = '█';
				boardMatrix[row - 1, col] = '█';
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
				Console.WriteLine($"{CurrentPlayerName} move:");
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


		public static bool TryParse(string args, out Command command)
		{
			var input = args.Split();

			if (input[0] == "move" && input[1].Length == 2)
			{
				char toCol = input[1][0];
				char toRow = input[1][1];
				command = new Command(input[0], toCol, toRow);
				return true;
			}
			else if (input[0] == "wall" && input[1].Length == 3)
			{
				char toCol = input[1][0];
				char toRow = input[1][1];
				char orientation = input[1][2];
				command = new Command(input[0], toCol, toRow, orientation);
				return true;
			}
			else if (input[0] == "restart")
			{
				command = new Command("restart");
				return true;
			}
			else if (input[0] == "restart" && input[1] == "game")
			{
				command = new Command("restart game");
				return true;
			}
			else if (input[0] == "exit")
			{
				command = new Command("exit");
				return true;
			}
			else if (input[0] == "help")
			{
				command = new Command("help");
				return true;
			}
			else
			{
				command = new();
				return false;
			}
		}


		public bool IsPlayingWithBot()
		{
			Console.WriteLine("Welcome at the Quoriod App!");
			while (true)
			{
				Console.WriteLine("Choose player mode (bot/players):");
				var answer = Console.ReadLine();
				switch (answer)
				{
					case "bot":
						return true;
					case "players":
						return false;
					default:
						Console.WriteLine("Incorrect input. Try again");
						break;
				}
			}
		}


		public char ChooseSide()
		{
			while (true)
			{
				Console.WriteLine("Okay. Choose your side (white/black)");
				var answer = Console.ReadLine();
				switch (answer)
				{
					case "white":
						return 'B';
					case "black":
						return 'W';
					default:
						Console.WriteLine("Incorrect input. Try again");
						break;
				}
			}
		}


		public void Help()
		{
			Console.WriteLine("* \"move <column><row>\" - moves a pawn. For example: \"move E2\"\n" +
				"* \"wall <column><row><orientation>\" - sets a wall. For example: \"wall W8h\"\n" +
                "* \"exit\" - exits the game\n" +
                "* \"restart\" - restarts the app\n" +
				"* \"restart game\" - restarts current game\n" +
				"* \"help\" - info\n");
			Console.WriteLine("Press ENTER to continue");
			Console.ReadLine();
		}


		public void ShowWinner(char pawnName)
		{
			string winner = "";
			switch (pawnName)
			{
				case 'W':
					winner = "white";
					break;
				case 'B':
					winner = "black";
					break;
			}
			Console.WriteLine($"Winner is {winner}");
		}
	}
}

