using System.Linq;

namespace SEBluePrintIO
{
    public class BasicBlock
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public string Type { get; set; }
        public string Subtype { get; set; }

        protected readonly Orientation orientation;

        public BasicBlock(string arg = "F", string arg2 = null)
        {
            orientation = new Orientation(arg, arg2);
        }

        internal virtual void Optimize(IShipGridModel model)
        {
        }

        internal virtual void WriteOut(IBlueprintWriter writer)
        {
            writer.WriteOutCube(Type, Subtype, X, Y, Z, orientation);
        }

    }
}
