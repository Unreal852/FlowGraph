using FlowGraph;
using FlowGraph.Events;
using FlowGraph.Nodes;
using FlowGraph.Nodes.Connections;
using FlowGraph.Nodes.Connectors;
using FlowGraph.Nodes.Item;
using System.Drawing;

namespace ImageGraphSample.Nodes
{
    public class GrayScaleNode : Node
    {
        public GrayScaleNode(Graph owner) : base(owner)
        {
            Title = "Gray Scale";
            Description = "This will apply a grayscale filter to the source image";
            Rectangle graphViewRectangle = owner.GetViewRectangle();
            Location = new GraphLocation(graphViewRectangle.Width / 2, graphViewRectangle.Height / 2);
            Size = new GraphSize(250, 100);
            AddItem(new GrayScaleImageItem() { Size = new GraphSize(Size.Width, 30) });
        }
    }

    public class GrayScaleImageItem : NodeItem, IImage, IConnectorHandler
    {
        public GrayScaleImageItem()
        {
            Input = new NodeConnector(this, ConnectorType.Input) { AllowMultipleConnections = false };
            Output = new NodeConnector(this, ConnectorType.Output) { AllowMultipleConnections = false };
        }

        /// <summary>
        /// Image
        /// </summary>
        public Image Image { get; private set; }

        public void OnConnected(ConnectorType type, NodeConnection connection)
        {
            if (type == ConnectorType.Input)
            {
                if (connection.To.Owner is IImage)
                    ProcessInput((IImage)connection.To.Owner);
            }
        }

        public void OnDisconnected(ConnectorType type, NodeConnection connection)
        {

        }

        public void OnInputValueChanged()
        {
            if (Input != null && Input.IsConnected)
            {
                NodeConnection connections = Input.Connections[0]; //We dont allow multiple connections so dont need to loop trougth all connections, just take the first one as it should only have one
                if (Input.Connections[0].To.Owner is IImage)
                    ProcessInput((IImage)Input.Connections[0].To.Owner);
            }
        }

        public override void OnRenderItem(ElementRenderEventArgs e)
        {

        }

        private void ProcessInput(IImage image)
        {
            if (image.Image != null)
                Image = ImageHelpers.GrayScale(new Bitmap(image.Image));
            CallInputValueChanged();
        }
    }
}
