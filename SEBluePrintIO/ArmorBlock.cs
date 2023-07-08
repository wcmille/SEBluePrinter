using System.Linq;

namespace SEBluePrintIO
{
    public class ArmorBlock : BasicBlock
    {
        internal override void Optimize(IShipGridModel model)
        {
            if (Subtype == "LargeBlockArmorBlock")
            {
                var neighbors = model.Neighbors(X, Y, Z);
                var nullCount = neighbors.Where(x => x == null).Count();
                var armorCount = neighbors.Where(x => x != null && x is ArmorBlock).Count();
                if (nullCount == 1)
                {
                    var localCube = model.LocalCube(X, Y, Z);
                    int nullDir = 5;
                    for (int i = 0; i < 6; ++i)
                    {
                        if (neighbors[i] == null)
                        {
                            nullDir = i;
                            break;
                        }
                    }
                    if (nullDir == 0 || nullDir == 2)
                    {
                        nullCount = 0;
                        int orF = 0;
                        if (localCube[0, 2, 1] == null)
                        {
                            orF = nullDir == 0 ? 3 : 4;
                            nullCount++;
                        }
                        if (localCube[0, 0, 1] == null)
                        {
                            orF = nullDir == 0 ? 5 : 3;
                            nullCount++;
                        }
                        if (localCube[2, 0, 1] == null)
                        {
                            orF = nullDir == 0 ? 1 : 5;
                            nullCount++;
                        }
                        if (localCube[2, 2, 1] == null)
                        {
                            orF = nullDir == 0 ? 4 : 1;
                            nullCount++;
                        }
                        if (nullCount == 1)
                        {
                            orientation.Up = Orientation.Opposite(nullDir);
                            orientation.Forward = Orientation.DirectionMap[orF];
                            Subtype = "LargeBlockArmorCornerInv";
                        }
                    }
                    else if (nullDir == 1 || nullDir == 3)
                    {
                        nullCount = 0;
                        int orF = 0;
                        if (localCube[1, 2, 0] == null)
                        {
                            orF = nullDir == 1 ? 0 : 4;
                            nullCount++;
                        }
                        if (localCube[1, 0, 0] == null)
                        {
                            orF = nullDir == 1 ? 5 : 0;
                            nullCount++;
                        }
                        if (localCube[1, 0, 2] == null)
                        {
                            orF = nullDir == 1 ? 2 : 5;
                            nullCount++;
                        }
                        if (localCube[1, 2, 2] == null)
                        {
                            orF = nullDir == 1 ? 4 : 2;
                            nullCount++;
                        }
                        if (nullCount == 1)
                        {
                            orientation.Up = Orientation.Opposite(nullDir);
                            orientation.Forward = Orientation.DirectionMap[orF];
                            Subtype = "LargeBlockArmorCornerInv";
                        }
                    }
                    else if (nullDir == 4 /*|| nullDir == 5*/)
                    {
                        orientation.Up = Orientation.Opposite(nullDir);
                        nullCount = 0;
                        if (localCube[0, 1, 0] == null)
                        {
                            orientation.Forward = Orientation.DirectionMap[0];
                            nullCount++;
                        }
                        if (localCube[2, 1, 0] == null)
                        {
                            orientation.Forward = Orientation.DirectionMap[1];
                            nullCount++;
                        }
                        if (localCube[2, 1, 2] == null)
                        {
                            orientation.Forward = Orientation.DirectionMap[2];
                            nullCount++;
                        }
                        if (localCube[0, 1, 2] == null)
                        {
                            orientation.Forward = Orientation.DirectionMap[3];
                            nullCount++;
                        }
                        if (nullCount == 1)
                        {
                            Subtype = "LargeBlockArmorCornerInv";
                        }
                    }
                    else if (nullDir == 5 /*|| nullDir == 5*/)
                    {
                        orientation.Up = Orientation.Opposite(nullDir);
                        nullCount = 0;
                        if (localCube[0, 1, 0] == null)
                        {
                            orientation.Forward = Orientation.DirectionMap[3];
                            nullCount++;
                        }
                        if (localCube[2, 1, 0] == null)
                        {
                            orientation.Forward = Orientation.DirectionMap[0];
                            nullCount++;
                        }
                        if (localCube[2, 1, 2] == null)
                        {
                            orientation.Forward = Orientation.DirectionMap[1];
                            nullCount++;
                        }
                        if (localCube[0, 1, 2] == null)
                        {
                            orientation.Forward = Orientation.DirectionMap[2];
                            nullCount++;
                        }
                        if (nullCount == 1)
                        {
                            Subtype = "LargeBlockArmorCornerInv";
                        }
                    }
                }
                else if (nullCount == 2)
                {
                    orientation.Forward = null;
                    for (int i = 0; i < 4; ++i)
                    {
                        if (neighbors[i] == null)
                        {
                            orientation.Forward = Orientation.DirectionMap[(i + 2) % 4];
                            if (orientation.Forward != null)
                            {
                                for (int j = i + 1; j < 6; ++j)
                                {
                                    //if the null sides are opposite ends, don't do it.
                                    if (neighbors[j] == null && ((j != i + 2) || (j > 3)))
                                    {
                                        orientation.Up = Orientation.DirectionMap[j];
                                        Subtype = "LargeBlockArmorSlope";
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    orientation.Forward = "Forward";
                }
                else if (nullCount == 3)
                {
                    orientation.Forward = null;
                    if (armorCount == 3)
                    {
                        for (int i = 0; i < 4; ++i)
                        {
                            if (neighbors[i] == null && neighbors[(i + 1) % 4] == null) orientation.Forward = Orientation.DirectionMap[(i + 2) % 4];
                            if (orientation.Forward != null)
                            {
                                for (int j = 4; j < 6; ++j)
                                {
                                    //if the null sides are opposite ends, don't do it.
                                    if (neighbors[j] == null)
                                    {
                                        if (j == 5)
                                        {
                                            orientation.Forward = Orientation.DirectionMap[(i + 3) % 4]; //Terrible Hack
                                        }
                                        orientation.Up = Orientation.DirectionMap[j];
                                        Subtype = "LargeBlockArmorCorner";
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    if (armorCount == 2)
                    {
                        for (int i = 0; i < 6; ++i)
                        {
                            if (neighbors[i] != null && !(neighbors[i] is ArmorBlock))
                            {
                                orientation.Up = Orientation.Opposite(i);
                                int[] axis = null;
                                if (i == 1) axis = new int[] { 0, 4, 2, 5 };
                                if (i == 3) axis = new int[] { 0, 5, 2, 4 };
                                if (axis != null)
                                {
                                    for (int j = 0; j < 4; ++j)
                                    {
                                        var thisNeighbor = neighbors[axis[j]];
                                        var lastNeighbor = neighbors[axis[(j + 3) % 4]];
                                        if (thisNeighbor != null && lastNeighbor != null)
                                        {
                                            orientation.Forward = Orientation.DirectionMap[axis[j]];
                                            Subtype = "LargeBlockArmorCornerSquare";
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    orientation.Forward = "Forward";
                }
                else if (nullCount == 4)
                {
                    orientation.Forward = null;
                    for (int i = 0; i < 4; ++i)
                    {
                        if (neighbors[i] != null)
                        {
                            orientation.Forward = Orientation.DirectionMap[i];
                            if (orientation.Forward != null)
                            {
                                for (int j = i + 1; j < 6; ++j)
                                {
                                    //if the null sides are opposite ends, don't do it.
                                    if (neighbors[j] != null && ((j != i + 2) || (j > 3)))
                                    {
                                        Subtype = "LargeBlockArmorSlope";
                                        orientation.Up = Orientation.Opposite(j);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    orientation.Forward = "Forward";
                }
            }
        }
    }
}
