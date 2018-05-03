using FlowGraph.Events;
using FlowGraph.Nodes.Connections;
using FlowGraph.Nodes.Connectors;
using FlowGraph.Nodes.Item;

namespace ImageGraphSample
{
    public class MergeInputItem : NodeItem, IConnectorHandler
    {
        public MergeInputItem()
        {
            Input = new NodeConnector(this, ConnectorType.Input);
        }

        public void OnConnected(ConnectorType type, NodeConnection connection)
        {
            
        }

        public void OnDisconnected(ConnectorType type, NodeConnection connection)
        {

        }

        public void OnValueChanged()
        {

        }

        public override void OnRenderItem(ElementRenderEventArgs e)
        {

        }
    }

    public class MergeOutputItem : NodeItem, IConnectorHandler
    {
        public MergeOutputItem()
        {
            Output = new NodeConnector(this, ConnectorType.Output);
        }

        public void OnConnected(ConnectorType type, NodeConnection connection)
        {

        }

        public void OnDisconnected(ConnectorType type, NodeConnection connection)
        {

        }

        public void OnValueChanged()
        {

        }

        public override void OnRenderItem(ElementRenderEventArgs e)
        {

        }
    }
}
