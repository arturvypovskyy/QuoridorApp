using System;
namespace quoridor
{
    public class Command
    {
        public string Name { get; set; } = string.Empty;

        public int ToCol { get; set; } = -1;

        public int ToRow { get; set; } = -1;

        public char? Orientation { get; set; } = null;


        public Command(string name = "", int toRow = -1, int toCol = -1, char? orientation = null)
        {
            Name = name;
            ToCol = toCol;
            ToRow = toRow;
            Orientation = orientation;
        }


        public void SetName(string name)
        {
            Name = name;
        }


        public void SetCoordinates(int toRow, int toCol)
        {
            ToCol = toCol;
            ToRow = toRow;
        }


        public void SetOrientation(char orientation)
        {
            Orientation = orientation;
        }
    }
}

