using System;
namespace quoridor
{
    public class Minimax
    {
        private Random rand = new Random();


        private Player GetPlayer(Position position, char name)
        {
            return position.playerBlack.PawnName == name ? position.playerBlack : position.playerWhite;
        }


        private Dictionary<char, List<Field>> GetPaths(Position position)
        {
            Dictionary<char, List<Field>> paths = new();
            List<Pawn> pawns = new();
            pawns.AddRange(position.PawnsOnBoard);
            for (int i = 0; i < pawns.Count; i++)
            {
                Pawn pawn = pawns[i];
            restart:
                try
                {
                    paths[pawn.Name] = position.GetShortestPathFor(pawn.Name);
                    if (paths[pawn.Name] == null) goto restart;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    goto restart;
                }
            }
            return paths;
        }


        private int BestWeight(Dictionary<char, List<Field>> paths, Position position, char name)
        {
            int weight = int.MaxValue;
            foreach (Pawn pawn in position.PawnsOnBoard)
            {
                if (pawn.Name != name)
                {
                    weight = Math.Min(weight, paths[pawn.Name][0].Length);
                }
            }
            return weight;
        }


        private double GetWeight(Position position, char name)
        {
            Dictionary<char, List<Field>> currentPaths = GetPaths(position);
            int weight = BestWeight(currentPaths, position, name);
            int botWeight = currentPaths[name][0].Length;
            return GetWeight(botWeight, weight);
        }


        private double GetWeight(int botWeight, int weight)
        {
            return botWeight - weight;
        }


        public Wall GetBestWall(Position position, char name)
        {
            if (GetPlayer(position, name).WallsLeft == 0) return null;
            Wall bestWall = null;
            double bestWeight = int.MaxValue;
            Dictionary<char, List<Field>> paths = GetPaths(position);
            foreach (Pawn pawn in position.PawnsOnBoard)
            {
                if (pawn.Name != name)
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
                                            double currentWeight = GetWeight(currentPosition, name);
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


        public Pawn getBestPawn(QuoridorEngine quoridorEngine, char name)
        {
            List<Field> path = quoridorEngine.GetShortestPathFor(name);
            if (path.Count == 1) return path[0].Pawn;
            return path[path.Count - 2].Pawn;
        }


        public double PerformBestAction(QuoridorEngine quoridorEngine, int depth, char name)
        {
            Position pawnPosition = new Position(quoridorEngine);
            Pawn pawn = getBestPawn(pawnPosition, name);
            pawnPosition.MovePiece(pawn.Name, pawn.Col, pawn.Row);
            double pawnWeight = GetWeight(pawnPosition, name);
            Position wallPosition = new Position(quoridorEngine);
            Wall wall = GetBestWall(wallPosition, name);
            if (wall != null) wallPosition.SetWall(wall.Orientation, wall.Row, wall.Col);
            double wallWeight = GetWeight(wallPosition, name);

            if (depth > 0)
            {
                pawnWeight = PerformBestAction(pawnPosition, depth - 1, name);
                if (wall != null) wallWeight = PerformBestAction(wallPosition, depth - 1, name);
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
                PerformBestAction(quoridorEngine, 3, quoridorEngine.currentPlayer.PawnName);
            }
            else
            {
                Console.ReadKey();
            }
            GC.Collect();
            //Console.WriteLine($"move({pawn.Name} {pawn.Col} {pawn.Row})");
        }
    }
}

