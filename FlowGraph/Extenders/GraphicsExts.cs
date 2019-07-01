using System.Drawing;

namespace FlowGraph
{
    public static class GraphicsExts
    {
        /// <summary>
        /// Draw circle
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="pen">Pen</param>
        /// <param name="centerX">Circle center X</param>
        /// <param name="centerY">Circle center Y</param>
        /// <param name="radius">Circle radius</param>
        public static void DrawCircle(this Graphics graphics, Pen pen, float centerX, float centerY, float radius)
        {
            float radius2 = radius * 2;
            graphics.DrawEllipse(pen, centerX - radius, centerY - radius, radius2, radius2);
        }

        /// <summary>
        /// Fill circle
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="brush">Brush</param>
        /// <param name="centerX">Circle center X</param>
        /// <param name="centerY">Circle center Y</param>
        /// <param name="radius">Circle radius</param>
        public static void FillCircle(this Graphics graphics, Brush brush, float centerX, float centerY, float radius)
        {
            float radius2 = radius * 2;
            graphics.FillEllipse(brush, centerX - radius, centerY - radius, radius2, radius2);
        }

        /// <summary>
        /// Draw rectangle around a center point
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="pen">Pen</param>
        /// <param name="xCenter">Rectangle center X</param>
        /// <param name="yCenter">Rectangle center Y</param>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        public static void DrawCenterRectangle(this Graphics graphics, Pen pen, int xCenter, int yCenter, int width, int height)
        {
            int x = xCenter - width / 2;
            int y = yCenter - height / 2;
            graphics.DrawRectangle(pen, x, y, width, height);
        }

        /// <summary>
        /// Fill rectangle around a center point
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="brush">Brush</param>
        /// <param name="xCenter">Rectangle center X</param>
        /// <param name="yCenter">Rectangle center Y</param>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        public static void FillCenterRectangle(this Graphics graphics, Brush brush, int xCenter, int yCenter, int width, int height)
        {
            int x = xCenter - width / 2;
            int y = yCenter - height / 2;
            graphics.FillRectangle(brush, x, y, width, height);
        }
    }
}