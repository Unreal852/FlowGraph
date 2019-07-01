namespace FlowGraph.Nodes.Compatibility
{
    public interface ICompatibility
    {
        bool CanConnect(IElement to);
    }
}