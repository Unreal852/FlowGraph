using System.Drawing;

namespace FlowGraph
{
    public static class RectangleExts
    {
        /// <summary>
        /// Checks if the specified point is in the rectangle
        /// </summary>
        /// <param name="rectangle">Rectangle</param>
        /// <param name="point">Point</param>
        /// <param name="offsetX">OffsetX</param>
        /// <param name="offsetY">OffsetY</param>
        /// <returns><see cref="true"/> if point is in rectangle, <see cref="false"/> otherwise</returns>
        public static bool Contains(this Rectangle rectangle, Point point, int offsetX, int offsetY)
        {
            int rX = rectangle.X - offsetX;
            int rY = rectangle.Y - offsetY;
            int rW = rectangle.X + rectangle.Width + offsetX;
            int rH = rectangle.Y + rectangle.Height + offsetY;
            return (rX < point.X && rY < point.Y && rW > point.X && rH > point.Y);
        }
    }
}
