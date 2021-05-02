using System.Collections.Generic;

namespace Blocks.Core
{
    public abstract class BlockMap
    {
        private BlockLine[] _lines;
        private int _maxOccupied = -1;

        public int X { get; set; }
        public int Y { get; set; }
        public int Width {
            get {
                var length = _lines[0]?.Map?.Length;
                return length.HasValue ? length.Value : 0;
            }
        }
        public int Height {
            get {
                var length = _lines?.Length;
                return length.HasValue ? length.Value : 0;
            }
        }

        public virtual bool this[int x, int y] {
            get {
                return _lines[y].Map[x];
            }
            set {
                var oldValue = _lines[y].Map[x];
                _lines[y].Map[x] = value;

                if (oldValue != value)
                {
                    _lines[y].Occupied += value ? 1 : -1;
                }
            }
        }
        public void Rotate()
        {
            var old = _lines;
            BuildMap(Height, Width);
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    this[i, j] = old[i].Map[j];
                }
            }
        }

        public void RotateInverse()
        {
            var old = _lines;
            BuildMap(Height, Width);
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    this[i, j] = old[i].Map[j];
                }
            }
        }

        public virtual void RemoveLine(int index)
        {
            for (int i = index; i > 0; i--)
            {
                _lines[i] = _lines[i - 1];
            }

            _lines[0] = new BlockLine()
            {
                Map = new bool[Width]
            };
        }

        public virtual bool Fits(int x, int y, BlockMap other)
        {
            if (y + other.Height < _maxOccupied)
            {
                return true;
            }

            if (y + other.Height > Height || x + other.Width > Width)
            {
                return false;
            }

            for (int j = other.Height - 1; j >= 0 ; j--)
            {
                if (_lines[j + y].Occupied > 0)
                {
                    for (int i = 0; i < other.Width; i++)
                    {
                        if (other[i, j] && this[i + x, j + y])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public virtual int Merge(BlockMap other)
        {
            var toRemove = new List<int>();

            for (int j = 0; j < other.Height; j++)
            {
                for (int i = 0; i < other.Width; i++)
                {
                    if (other[i, j])
                    {
                        this[i + other.X, j + other.Y] = true;
                    }
                }

                if (_lines[j + other.Y].Occupied == Width)
                {
                    toRemove.Add(j + other.Y);
                }
            }

            foreach (var k in toRemove)
            {
                RemoveLine(k);
            }

            return toRemove.Count;
        }

        protected virtual void BuildMap(int width, int height)
        {
            _lines = new BlockLine[height];

            for (int i = 0; i < height; i++)
            {
                _lines[i] = new BlockLine() { Map = new bool[width] };
            }
        }

        private class BlockLine
        {
            public int Occupied { get; set; }
            public bool[] Map { get; set; }
        }
    }
}
