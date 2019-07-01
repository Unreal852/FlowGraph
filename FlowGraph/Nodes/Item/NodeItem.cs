using FlowGraph.Events;
using FlowGraph.Nodes.Connections;
using FlowGraph.Nodes.Connectors;
using System.Drawing;

namespace FlowGraph.Nodes.Item
{
    public abstract class NodeItem : IElement
    {
        private GraphLocation m_location = new GraphLocation(0, 0);

        private GraphSize m_size = new GraphSize(0, 0);

        private NodeConnector m_input  = null;
        private NodeConnector m_output = null;

        public NodeItem()
        {
        }

        /// <summary>
        /// Owner
        /// </summary>
        public IElement Owner { get; internal set; }

        /// <summary>
        /// Can this item be selected
        /// </summary>
        public bool CanBeSelected { get; } = false;

        /// <summary>
        /// Select node
        /// </summary>
        public bool Selected { get; set; } = false;

        /// <summary>
        /// Item bounds
        /// </summary>
        public Rectangle Bounds { get; protected set; }

        /// <summary>
        /// Item location
        /// </summary>
        public GraphLocation Location
        {
            get => m_location;
            set
            {
                m_location = value;
                UpdateBounds();
            }
        }

        /// <summary>
        /// Item size
        /// </summary>
        public GraphSize Size
        {
            get => m_size;
            set
            {
                m_size = value;
                UpdateBounds();
            }
        }


        /// <summary>
        /// Input connector, this can be null if there is no input requiered
        /// </summary>
        public NodeConnector Input
        {
            get => m_input;
            set
            {
                if(m_input == value)
                    return;
                m_input = value;
                UpdateBounds();
            }
        }

        /// <summary>
        /// Output connector, this can be null if there is no input requiered
        /// </summary>
        public NodeConnector Output
        {
            get => m_output;
            set
            {
                if(m_output == value)
                    return;
                m_output = value;
                UpdateBounds();
            }
        }

        /// <summary>
        /// Call the value changed of all output
        /// </summary>
        public void CallInputValueChanged()
        {
            if(Output != null && Output.IsConnected)
            {
                foreach(NodeConnection con in Output.Connections)
                {
                    if(con.To.Owner is IConnectorHandler)
                        ((IConnectorHandler)con.To.Owner).OnInputValueChanged();
                }
            }
        }

        /// <summary>
        /// Update bounds
        /// </summary>
        internal void UpdateBounds(GraphLocation location = null) // Todo: measure text to avoid out of node string
        {
            if(location != null)
                Location = location;
            Bounds = new Rectangle(Location.X, Location.Y, Size.Width, Size.Height);
            Input?.UpdateBounds(new GraphLocation(Bounds.X, Bounds.Y + (Bounds.Size.Height / 2)));
            Output?.UpdateBounds(new GraphLocation(Bounds.X + Bounds.Size.Width, Bounds.Y + (Bounds.Size.Height / 2)));
        }

        /// <summary>
        /// Find element at the specified point
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns><see cref="IElement"/> if a element has been found, <see cref="null"/> otherwise</returns>
        public IElement FindElementAt(Point point)
        {
            IElement element;
            if(Input != null)
            {
                element = Input.FindElementAt(point);
                if(element != null)
                    return element;
            }

            if(Output != null)
            {
                element = Output.FindElementAt(point);
                if(element != null)
                    return element;
            }

            if(Bounds.Contains(point))
                return this;
            return null;
        }

        /// <summary>
        /// Post render
        /// </summary>
        public void OnRender(ElementRenderEventArgs e)
        {
            OnRenderItem(e);
            Input?.OnRender(e);
            Output?.OnRender(e);
        }

        /// <summary>
        /// Render item
        /// </summary>
        /// <param name="e"></param>
        public abstract void OnRenderItem(ElementRenderEventArgs e);
    }
}