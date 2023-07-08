using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEBluePrintIO
{
    public class LargeBlock : BasicBlock
    {
        readonly int dx;
        readonly int dy;
        readonly int dz;
        public LargeBlock(int d, string args = "F", string args2 = null) : base(args, args2)
        {
            this.dx = d;
            this.dy = d;
            this.dz = d;
        }
        public LargeBlock(int dx, int dy, int dz, string args = "F", string args2 = null) : base(args, args2)
        {
            this.dx = dx;
            this.dy = dy;
            this.dz = dz;
            if (args == "L" || args == "R")
            {
                this.dz = dx;
                this.dx = dz;
            }
            if ((args == "D" || args == "U") || (args2 == "F" || args2 == "B"))
            {
                this.dz = dy;
                this.dy = dz;
            }
            if (args2 == "L" || args2 == "R")
            {
                this.dy = dx;
                this.dx = dy;
            }
        }

        internal void ReserveSpace(int z, int y, int x, BasicBlock[,,] blockDecks)
        {
            for (int ix = 0; ix < dx ; ++ix)
            {
                for (int iy = 0; iy < dy; ++iy)
                {
                    for (int iz = 0; iz < dz; ++iz)
                    {
                        blockDecks[x + ix, y + iy, z + iz] = this;
                    }
                }
            }
        }

    }
}
