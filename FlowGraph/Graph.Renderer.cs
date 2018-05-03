using FlowGraph.Events;
using FlowGraph.Nodes.Connectors;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlowGraph
{
    public partial class Graph
    {
        /// <summary>
        /// Handle paint
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            long renderStart = Environment.TickCount;
            base.OnPaint(e);

            if (e.Graphics == null)
                return;

            e.Graphics.PageUnit = GraphicsUnit.Pixel;
            e.Graphics.CompositingQuality = CompositingQuality.GammaCorrected;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            UpdateMatrices();

            e.Graphics.Transform = Transformation;

            OnDrawBackground(e);

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            if (m_graphElements.Count > 0)
            {
                ElementRenderEventArgs renderEvent = new ElementRenderEventArgs(e.Graphics, GetTransformedLocation());
                foreach (IElement element in VisibleElements)
                    element.OnRender(renderEvent);
            }

            if (EditMode == GraphEditMode.SelectingBox)
            {
                Rectangle selectionRectangle = GetSelectionRectangle();
                if (FillSelectionRectangle)
                    e.Graphics.FillRectangle(m_selectionColor.Brush, selectionRectangle);
                e.Graphics.DrawRectangle(m_outlineSelectionColor.Pen, selectionRectangle);
            }
            else if (EditMode == GraphEditMode.Linking)
            {
                NodeConnector from = SelectedElement as NodeConnector;
                if (from != null)
                    e.Graphics.DrawLine(Pens.Yellow, from.Bounds.X, from.Bounds.Y, m_transformed_location.X, m_transformed_location.Y);
            }

            e.Graphics.SmoothingMode = SmoothingMode.None;

            m_renderTime = (Environment.TickCount - renderStart);

            if (ShowDebugInfos)
            {
                FrameRate.Call(); //Normally we should call the framerate at the end of a rendering method, but here we want to ignore the debug render time
                OnDrawDebug(e);
            }
        }

        /// <summary>
        /// Draw graph background
        /// </summary>
        protected virtual void OnDrawBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            if (!ShowGrid)
                return;

            PointF[] points = new PointF[] { new PointF(e.ClipRectangle.Left, e.ClipRectangle.Top), new PointF(e.ClipRectangle.Right, e.ClipRectangle.Bottom) };

            InverseTransformation.TransformPoints(points);

            float left = points[0].X;
            float right = points[1].X;
            float top = points[0].Y;
            float bottom = points[1].Y;

            float smallStepScaled = SmallGridStep;
            float largeStepScaled = LargeGridStep;

            if (smallStepScaled > 3)
            {
                float smallXOffset = ((float)Math.Round(left / smallStepScaled) * smallStepScaled);
                float smallYOffset = ((float)Math.Round(top / smallStepScaled) * smallStepScaled);
                for (float x = smallXOffset; x < right; x += smallStepScaled)
                    e.Graphics.DrawLine(m_smallGridStepColor.Pen, x, top, x, bottom);
                for (float y = smallYOffset; y < bottom; y += smallStepScaled)
                    e.Graphics.DrawLine(m_smallGridStepColor.Pen, left, y, right, y);
            }

            if (largeStepScaled > 3)
            {
                float largeXOffset = ((float)Math.Round(left / largeStepScaled) * largeStepScaled);
                float largeYOffset = ((float)Math.Round(top / largeStepScaled) * largeStepScaled);
                for (float x = largeXOffset; x < right; x += largeStepScaled)
                    e.Graphics.DrawLine(m_largeGridStepColor.Pen, x, top, x, bottom);
                for (float y = largeYOffset; y < bottom; y += largeStepScaled)
                    e.Graphics.DrawLine(m_largeGridStepColor.Pen, left, y, right, y);
            }
        }

        /// <summary>
        /// Draws debug information
        /// </summary>
        private void OnDrawDebug(PaintEventArgs e)
        {
            e.Graphics.Transform = new Matrix();
            e.Graphics.DrawString($"FPS: {FrameRate.FPS}", DebugFont, Brushes.White, new PointF(0.0f, 0.0f));
            e.Graphics.DrawString($"Total Render Time: {m_renderTime}ms", DebugFont, Brushes.White, new PointF(0.0f, 15.0f));
            e.Graphics.DrawString($"Edit Mode: {EditMode}", DebugFont, Brushes.White, new PointF(0.0f, 30.0f));
            e.Graphics.DrawString($"View Zoom: {Zoom.Zoom.ToString("0.000")}", DebugFont, Brushes.White, new PointF(0.0f, 45.0f));
            e.Graphics.DrawString($"Elements: {Elements.Count}", DebugFont, Brushes.White, new PointF(0.0f, 60.0f));
            e.Graphics.DrawString($"Visible Elements: {VisibleElements.Count}", DebugFont, Brushes.White, new PointF(0.0f, 75.0f));
        }

        /// <summary>
        /// On paint background
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {

        }
    }
}
