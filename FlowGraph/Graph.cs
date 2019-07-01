using FlowGraph.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace FlowGraph
{
    public partial class Graph : Control
    {
        public readonly Matrix Transformation        = new Matrix();
        public readonly Matrix InverseTransformation = new Matrix();

        private float m_smallGridStep = 16.0f;
        private float m_largeGridStep = 16.0f * 8.0f;

        private bool m_dragElement = false;

        private bool m_showGrid       = true;
        private bool m_showDebugInfos = true;

        private bool m_timedRedraw = false;

        //private Point m_lastLocation;
        private Point m_lastLocation;

        private Point m_originalLocation;

        //private Point m_originalMouseLocation;
        private Point m_transformed_location;

        private Font m_debugFont = SystemFonts.DefaultFont;

        private GraphColor m_smallGridStepColor    = new GraphColor(Color.FromArgb(54, 54, 54));
        private GraphColor m_largeGridStepColor    = new GraphColor(Color.FromArgb(26, 26, 26));
        private GraphColor m_selectionColor        = new GraphColor(Color.FromArgb(30, 128, 90, 30));
        private GraphColor m_outlineSelectionColor = new GraphColor(Color.DarkOrange);
        private GraphColor m_linkingColor          = new GraphColor(Color.Yellow);

        private Timer m_timedRedrawTimer; // Todo: Create a hight precision timer; this one only use int...

        private readonly List<IElement> m_graphElements = new List<IElement>();

        public Graph()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.UserPaint, true);
            BackColor = Color.FromArgb(42, 42, 42);
            Selection = new GraphSelection(this);
        }

        /// <summary>
        /// Graph zoom
        /// </summary>
        [Browsable(false)]
        public GraphZoom Zoom { get; } = new GraphZoom();

        /// <summary>
        /// Graph selection
        /// </summary>
        [Browsable(false)]
        public GraphSelection Selection { get; }

        /// <summary>
        /// Edit mode
        /// </summary>
        [Browsable(false)]
        public EGraphEditMode EditMode { get; private set; }

        /// <summary>
        /// Graph elements
        /// </summary>
        [Browsable(false)]
        public ICollection<IElement> Elements => m_graphElements;

        /// <summary>
        /// Verify if we are Idle
        /// </summary>
        [Browsable(false)]
        public bool IsIdle => EditMode == EGraphEditMode.Idle;

        /// <summary>
        /// Verify if we are linking
        /// </summary>
        [Browsable(false)]
        public bool IsLinking => EditMode == EGraphEditMode.Linking;

        /// <summary>
        /// Verify if we are moving selection
        /// </summary>
        [Browsable(false)]
        public bool IsMovingSelection => EditMode == EGraphEditMode.MovingSelection;

        /// <summary>
        /// Verify if we are scrolling
        /// </summary>
        [Browsable(false)]
        public bool IsScrolling => EditMode == EGraphEditMode.Scrolling;

        /// <summary>
        /// Verify if we are selecting
        /// </summary>
        [Browsable(false)]
        public bool IsSelecting => EditMode == EGraphEditMode.Selecting;

        /// <summary>
        /// Verify if we are selecting box
        /// </summary>
        [Browsable(false)]
        public bool IsSelectingBox => EditMode == EGraphEditMode.SelectingBox;

        /// <summary>
        /// Verify if we are zooming
        /// </summary>
        [Browsable(false)]
        public bool IsZooming => EditMode == EGraphEditMode.Zooming;

        /// <summary>
        /// Should we fill the selection rectangle
        /// </summary>
        [Description("Fill the selection rectangle"), Category("Appearance")]
        public bool FillSelectionRectangle { get; set; } = false;

        /// <summary>
        /// The align margin between the aligned elements
        /// </summary>
        [Description("The align margin between the aligned elements"), Category("Appearance")]
        public int AlignMargin { get; set; } = 15;

        /// <summary>
        /// The interval between each redraw call
        /// </summary>
        [Description("The interval between each redraw call")]
        public double TimedRedrawInterval = 15; // This should be 16.666666

        /// <summary>
        /// Currently selected element
        /// </summary>
        [Browsable(false)]
        public IElement SelectedElement { get; set; }

        /// <summary>
        /// Graph visible elements
        /// </summary>
        [Browsable(false)]
        public ICollection<IElement> VisibleElements
        {
            get
            {
                List<IElement> elements = new List<IElement>();
                Rectangle viewRectangle = GetViewRectangle();
                foreach(IElement element in m_graphElements)
                    if(viewRectangle.IntersectsWith(element.Bounds))
                        elements.Add(element);
                elements.Reverse();
                return elements;
            }
        }

        /// <summary>
        /// Enable timed redraw, users actions will not redraw the graph
        /// </summary>
        public bool TimedRedraw
        {
            get => m_timedRedraw;
            set
            {
                if(m_timedRedraw == value)
                    return;
                m_timedRedraw = value;
                if(value)
                {
                    m_timedRedrawTimer = new Timer();
                    m_timedRedrawTimer.Interval = (int)TimedRedrawInterval;
                    m_timedRedrawTimer.Tick += (sender, e) => Invalidate();
                    m_timedRedrawTimer.Start();
                }
                else
                {
                    m_timedRedrawTimer.Stop();
                    m_timedRedrawTimer = null;
                }
            }
        }

        /// <summary>
        /// Show grid
        /// </summary>
        [Description("Show grid background"), Category("Appearance")]
        public bool ShowGrid
        {
            get => m_showGrid;
            set
            {
                if(m_showGrid == value)
                    return;
                m_showGrid = value;
                Redraw();
            }
        }

        /// <summary>
        /// Show debug infos
        /// </summary>
        [Description("Show debug informations"), Category("Debug")]
        public bool ShowDebugInfos
        {
            get => m_showDebugInfos;
            set
            {
                if(m_showDebugInfos == value)
                    return;
                m_showDebugInfos = value;
                Redraw();
            }
        }

        /// <summary>
        /// The distance between the smallest grid lines
        /// </summary>
        [Description("The distance between the smallest grid lines"), Category("Appearance")]
        public float SmallGridStep
        {
            get => m_smallGridStep;
            set
            {
                if(m_smallGridStep == value)
                    return;
                m_smallGridStep = value;
                Redraw();
            }
        }

        /// <summary>
        /// The distance between the largest grid lines
        /// </summary>
        [Description("The distance between the largest grid lines"), Category("Appearance")]
        public float LargeGridStep
        {
            get => m_largeGridStep;
            set
            {
                if(m_largeGridStep == value)
                    return;
                m_largeGridStep = value;
                Redraw();
            }
        }

        /// <summary>
        /// The color for the grid lines with the smallest gap between them
        /// </summary>
        [Description("The color for the grid lines with the smallest gap between them"), Category("Appearance")]
        public Color SmallGridStepColor
        {
            get => m_smallGridStepColor.Color;
            set
            {
                if(m_smallGridStepColor.Color == value)
                    return;
                m_smallGridStepColor = new GraphColor(value);
                Redraw();
            }
        }

        /// <summary>
        /// The color for the grid lines with the largest gap between them
        /// </summary>
        [Description("The color for the grid lines with the largest gap between them"), Category("Appearance")]
        public Color LargeGridStepColor
        {
            get => m_largeGridStepColor.Color;
            set
            {
                if(m_largeGridStepColor.Color == value)
                    return;
                m_largeGridStepColor = new GraphColor(value);
                Redraw();
            }
        }

        /// <summary>
        /// The color for the selection rectangle. Setting this with transparent color ( alpha ) will result in a super slow rendering
        /// </summary>
        [Description("The color for the selection rectangle. Setting this with transparent color ( alpha ) will result in a super slow rendering"), Category("Appearance")]
        public Color SelectionColor
        {
            get => m_selectionColor.Color;
            set
            {
                if(m_selectionColor.Color == value)
                    return;
                m_selectionColor = new GraphColor(value);
                Redraw();
            }
        }

        /// <summary>
        /// The color for the outline of the selection rectangle
        /// </summary>
        [Description("The color for the outline of the selection rectangle"), Category("Appearance")]
        public Color OutlineSelectionColor
        {
            get => m_outlineSelectionColor.Color;
            set
            {
                if(m_outlineSelectionColor.Color == value)
                    return;
                m_outlineSelectionColor = new GraphColor(value);
                Redraw();
            }
        }

        /// <summary>
        /// The color of the connection while beeing linked
        /// </summary>
        [Description("The color of the connection while beeing linked"), Category("Appearance")]
        public Color LinkingColor
        {
            get => m_linkingColor.Color;
            set
            {
                if(m_linkingColor.Color == value)
                    return;
                m_linkingColor = new GraphColor(value);
                Redraw();
            }
        }

        /// <summary>
        /// The debug informations font
        /// </summary>
        [Description("The debug informations font"), Category("Debug")]
        public Font DebugFont
        {
            get => m_debugFont;
            set
            {
                if(m_debugFont == null || m_debugFont == value)
                    return;
                m_debugFont = value;
                Redraw();
            }
        }

        /// <summary>
        /// Set edit mode
        /// </summary>
        /// <param name="editMode">Edit Mode</param>
        public void SetEditMode(EGraphEditMode editMode)
        {
            EditMode = editMode;
            Redraw();
        }

        /// <summary>
        /// Gets the view rectangle
        /// </summary>
        /// <returns><see cref="Rectangle"/> View rectangle</returns>
        public Rectangle GetViewRectangle()
        {
            int centerX = (int)(((Width / 2) - Transformation.OffsetX) / Zoom.Zoom);
            int centerY = (int)(((Height / 2) - Transformation.OffsetY) / Zoom.Zoom);
            int viewWidth = (int)((Width) / Zoom.Zoom);
            int viewHeight = (int)((Height) / Zoom.Zoom);
            return new Rectangle(centerX - (viewWidth / 2), centerY - (viewHeight / 2), viewWidth, viewHeight);
        }

        /// <summary>
        /// Gets the selection rectangle
        /// </summary>
        /// <returns><see cref="Rectangle"/> Selection</returns>
        public Rectangle GetSelectionRectangle()
        {
            Point transformed_location = GetTransformedLocation();
            int x = Math.Min(transformed_location.X, m_originalLocation.X);
            int y = Math.Min(transformed_location.Y, m_originalLocation.Y);
            int width = Math.Max(transformed_location.X, m_originalLocation.X) - x;
            int height = Math.Max(transformed_location.Y, m_originalLocation.Y) - y;
            return new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Gets the transformed location based on the current snapped one
        /// </summary>
        /// <returns><see cref="Point"/> Transformed location</returns>
        public Point GetTransformedLocation()
        {
            return m_transformed_location;
            /*
            Point[] points = new Point[] { m_lastLocation };
            InverseTransformation.TransformPoints(points);
            Point transformed_location = points[0];
            // if (abortDrag) transformed_location = originalLocation; 
            return transformed_location; */
        }

        /// <summary>
        /// Move selected element
        /// </summary>
        /// <param name="currentLocation">Current view location</param>
        /// <param name="element">element to move</param>
        /// <param name="deltaX">Delta X</param>
        /// <param name="deltaY">Delta Y</param>
        private void MoveElement(IElement element, float deltaX, float deltaY)
        {
            element.Location = new GraphLocation((int)Math.Round(element.Location.X - deltaX), (int)Math.Round(element.Location.Y - deltaY));
            if(element is NodeGroup group)
            {
                foreach(Node n in group.Nodes)
                    MoveElement(n, deltaX, deltaY);
            }
        }

        /// <summary>
        /// Move view
        /// </summary>
        /// <param name="currentLocation">Current view location</param>
        /// <param name="x">To X</param>
        /// <param name="y">To Y</param>
        private void MoveView(float x, float y)
        {
            Zoom.Translation = new PointF((Zoom.Translation.X - x * Zoom.Zoom), (Zoom.Translation.Y - y * Zoom.Zoom));
        }

        /// <summary>
        /// Gets the original mouse location
        /// </summary>
        /// <returns><see cref="Point"/> Original mouse location</returns>
        public Point GetOriginalMouseLocation()
        {
            if(m_originalLocation != null)
            {
                Point[] points = new Point[] {m_originalLocation};
                Transformation.TransformPoints(points);
                return PointToScreen(new Point(points[0].X, points[0].Y)); // Todo: creating a new one ? why not getting the one directly in the points
            }

            return new Point(0, 0);
        }

        /// <summary>
        /// The the transformed location
        /// </summary>
        /// <param name="baseLoc">Base location</param>
        private void SetTransformedLocation(Point baseLoc)
        {
            Point[] points = new Point[] {baseLoc};
            InverseTransformation.TransformPoints(points);
            m_transformed_location = points[0];
        }

        /// <summary>
        /// Add a new element
        /// </summary>
        /// <param name="element">Element to add</param>
        /// <param name="select">Select element</param>
        public void AddElement(IElement element, bool select = false)
        {
            if(m_graphElements.Contains(element))
                return;
            m_graphElements.Add(element);
            if(select && element.CanBeSelected)
                Selection.AddElement(element);
            Redraw();
        }

        /// <summary>
        /// Remove element 
        /// </summary>
        /// <param name="element">Element to remove</param>
        public void RemoveElement(IElement element)
        {
            if(m_graphElements.Contains(element))
            {
                m_graphElements.Remove(element);
                Redraw();
            }
        }

        /// <summary>
        /// Clear elements
        /// </summary>
        public void ClearElements()
        {
            m_graphElements.Clear();
            Redraw();
        }

        /// <summary>
        /// Gets elements
        /// </summary>
        /// <param name="onlyVisible">Get only elements that are in the user view</param>
        /// <returns><see cref="IElement"/> Elements</returns>
        public IElement[] GetElements(bool onlyVisible)
        {
            return (onlyVisible ? VisibleElements : Elements).ToArray();
        }

        /// <summary>
        /// Finds element at the specified location
        /// </summary>
        /// <param name="location">Location to lookup</param>
        /// <param name="onlyVisible">Only look for user visible element</param>
        /// <returns><see cref="IElement"/> if a element has been found, <see cref="null"/> otherwise</returns>
        public IElement FindElementAt(Point location, bool onlyVisible)
        {
            foreach(IElement element in GetElements(onlyVisible))
            {
                IElement foundElement = element.FindElementAt(location);
                if(foundElement != null)
                    return foundElement;
            }

            return null;
        }

        /// <summary>
        /// Finds element at the specified location that match the given type
        /// </summary>
        /// <typeparam name="T">Element Type</typeparam>
        /// <param name="location">Location to lookup</param>
        /// <param name="onlyVisible">Only look for user visible element</param>
        /// <returns><see cref="IElement"/> if a element has been found, <see cref="null"/> otherwise</returns>
        public IElement FindElementAt<T>(Point location, bool onlyVisible) where T : IElement
        {
            foreach(IElement element in GetElements(onlyVisible))
            {
                IElement foundElement = element.FindElementAt(location);
                if(foundElement is T e)
                    return e;
            }

            return null;
        }

        /// <summary>
        /// Finds elements at the specified location
        /// </summary>
        /// <param name="location">Location to lookup</param>
        /// <param name="onlyVisible">Only look for user visible element</param>
        /// <returns><see cref="IElement[]"/> Elements</returns>
        public IElement[] FindElementsAt(Point location, bool onlyVisible)
        {
            List<IElement> elements = new List<IElement>();
            foreach(IElement element in GetElements(onlyVisible))
            {
                IElement foundElement = element.FindElementAt(location);
                if(foundElement != null)
                    elements.Add(foundElement);
            }

            return elements.ToArray();
        }

        /// <summary>
        /// Finds elements at the specified location that match the given type
        /// </summary>
        /// <typeparam name="T">Element Type</typeparam>
        /// <param name="location">Location to lookup</param>
        /// <param name="onlyVisible">Only look for user visible element</param>
        /// <returns><see cref="IElement[]"/> Elements</returns>
        public IElement[] FindElementsAt<T>(Point location, bool onlyVisible) where T : IElement
        {
            List<IElement> elements = new List<IElement>();
            foreach(IElement element in GetElements(onlyVisible))
            {
                IElement foundElement = element.FindElementAt(location);
                if(foundElement is T e)
                    elements.Add(e);
            }

            return elements.ToArray();
        }

        /// <summary>
        /// Selects elements 
        /// </summary>
        private void SelectElements()
        {
            Rectangle selectRectangle = GetSelectionRectangle();
            List<IElement> selectedElements = new List<IElement>();
            foreach(IElement element in m_graphElements)
            {
                if(!element.CanBeSelected || element.Owner != null)
                    continue;
                if(element.Bounds.IntersectsWith(selectRectangle))
                    selectedElements.Add(element);
            }

            Selection.UnselectAll();
            Selection.AddElements(selectedElements);
        }

        /// <summary>
        /// Call a action on all elements at the specified location
        /// </summary>
        /// <param name="location">Location</param>
        /// <param name="action">Action to execute</param>
        /// <param name="onlyVisible">Find only visible elements</param>
        public void CallElementsAt(Point location, Action<IElement> action, bool onlyVisible = true)
        {
            foreach(IElement element in FindElementsAt(location, onlyVisible))
                action(element);
        }

        /// <summary>
        /// Bring element to front
        /// </summary>
        /// <param name="element">Element</param>
        public void BringElementToFront(IElement element)
        {
            m_graphElements.Remove(element);
            m_graphElements.Insert(0, element);
            if(element is NodeGroup group)
            {
                foreach(Node node in group.Nodes)
                {
                    m_graphElements.Remove(node);
                    m_graphElements.Insert(0, node);
                }
            }

            Redraw();
        }

        public void Redraw(bool instantly = false)
        {
            if(TimedRedraw)
                return;
            if(instantly)
                Refresh();
            else
                Invalidate();
        }

        /// <summary>
        /// Update matrices
        /// </summary>
        private void UpdateMatrices()
        {
            if(Zoom.Zoom < 0.25f)
                Zoom.Zoom = 0.25f;
            if(Zoom.Zoom > 5.00f)
                Zoom.Zoom = 5.00f;

            PointF center = new PointF(Width / 2.0f, Height / 2.0f);

            Transformation.Reset();
            Transformation.Translate(Zoom.Translation.X, Zoom.Translation.Y);
            Transformation.Translate(center.X, center.Y);
            Transformation.Scale(Zoom.Zoom, Zoom.Zoom);
            Transformation.Translate(-center.X, -center.Y);

            InverseTransformation.Reset();
            InverseTransformation.Translate(center.X, center.Y);
            InverseTransformation.Scale(1.0f / Zoom.Zoom, 1.0f / Zoom.Zoom);
            InverseTransformation.Translate(-center.X, -center.Y);
            InverseTransformation.Translate(-Zoom.Translation.X, -Zoom.Translation.Y);
        }

        /// <summary>
        /// Align given elements
        /// </summary>
        /// <param name="elements">Elements to align</param>
        /// <param name="align">Align type</param>
        public void AlignElements(ICollection<IElement> elements, EAlignType align)
        {
            if(elements.Count > 1)
            {
                switch(align)
                {
                    case EAlignType.Vertically:
                    {
                        IElement lastElement = elements.ElementAt(0);
                        foreach(IElement element in elements)
                        {
                            if(element == lastElement)
                                continue;
                            element.Location = new GraphLocation(lastElement.Location.X, (lastElement.Location.Y + lastElement.Size.Height) + AlignMargin);
                            lastElement = element;
                        }
                    }
                        break;
                    case EAlignType.Horizontally:
                    {
                        IElement lastElement = elements.ElementAt(0);
                        foreach(IElement element in elements)
                        {
                            if(element == lastElement)
                                continue;
                            element.Location = new GraphLocation((lastElement.Location.X + lastElement.Size.Width) + AlignMargin, lastElement.Location.Y);
                            lastElement = element;
                        }
                    }
                        break;
                    case EAlignType.Diagonally:
                    {
                        IElement lastElement = elements.ElementAt(0);
                        foreach(IElement element in elements)
                        {
                            if(element == lastElement)
                                continue;
                            element.Location = new GraphLocation((lastElement.Location.X + lastElement.Size.Width) + AlignMargin, (lastElement.Location.Y + lastElement.Size.Height) + AlignMargin);
                            lastElement = element;
                        }
                    }
                        break;
                }

                Redraw();
            }
        }
    }
}