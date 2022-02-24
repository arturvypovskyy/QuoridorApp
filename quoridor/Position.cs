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
            //Console.WriteLine("W shortest path");
            //foreach (var step in GetShortestPathFor('W'))
            //{
            //    Console.WriteLine($"name: {step.Pawn.Name} col: {step.Pawn.Col} row: {step.Pawn.Row}");
            //}
            
            var minimizerPlayerPathLength = GetShortestPathFor('B').Count;
            //Console.WriteLine("B shortest path");
            //foreach (var step in GetShortestPathFor('B'))
            //{
            //    Console.WriteLine($"name: {step.Pawn.Name} col: {step.Pawn.Col} row: {step.Pawn.Row}");
            //}

            return minimizerPlayerPathLength - maximizerPlayerPathLength;
        }
    }
}

