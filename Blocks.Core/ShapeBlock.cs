using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blocks.Core
{
    public class ShapeBlock: Block
    {

        public ShapeBlock(int width, int height, params (int,int)[] args)
        {
            if (args == null || args.Length == 0)
            {
                throw new ArgumentNullException("You must provide shape data.");
            }

            BuildMap(width, height);

            foreach (var item in args)
            {
                this[item.Item1, item.Item2] = true;
            }
        }

        public override void Transform(string name)
        {
            if (name == "Rotate")
            {
                Rotate();
            }
        }

        internal override void OnStarted()
        {

        }

        internal override void OnStopped()
        {

        }
    }
}
