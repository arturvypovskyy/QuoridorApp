using System;
namespace quoridor
{
	public class Controller
	{
		public BoardView boardView = new();

		public QuoridorEngine quoridorEngine = new();

		public Bot bot = new('W', true);

		public Minimax minimax = new();

		public Controller() { }


		public void Start()
		{
			GameInitializer();
			while (true)
			{
				ViewDidLoad();
				if (quoridorEngine.IsGameEnded())
				{
					boardView.ShowWinner(quoridorEngine.currentPlayer.PawnName);
				}
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


		public void GameInitializer()
		{
			bot.IsWorking = boardView.IsPlayingWithBot();
			if (bot.IsWorking)
			{
				bot.PawnName = boardView.ChooseSide();
			}
			boardView.Help();
			quoridorEngine.GameInitializer();
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
			//Console.WriteLine($"Your command is {command.Name} col: {command.ToCol}, row: {command.ToRow} optional{command.Orientation}");
			try
			{
				switch (command.Name)
				{
					case "move":
						int toCol = pawnsColumnGrid[command.ToCol];
						int toRow = int.Parse(Convert.ToString(command.ToRow));
						//Console.WriteLine($"Your command is {quoridorEngine.currentPlayer.PawnName} col: {toCol}, row: {toRow}");
						quoridorEngine.MovePiece(name: quoridorEngine.currentPlayer.PawnName, toCol: toCol, toRow: toRow);
						break;
					case "wall":
						int toColWall = wallColumnGrid[command.ToCol];
						int toRowWall = int.Parse(Convert.ToString(command.ToRow));
						quoridorEngine.SetWall(orientation: command.Orientation, toCol: toColWall, toRow: toRowWall);
						break;
					case "restart":
						GameInitializer();
						break;
					case "restart game":
						quoridorEngine.GameInitializer();
						break;
					case "help":
						boardView.Help();
						break;
					case "exit":
						quoridorEngine.Exit();
						break;
				}
			}
			catch
			{
				//Console.WriteLine("Incorrect input");
			}
		}


		public void Read()
		{
            if (quoridorEngine.currentPlayer.PawnName == bot.PawnName && bot.IsWorking)
            {
				BotAction();
			}
            else
			{ 
			    //AIBotAction();
				CommandRun(boardView.Read());
			}
		}


        public void BotAction()
        {
            if (!bot.IsWorking)
            {
                return;
            }
            List<string> commands = new() { "move", "wall" };
            if (quoridorEngine.currentPlayer.WallsLeft == 0)
                commands.Remove("wall");
            Random random = new();
            switch (commands[random.Next(commands.Count)])
            {
                case "move":
                    var pawn = quoridorEngine.GetPawn(bot.PawnName);
                    if (pawn is null)
                        throw new ArgumentNullException(nameof(pawn));
                    quoridorEngine.GetPossibleMoves(pawn);
                    var possibleMove = quoridorEngine.possibleMoves[random.Next(quoridorEngine.possibleMoves.Count)];
                    quoridorEngine.MovePiece(name: bot.PawnName, toCol: possibleMove.Col, toRow: possibleMove.Row);
                    break;
                case "wall":
                    while (true)
                    {
                        var possibleWall = quoridorEngine.possibleWalls[random.Next(quoridorEngine.possibleWalls.Count)];
                        quoridorEngine.SetWall(orientation: possibleWall.Orientation, toCol: possibleWall.Col, toRow: possibleWall.Row);
                        if (quoridorEngine.WallsOnBoard.Where(x => x.Orientation == possibleWall.Orientation && x.Col == possibleWall.Col && x.Row == possibleWall.Row).Any())
                            break;
                    }
                    break;
            }
        }

        //AI bot
        public void AIBotAction()
        {
			minimax.GetMove(new Position(quoridorEngine));
		}
    }
}

