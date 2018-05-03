using System.Drawing;
using System.Windows.Forms;

namespace FlowGraph
{
    public interface IInputHandler
    {
        /// <summary>
        /// Called when this is clicked
        /// </summary>
        void OnClick(Point mouseLoc);

        /// <summary>
        /// Called when this is double clicked
        /// </summary>
        void OnDoubleClick(Point mouseLoc);

        /// <summary>
        /// Called when a keyboard key is pressed
        /// </summary>
        /// <param name="e">Event</param>
        void OnKeyDown(KeyEventArgs e);

        /// <summary>
        /// Called when a keyboard key is released
        /// </summary>
        /// <param name="e">Event</param>
        void OnKeyUp(KeyEventArgs e);
    }
}
