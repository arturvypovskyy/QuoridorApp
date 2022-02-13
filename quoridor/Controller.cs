using System;
namespace quoridor
{
	public class Controller
	{
		public BoardView boardView = new();

		public QuoridorEngine quoridorEngine = new();


		public void Start()
		{
			quoridorEngine.GameInitializer();
			while (true)
			{
				ViewDidLoad();
				Read();
			}
		}


		public void ViewDidLoad()
		{
			boardView.shadownPawns = quoridorEngine.PawnsOnBoard;
			boardView.shadowWalls = quoridorEngine.WallsOnBoard;
			boardView.CurrentPlayerName = quoridorEngine.currentPlayer.PawnName;
			boardView.ViewDisplay();
		}


		private void CommandRun(Command command)
		{
			var pawnsColumnGrid = new Dictionary<char, int>()
			{
				{'A', 1},
				{'B', 2},
				{'C', 3},
				{'D', 4},
				{'E', 5},
				{'F', 6},
				{'G', 7},
				{'H', 8},
				{'I', 9}
			};
			var wallColumnGrid = new Dictionary<char, int>()
			{
				{'S', 1},
				{'T', 2},
				{'U', 3},
				{'V', 4},
				{'W', 5},
				{'X', 6},
				{'Y', 7},
				{'Z', 8}
			};

			Console.WriteLine($"Your command is {command.Name} col: {command.ToCol}, row: {command.ToRow} optional{command.Orientation}");

			switch (command.Name)
			{
				case "move":
					int toCol = pawnsColumnGrid[command.ToCol];
					int toRow = int.Parse(Convert.ToString(command.ToRow));
					Console.WriteLine($"Your command is {quoridorEngine.currentPlayer.PawnName} col: {toCol}, row: {toRow}");
					quoridorEngine.MovePiece(name: quoridorEngine.currentPlayer.PawnName, toCol: toCol, toRow: toRow);
					break;
				case "wall":
					int toColWall = wallColumnGrid[command.ToCol];
					int toRowWall = int.Parse(Convert.ToString(command.ToRow));
					quoridorEngine.SetWall(orientation: command.Orientation, toCol: toColWall, toRow: toRowWall);
					break;
			}
		}


		public void Read()
		{
			CommandRun(boardView.Read());
		}


		public Controller(){}
	}
}

