using FlowGraph;
using FlowGraph.Events;
using FlowGraph.Nodes;
using FlowGraph.Nodes.Connections;
using FlowGraph.Nodes.Connectors;
using FlowGraph.Nodes.Item;
using System.Drawing;

namespace ImageGraphSample.Nodes
{
    public class InputNode : Node
    {
        public InputNode(Image img, Graph owner) : base(owner)
        {
            Title = "Input Image";
            Description = "This is the original image";
            Rectangle graphViewRectangle = owner.GetViewRectangle();
            Size = new GraphSize(150, 60);
            Location = new GraphLocation(graphViewRectangle.X + 10, graphViewRectangle.Height / 2);
            AddItem(new InputImageItem(img) { Size = new GraphSize(Size.Width, 30) });
        }
    }

    public class InputImageItem : NodeItem, IImage, IConnectorHandler
    {
        public InputImageItem(Image image)
        {
            Image = image;
            Output = new NodeConnector(this, ConnectorType.Output);
        }

        /// <summary>
        /// Input image
        /// </summary>
        public Image Image { get; }

        public void OnConnected(ConnectorType type, NodeConnection connection)
        {

        }

        public void OnDisconnected(ConnectorType type, NodeConnection connection)
        {

        }

        public void OnInputValueChanged()
        {

        }

        public override void OnRenderItem(ElementRenderEventArgs e)
        {

        }
    }
}
