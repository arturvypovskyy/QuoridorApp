using System;
namespace quoridor
{
    class Program
    {
        public static int Main(string[] args)
        {
            Controller controller = new();
            controller.Start();
            return (int)ReturnCode.Success;
        }
    }
}

