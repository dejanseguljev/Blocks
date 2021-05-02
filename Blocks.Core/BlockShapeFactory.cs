using System;
using System.Collections.Generic;

namespace Blocks.Core
{
    public static class BlockShapeFactory
    {
        private static readonly List<(int, int)[]> _shapes = new List<(int, int)[]>()
        {
            new (int, int)[]
            {
                (0,0), (0,1), (0,2),
                (1,0), (1,1), (1,2),
                (2,0), (2,1), (2,2)
            },
            new (int, int)[]
            {
                (0,0), (0,1), (0,2)
            },
            new (int, int)[]
            {
                (0,0), (1,0), (2,0)
            },
            new (int, int)[]
            {
                (0,0), (0,1), (0,2),
                              (1,2),
                              (2,2)
            },
            new (int, int)[]
            {
                (0,2),
                (1,1),
                (2,0),
            },
             new (int, int)[]
            {
                       (0,1),
                (1,0), (1,1), (1,2),
                       (2,1)
            },
            new (int, int)[]
            {
                (0,0),        (0,2),
                (1,0), (1,1), (1,2),
                       (2,1),
            },
            new (int, int)[]
            {
                (0,0),
                (1,1),
                (2,2),
            },
        };

        private static readonly List<(int, int)> _dimensions = new List<(int, int)>()
        {
                (3,3), (1,3), (3,1), (3,3), (3,3), (3,3), (3,3), (3,3)
        };

        public static Block Build()
        {
            var rand = new Random();

            var shapeIndex = rand.Next(0, _shapes.Count - 1);
            var data = _shapes[shapeIndex];
            var dim = _dimensions[shapeIndex];
            return new ShapeBlock(dim.Item1, dim.Item2, data);
        }
    }
}
