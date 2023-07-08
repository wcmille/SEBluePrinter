namespace SEBluePrintIO
{
    public class NullBlock : BasicBlock
    {
        public NullBlock(string code) : base()
        {
            base.Type = code;
        }

        internal override void WriteOut(IBlueprintWriter writer)
        {
            //No operation.
        }
    }
}
