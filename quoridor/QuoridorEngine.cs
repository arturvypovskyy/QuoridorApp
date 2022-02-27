using System;

namespace quoridor
{
    public class QuoridorEngine
    {
        public List<Pawn> PawnsOnBoard = new();

        public List<Wall> WallsOnBoard = new();

        public List<Pawn> possibleMoves = new();

        public List<Wall> possibleWalls = new();

        public readonly Player playerWhite = new('W', 10);

        public readonly Player playerBlack = new('B', 10);

        public Player currentPlayer;



        public QuoridorEngine() {
            currentPlayer = playerWhite;
        }


        public void GameInitializer()
        {
            PawnsOnBoard.Clear();
            WallsOnBoard.Clear();
            PawnsOnBoard.Add(new Pawn(name: 'W', col: 5, row: 9));
            PawnsOnBoard.Add(new Pawn(name: 'B', col: 5, row: 1));
            //PawnsOnBoard.Add(new Pawn(name: 'W', col: 5, row: 5));
            //PawnsOnBoard.Add(new Pawn(name: 'B', col: 6, row: 4));
            GetAllPossibleWalls();
        }


        public void MovePiece(char name, int toCol, int toRow)
        {
            if (IsGameEnded())
            {
                return;
            }
            Pawn? pawn = GetPawn(name);
            if (pawn is not null)
            {
                GetPossibleMoves(pawn);
                if (!ContainsPawn(new Pawn(name, toCol, toRow), possibleMoves))
                {
                    //Console.WriteLine("Forbidden move");
                   // ShowPossibleMoves(possibleMoves);
                }
                else
                {
                    PawnsOnBoard.Add(new Pawn(name: pawn.Name, col: toCol, row: toRow));
                    PawnsOnBoard.Remove(pawn);
                    if (!IsGameEnded())
                    {
                        ChangePlayer();
                    }
                }
            }
            //else
            //{
            //    Console.WriteLine("Unknown pawn");
            //}
        }


        //private static void ShowPossibleMoves(List<Pawn> pawns)
        //{
        //    Console.WriteLine("Possible moves:");
        //    foreach (var pawn in pawns)
        //    {
        //        Console.WriteLine($"{pawn.Name} col: {pawn.Col}  row: {pawn.Row}");
        //    }
        //}


        private static bool ContainsPawn(Pawn pawn, List<Pawn> pawns)
        {
            foreach (var possiblePawn in pawns)
            {
                if (pawn.Name == possiblePawn.Name &&
                    pawn.Row == possiblePawn.Row &&
                    pawn.Col == possiblePawn.Col)
                {
                    return true;
                }
            }
            return false;
        }


        public void GetPossibleMoves(Pawn pawn)
        {
            possibleMoves.Clear();
            //adding possible moves from the start
            possibleMoves.Add(new Pawn(pawn.Name, col: pawn.Col, row: pawn.Row + 1));
            possibleMoves.Add(new Pawn(pawn.Name, col: pawn.Col, row: pawn.Row - 1));
            possibleMoves.Add(new Pawn(pawn.Name, col: pawn.Col + 1, row: pawn.Row));
            possibleMoves.Add(new Pawn(pawn.Name, col: pawn.Col - 1, row: pawn.Row));
            //adding possible moves to jump and removing pawn layering
            //forward
            var pawnsToJump = PawnsOnBoard
                .Select(x => x)
                .Where(x => x.Col == pawn.Col && x.Row == pawn.Row - 1);
            if (pawnsToJump.Any())
            {
                possibleMoves.Add(new Pawn(name: pawn.Name, col: pawn.Col, row: pawn.Row - 2));
                possibleMoves.RemoveAll(x => x.Col == pawn.Col && x.Row == pawn.Row - 1);
            }
            //backward
            pawnsToJump = PawnsOnBoard
                .Select(x => x)
                .Where(x => x.Col == pawn.Col && x.Row == pawn.Row + 1);
            if (pawnsToJump.Any())
            {
                possibleMoves.Add(new Pawn(name: pawn.Name, col: pawn.Col, row: pawn.Row + 2));
                possibleMoves.RemoveAll(x => x.Col == pawn.Col && x.Row == pawn.Row + 1);
            }
            //to the left
            pawnsToJump = PawnsOnBoard
                .Select(x => x)
                .Where(x => x.Col == pawn.Col - 1 && x.Row == pawn.Row);
            if (pawnsToJump.Any())
            {
                possibleMoves.Add(new Pawn(name: pawn.Name, col: pawn.Col - 2, row: pawn.Row));
                possibleMoves.RemoveAll(x => x.Col == pawn.Col - 1 && x.Row == pawn.Row);
            }
            //to the right
            pawnsToJump = PawnsOnBoard
                .Select(x => x)
                .Where(x => x.Col == pawn.Col + 1 && x.Row == pawn.Row);
            if (pawnsToJump.Any())
            {
                possibleMoves.Add(new Pawn(name: pawn.Name, col: pawn.Col + 2, row: pawn.Row));
                possibleMoves.RemoveAll(x => x.Col == pawn.Col + 1 && x.Row == pawn.Row);
            }

            //removing possible moves according to walls
            //wall forward
            if (ContainsWall(new Wall(orientation: 'h', col: pawn.Col, row: pawn.Row - 1), WallsOnBoard) ||
                ContainsWall(new Wall(orientation: 'h', col: pawn.Col - 1, row: pawn.Row - 1), WallsOnBoard))
            {
                possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col && x.Row == pawn.Row - 1);
                possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col && x.Row == pawn.Row - 2);
            }
            if (ContainsWall(new Wall(orientation: 'h', col: pawn.Col, row: pawn.Row - 2), WallsOnBoard) ||
                ContainsWall(new Wall(orientation: 'h', col: pawn.Col - 1, row: pawn.Row - 2), WallsOnBoard))
            {
                possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col && x.Row == pawn.Row - 2);
            }

            //wall backward
            if (ContainsWall(new Wall(orientation: 'h', col: pawn.Col, row: pawn.Row), WallsOnBoard) ||
                ContainsWall(new Wall(orientation: 'h', col: pawn.Col - 1, row: pawn.Row), WallsOnBoard))
            {
                possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col && x.Row == pawn.Row + 1);
                possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col && x.Row == pawn.Row + 2);
            }
            if (ContainsWall(new Wall(orientation: 'h', col: pawn.Col, row: pawn.Row + 1), WallsOnBoard) ||
                ContainsWall(new Wall(orientation: 'h', col: pawn.Col - 1, row: pawn.Row + 1), WallsOnBoard))
            {
                possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col && x.Row == pawn.Row + 2);
            }
            //wall to the left
            if (ContainsWall(new Wall(orientation: 'v', col: pawn.Col - 1, row: pawn.Row), WallsOnBoard) ||
                ContainsWall(new Wall(orientation: 'v', col: pawn.Col - 1, row: pawn.Row - 1), WallsOnBoard))
            {
                possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col - 1 && x.Row == pawn.Row);
                possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col - 2 && x.Row == pawn.Row);
            }
            if (ContainsWall(new Wall(orientation: 'v', col: pawn.Col - 2, row: pawn.Row), WallsOnBoard) ||
                ContainsWall(new Wall(orientation: 'v', col: pawn.Col - 2, row: pawn.Row - 1), WallsOnBoard))
            {
                possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col - 2 && x.Row == pawn.Row);
            }
            //wall to the right
            if (ContainsWall(new Wall(orientation: 'v', col: pawn.Col, row: pawn.Row), WallsOnBoard) ||
                ContainsWall(new Wall(orientation: 'v', col: pawn.Col, row: pawn.Row - 1), WallsOnBoard))
            {
                possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col + 1 && x.Row == pawn.Row);
                possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col + 2 && x.Row == pawn.Row);
            }
            if (ContainsWall(new Wall(orientation: 'v', col: pawn.Col + 1, row: pawn.Row), WallsOnBoard)
                || ContainsWall(new Wall(orientation: 'v', col: pawn.Col + 1, row: pawn.Row - 1), WallsOnBoard))
            {
                possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col + 2 && x.Row == pawn.Row);
            }
            //diagonal moves
            //forward
            if (PawnsOnBoard.Where(x => x.Col == pawn.Col && x.Row == pawn.Row - 1).Any())
            {
                if (WallsOnBoard.Where(x => x.Orientation == 'h' && x.Col == pawn.Col && x.Row == pawn.Row - 2).Any()
                    || WallsOnBoard.Where(x => x.Orientation == 'h' && x.Col == pawn.Col - 1 && x.Row == pawn.Row - 2).Any()
                    || PawnsOnBoard.Where(x => x.Row == 1).Any())
                {
                    possibleMoves.Add(new Pawn(pawn.Name, pawn.Col - 1, pawn.Row - 1));
                    possibleMoves.Add(new Pawn(pawn.Name, pawn.Col + 1, pawn.Row - 1));

                    if (WallsOnBoard.Where(x => x.Orientation == 'v' && x.Col == pawn.Col && x.Row == pawn.Row - 2).Any()
                        || WallsOnBoard.Where(x => x.Orientation == 'v' && x.Col == pawn.Col && x.Row == pawn.Row - 1).Any())
                    {
                        possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col + 1 && x.Row == pawn.Row - 1);
                    }
                    if (WallsOnBoard.Where(x => x.Orientation == 'v' && x.Col == pawn.Col - 1 && x.Row == pawn.Row - 2).Any()
                        || WallsOnBoard.Where(x => x.Orientation == 'v' && x.Col == pawn.Col - 1 && x.Row == pawn.Row - 1).Any())
                    {
                        possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col - 1 && x.Row == pawn.Row - 1);
                    }
                }
            }
            //backward
            if (PawnsOnBoard.Where(x => x.Col == pawn.Col && x.Row == pawn.Row + 1).Any())
            {
                if (WallsOnBoard.Where(x => x.Orientation == 'h' && x.Col == pawn.Col && x.Row == pawn.Row + 1).Any()
                    || WallsOnBoard.Where(x => x.Orientation == 'h' && x.Col == pawn.Col - 1 && x.Row == pawn.Row + 1).Any()
                    || PawnsOnBoard.Where(x => x.Row == 9).Any())
                {
                    possibleMoves.Add(new Pawn(pawn.Name, pawn.Col - 1, pawn.Row + 1));
                    possibleMoves.Add(new Pawn(pawn.Name, pawn.Col + 1, pawn.Row + 1));

                    if (WallsOnBoard.Where(x => x.Orientation == 'v' && x.Col == pawn.Col && x.Row == pawn.Row).Any()
                        || WallsOnBoard.Where(x => x.Orientation == 'v' && x.Col == pawn.Col && x.Row == pawn.Row + 1).Any())
                    {
                        possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col + 1 && x.Row == pawn.Row + 1);
                    }
                    if (WallsOnBoard.Where(x => x.Orientation == 'v' && x.Col == pawn.Col - 1 && x.Row == pawn.Row).Any()
                        || WallsOnBoard.Where(x => x.Orientation == 'v' && x.Col == pawn.Col - 1 && x.Row == pawn.Row + 1).Any())
                    {
                        possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col - 1 && x.Row == pawn.Row + 1);
                    }
                }
            }
            //to the left
            if (PawnsOnBoard.Where(x => x.Col == pawn.Col - 1 && x.Row == pawn.Row).Any())
            {
                if (WallsOnBoard.Where(x => x.Orientation == 'v' && x.Col == pawn.Col - 2 && x.Row == pawn.Row - 1).Any()
                    || WallsOnBoard.Where(x => x.Orientation == 'v' && x.Col == pawn.Col - 2 && x.Row == pawn.Row).Any()
                    || PawnsOnBoard.Where(x => x.Col == 1).Any())
                {
                    possibleMoves.Add(new Pawn(pawn.Name, pawn.Col - 1, pawn.Row - 1));
                    possibleMoves.Add(new Pawn(pawn.Name, pawn.Col - 1, pawn.Row + 1));

                    if (WallsOnBoard.Where(x => x.Orientation == 'h' && x.Col == pawn.Col - 1 && x.Row == pawn.Row - 1).Any()
                    || WallsOnBoard.Where(x => x.Orientation == 'h' && x.Col == pawn.Col - 2 && x.Row == pawn.Row - 1).Any())
                    {
                        possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col - 1 && x.Row == pawn.Row - 1);
                    }
                    if (WallsOnBoard.Where(x => x.Orientation == 'h' && x.Col == pawn.Col - 2 && x.Row == pawn.Row).Any()
                    || WallsOnBoard.Where(x => x.Orientation == 'h' && x.Col == pawn.Col - 1 && x.Row == pawn.Row).Any())
                    {
                        possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col - 1 && x.Row == pawn.Row + 1);
                    }
                }
            }
            //to the right
            if (PawnsOnBoard.Where(x => x.Col == pawn.Col + 1 && x.Row == pawn.Row).Any())
            {
                if (WallsOnBoard.Where(x => x.Orientation == 'v' && x.Col == pawn.Col + 1 && x.Row == pawn.Row - 1).Any()
                    || WallsOnBoard.Where(x => x.Orientation == 'v' && x.Col == pawn.Col + 1 && x.Row == pawn.Row).Any()
                    || PawnsOnBoard.Where(x => x.Col == 9).Any())
                {
                    possibleMoves.Add(new Pawn(pawn.Name, pawn.Col + 1, pawn.Row - 1));
                    possibleMoves.Add(new Pawn(pawn.Name, pawn.Col + 1, pawn.Row + 1));

                    if (WallsOnBoard.Where(x => x.Orientation == 'h' && x.Col == pawn.Col && x.Row == pawn.Row - 1).Any()
                    || WallsOnBoard.Where(x => x.Orientation == 'h' && x.Col == pawn.Col + 1 && x.Row == pawn.Row - 1).Any())
                    {
                        possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col + 1 && x.Row == pawn.Row - 1);
                    }
                    if (WallsOnBoard.Where(x => x.Orientation == 'h' && x.Col == pawn.Col && x.Row == pawn.Row).Any()
                    || WallsOnBoard.Where(x => x.Orientation == 'h' && x.Col == pawn.Col + 1 && x.Row == pawn.Row).Any())
                    {
                        possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col + 1 && x.Row == pawn.Row + 1);
                    }
                }
            }


            //removing impossible moves cause of boards
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                Pawn possibleMove = possibleMoves[i];
                if (possibleMove.Col > 9 ||
                    possibleMove.Col < 1 ||
                    possibleMove.Row > 9 ||
                    possibleMove.Row < 1)
                {
                    possibleMoves.RemoveAll(x => x.Col == possibleMove.Col && x.Row == possibleMove.Row);
                    i--;
                }
            }
        }


        public bool SetWall(char? orientation, int toRow, int toCol)
        {
            if (IsGameEnded())
            {
                return false;
            }
            if (orientation is null)
            {
                //Console.WriteLine("Incorrect orientation");
            }
            else if (currentPlayer.WallsLeft == 0)
            {
                //Console.WriteLine("No walls left");
            }
            else if (!ContainsWall(new Wall(orientation: orientation, row: toRow, col: toCol), possibleWalls))
            {
                //Console.WriteLine("Forbidden wall");
            }
            else
            {
                WallsOnBoard.Add(new Wall(orientation, toRow, toCol));
                
           
                if(GetShortestPathFor('W') is not null && GetShortestPathFor('B') is not null)
                {
                    currentPlayer.WallsLeft -= 1;

                    possibleWalls.RemoveAll(x => x.Orientation == 'h' && x.Col == toCol && x.Row == toRow);
                    possibleWalls.RemoveAll(x => x.Orientation == 'v' && x.Col == toCol && x.Row == toRow);

                    if (orientation == 'h')
                    {
                        possibleWalls.RemoveAll(x => x.Orientation == 'h' && x.Col == toCol + 1 && x.Row == toRow);
                        possibleWalls.RemoveAll(x => x.Orientation == 'h' && x.Col == toCol - 1 && x.Row == toRow);
                    }
                    else if (orientation == 'v')
                    {
                        possibleWalls.RemoveAll(x => x.Orientation == 'v' && x.Col == toCol && x.Row == toRow + 1);
                        possibleWalls.RemoveAll(x => x.Orientation == 'v' && x.Col == toCol && x.Row == toRow - 1);
                    }
                    ChangePlayer();
                    return true;
                }
                else
                {
                    WallsOnBoard.RemoveAll(x => x.Orientation == orientation && x.Row == toRow && x.Col == toCol);
                    //Console.WriteLine("Impossible wall");
                }

            }
            return false;
        }


        private void GetAllPossibleWalls()
        {
            possibleWalls.Clear();
            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    possibleWalls.Add(new Wall(orientation: 'h', col: i, row: j));
                    possibleWalls.Add(new Wall(orientation: 'v', col: i, row: j));
                }
            }
        }


        public static bool ContainsWall(Wall wall, List<Wall> possibleWalls)
        {
            foreach (var possibleWall in possibleWalls)
            {
                if (possibleWall.Col == wall.Col &&
                    possibleWall.Row == wall.Row &&
                    possibleWall.Orientation == wall.Orientation)
                {
                    return true;
                }
            }
            return false;
        }


        public Pawn? GetPawn(char name)
        {
            foreach (var pawn in PawnsOnBoard)
            {
                if (pawn.Name == name)
                {
                    return pawn;
                }
            }
            return null;
        }


        public void ChangePlayer()
        {
            if (currentPlayer.PawnName == 'W')
            {
                currentPlayer = playerBlack;
            }
            else
            {
                currentPlayer = playerWhite;
            }
        }


        public List<Field>? GetShortestPathFor(char playerName)
        {
            int sumWalls = 20 - (playerBlack.WallsLeft + playerWhite.WallsLeft);
            return  GetShortestPathFor(playerName,0, new int[] {10 + 5 * sumWalls}, new List<Field>());
        }


        private List<Field>? GetShortestPathFor(char playerName, int length, int[] min, List<Field> allFields)
        {
            if (length > min[0]) return null;
            List<Field> minPath = null;
            var currentPawn = GetPawn(playerName);
            if (currentPawn is null)
                throw new ArgumentNullException(null, nameof(currentPawn));
            //creating first current field
            var currentField = new Field(pawn: currentPawn, length: length);
            for (int i = 0; i < allFields.Count; i++)
            {
                Field field = allFields[i];
                if (field.Pawn.Col == currentField.Pawn.Col && field.Pawn.Row == currentField.Pawn.Row)
                {
                    if (currentField.Length >= field.Length)
                    {
                        return null;
                    }
                    allFields.Remove(field);
                }
            }
            allFields.Add(currentField);
            if (IsGameEnded())
            {
                minPath = new();
                minPath.Add(currentField);
                return minPath;
            }
            //star finding path
            //finding the lowest cost field
            GetPossibleMoves(currentPawn);
            List<Pawn> allMoves = new();
            allMoves.AddRange(possibleMoves);
            PawnsOnBoard.Remove(currentPawn);
            foreach (Pawn pawn in allMoves)
            {
                PawnsOnBoard.Add(pawn);
                var path = GetShortestPathFor(playerName, length + 1, min, allFields);
                if (path != null)
                {
                    if (minPath == null)
                    {
                        minPath = path;
                    }
                    else
                    {
                        Field minField = minPath[0];
                        Field curField = path[0];
                        if (minField.Length > curField.Length)
                        {
                            minPath = path;
                        }
                    }
                    min[0] = minPath[0].Length;
                }
                PawnsOnBoard.Remove(pawn);
            }
            PawnsOnBoard.Add(currentPawn);
            if (minPath != null)
            {
                minPath.Add(currentField);
            }
            return minPath;
        }


        //public Stack<Field>? GetShortestPathFor(char playerName)
        //{
        //    var openList = new List<Field>();
        //    var forbidenPawns = new List<Pawn>();
        //    var realForbidenPawns = new List<Pawn>();
        //    var path = new Stack<Field>();

        //    //finding where path begins 
        //    var startPawn = GetPawn(playerName);
        //    if (startPawn is null)
        //        throw new ArgumentNullException(null, nameof(startPawn));
        //    var currentField = new Field(pawn: startPawn, length: 0);
        //    // saving pawns on board positions and removing pawns from the board
        //    PawnsOnBoard.RemoveAll(x => x.Name == startPawn.Name && x.Col == startPawn.Col && x.Row == startPawn.Row);
        //    int counter = 0;
        //    while (true)
        //    {
        //        //adding current field to closed list and path stack
        //        path.Push(currentField);
        //        //Console.WriteLine($"name: {currentField.Pawn.Name} col: {currentField.Pawn.Col} row: {currentField.Pawn.Row} ");
        //        forbidenPawns.Add(currentField.Pawn);
        //        //Console.WriteLine($"name : {currentField.Pawn.Name} " +
        //        //   $"col: {currentField.Pawn.Col} row: {currentField.Pawn.Row} weight: {currentField.Weight}");
        //        //if we reached the goal row
        //        int goalRow = playerName == 'W' ? 1 : 9;
        //        foreach (var field in openList)
        //        {
        //            //if we reached the goal row
        //            if (field.Pawn.Row == goalRow)
        //            {
        //                // adding removed pawns to the board
        //                PawnsOnBoard.Add(startPawn);
        //                return path;
        //            }
        //        }

        //        //generating open list
        //        GetPossibleMoves(currentField.Pawn);
        //        foreach (var pawn in forbidenPawns)
        //        {
        //            possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col && x.Row == pawn.Row);
        //        }
        //        foreach (var pawn in realForbidenPawns)
        //        {
        //            possibleMoves.RemoveAll(x => x.Name == pawn.Name && x.Col == pawn.Col && x.Row == pawn.Row);
        //        }

        //        //if there is no possible moves
        //        if (possibleMoves.Count == 0)
        //        {
        //            if (path.Count == 1)
        //            {
        //                // adding removed pawns to the board
        //                PawnsOnBoard.Add(startPawn);
        //                return null;
        //            }
        //            else
        //            {
        //                forbidenPawns.Clear();
        //                realForbidenPawns.Add(currentField.Pawn);
        //                currentField = new Field(startPawn, 0);
        //                path.Clear();
        //                openList.Clear();
        //                continue;
        //            }
        //        }
        //        //clear open list from previous iteration
        //        openList.Clear();
        //        foreach (var possibleMove in possibleMoves)
        //        {
        //            openList.Add(new Field(possibleMove, currentField.Length + 10));
        //        }
        //        //searching for min weight field in open list
        //        var minWeightField = openList[0];
        //        for (int i = 1; i < openList.Count; i++)
        //        {
        //            if (openList[i].Weight < minWeightField.Weight)
        //            {
        //                minWeightField = openList[i];
        //            }
        //        }
        //        //setting new current field
        //        currentField = minWeightField;
        //        counter++;
        //    }
        //}


        public void Exit()
        {
            Environment.Exit((int)ReturnCode.Success);
        }


        public bool IsGameEnded()
        {
            return PawnsOnBoard.Where(x => (x.Name == 'W' && x.Row == 1)
            || (x.Name == 'B' && x.Row == 9)).Any();
        } 
    }
}

