using System;
namespace quoridor
{
    public class Bot
    {
        public char PawnName { get; set; }

        public bool IsWorking { get; set; }


        public Bot(char pawnName, bool isWorking)
        {
            PawnName = pawnName;
            IsWorking = isWorking;
        }
    }
}

