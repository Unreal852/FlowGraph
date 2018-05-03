using System;
using System.Drawing;

namespace FlowGraph.Events
{
    public class ElementRenderEventArgs : EventArgs
    {
        public ElementRenderEventArgs(Graphics graphics, Point mouseLoc, object tag = null)
        {
            Graphics = graphics;
            MouseLocation = mouseLoc;
            Tag = tag;
        }

        /// <summary>
        /// Graphics instance
        /// </summary>
        public Graphics Graphics { get; }

        /// <summary>
        /// Current mouse location
        /// </summary>
        public Point MouseLocation { get; }

        /// <summary>
        /// Tag object
        /// </summary>
        public object Tag { get; }
    }
}
