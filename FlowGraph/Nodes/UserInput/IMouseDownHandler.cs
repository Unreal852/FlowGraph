using System.Windows.Forms;

namespace FlowGraph.Nodes.UserInput
{
    public interface IMouseDownHandler
    {
        void OnMouseDown(MouseEventArgs e);
    }
}