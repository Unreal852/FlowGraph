using FlowGraph.Events;
using System.Drawing;

namespace FlowGraph.Nodes.Item.Items
{
    public class LabelItem : NodeItem
    {
        public LabelItem()
        {
            Input = new Connectors.NodeConnector(this, Connectors.ConnectorType.Input);
            Output = new Connectors.NodeConnector(this, Connectors.ConnectorType.Output);
        }

        /// <summary>
        /// Render item
        /// </summary>
        public override void OnRenderItem(ElementRenderEventArgs e)
        {
            //e.Graphics.DrawRectangle(Pens.Red, Bounds);
            e.Graphics.DrawString("Node Item", SystemFonts.DefaultFont, Brushes.White, Location.X + 10, Location.Y + 8);
        }
    }
}
