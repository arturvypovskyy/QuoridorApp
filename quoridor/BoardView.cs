using System;
namespace quoridor
{
	public class BoardView
	{
		public  List<Pawn> pawns = new List<Pawn>();

		public void DrawBoard()
		{
			pawns.Add(new Pawn("W", 1, 1));

			var row = new char[9];
			row[0] = 'W';
			var rowView = String.Format(" _   _   _   _   _   _   _   _   _ \n" +
				                    "|{0}| |{1}| |{2}| |{3}| |{4}| |{5}| |{6}| |{7}| |{8}|\n" +
				                    " ‾   ‾   ‾   ‾   ‾   ‾   ‾   ‾   ‾ \n" +
                                    "                                   \n",
									row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8]);
			for (int i = 0; i <= 9; i++)
			{
				Console.WriteLine(row);
			}
		}

		public BoardView()
		{
		}
	}
}

