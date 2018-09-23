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
            if (e.Button == MouseButtons.Left && EditMode != GraphEditMode.SelectingBox)
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
            SetEditMode(GraphEditMode.Idle);
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
                if (SelectedElement is NodeConnector)
                {
                    if (EditMode != GraphEditMode.Linking)
                        SetEditMode(GraphEditMode.Linking);
                }
                else if (SelectedElement is IExpandableElement expandableElement && group.GripBounds.Contains(m_transformed_location))
                {
                    SetEditMode(GraphEditMode.ExpandingGroup);
                }
                else
                {
                    if (EditMode != GraphEditMode.SelectingBox && EditMode != GraphEditMode.MovingSelection && !m_dragElement)
                        SetEditMode(GraphEditMode.SelectingBox);
                    else if (EditMode != GraphEditMode.ExpandingGroup && EditMode != GraphEditMode.MovingSelection && EditMode != GraphEditMode.SelectingBox && m_dragElement)
                        SetEditMode(GraphEditMode.MovingSelection);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (EditMode != GraphEditMode.Scrolling)
                    SetEditMode(GraphEditMode.Scrolling);
            }
            else if (e.Button == MouseButtons.Middle)
            {

            }
            else
            {
                m_lastLocation = currentLocation;
                Redraw();
                return;
            }

            /*
            if (EditMode != GraphEditMode.Scrolling && EditMode != GraphEditMode.SelectingBox && e.Button == MouseButtons.Right)
                SetEditMode(GraphEditMode.Scrolling);
            else if (EditMode != GraphEditMode.MovingSelection && EditMode != GraphEditMode.SelectingBox && m_dragElement && e.Button == MouseButtons.Left)
                SetEditMode(GraphEditMode.MovingSelection);
            else if (EditMode != GraphEditMode.SelectingBox && EditMode != GraphEditMode.MovingSelection && !m_dragElement && e.Button == MouseButtons.Left)
                SetEditMode(GraphEditMode.SelectingBox);
            else if(EditMode != GraphEditMode.Linking)
                SetEditMode(GraphEditMode.Linking); */

            if (Math.Abs(deltaX) > 1 || Math.Abs(deltaY) > 1)
            {
                switch (EditMode)
                {
                    case GraphEditMode.Scrolling:
                        {
                            MoveView(deltaX, deltaY);
                            m_lastLocation = currentLocation;
                            Redraw();
                        }
                        break;
                    case GraphEditMode.SelectingBox:
                        {
                            SelectElements();
                            m_lastLocation = currentLocation;
                            Redraw();
                        }
                        break;
                    case GraphEditMode.MovingSelection:
                        {
                            foreach (IElement sElement in Selection.Elements)
                                MoveElement(sElement, deltaX, deltaY);
                            m_lastLocation = currentLocation;
                            Redraw();
                        }
                        break;
                    case GraphEditMode.Linking:
                        {
                            m_lastLocation = currentLocation;
                            Redraw();
                        }
                        break;
                    case GraphEditMode.ExpandingGroup:
                        {
                            NodeGroup group = SelectedElement.As<NodeGroup>();
                            group.Size = new GraphSize((int)(m_transformed_location.X - group.Location.X), (int)(m_transformed_location.Y - group.Location.Y));
                            //SelectedElement.As<NodeGroup>()?.ExpandTo(currentLocation);
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
                AlignElements(Selection.Elements, AlignType.Vertically);
            else if (e.KeyCode == Keys.H)
                AlignElements(Selection.Elements, AlignType.Horizontally);
            else if (e.KeyCode == Keys.D)
                AlignElements(Selection.Elements, AlignType.Diagonally);
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
