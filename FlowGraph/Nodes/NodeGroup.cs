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

        public NodeGroup()
        {
            UpdateBounds();
        }

        /// <summary>
        /// Owner, null for this item
        /// </summary>
        public IElement Owner { get; }

        /// <summary>
        /// Can this element be selected
        /// </summary>
        public bool CanBeSelected { get; set; } = true;

        /// <summary>
        /// Is this element selected
        /// </summary>
        public bool Selected { get; set; } = false;

        /// <summary>
        /// Header bounds
        /// </summary>
        public Rectangle HeaderBounds { get; private set; }

        /// <summary>
        /// Body bounds
        /// </summary>
        public Rectangle Bounds { get; private set; }

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

        internal void UpdateBounds()   // Todo: measure text to avoid out of node string
        {
            Bounds = new Rectangle(Location.X, Location.Y, Size.Width, Size.Height);
            HeaderBounds = new Rectangle(Location.X, Location.Y - 20, Size.Width / 2, 20);
            /*
            if (m_nodes.Count > 0)
            {
                int itemsHeight = 0;
                for (int i = 0; i < m_items.Count; i++)
                {
                    NodeItem item = m_items[i];
                    item.UpdateBounds(new GraphLocation(Location.X, (Location.Y + HeaderBounds.Height + itemsHeight) + (ItemsMargin * i)));
                    itemsHeight += item.Size.Height;
                }
            } */
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
            if (Bounds.Contains(point))
                return this;
            return null;
        }

        /// <summary>
        /// Render this element
        /// </summary>
        public void OnRender(ElementRenderEventArgs e) //Todo: maybe cache all points in the updatebounds method 
        {
            Point headerUpperLeft = new Point(HeaderBounds.X, HeaderBounds.Y);
            Point headerUpperRight = new Point(HeaderBounds.X + HeaderBounds.Width, HeaderBounds.Y);
            Point upperLeft = new Point(Bounds.X, Bounds.Y);
            Point upperRight = new Point(Bounds.X + Bounds.Width, Bounds.Y);
            Point lowerLeft = new Point(Bounds.X, Bounds.Y + Bounds.Height);
            Point lowerRight = new Point(Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height);

            e.Graphics.DrawLine(Pens.Red, headerUpperLeft, headerUpperRight);
            e.Graphics.DrawLine(Pens.Red, headerUpperLeft, upperLeft);
            e.Graphics.DrawLine(Pens.Red, headerUpperRight.X, headerUpperRight.Y, upperLeft.X + HeaderBounds.Width, upperLeft.Y);

            e.Graphics.DrawLine(Pens.Red, upperLeft, lowerLeft);
            e.Graphics.DrawLine(Pens.Red, upperRight, lowerRight);
            e.Graphics.DrawLine(Pens.Red, lowerLeft, lowerRight);
            e.Graphics.DrawLine(Pens.Red, upperLeft.X + HeaderBounds.Width, upperLeft.Y, upperRight.X, upperRight.Y);
        }
    }
}
