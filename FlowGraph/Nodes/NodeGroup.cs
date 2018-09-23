using FlowGraph.Events;
using System.Collections.Generic;
using System.Drawing;

namespace FlowGraph.Nodes
{
    public class NodeGroup : IElement
    {
        private GraphLocation m_location = new GraphLocation(0, 0);

        private GraphSize m_size = new GraphSize(700, 700);

        private List<Node> m_nodes = new List<Node>();

        public NodeGroup(Graph graph)
        {
            Graph = graph;
            UpdateBounds();
        }

        /// <summary>
        /// Owner, null for this item
        /// </summary>
        public IElement Owner { get; }

        /// <summary>
        /// Owner graph
        /// </summary>
        public Graph Graph { get; }

        /// <summary>
        /// Can this element be selected
        /// </summary>
        public bool CanBeSelected { get; set; } = true;

        /// <summary>
        /// Is this element selected
        /// </summary>
        public bool Selected { get; set; } = false;

        /// <summary>
        /// Nodes
        /// </summary>
        public Node[] Nodes => m_nodes.ToArray();

        /// <summary>
        /// Header bounds
        /// </summary>
        public Rectangle HeaderBounds { get; private set; }

        /// <summary>
        /// Body bounds
        /// </summary>
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Grip Bounds
        /// </summary>
        public Rectangle GripBounds { get; private set; }

        /// <summary>
        /// Header upper left point
        /// </summary>
        public Point HeaderUpperLeft { get; private set; }

        /// <summary>
        /// Header upper right point
        /// </summary>
        public Point HeaderUpperRight { get; private set; }

        /// <summary>
        /// Upper left point
        /// </summary>
        public Point UpperLeft { get; private set; }

        /// <summary>
        /// Upper right point
        /// </summary>
        public Point UpperRight { get; private set; }

        /// <summary>
        /// Lower left point
        /// </summary>
        public Point LowerLeft { get; private set; }

        /// <summary>
        /// Lower right
        /// </summary>
        public Point LowerRight { get; private set; }

        /// <summary>
        /// Location
        /// </summary>
        public GraphLocation Location
        {
            get => m_location;
            set
            {
                if (m_location == value || (m_location.X == value.X && m_location.Y == value.Y))
                    return;
                m_location = value;
                UpdateBounds();
            }
        }

        /// <summary>
        /// Size
        /// </summary>
        public GraphSize Size
        {
            get => m_size;
            set
            {
                if (m_size == value || (m_size.Width == value.Width && m_size.Height == value.Height))
                    return;
                m_size = value;
                UpdateBounds();
            }
        }


        /// <summary>
        /// Header color
        /// </summary>
        public GraphColor HeaderColor { get; set; } = new GraphColor(Color.FromArgb(62, 62, 66));

        /// <summary>
        /// Header color when this node is selected
        /// </summary>
        public GraphColor SelectedHeaderColor { get; set; } = new GraphColor(Color.FromArgb(90, 90, 90));

        /// <summary>
        /// Background color
        /// </summary>
        public GraphColor BackgroundColor { get; set; } = new GraphColor(Color.FromArgb(30, 30, 30));

        /// <summary>
        /// Background color when this node is selected
        /// </summary>
        public GraphColor SelectedBackgroundColor { get; set; } = new GraphColor(Color.FromArgb(60, 60, 60));

        /// <summary>
        /// Outline color
        /// </summary>
        public GraphColor OutlineColor { get; set; } = new GraphColor(Color.FromArgb(0, 0, 0));

        /// <summary>
        /// Outline color when this node is selected
        /// </summary>
        public GraphColor SelectedOutlineColor { get; set; } = new GraphColor(Color.Orange);

        /// <summary>
        /// Title color
        /// </summary>
        public GraphColor TitleColor { get; set; } = new GraphColor(Color.White);

        /// <summary>
        /// Description color
        /// </summary>
        public GraphColor DescriptionColor { get; set; } = new GraphColor(Color.Gray);

        /// <summary>
        /// Title font
        /// </summary>
        public Font TitleFont { get; set; } = SystemFonts.DefaultFont;

        /// <summary>
        /// Description font
        /// </summary>
        public Font DescriptionFont { get; set; } = new Font(new FontFamily(SystemFonts.DefaultFont.Name), 8f, FontStyle.Italic);

        internal void UpdateBounds()   // Todo: measure text to avoid out of node string
        {
            Bounds = new Rectangle(Location.X, Location.Y, Size.Width, Size.Height);
            HeaderBounds = new Rectangle(Location.X, Location.Y - 20, Size.Width / 2, 20);
            HeaderUpperLeft = new Point(HeaderBounds.X, HeaderBounds.Y);
            HeaderUpperRight = new Point(HeaderBounds.X + HeaderBounds.Width, HeaderBounds.Y);
            UpperLeft = new Point(Bounds.X, Bounds.Y);
            UpperRight = new Point(Bounds.X + Bounds.Width, Bounds.Y);
            LowerLeft = new Point(Bounds.X, Bounds.Y + Bounds.Height);
            LowerRight = new Point(Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height);
            GripBounds = new Rectangle(LowerRight.X - 10, LowerRight.Y - 10, 20, 20);
        }

        /// <summary>
        /// Find the element at the specified point
        /// </summary>
        /// <param name="point">Location</param>
        /// <returns><see cref="IElement"/> found element, <see cref="null"/> otherwise</returns>
        public IElement FindElementAt(Point point)
        {
            if (HeaderBounds.Contains(point))
                return this;
            foreach (Node node in m_nodes)
            {
                IElement element = node.FindElementAt(point);
                if (element != null)
                    return element;
            }
            if (Bounds.Contains(point) || GripBounds.Contains(point))
                return this;
            return null;
        }

        /// <summary>
        /// Add a new node into the group
        /// </summary>
        /// <param name="node">Node</param>
        public void AddNode(Node node)
        {
            if (!m_nodes.Contains(node))
            {
                node.Owner = this;
                m_nodes.Add(node);
            }
        }

        /// <summary>
        /// Remove the specified node from the group
        /// </summary>
        /// <param name="node">Node</param>
        public void RemoveNode(Node node)
        {
            if (m_nodes.Contains(node))
            {
                node.Owner = null;
                m_nodes.Remove(node);
            }
        }

        /// <summary>
        /// Remove a node from the group at a specified index
        /// </summary>
        /// <param name="index">Node Index</param>
        public void RemoveNode(int index)
        {
            if (m_nodes.Count <= index)
            {
                m_nodes[index].Owner = null;
                m_nodes.RemoveAt(index);
            }
        }

        public void ExpandTo(Point point)
        {
            //Size = new GraphSize(Size.Width - point.X, Size.Height - point.Y);
        }

        /// <summary>
        /// Render this element
        /// </summary>
        public virtual void OnRender(ElementRenderEventArgs e) //Todo: maybe cache all points in the updatebounds method 
        {
            if (Selected)
            {
                e.Graphics.DrawLine(SelectedOutlineColor.Pen, HeaderUpperLeft, HeaderUpperRight);
                e.Graphics.DrawLine(SelectedOutlineColor.Pen, HeaderUpperLeft, UpperLeft);
                e.Graphics.DrawLine(SelectedOutlineColor.Pen, HeaderUpperRight.X, HeaderUpperRight.Y, UpperLeft.X + HeaderBounds.Width, UpperLeft.Y);
                e.Graphics.DrawLine(SelectedOutlineColor.Pen, UpperLeft, LowerLeft);
                e.Graphics.DrawLine(SelectedOutlineColor.Pen, UpperRight, LowerRight);
                e.Graphics.DrawLine(SelectedOutlineColor.Pen, LowerLeft, LowerRight);
                e.Graphics.DrawLine(SelectedOutlineColor.Pen, UpperLeft.X + HeaderBounds.Width, UpperLeft.Y, UpperRight.X, UpperRight.Y);
                e.Graphics.FillRectangle(SelectedHeaderColor.Brush, HeaderBounds);
                e.Graphics.FillRectangle(SelectedBackgroundColor.Brush, Bounds);
            }
            else
            {
                e.Graphics.DrawLine(OutlineColor.Pen, HeaderUpperLeft, HeaderUpperRight);
                e.Graphics.DrawLine(OutlineColor.Pen, HeaderUpperLeft, UpperLeft);
                e.Graphics.DrawLine(OutlineColor.Pen, HeaderUpperRight.X, HeaderUpperRight.Y, UpperLeft.X + HeaderBounds.Width, UpperLeft.Y);
                e.Graphics.DrawLine(OutlineColor.Pen, UpperLeft, LowerLeft);
                e.Graphics.DrawLine(OutlineColor.Pen, UpperRight, LowerRight);
                e.Graphics.DrawLine(OutlineColor.Pen, LowerLeft, LowerRight);
                e.Graphics.DrawLine(OutlineColor.Pen, UpperLeft.X + HeaderBounds.Width, UpperLeft.Y, UpperRight.X, UpperRight.Y);
                e.Graphics.FillRectangle(HeaderColor.Brush, HeaderBounds);
                e.Graphics.FillRectangle(BackgroundColor.Brush, Bounds);
            }

            e.Graphics.DrawRectangle(Pens.Red, GripBounds);
        }
    }
}
