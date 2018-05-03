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
    public class MergeNode : Node
    {
        public MergeNode(Graph owner) : base(owner)
        {
            Title = "Merge Images";
            Description = "This will merge the specified images";
            Rectangle graphViewRectangle = owner.GetViewRectangle();
            Size = new GraphSize(150, 60);
            Location = new GraphLocation(graphViewRectangle.Width / 2, graphViewRectangle.Height / 2);
            AddItem(new IntValueItem(ConnectorType.Input) { Size = new GraphSize(Size.Width, 20) });
            AddItem(new IntValueItem(ConnectorType.Input) { Size = new GraphSize(Size.Width, 20) });
            AddItem(new MergeInputItem() { Size = new GraphSize(Size.Width, 30) });
            AddItem(new MergeOutputItem() { Size = new GraphSize(Size.Width, 1) });
            AddItem(new MergeInputItem() { Size = new GraphSize(Size.Width, 30) });
            AddItem(new IntValueItem(ConnectorType.Input) { Size = new GraphSize(Size.Width, 20) });
            AddItem(new IntValueItem(ConnectorType.Input) { Size = new GraphSize(Size.Width, 20) });
        }
    }

    public class MergeInputItem : NodeItem, IConnectorHandler, IImage
    {
        public MergeInputItem()
        {
            Input = new NodeConnector(this, ConnectorType.Input) {  AllowMultipleConnections = false };
        }

        public Image Image { get; set; }

        public void OnConnected(ConnectorType type, NodeConnection connection)
        {
            if (type == ConnectorType.Input)
            {
                if(connection.To.Owner is IImage)
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

        private void ProcessInput(IImage image)
        {
            Image = image.Image;
            Node node = (Node)Owner;
            MergeOutputItem output = node.GetItems<MergeOutputItem>().Cast<MergeOutputItem>().ToArray()[0];
            output.Process();
        }

        public override void OnRenderItem(ElementRenderEventArgs e)
        {

        }
    }

    public class MergeOutputItem : NodeItem, IConnectorHandler, IImage
    {
        public MergeOutputItem()
        {
            Output = new NodeConnector(this, ConnectorType.Output);
        }

        /// <summary>
        /// Image
        /// </summary>
        public Image Image { get; private set; }

        public void OnConnected(ConnectorType type, NodeConnection connection)
        {

        }

        public void OnDisconnected(ConnectorType type, NodeConnection connection)
        {

        }

        public void OnInputValueChanged()
        {

        }

        public void Process()
        {
            Node node = (Node)Owner;
            MergeInputItem[] inputItems = node.GetItems<MergeInputItem>().Cast<MergeInputItem>().ToArray();
            if (inputItems[0].Image != null && inputItems[1].Image != null)
            {
                IntValueItem[] valueItems = node.GetItems<IntValueItem>().Cast<IntValueItem>().ToArray();
                Image = ImageHelpers.Merge(inputItems[0].Image, inputItems[1].Image, valueItems[0].Value, valueItems[1].Value, valueItems[2].Value, valueItems[3].Value);
            }
            CallInputValueChanged();
        }

        public override void OnRenderItem(ElementRenderEventArgs e)
        {

        }
    }
}
