using System;
namespace quoridor
{
    public class Minimax
    {
        private Random rand = new Random();

        private Dictionary<char, List<Field>> GetPaths(Position position)
        {
            Dictionary<char, List<Field>> paths = new();
            List<Pawn> pawns = new();
            pawns.AddRange(position.PawnsOnBoard);
            for (int i = 0; i < pawns.Count; i++)
            {
                Pawn pawn = pawns[i];
                paths[pawn.Name] = position.GetShortestPathFor(pawn.Name);
            }
            return paths;
        }


        private int BestWeight(Dictionary<char, List<Field>> paths, Position position)
        {
            int weight = int.MaxValue;
            foreach (Pawn pawn in position.PawnsOnBoard)
            {
                if (pawn.Name != position.currentPlayer.PawnName)
                {
                    weight = Math.Min(weight, paths[pawn.Name][0].Length);
                }
            }
            return weight;
        }


        private double GetWeight(Position position)
        {
            Dictionary<char, List<Field>> currentPaths = GetPaths(position);
            int weight = BestWeight(currentPaths, position);
            int botWeight = currentPaths[position.currentPlayer.PawnName][0].Length;
            return GetWeight(botWeight, weight);
        }


        private double GetWeight(int botWeight, int weight)
        {
            if (botWeight == 0) return 0;
            if (weight == 0) return 1;
            return (double)botWeight / weight;
        }


        public Wall GetBestWall(Position position)
        {
            if (position.currentPlayer.WallsLeft == 0) return null;
            Wall bestWall = null;
            double bestWeight = int.MaxValue;
            Dictionary<char, List<Field>> paths = GetPaths(position);
            foreach (Pawn pawn in position.PawnsOnBoard)
            {
                if (pawn.Name != position.currentPlayer.PawnName)
                {
                    List<Field> path = paths[pawn.Name];
                    List<Wall> walls = new();
                    foreach (Field field in path)
                    {
                        Position currentPosition = new Position(position);
                        foreach (char type in new char[] { 'v', 'h' })
                            foreach (int row in new int[] { 0, -1 })
                                foreach (int col in new int[] { 0, -1 })
                                {
                                    Wall currentWall = new Wall(type, field.Pawn.Row + row, field.Pawn.Col + col);
                                    if (!QuoridorEngine.ContainsWall(currentWall, walls))
                                    {
                                        if (currentPosition.SetWall(currentWall.Orientation, currentWall.Row, currentWall.Col))
                                        {
                                            double currentWeight = 1 - GetWeight(currentPosition);
                                            if (currentWeight < bestWeight)
                                            {
                                                bestWeight = currentWeight;
                                                bestWall = currentWall;
                                            }
                                            else if (currentWeight == bestWeight && rand.Next(2) == 1)
                                            {
                                                bestWeight = currentWeight;
                                                bestWall = currentWall;
                                            }
                                            currentPosition = new Position(position);
                                        }
                                    }
                                    else
                                    {
                                        walls.Add(currentWall);
                                    }
                                }
                    }
                }
            }
            return bestWall;
        }


        public Pawn getBestPawn(QuoridorEngine quoridorEngine)
        {
            List<Field> path = quoridorEngine.GetShortestPathFor(quoridorEngine.currentPlayer.PawnName);
            if (path.Count == 1) return path[0].Pawn;
            return path[path.Count - 2].Pawn;
        }


        public double PerformBestAction(QuoridorEngine quoridorEngine, int depth)
        {
            Position pawnPosition = new Position(quoridorEngine);
            Pawn pawn = getBestPawn(pawnPosition);
            pawnPosition.MovePiece(pawn.Name, pawn.Col, pawn.Row);
            double pawnWeight = 1 - GetWeight(pawnPosition);
            Position wallPosition = new Position(quoridorEngine);
            Wall wall = GetBestWall(wallPosition);
            if (wall != null) wallPosition.SetWall(wall.Orientation, wall.Row, wall.Col);
            double wallWeight = 1 - GetWeight(wallPosition);
            if (wall != null) wallWeight = 1;

            if (depth > 0)
            {
                pawnWeight = PerformBestAction(pawnPosition, depth - 1);
                if (wall != null) wallWeight = PerformBestAction(wallPosition, depth - 1);
            }
            if (pawnWeight <= wallWeight || wall == null)
            {
                quoridorEngine.MovePiece(pawn.Name, pawn.Col, pawn.Row);
                return pawnWeight;
            }
            else
            {
                quoridorEngine.SetWall(wall.Orientation, wall.Row, wall.Col);
                return wallWeight;
            }
        }


        public void GetMove(QuoridorEngine quoridorEngine)
        {
            if (!quoridorEngine.IsGameEnded())
            {
                PerformBestAction(quoridorEngine, 3);
            }
            else
            {
                Console.ReadKey();
            }
            GC.Collect();
        }
    }
}

