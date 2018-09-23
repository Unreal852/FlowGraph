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
            long d_renderStart = DateTime.UtcNow.Ticks;

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
                m_debugElementsRenderTime = DateTime.UtcNow.Ticks;
                foreach (IElement element in VisibleElements)
                    element.OnRender(renderEvent);
                m_debugElementsRenderTime = (DateTime.UtcNow.Ticks - m_debugElementsRenderTime);
            }

            switch(EditMode)
            {
                case GraphEditMode.SelectingBox:
                    {
                        Rectangle selectionRectangle = GetSelectionRectangle();
                        if (FillSelectionRectangle)
                            e.Graphics.FillRectangle(m_selectionColor.Brush, selectionRectangle);
                        e.Graphics.DrawRectangle(m_outlineSelectionColor.Pen, selectionRectangle);
                    }
                    break;
                case GraphEditMode.Linking:
                    {
                        NodeConnector from = SelectedElement as NodeConnector;
                        if (from != null)
                            e.Graphics.DrawLine(Pens.Yellow, from.Bounds.X, from.Bounds.Y, m_transformed_location.X, m_transformed_location.Y);
                    }
                    break;
            }

            e.Graphics.SmoothingMode = SmoothingMode.None;

            m_debugTotalRenderTime = (DateTime.UtcNow.Ticks - d_renderStart);

            if (ShowDebugInfos)
            {
                DebugCall();  //we should call the framerate at the end of a rendering method, but here we want to ignore the debug render time
                OnDrawDebug(e);
            }
        }

        /// <summary>
        /// Draw graph background
        /// </summary>
        protected virtual void OnDrawBackground(PaintEventArgs e)
        {
            m_debugBackgroundRenderTime = DateTime.UtcNow.Ticks;

            e.Graphics.Clear(BackColor);

            if (!ShowGrid)
            {
                m_debugBackgroundRenderTime = (DateTime.UtcNow.Ticks - m_debugBackgroundRenderTime);
                return;
            }

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

            m_debugBackgroundRenderTime = (DateTime.UtcNow.Ticks - m_debugBackgroundRenderTime);
        }

        /// <summary>
        /// Draws debug information
        /// </summary>
        private void OnDrawDebug(PaintEventArgs e)
        {
            e.Graphics.Transform = new Matrix();

            (string Text, Color Color)[] debugLines = new (string Text, Color Color)[]
            {
                ($"FPS: {FPS}", FPS >= 50 ? Color.GreenYellow : FPS >= 30 ? Color.Yellow : Color.Red),
                ($"Elements Render Time: {ElementsRenderTime.ToString("0.00")}ms", ElementsRenderTime <= 16.66 ? Color.GreenYellow : ElementsRenderTime <= 33.33 ? Color.Yellow : Color.Red),
                ($"Background Render Time: {BackgroundRenderTime.ToString("0.00")}ms", BackgroundRenderTime <= 16.66 ? Color.GreenYellow : BackgroundRenderTime <= 33.33 ? Color.Yellow : Color.Red),
                ($"Total Render Time: {TotalRenderTime.ToString("0.00")}ms", TotalRenderTime <= 16.66 ? Color.GreenYellow : TotalRenderTime <= 33.33 ? Color.Yellow : Color.Red),
                ($"Edit Mode: {EditMode}", Color.GreenYellow),
                ($"View Zoom: {Zoom.Zoom.ToString("0.00")}", Color.GreenYellow),
                ($"Elements: {Elements.Count}", Color.GreenYellow),
                ($"Visible Elements: {VisibleElements.Count}", Color.GreenYellow),
            };

            for (int i = 0; i < debugLines.Length; i++)
                TextRenderer.DrawText(e.Graphics, debugLines[i].Text, DebugFont, new Point(0, i * 15), debugLines[i].Color);
        }

        /// <summary>
        /// On paint background
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {

        }
    }
}
