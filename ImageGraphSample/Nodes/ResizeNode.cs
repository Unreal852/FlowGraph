using FlowGraph;
using FlowGraph.Events;
using FlowGraph.Nodes;
using FlowGraph.Nodes.Connections;
using FlowGraph.Nodes.Connectors;
using FlowGraph.Nodes.Item;
using System.Drawing;
using System.Linq;

namespace ImageGraphSample.Nodes
{
    public class ResizeNode : Node
    {
        public ResizeNode(Graph owner) : base(owner)
        {
            Title = "Resize";
            Description = "This will resize the image";
            Rectangle graphViewRectangle = owner.GetViewRectangle();
            Location = new GraphLocation(graphViewRectangle.Width / 2, graphViewRectangle.Height / 2);
            Size = new GraphSize(250, 100);
            Add(new ResizeImageItem() { Size = new GraphSize(Size.Width, 30) });
            Add(new IntValueItem(ConnectorType.Input) { Size = new GraphSize(Size.Width, 30) });
            Add(new IntValueItem(ConnectorType.Input) { Size = new GraphSize(Size.Width, 30) });
        }
    }

    public class ResizeImageItem : NodeItem, IConnectorHandler, IImage
    {
        public ResizeImageItem()
        {
            Input = new NodeConnector(this, ConnectorType.Input);
            Output = new NodeConnector(this, ConnectorType.Output);
        }

        /// <summary>
        /// Image
        /// </summary>
        public Image Image { get; private set; }

        public void OnConnected(ConnectorType type, NodeConnection connection)
        {
            if (type == ConnectorType.Input && connection.To.Owner is IImage)
            {
                IImage image = (IImage)connection.To.Owner;
                if (image.Image != null)
                {
                    IntValueItem[] items = ((Node)Owner).GetItems<IntValueItem>().Cast<IntValueItem>().ToArray();
                    Image = ImageHelpers.ResizeWithoutProportions(image.Image, items[0].Value, items[1].Value);
                }
            }
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
