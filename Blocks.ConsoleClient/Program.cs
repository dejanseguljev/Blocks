using Blocks.Core;
using System;

namespace Blocks.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new Board(new ConsoleRenderer());
            board.Start();

            while (true)
            {
                var command = Console.ReadKey();
                if (command.Key == ConsoleKey.LeftArrow)
                {
                    board.Transform("MoveLeft");
                }
                else if (command.Key == ConsoleKey.RightArrow)
                {
                    board.Transform("MoveRight");
                }
                else if (command.Key == ConsoleKey.UpArrow)
                {
                    board.Transform("Rotate");
                }
            }
        }
    }
}
