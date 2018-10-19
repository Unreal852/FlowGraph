using FlowGraph.Nodes;
using FlowGraph.Nodes.Connectors;
using FlowGraph.Nodes.UserInput;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlowGraph
{
    public partial class Graph
    {
        /// <summary>
        /// Handle mouse wheel for zoomin/out
        /// </summary>
        protected override void OnMouseWheel(MouseEventArgs e) // Todo: zoom to mouse location
        {
            base.OnMouseWheel(e);

            Zoom.Zoom *= (float)Math.Pow(2, e.Delta / 480.0f);

            //Todo: Zoom Event

            Redraw();
        }

        /// <summary>
        /// Handle mouse down
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            m_lastLocation = e.Location;
            SetTransformedLocation(e.Location);
            m_originalLocation = m_transformed_location;
            if (e.Button == MouseButtons.Left)
            {
                IElement clickedElement = FindElementAt(Point.Round(m_transformed_location), true);
                if (clickedElement != null)
                {
                    clickedElement.As<IMouseDownHandler>()?.OnMouseDown(e);
                    if (clickedElement.CanBeSelected && !clickedElement.Selected)
                    {
                        Selection.UnselectAll();
                        Selection.AddElement(clickedElement);
                        BringElementToFront(clickedElement);
                    }
                    m_dragElement = true;
                }
                else
                {
                    m_dragElement = false;
                }
                SelectedElement = clickedElement;
                if (SelectedElement != null)
                    SelectedElement.Selected = true;
            }
        }

        /// <summary>
        /// Handle mouse up
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left && EditMode != EGraphEditMode.SelectingBox)
            {
                IElement clickedElement = FindElementAt(m_transformed_location, true);
                if (clickedElement != null)
                {
                    System.Diagnostics.Debug.Print(clickedElement.ToString());
                    clickedElement.As<IMouseUpHandler>()?.OnMouseUp(e);
                    if (clickedElement is Node node)
                    {
                        if (node.Owner is NodeGroup group)
                        {
                            if (!group.Bounds.FullyContains(node.Bounds))
                                group.RemoveNode(node);
                        }
                    }
                    else if (clickedElement is NodeConnector)
                    {
                        NodeConnector from = SelectedElement as NodeConnector;
                        if (from != null)
                        {
                            NodeConnector to = (NodeConnector)clickedElement;
                            from.Connect(to);
                            to.Connect(from);
                        }
                    }
                    else if (FindElementAt<NodeGroup>(m_transformed_location, true) is NodeGroup group)
                    {
                        foreach (IElement element in Selection)
                        {
                            if (element is Node elementNode)
                            {
                                if (group.Bounds.FullyContains(elementNode.Bounds))
                                    group.AddNode(elementNode);
                            }
                        }
                    }
                    else
                        Selection.UnselectAll();
                }
                else
                {
                    Selection.UnselectAll();
                }
            }
            m_dragElement = false;
            SetEditMode(EGraphEditMode.Idle);
        }

        /// <summary>
        /// Handle mouse double click
        /// </summary>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            Point mouseLoc = GetTransformedLocation();

            IElement element = FindElementAt(mouseLoc, true);
            element?.As<IMouseDoubleClickHandler>()?.OnMouseDoubleClick(mouseLoc);
        }

        /// <summary>
        /// Handle mouse move
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            Point currentLocation = e.Location;
            SetTransformedLocation(currentLocation);

            float deltaX = (m_lastLocation.X - currentLocation.X) / Zoom.Zoom;
            float deltaY = (m_lastLocation.Y - currentLocation.Y) / Zoom.Zoom;

            if (e.Button == MouseButtons.Left)
            {
                if (EditMode == EGraphEditMode.Idle)
                {
                    if (SelectedElement is NodeConnector)
                    {
                        if (EditMode != EGraphEditMode.Linking)
                            SetEditMode(EGraphEditMode.Linking);
                    }
                    else if (SelectedElement is IExpandableElement expandableElement && expandableElement.IsInAnyGrip(m_transformed_location))
                    {
                        switch (expandableElement.GetCurrentGrip(m_transformed_location))
                        {
                            case EGrip.Bottom:
                                SetEditMode(EGraphEditMode.ExpandingBottom);
                                break;
                            case EGrip.BottomRight:
                                SetEditMode(EGraphEditMode.ExpandingBottomRight);
                                break;
                            case EGrip.Right:
                                SetEditMode(EGraphEditMode.ExpandingRight);
                                break;
                        }
                    }
                    else
                    {
                        if (!m_dragElement)
                            SetEditMode(EGraphEditMode.SelectingBox);
                        else if (m_dragElement)
                            SetEditMode(EGraphEditMode.MovingSelection);
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (EditMode == EGraphEditMode.Idle)
                {
                    if (EditMode != EGraphEditMode.Scrolling)
                        SetEditMode(EGraphEditMode.Scrolling);
                }
            }
            else if (e.Button == MouseButtons.Middle)
            {

            }
            else
            {
                IElement element = FindElementAt(m_transformed_location, true);
                if (element is IExpandableElement expandableElement)
                {
                    switch(expandableElement.GetCurrentGrip(m_transformed_location))
                    {
                        case EGrip.BottomRight:
                            Cursor.Current = Cursors.SizeNWSE;
                            break;
                        case EGrip.Right:
                            Cursor.Current = Cursors.SizeWE;
                            break;
                        case EGrip.Bottom:
                            Cursor.Current = Cursors.SizeNS;
                            break;
                    }
                }
                else
                    Cursor.Current = Cursors.Default;
                m_lastLocation = currentLocation;
                Redraw();
                return;
            }

            if (Math.Abs(deltaX) > 1 || Math.Abs(deltaY) > 1)
            {
                switch (EditMode)
                {
                    case EGraphEditMode.Scrolling:
                        {
                            MoveView(deltaX, deltaY);
                            m_lastLocation = currentLocation;
                            Redraw();
                        }
                        break;
                    case EGraphEditMode.SelectingBox:
                        {
                            SelectElements();
                            m_lastLocation = currentLocation;
                            Redraw();
                        }
                        break;
                    case EGraphEditMode.MovingSelection:
                        {
                            foreach (IElement sElement in Selection.Elements)
                                MoveElement(sElement, deltaX, deltaY);
                            m_lastLocation = currentLocation;
                            Redraw();
                        }
                        break;
                    case EGraphEditMode.Linking:
                        {
                            m_lastLocation = currentLocation;
                            Redraw();
                        }
                        break;
                    case EGraphEditMode.ExpandingBottomRight:
                        {
                            if (SelectedElement is IExpandableElement expandableElement)
                            {
                                Cursor.Current = Cursors.SizeNWSE;
                                expandableElement.Size = new GraphSize((m_transformed_location.X - expandableElement.Location.X), (m_transformed_location.Y - expandableElement.Location.Y));
                            }
                            m_lastLocation = currentLocation;
                            Redraw();
                        }
                        break;
                    case EGraphEditMode.ExpandingRight:
                        {
                            if (SelectedElement is IExpandableElement expandableElement)
                            {
                                Cursor.Current = Cursors.SizeWE;
                                expandableElement.Size = new GraphSize((m_transformed_location.X - expandableElement.Location.X), expandableElement.Size.Height);
                            }
                            m_lastLocation = currentLocation;
                            Redraw();
                        }
                        break;
                    case EGraphEditMode.ExpandingBottom:
                        {
                            if (SelectedElement is IExpandableElement expandableElement)
                            {
                                Cursor.Current = Cursors.SizeNS;
                                expandableElement.Size = new GraphSize(expandableElement.Size.Width, (m_transformed_location.Y - expandableElement.Location.Y));
                            }
                            m_lastLocation = currentLocation;
                            Redraw();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Handle key down
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.V)
                AlignElements(Selection.Elements, EAlignType.Vertically);
            else if (e.KeyCode == Keys.H)
                AlignElements(Selection.Elements, EAlignType.Horizontally);
            else if (e.KeyCode == Keys.D)
                AlignElements(Selection.Elements, EAlignType.Diagonally);
            else if (e.KeyCode == Keys.G)
                AddElement(new NodeGroup(this));

            SelectedElement?.As<IKeyDownHandler>()?.OnKeyDown(e);
        }

        /// <summary>
        /// Handle key up
        /// </summary>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);


            SelectedElement?.As<IKeyUpHandler>()?.OnKeyUp(e);
        }
    }
}
