using FlowGraph;
using FlowGraph.Events;
using FlowGraph.Nodes;
using FlowGraph.Nodes.Connections;
using FlowGraph.Nodes.Connectors;
using FlowGraph.Nodes.Item;
using FlowGraph.Nodes.UserInput;
using System.Drawing;
using System.Windows.Forms;

namespace ImageGraphSample.Nodes
{
    public class IntNode : Node
    {
        public IntNode(Graph owner) : base(owner)
        {
            Title = "Int value";
            Description = "This is a interger value";
            Rectangle graphViewRectangle = owner.GetViewRectangle();
            Size = new GraphSize(150, 60);
            Location = new GraphLocation(graphViewRectangle.Width / 2, graphViewRectangle.Height / 2);
            Add(new IntValueItem(ConnectorType.Output) { Size = new GraphSize(Size.Width, 30) });
        }
    }

    public class IntValueItem : NodeItem, IMouseDoubleClickHandler, IConnectorHandler
    {
        public IntValueItem(ConnectorType type)
        {
            if (type == ConnectorType.Output)
                Output = new NodeConnector(this, ConnectorType.Output);
            else
                Input = new NodeConnector(this, ConnectorType.Input);
        }

        public int Value { get; private set; } = 0;

        public void OnClick(Point mouseLoc)
        {

        }

        public void OnConnected(ConnectorType type, NodeConnection connection)
        {
            if (type == ConnectorType.Input)
            {
                if (connection.To.Owner is IntValueItem)
                {
                    Value = ((IntValueItem)connection.To.Owner).Value;
                }
            }
        }

        public void OnDisconnected(ConnectorType type, NodeConnection connection)
        {

        }

        public void OnMouseDoubleClick(Point mouseLoc)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter the new int value", "INT VALUE", Value.ToString());
            if (!string.IsNullOrWhiteSpace(input))
            {
                int parsed;
                if (int.TryParse(input, out parsed))
                {
                    Value = parsed;
                    CallInputValueChanged();
                }
            }
        }

        public void OnInputValueChanged()
        {

        }

        public override void OnRenderItem(ElementRenderEventArgs e)
        {
            if (Input != null)
            {
                e.Graphics.DrawString($"Value: {Value}", SystemFonts.DefaultFont, Brushes.White, Bounds.X + 10, Bounds.Y + 8);
            }
            else
            {
                e.Graphics.DrawString($"Value: {Value}", SystemFonts.DefaultFont, Brushes.White, (Bounds.X + Bounds.Width) - 65, Bounds.Y + 8);
            }
        }
    }
}
