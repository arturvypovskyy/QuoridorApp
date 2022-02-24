using System;
namespace quoridor
{
    public class Minimax
    {


        //public Minimax(char maximizerPawnName)
        //{
        //    MaximizerPawnName = maximizerPawnName;
        //}


        public int Algorithm(Position position, int depth, int alpha, int beta, bool isMaximizingPlayer)
        {
            if (depth == 0 || position.IsGameEnded())
            {
                return position.GetStaticEvaluation();
            }
            var children = GetChildrenOf(position);
            if (isMaximizingPlayer)
            {
                int maxEvaluation = int.MinValue;
                foreach (var child in children)
                {
                    int evaluation = Algorithm(position: child, depth: depth - 1, alpha: alpha, beta: beta, isMaximizingPlayer: false);
                    maxEvaluation = Math.Max(maxEvaluation, evaluation);
                    alpha = Math.Max(alpha, evaluation);
                    if (beta <= alpha)
                        break;
                }
                return maxEvaluation;
            }
            else
            {
                int minEvaluation = int.MaxValue;
                foreach (var child in children)
                {
                    int evaluation = Algorithm(position: child, depth: depth - 1, alpha: alpha, beta: beta, isMaximizingPlayer: true);
                    minEvaluation = Math.Min(minEvaluation, evaluation);
                    beta = Math.Min(beta, evaluation);
                    if (beta <= alpha)
                        break;
                }
                return minEvaluation;
            }
        }


        public static List<Position> GetChildrenOf(Position position)
        {
            var children = new List<Position>();
            var pawn = position.GetPawn(position.currentPlayer.PawnName);
            if (pawn is null)
                throw new ArgumentNullException(null, nameof(pawn));

            position.GetPossibleMoves(pawn);
            //to delete count
            //int count = 1;
            foreach (var possibleMove in position.possibleMoves)
            {
                var newPosition = new Position(position);
                newPosition.MovePiece(name: possibleMove.Name, toCol: possibleMove.Col, toRow: possibleMove.Row);
                children.Add(newPosition);
                //output
                //Console.WriteLine($"{count} Child of position");
                //foreach (var pawnOnBoard in newPosition.PawnsOnBoard)
                //{
                //    Console.WriteLine($"name: {pawnOnBoard.Name} col: {pawnOnBoard.Col} row: {pawnOnBoard.Row}");
                //}
                //count++;
            }
            return children;
        }


        public void GetMove(Position fromPosition)
        {
            int fromPositionEvaluation = Algorithm(position: fromPosition, depth: 10, alpha: int.MinValue, beta: int.MaxValue, isMaximizingPlayer: true); 
            var children = GetChildrenOf(fromPosition);
            foreach (var child in children)
            {
                var toPositionEvaluation = Algorithm(position: child, depth: 9, alpha: int.MinValue, beta: int.MaxValue, isMaximizingPlayer: false);
                if (fromPositionEvaluation == toPositionEvaluation)
                {
                    foreach (var pawn in child.PawnsOnBoard)
                    {
                        if (!fromPosition.PawnsOnBoard.Where(x => x.Name == pawn.Name && x.Col == pawn.Col && x.Row == pawn.Row).Any())
                        {
                            Console.WriteLine($"move({pawn.Name} {pawn.Col} {pawn.Row})");
                            break;
                        }
                    }
                }
            }
        }
    }
}

