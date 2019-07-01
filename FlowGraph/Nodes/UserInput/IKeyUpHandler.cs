using System.Windows.Forms;

namespace FlowGraph.Nodes.UserInput
{
    public interface IKeyUpHandler
    {
        void OnKeyUp(KeyEventArgs e);
    }
}