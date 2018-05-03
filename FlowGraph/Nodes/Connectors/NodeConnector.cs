using FlowGraph.Events;
using FlowGraph.Nodes.Compatibility;
using FlowGraph.Nodes.Connections;
using System.Collections.Generic;
using System.Drawing;

namespace FlowGraph.Nodes.Connectors
{
    public class NodeConnector : IElement, ICompatibility
    {
        private GraphLocation m_location = new GraphLocation(0, 0);

        private GraphSize m_size = new GraphSize(10, 10);

        private List<NodeConnection> m_connections = new List<NodeConnection>();

        public NodeConnector(IElement owner, ConnectorType connector)
        {
            Owner = owner;
            Type = connector;
        }

        /// <summary>
        /// Connector owner
        /// </summary>
        public IElement Owner { get; }

        /// <summary>
        /// Connector type
        /// </summary>
        public ConnectorType Type { get; }

        /// <summary>
        /// Select connector
        /// </summary>
        public bool Selected { get; set; } = false;

        /// <summary>
        /// Can this connector be selected
        /// </summary>
        public bool CanBeSelected { get; set; } = false;

        /// <summary>
        /// Allow multiple connections
        /// </summary>
        public bool AllowMultipleConnections { get; set; } = false;

        /// <summary>
        /// Gets if we got at least one connection
        /// </summary>
        public bool IsConnected => m_connections.Count > 0;

        /// <summary>
        /// Connections
        /// </summary>
        public NodeConnection[] Connections => m_connections.ToArray();

        /// <summary>
        /// Rectangle bounds
        /// </summary>
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// As connectors can live out of his parent, we need another bounds
        /// </summary>
        public Rectangle ConnectorBounds { get; private set; }

        /// <summary>
        /// Connector location
        /// </summary>
        public GraphLocation Location
        {
            get => m_location;
            set
            {
                if (m_location == value || m_location.X == value.X && m_location.Y == value.Y)
                    return;
                m_location = value;
                UpdateBounds();
            }
        }

        /// <summary>
        /// Connector size
        /// </summary>
        public GraphSize Size
        {
            get => m_size;
            set
            {
                if (m_size == value || m_size.Width == value.Width || m_size.Height == value.Height)
                    return;
                m_size = value;
                UpdateBounds();
            }
        }

        /// <summary>
        /// The outline connector color
        /// </summary>
        public GraphColor OutlineConnectorColor { get; set; } = new GraphColor(Color.White);

        /// <summary>
        /// The connector color
        /// </summary>
        public GraphColor ConnectorColor { get; set; } = new GraphColor(Color.Gray);

        /// <summary>
        /// Can these elements connect
        /// </summary>
        /// <param name="to">Element to connect</param>
        /// <returns><see cref="true"/> if they can connect, false otherwise</returns>
        public virtual bool CanConnect(IElement to)
        {
            NodeConnector nodeConnector = to as NodeConnector;
            return (nodeConnector != null && nodeConnector.Type != Type && nodeConnector.Owner != Owner);
        }

        /// <summary>
        /// Create a new connection
        /// </summary>
        /// <param name="to">To connector</param>
        public void Connect(NodeConnector to)
        {
            if (CanConnect(to) && to.CanConnect(this))
            {
                NodeConnection nodeCon = new NodeConnection(this, to);
                m_connections.Add(nodeCon);
                if (Owner is IConnectorHandler)
                    ((IConnectorHandler)Owner).OnConnected(Type, nodeCon);
            }
        }

        /// <summary>
        /// Disconnect
        /// </summary>
        /// <param name="connection">Connection</param>
        public void Disconnect(NodeConnection connection)
        {
            if (m_connections.Contains(connection))
                m_connections.Remove(connection);
            if(Owner is IConnectorHandler)
                ((IConnectorHandler)Owner).OnDisconnected(Type, connection);
        }

        /// <summary>
        /// Update bounds
        /// </summary>
        /// <param name="location">Location</param>
        internal void UpdateBounds(GraphLocation location = null)
        {
            if (location != null)
                Location = location;
            Bounds = new Rectangle(Location.X, Location.Y, Size.Width, Size.Height);
            ConnectorBounds = new Rectangle(Location.X - Bounds.Width / 2, Location.Y - Bounds.Height / 2, Size.Width, Size.Height);
        }

        /// <summary>
        /// Find element at the specified point
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns><see cref="IElement"/> if a element has been found, <see cref="null"/> otherwise</returns>
        public IElement FindElementAt(Point point)
        {
            if (ConnectorBounds.Contains(point))
                return this;
            foreach (NodeConnection connection in m_connections)
            {
                IElement element = connection.FindElementAt(point);
                if (element != null)
                    return element;
            }
            return null;
        }

        /// <summary>
        /// Render connector
        /// </summary>
        public void OnRender(ElementRenderEventArgs e)
        {
            if (Type == ConnectorType.Input)
            {
                foreach (NodeConnection nc in Connections)
                    nc.OnRender(e);
            }
            e.Graphics.DrawCircle(OutlineConnectorColor.Pen, Bounds.X, Bounds.Y, 5);
            e.Graphics.FillCircle(ConnectorColor.Brush, Bounds.X, Bounds.Y, 5);
        }
    }
}
