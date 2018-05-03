using System.Drawing;

namespace FlowGraph
{
    public class GraphZoom
    {
        public GraphZoom()
        {

        }

        /// <summary>
        /// Zoom translation
        /// </summary>
        public PointF Translation { get; set; } = new PointF();

        /// <summary>
        /// Current zoom
        /// </summary>
        public float Zoom { get; set; } = 1.0f;
    }
}
