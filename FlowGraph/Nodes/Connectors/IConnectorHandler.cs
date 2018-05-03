using FlowGraph.Nodes.Connections;

namespace FlowGraph.Nodes.Connectors
{
    public interface IConnectorHandler
    {
        /// <summary>
        /// Called when a connection occurs 
        /// </summary>
        /// <param name="type">The local connector who got connected</param>
        /// <param name="connection">Node connection infos</param>
        void OnConnected(ConnectorType type, NodeConnection connection);

        /// <summary>
        /// Called when a connection is disconnected
        /// </summary>
        /// <param name="type">The local connector who got disconnected</param>
        /// <param name="connection">Node connection infos</param>
        void OnDisconnected(ConnectorType type, NodeConnection connection);

        /// <summary>
        /// Called when the input value is changed ( input connector must be connected to a output one )
        /// </summary>
        void OnInputValueChanged();
    }
}
