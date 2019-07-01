using FlowGraph.Nodes.Connections;
using FlowGraph.Nodes.Connectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace FlowGraph.Helpers
{
    public static class RenderHelper
    {
        /// <summary>
        /// Get line path
        /// </summary>
        /// <param name="x1">From X</param>
        /// <param name="y1">From Y</param>
        /// <param name="x2">To X</param>
        /// <param name="y2">To Y</param>
        /// <param name="centerX">Center X</param>
        /// <param name="centerY">Center Y</param>
        /// <param name="include_arrow">Include arrow in the end of the line</param>
        /// <param name="connectorSize">Arrow size</param>
        /// <param name="extra_thickness">Extra thickness</param>
        /// <returns><see cref="GraphicsPath"/> Path</returns>
        public static (GraphicsPath Path, float CenterX, float CenterY) GetLinePath(float x1, float y1, float x2, float y2, bool include_arrow = false, int connectorSize = 1, float extra_thickness = 0)
        {
            (List<PointF> Points, float CenterX, float CenterY) result = GetLinePoints(x1, y1, x2, y2, connectorSize, extra_thickness);
            GraphicsPath path = new GraphicsPath(FillMode.Winding);
            path.AddLines(result.Points.ToArray());
            if(include_arrow)
                path.AddLines(GetArrowPoints(x2, y2, connectorSize, extra_thickness).ToArray());
            path.CloseFigure();
            return (path, result.CenterX, result.CenterY);
        }

        /// <summary>
        /// Get points
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="connectorSize">Connector size</param>
        /// <param name="extra_thickness">extra thickness</param>
        /// <returns><see cref="PointF[]"/> Points</returns>
        public static PointF[] GetArrowPoints(float x, float y, int connectorSize = 1, float extra_thickness = 0)
        {
            return new PointF[]
            {
                new PointF(x - (connectorSize + 1.0f) - extra_thickness, y + (connectorSize / 1.5f) + extra_thickness), new PointF(x + 1.0f + extra_thickness, y),
                new PointF(x - (connectorSize + 1.0f) - extra_thickness, y - (connectorSize / 1.5f) - extra_thickness)
            };
        }

        /// <summary>
        /// Get lines point
        /// </summary>
        /// <param name="x1">From X</param>
        /// <param name="y1">From Y</param>
        /// <param name="x2">To X</param>
        /// <param name="y2">To Y</param>
        /// <param name="centerX">Center X</param>
        /// <param name="centerY">Center Y</param>
        /// <param name="connectorSize">Arrow size</param>
        /// <param name="extra_thickness">Extra thickness</param>
        /// <returns><see cref="List{PointF}"/> Points</returns>
        public static (List<PointF> Points, float CenterX, float CenterY) GetLinePoints(float x1, float y1, float x2, float y2, int connectorSize = 1, float extra_thickness = 0)
        {
            float centerX;
            float centerY;
            float widthX = (x2 - x1);
            float lengthX = Math.Max(60, Math.Abs(widthX / 2)); //+ Math.Max(0, -widthX / 2)
            int lengthY = 0;                                    // Math.Max(-170, Math.Min(-120.0f, widthX - 120.0f)) + 120.0f; 
            if(widthX < 120)
                lengthX = 60;
            float yB = ((y1 + y2) / 2) + lengthY; // (y2 + ((y1 - y2) / 2) * 0.75f) + lengthY;
            float yC = y2 + yB;
            float xC = (x1 + x2) / 2;
            float xA = x1 + lengthX;
            float xB = x2 - lengthX;
            // if (widthX >= 120) { xA = xB = xC = x2 - 60; }
            List<PointF> points = new List<PointF> {new PointF(x1, y1), new PointF(xA, y1), new PointF(xB, y2), new PointF(x2 - connectorSize - extra_thickness, y2)};
            float t = 1.0f; //Math.Min(1, Math.Max(0, (widthX - 30) / 60.0f));
            float yA = (yB * t) + (yC * (1 - t));
            if(widthX <= 120)
            {
                points.Insert(2, new PointF(xB, yA));
                points.Insert(2, new PointF(xC, yA));
                points.Insert(2, new PointF(xA, yA));
            }

            using(GraphicsPath tempPath = new GraphicsPath())
            {
                tempPath.AddBeziers(points.ToArray());
                tempPath.Flatten();
                points = tempPath.PathPoints.ToList();
            }

            PointF[] angles = new PointF[points.Count - 1];
            float[] lengths = new float[points.Count - 1];
            float totalLength = 0;
            centerX = 0;
            centerY = 0;
            points.Add(points[points.Count - 1]);
            for(int i = 0; i < points.Count - 2; i++)
            {
                PointF pt1 = points[i];
                PointF pt2 = points[i + 1];
                PointF pt3 = points[i + 2];
                float deltaX = ((pt2.X - pt1.X) + (pt3.X - pt2.X));
                float deltaY = ((pt2.Y - pt1.Y) + (pt3.Y - pt2.Y));
                float length = (float)Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
                if(length <= 1.0f)
                {
                    points.RemoveAt(i);
                    i--;
                    continue;
                }

                lengths[i] = length;
                totalLength += length;
                angles[i].X = deltaX / length;
                angles[i].Y = deltaY / length;
            }

            float midLength = (totalLength / 2.0f); // * 0.75f;
            float startWidth = extra_thickness + 0.75f;
            float endWidth = extra_thickness + (connectorSize / 3.5f);
            float currentLength = 0;
            List<PointF> newPoints = new List<PointF>();
            newPoints.Add(points[0]);
            for(int i = 0; i < points.Count - 2; i++)
            {
                PointF angle = angles[i];
                PointF point = points[i + 1];
                float length = lengths[i];
                float width = (((currentLength * (endWidth - startWidth)) / totalLength) + startWidth);
                float angleX = angle.X * width;
                float angleY = angle.Y * width;
                float newLength = currentLength + length;
                if(currentLength <= midLength && newLength >= midLength)
                {
                    float dX = point.X - points[i].X;
                    float dY = point.Y - points[i].Y;
                    float t1 = midLength - currentLength;
                    float l = length;
                    centerX = points[i].X + ((dX * t1) / l);
                    centerY = points[i].Y + ((dY * t1) / l);
                }

                PointF pt1 = new PointF(point.X - angleY, point.Y + angleX);
                PointF pt2 = new PointF(point.X + angleY, point.Y - angleX);
                if(Math.Abs(newPoints[newPoints.Count - 1].X - pt1.X) > 1.0f || Math.Abs(newPoints[newPoints.Count - 1].Y - pt1.Y) > 1.0f)
                    newPoints.Add(pt1);
                if(Math.Abs(newPoints[0].X - pt2.X) > 1.0f || Math.Abs(newPoints[0].Y - pt2.Y) > 1.0f)
                    newPoints.Insert(0, pt2);
                currentLength = newLength;
            }

            return (newPoints, centerX, centerY);
        }

        /// <summary>
        /// Gets the connection region of the specified Node Connection
        /// </summary>
        /// <param name="connection">Node connection</param>
        /// <returns><see cref="Region"/> Connection Region</returns>
        public static Region GetConnectionRegion(NodeConnection connection)
        {
            NodeConnector to = connection.To;
            NodeConnector from = connection.From;
            RectangleF toBounds = to.Bounds;
            RectangleF fromBounds = from.Bounds;
            float x1 = (toBounds.Left + toBounds.Right) / 2.0f;
            float y1 = (toBounds.Top + toBounds.Bottom) / 2.0f;
            float x2 = (fromBounds.Left + fromBounds.Right) / 2.0f;
            float y2 = (fromBounds.Top + fromBounds.Bottom) / 2.0f;
            Region region;
            using(GraphicsPath linePath = GetLinePath(x1, y1, x2, y2, false, extra_thickness: 5.0f).Path)
                region = new Region(linePath);
            return region;
        }
    }
}