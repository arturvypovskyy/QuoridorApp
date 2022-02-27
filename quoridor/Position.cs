using System;
namespace quoridor
{
    public class Position: QuoridorEngine
    { 
        public Position(Position previousPosition)
        {
            /*
            foreach (var pawn in previousPosition.PawnsOnBoard)
            {
                PawnsOnBoard.Add(new Pawn(name: pawn.Name, col: pawn.Col, row: pawn.Row));
            }
            foreach (var wall in previousPosition.WallsOnBoard)
            {
                WallsOnBoard.Add(new Wall(orientation: wall.Orientation, col: wall.Col, row: wall.Row));
            }
            foreach (var pawn in previousPosition.possibleMoves)
            {
                possibleMoves.Add(new Pawn(name: pawn.Name, col: pawn.Col, row: pawn.Row));
            }
            foreach (var wall in previousPosition.possibleWalls)
            {
                possibleWalls.Add(new Wall(orientation: wall.Orientation, col: wall.Col, row: wall.Row));
            }*/
            PawnsOnBoard.AddRange(previousPosition.PawnsOnBoard);
            WallsOnBoard.AddRange(previousPosition.WallsOnBoard);
            possibleMoves.AddRange(previousPosition.possibleMoves);
            possibleWalls.AddRange(previousPosition.possibleWalls);
            currentPlayer = previousPosition.currentPlayer == previousPosition.playerWhite ? playerWhite : playerBlack;
            playerBlack.WallsLeft = previousPosition.playerBlack.WallsLeft;
            playerWhite.WallsLeft = previousPosition.playerWhite.WallsLeft;
        }


        public Position(QuoridorEngine originalModel)
        {/*
            foreach (var pawn in originalModel.PawnsOnBoard)
            {
                PawnsOnBoard.ra.Add(new Pawn(name: pawn.Name, col: pawn.Col, row: pawn.Row));
            }
            foreach (var wall in originalModel.WallsOnBoard)
            {
                WallsOnBoard.Add(new Wall(orientation: wall.Orientation, col: wall.Col, row: wall.Row));
            }
            foreach (var pawn in originalModel.possibleMoves)
            {
                possibleMoves.Add(new Pawn(name: pawn.Name, col: pawn.Col, row: pawn.Row));
            }
            foreach (var wall in originalModel.possibleWalls)
            {
                possibleWalls.Add(new Wall(orientation: wall.Orientation, col: wall.Col, row: wall.Row));
            }*/
            PawnsOnBoard.AddRange(originalModel.PawnsOnBoard);
            WallsOnBoard.AddRange(originalModel.WallsOnBoard);
            possibleMoves.AddRange(originalModel.possibleMoves);
            possibleWalls.AddRange(originalModel.possibleWalls);
            currentPlayer = originalModel.currentPlayer == originalModel.playerWhite ? playerWhite : playerBlack;
            playerBlack.WallsLeft = originalModel.playerBlack.WallsLeft;
            playerWhite.WallsLeft = originalModel.playerWhite.WallsLeft;
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

