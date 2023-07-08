using System.Linq;

namespace SEBluePrintIO
{
    public class OutwardFacing : BasicBlock
    {
        public OutwardFacing(string arg = "F", string arg2 = null) : base(arg, arg2)
        {
        }

        internal override void Optimize(IShipGridModel model)
        {
            var neighbors = model.Neighbors(X, Y, Z);
            var nullCount = neighbors.Where(x => x == null).Count();
            if (nullCount == 1)
            {
                for (int i = 0; i < 6; ++i)
                {
                    if (neighbors[i] == null)
                    {
                        orientation.Forward = Orientation.DirectionMap[i];
                        if (i >= 4) orientation.Up = Orientation.DirectionMap[0];
                    }
                }
            }
        }

    }
}
