namespace FlowGraph
{
    public interface IExpandableElement : IElement
    {
        EGrip Grips { get; }

        GraphSize MinSize { get; }
        GraphSize MaxSize { get; }
    }
}
