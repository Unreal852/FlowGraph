using System.Drawing;

namespace FlowGraph
{
    public class GraphColor
    {
        public GraphColor(Color color)
        {
            Color = color;
            Pen = new Pen(color);
            Brush = new SolidBrush(Color);
        }
        
        public GraphColor(Color color, Pen pen)
        {
            Color = color;
            Pen = pen;
            Brush = new SolidBrush(Color);
        }

        /// <summary>
        /// Color
        /// </summary>
        public Color Color { get; }

        /// <summary>
        /// Pen
        /// </summary>
        public Pen Pen { get; }

        /// <summary>
        /// Brush
        /// </summary>
        public Brush Brush { get; }
    }
}
