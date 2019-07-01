using System.Windows.Forms;

namespace FlowGraph.Nodes.UserInput
{
    public interface IKeyDownHandler
    {
        void OnKeyDown(KeyEventArgs e);
    }
}