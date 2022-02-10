using System;
namespace quoridor
{
    class Program
    {
        public static int Main(string[] args)
        {
            
            BoardView board = new();
            board.DrawBoard();
            return (int)ReturnCode.Success;
        }
    }
}

