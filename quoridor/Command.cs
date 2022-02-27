using System;
namespace quoridor
{
    public class Command
    {
        public string Name { get; set; } = string.Empty;

        public char ToCol { get; set; }

        public char ToRow { get; set; }

        public char? Orientation { get; set; } = null;


        public Command(string name = "", char toCol = 'p', char toRow = 'p', char? orientation = null)
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


        public void SetCoordinates(char toCol, char toRow)
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

