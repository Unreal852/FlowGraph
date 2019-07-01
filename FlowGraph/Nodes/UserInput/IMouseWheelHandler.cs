using System.Windows.Forms;

namespace FlowGraph.Nodes.UserInput
{
    public interface IMouseWheelHandler
    {
        void OnMouseWheelUp(MouseEventArgs e);
        void OnMouseWheelDown(MouseEventArgs e);
    }
}