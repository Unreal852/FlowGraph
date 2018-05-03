using FlowGraph;
using FlowGraph.Events;
using FlowGraph.Nodes;
using FlowGraph.Nodes.Connections;
using FlowGraph.Nodes.Connectors;
using FlowGraph.Nodes.Item;
using System.Drawing;

namespace ImageGraphSample.Nodes
{
    public class OutputNode : Node
    {
        public OutputNode(Graph owner) : base(owner)
        {
            Title = "Output Image";
            Description = "This is the final image";
            Rectangle graphViewRectangle = owner.GetViewRectangle();
            Size = new GraphSize(150, 60);
            Location = new GraphLocation((graphViewRectangle.X + graphViewRectangle.Width) - (Size.Width + 10), graphViewRectangle.Height / 2);
            AddItem(new OutputImageItem() { Size = new GraphSize(Size.Width, 30) });
        }
    }

    public class OutputImageItem : NodeItem, IConnectorHandler
    {
        public OutputImageItem()
        {
            Input = new NodeConnector(this, ConnectorType.Input);
        }

        public void OnConnected(ConnectorType type, NodeConnection connection)
        {
            frmMain.Instance.SetPreview(((IImage)connection.To.Owner).Image);
        }

        public void OnDisconnected(ConnectorType type, NodeConnection connection)
        {

        }

        public void OnInputValueChanged()
        {
            System.Diagnostics.Debug.Print("ddd");
            if (Input.IsConnected)
                frmMain.Instance.SetPreview(((IImage)Input.Connections[0].To.Owner).Image);
        }

        public override void OnRenderItem(ElementRenderEventArgs e)
        {

        }
    }
}
