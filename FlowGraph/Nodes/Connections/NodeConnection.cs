using FlowGraph.Events;
using FlowGraph.Helpers;
using FlowGraph.Nodes.Connectors;
using FlowGraph.Nodes.UserInput;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FlowGraph.Nodes.Connections
{
    public class NodeConnection : IElement, IMouseDoubleClickHandler
    {
        public NodeConnection(NodeConnector from, NodeConnector to)
        {
            From = from;
            To = to;
        }

        /// <summary>
        /// Owner
        /// </summary>
        public IElement Owner { get; }

        /// <summary>
        /// From connector
        /// </summary>
        public NodeConnector From { get; }

        /// <summary>
        /// To connector
        /// </summary>
        public NodeConnector To { get; }

        /// <summary>
        /// Can this node be selected
        /// </summary>
        public bool CanBeSelected { get; set; } = false;

        /// <summary>
        /// Select connection
        /// </summary>
        public bool Selected { get; set; } = false;

        /// <summary>
        /// Mouse hovering the connection
        /// </summary>
        public bool Hovered { get; set; } = false;

        /// <summary>
        /// Connection color
        /// </summary>
        public GraphColor ConnectionColor { get; set; } = new GraphColor(Color.Green);

        /// <summary>
        /// Hovered connection color
        /// </summary>
        public GraphColor HoveredConnectionColor { get; set; } = new GraphColor(Color.GreenYellow);

        /// <summary>
        /// Connection location
        /// </summary>
        public GraphLocation Location
        {
            get => throw new NotSupportedException($"{nameof(Location)} can't be used in {nameof(NodeConnection)}");
            set => throw new NotSupportedException($"{nameof(Location)} can't be used in {nameof(NodeConnection)}");
        }

        /// <summary>
        /// Connection size
        /// </summary>
        public GraphSize Size
        {
            get => throw new NotSupportedException($"{nameof(Size)} can't be used in {nameof(NodeConnection)}");
            set => throw new NotSupportedException($"{nameof(Size)} can't be used in {nameof(NodeConnection)}");
        }

        /// <summary>
        /// Connection bounds
        /// </summary>
        public Rectangle Bounds => throw new NotSupportedException($"{nameof(Bounds)} can't be used in {nameof(NodeConnection)}");

        /// <summary>
        /// Find element at the specified point
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns><see cref="IElement"/> if a element has been found, <see cref="null"/> otherwise</returns>
        public IElement FindElementAt(Point point)
        {
            Region region = RenderHelper.GetConnectionRegion(this);
            if(region != null && region.IsVisible(point))
                return this;
            return null;
        }

        /// <summary>
        /// Called when this item is selected and the user double click
        /// </summary>
        public void OnMouseDoubleClick(Point mouseLoc)
        {
            From?.Disconnect(this);
            To?.Disconnect(this);
        }

        /// <summary>
        /// Render connection
        /// </summary>
        public void OnRender(ElementRenderEventArgs e)
        {
            /*
            GraphicsPath gPath = RenderHelper.GetLinePath(To.Bounds.X, To.Bounds.Y, From.Bounds.X, From.Bounds.Y).Path;
            Region reg = new Region(gPath);
            Hovered = reg.IsVisible(e.MouseLocation);
            if (Hovered)
                e.Graphics.DrawPath(HoveredConnectionColor.Pen, gPath);
            else
                e.Graphics.DrawPath(ConnectionColor.Pen, gPath); 
                */
            GraphicsPath ggPath = new GraphicsPath();
            ggPath.AddLine(From.Bounds.X, From.Bounds.Y, From.Bounds.X - 25, From.Bounds.Y);
            ggPath.AddLine(From.Bounds.X - 25, From.Bounds.Y, To.Bounds.X + 25, To.Bounds.Y);
            ggPath.AddLine(To.Bounds.X, To.Bounds.Y, To.Bounds.X + 25, To.Bounds.Y);

            Region reg = new Region(ggPath);
            Hovered = reg.IsVisible(e.MouseLocation);
            if(Hovered)
                e.Graphics.DrawPath(HoveredConnectionColor.Pen, ggPath);
            else
                e.Graphics.DrawPath(ConnectionColor.Pen, ggPath);
        }
    }
}