using Blocks.Core;
using System;

namespace Blocks.ConsoleClient
{
    public class ConsoleRenderer : IRenderer
    {
        public void Render(int x, int y, int width, int height, BlockMap blockMap, BlockMap background = null)
        {
            for (int j = 0; j < height; j++)
            {
                Console.SetCursorPosition(x, j + y);

                for (int i = 0; i < width; i++)
                {
                    if (background != null)
                    {
                        if (blockMap[i + x - blockMap.X, j + y - blockMap.Y])
                        {
                            Console.Write("#");
                        }
                        else
                        {
                            Console.Write(background[i + x, j + y] ? "#" : " ");
                        }
                    }
                    else
                    {
                        Console.Write(blockMap[i + x - blockMap.X, j + y - blockMap.Y] ? "#" : " ");
                    }
                }
            }
        }
    }
}
