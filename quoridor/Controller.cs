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
			boardView.ViewDisplay();
		}


		private void CommandRun(Command command)
		{
			switch (command.Name)
			{
				case "move":
					quoridorEngine.MovePiece(quoridorEngine.currentPlayer.PawnName, command.ToRow, command.ToCol);
					break;
				case "wall":
					quoridorEngine.SetWall(command.Orientation,command.ToRow, command.ToCol);
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

