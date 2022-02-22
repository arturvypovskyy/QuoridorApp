using System;
namespace quoridor
{
    public class Position: QuoridorEngine
    { 
        public Position(Position previousPosition)
        {
            foreach (var pawn in previousPosition.PawnsOnBoard)
            {
                PawnsOnBoard.Add(new Pawn(name: pawn.Name, col: pawn.Col, row: pawn.Row));
            }
            foreach (var wall in previousPosition.WallsOnBoard)
            {
                WallsOnBoard.Add(new Wall(orientation: wall.Orientation, col: wall.Col, row: wall.Row));
            }
            //PawnsOnBoard = previousPosition.PawnsOnBoard;
            //WallsOnBoard = previousPosition.WallsOnBoard;
        }


        public Position(QuoridorEngine originalModel)
        {
            foreach (var pawn in originalModel.PawnsOnBoard)
            {
                PawnsOnBoard.Add(new Pawn(name: pawn.Name, col: pawn.Col, row: pawn.Row));
            }
            foreach (var wall in originalModel.WallsOnBoard)
            {
                WallsOnBoard.Add(new Wall(orientation: wall.Orientation, col: wall.Col, row: wall.Row));
            }
        }


        public int GetStaticEvaluation()
        {
            var maximizerPlayerPathLength = GetShortestPathFor('W').Count;
            var minimizerPlayerPathLength = GetShortestPathFor('B').Count;
            return minimizerPlayerPathLength - maximizerPlayerPathLength;
        }
    }
}

