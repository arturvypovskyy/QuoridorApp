using System;
namespace quoridor
{
    public class Field
    {
        public Pawn Pawn { get; set; }

        public int Length { get; set; }

        public int Weight { get; set; }


        public Field(Pawn pawn, int length)
        {
            Pawn = pawn;
            Length = length;
            int heuristicApproximation = pawn.Name == 'B' ? (9 - pawn.Row) * 10 : -10 * (1 - pawn.Row);
            Weight = Length + heuristicApproximation;
        }
    }
}

