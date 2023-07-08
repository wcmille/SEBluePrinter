using System.Linq;

namespace SEBluePrintIO
{
    public class WindowBlock : BasicBlock
    {
        public WindowBlock(string arg = "F", string arg2 = null) : base(arg, arg2)
        {
        }

        internal override void Optimize(IShipGridModel model)
        {
            var neighbors = model.Neighbors(X, Y, Z);
            var nullCount = neighbors.Where(x => x == null).Count();
            if (nullCount == 1)
            {
                orientation.Forward = null;
                for (int i = 0; i < 6; ++i)
                {
                    if (neighbors[i] == null)
                    {
                        if (i < 4)
                        {
                            orientation.Forward = Orientation.DirectionMap[((i + 3) % 4)];
                            orientation.Up = Orientation.DirectionMap[4];
                        }
                        else
                        {
                            orientation.Forward = Orientation.DirectionMap[0];
                            orientation.Up = Orientation.DirectionMap[i == 4 ? 3:1 ];
                        }
                        Subtype = "Window1x1FlatInv";
                        return;
                    }
                }
                orientation.Forward = "Forward";
            }
            if (nullCount == 2)
            {
                orientation.Forward = null;
                for (int i = 0; i < 4; ++i)
                {
                    if (neighbors[i] == null) orientation.Forward = Orientation.DirectionMap[(i) % 4];
                    if (orientation.Forward != null)
                    {
                        for (int j = i + 1; j < 6; ++j)
                        {
                            //if the null sides are opposite ends, don't do it.
                            if (neighbors[j] == null && ((j != i + 2) || (j > 3)))
                            {
                                orientation.Up = Orientation.DirectionMap[j];
                                Subtype = "Window1x1Slope";
                                return;
                            }
                        }
                    }
                }
                orientation.Forward = "Forward";
            }
            if (nullCount == 3)
            {
                orientation.Forward = null;
                for (int i = 0; i < 4; ++i)
                {
                    if (neighbors[i] == null && neighbors[(i + 1) % 4] == null) orientation.Forward = Orientation.DirectionMap[(i + 3) % 4];
                    if (orientation.Forward != null)
                    {
                        for (int j = 4; j < 6; ++j)
                        {
                            //if the null sides are opposite ends, don't do it.
                            //this means that there *must* be a null side in the up/down direction.
                            if (neighbors[j] == null)
                            {
                                if (j == 5)
                                {
                                    orientation.Forward = Orientation.DirectionMap[(i + 2) % 4]; //Terrible Hack
                                }
                                orientation.Up = Orientation.DirectionMap[j];
                                Subtype = "Window1x1Face";
                                return;
                            }
                        }
                    }
                }
                orientation.Forward = "Forward";
                //Subtype = "LargeBlockArmorCorner";
            }
        }
    }
}
