using System;
using System.Threading;

namespace Blocks.Core
{
    public class Board : BlockMap
    {
        private int _score;
        private int _blocks;
        private Block _currentBlock;
        private Timer _timer;
        private readonly IRenderer _renderer;
        private object _syncRoot = new object();

        public const int DefaultWidth = 40;
        public const int DefaultHeight = 20;

        public Board(IRenderer renderer, int width = DefaultWidth, int height = DefaultHeight)
        {
            _renderer = renderer;

            BuildMap(width, height);
        }

        public void Transform(string name)
        {
            lock (_syncRoot)
            {
                var block = _currentBlock;

                if (block == null)
                {
                    return;
                }

                switch (name)
                {
                    case "MoveLeft":
                        if (block.X <= 0)
                        {
                            return;
                        }

                        if (Fits(block.X - 1, block.Y, block))
                        {
                            _renderer.Render(block.X, block.Y, block.Width, block.Height, this);
                            block.X -= 1;
                            _renderer.Render(block.X, block.Y, block.Width, block.Height, block, this);
                        }
                        break;
                    case "MoveRight":
                        if ((block.X + block.Width + 1) > Width)
                        {
                            return;
                        }

                        if (Fits(block.X + 1, block.Y, block))
                        {
                            _renderer.Render(block.X, block.Y, block.Width, block.Height, this);
                            block.X += 1;
                            _renderer.Render(block.X, block.Y, block.Width, block.Height, block, this);
                        }
                        break;
                    default:

                        var x = block.X;
                        var y = block.Y;
                        var width = block.Width;
                        var height = block.Height;

                        block.Transform(name);

                        if (Fits(block.X, block.Y, block))
                        {
                            _renderer.Render(x, y, width, height, this);
                            _renderer.Render(block.X, block.Y, block.Width, block.Height, block, this);
                        }
                        else
                        {
                            block.Transform(name);
                        }
                        break;
                }
            }
        }

        public void Start()
        {
            Console.CursorVisible = false;

            _timer = new System.Threading.Timer(new TimerCallback(Tick));
            _timer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(500));

            _renderer.Render(0, 0, Width, Height, this);
            DisplayScore();
        }

        public void Stop()
        {
            _timer.Dispose();
           
        }

        public void AddBlock(Block block)
        {
            

        }

        private void Tick(object? state)
        {
            lock (_syncRoot)
            {
                if (_currentBlock == null)
                {
                    _blocks++;
                    _score++;
                    _currentBlock = BlockShapeFactory.Build();
                    _currentBlock.X = Width / 2 - _currentBlock.Width / 2;
                    _currentBlock.Y = 0;

                    if (!Fits(_currentBlock.X, _currentBlock.Y, _currentBlock))
                    {
                        GameOver();
                    }

                    _currentBlock.OnStarted();
                }

                if (Fits(_currentBlock.X, _currentBlock.Y + 1, _currentBlock))
                {
                    _renderer.Render(_currentBlock.X, _currentBlock.Y, _currentBlock.Width, _currentBlock.Height, this);
                    _currentBlock.Y++;
                    _renderer.Render(_currentBlock.X, _currentBlock.Y, _currentBlock.Width, _currentBlock.Height, _currentBlock, this);
                }
                else
                {
                    _currentBlock.OnStopped();

                    int removed = Merge(_currentBlock);

                    if (removed > 0)
                    {
                        _score += removed * 20;
                        _renderer.Render(0, 0, Width, _currentBlock.Y + _currentBlock.Height, this);
                    }
                    else
                    {
                        _renderer.Render(_currentBlock.X, _currentBlock.Y, _currentBlock.Width, _currentBlock.Height, this);
                    }

                    _currentBlock = null;
                }

                UpdateScore();
            }
        }


        private void DisplayScore()
        {
            Console.SetCursorPosition(Width + 2, 2);
            Console.Write("Score:");

            UpdateScore();
        }
        private void UpdateScore()
        {
            Console.SetCursorPosition(Width + 2, 4);
            Console.Write(_score);            
        }

        private void GameOver()
        {
            Console.SetCursorPosition(Width + 2, 6);
            Console.Write("Game Over");
            _timer.Dispose();
        }
    } 
}
