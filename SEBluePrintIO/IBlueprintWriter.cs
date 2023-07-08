using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEBluePrintIO
{
    interface IBlueprintWriter
    {
        public void WriteOutCube(string objType, string subType, int x, int y, int z, Orientation orientation);
    }
}
