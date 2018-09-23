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
        public static bool Contains(this Rectangle rectangle, int x, int y, int offsetX = 0, int offsetY = 0)
        {
            int rX = rectangle.X - offsetX;
            int rY = rectangle.Y - offsetY;
            int rW = rectangle.X + rectangle.Width + offsetX;
            int rH = rectangle.Y + rectangle.Height + offsetY;
            return (rX < x && rY < y && rW > x && rH > y);
        }

        public static bool Contains(this Rectangle rectangle, Point point, int offsetX = 0, int offsetY = 0)
        {
            return rectangle.Contains(point.X, point.Y);
        }

        public static bool FullyContains(this Rectangle baseRectangle, Rectangle rectangle, int offsetX = 0, int offsetY = 0)
        {
            return (baseRectangle.Contains(rectangle.X, rectangle.Y, 0, 0) &&
                    baseRectangle.Contains(rectangle.X + rectangle.Width, rectangle.Y, 0, 0) &&
                    baseRectangle.Contains(rectangle.X, rectangle.Y + rectangle.Height, 0, 0) &&
                    baseRectangle.Contains(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, 0, 0));
        }
    }
}
