using FlowGraph.Events;
using FlowGraph.Nodes.Item;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace FlowGraph.Nodes
{
    public class Node : IElement, ICollection<NodeItem>
    {
        private GraphLocation m_location = new GraphLocation(0, 0);

        private GraphSize m_size = new GraphSize(300, 200);

        private List<NodeItem> m_items = new List<NodeItem>();

        public Node(Graph graph)
        {
            Graph = graph;
        }

        /// <summary>
        /// Node Owner
        /// </summary>
        public IElement Owner { get; internal set; }

        /// <summary>
        /// Parent Graph
        /// </summary>
        public Graph Graph { get; }

        /// <summary>
        /// Node title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Node description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Node items margin
        /// </summary>
        public int ItemsMargin { get; set; } = 10;

        /// <summary>
        /// Select node
        /// </summary>
        public bool Selected { get; set; } = false;

        /// <summary>
        /// Can this node be selected
        /// </summary>
        public bool CanBeSelected { get; set; } = true;

        /// <summary>
        /// Gets the elements count
        /// </summary>
        public int Count => m_items.Count;

        /// <summary>
        /// Is this readonly
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Node tag
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Header bounds
        /// </summary>
        public Rectangle HeaderBounds { get; private set; }

        /// <summary>
        /// Node bounds
        /// </summary>
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Header color
        /// </summary>
        public GraphColor HeaderColor { get; set; } = new GraphColor(Color.FromArgb(62, 62, 66));

        /// <summary>
        /// Header color when this node is selected
        /// </summary>
        public GraphColor SelectedHeaderColor { get; set; } = new GraphColor(Color.FromArgb(90, 90, 90));

        /// <summary>
        /// Background color
        /// </summary>
        public GraphColor BackgroundColor { get; set; } = new GraphColor(Color.FromArgb(30, 30, 30));

        /// <summary>
        /// Background color when this node is selected
        /// </summary>
        public GraphColor SelectedBackgroundColor { get; set; } = new GraphColor(Color.FromArgb(60, 60, 60));

        /// <summary>
        /// Outline color
        /// </summary>
        public GraphColor OutlineColor { get; set; } = new GraphColor(Color.FromArgb(0, 0, 0));

        /// <summary>
        /// Outline color when this node is selected
        /// </summary>
        public GraphColor SelectedOutlineColor { get; set; } = new GraphColor(Color.Orange);

        /// <summary>
        /// Title color
        /// </summary>
        public GraphColor TitleColor { get; set; } = new GraphColor(Color.White);

        /// <summary>
        /// Description color
        /// </summary>
        public GraphColor DescriptionColor { get; set; } = new GraphColor(Color.Gray);

        /// <summary>
        /// Title font
        /// </summary>
        public Font TitleFont { get; set; } = SystemFonts.DefaultFont;

        /// <summary>
        /// Description font
        /// </summary>
        public Font DescriptionFont { get; set; } = new Font(new FontFamily(SystemFonts.DefaultFont.Name), 8f, FontStyle.Italic);

        /// <summary>
        /// Node location
        /// </summary>
        public GraphLocation Location
        {
            get => m_location;
            set
            {
                if(m_location == value || (m_location.X == value.X && m_location.Y == value.Y))
                    return;
                m_location = value;
                UpdateBounds();
            }
        }

        /// <summary>
        /// Node size
        /// </summary>
        public GraphSize Size
        {
            get => m_size;
            set
            {
                if(m_size == value || (m_size.Width == value.Width && m_size.Height == value.Height))
                    return;
                m_size = value;
                UpdateBounds();
            }
        }

        /// <summary>
        /// Gets the total amounts of all items width/height combined
        /// </summary>
        /// <returns><see cref="(int Width, int Height)"/> Width and height of all items combined</returns>
        private (int Width, int Height) GetTotalItemsSize()
        {
            int height = 0;
            int width = 0;
            foreach(NodeItem item in m_items)
            {
                height += item.Size.Height;
                width += item.Size.Width;
            }

            return (width, height);
        }

        /// <summary>
        /// Update bounds
        /// </summary>
        private void UpdateBounds() // Todo: measure text to avoid out of node string
        {
            Bounds = new Rectangle(Location.X, Location.Y, Size.Width, Size.Height);
            HeaderBounds = new Rectangle(Location.X, Location.Y, Size.Width, string.IsNullOrWhiteSpace(Description) ? 20 : 30);
            if(m_items.Count > 0)
            {
                int itemsHeight = 0;
                for(int i = 0; i < m_items.Count; i++)
                {
                    NodeItem item = m_items[i];
                    item.UpdateBounds(new GraphLocation(Location.X, (Location.Y + HeaderBounds.Height + itemsHeight) + (ItemsMargin * i)));
                    itemsHeight += item.Size.Height;
                }
            }
        }

        /// <summary>
        /// Find element at the specified point
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns><see cref="IElement"/> if a element has been found, <see cref="null"/> otherwise</returns>
        public IElement FindElementAt(Point point)
        {
            foreach(NodeItem item in m_items)
            {
                IElement element = item.FindElementAt(point);
                if(element != null)
                    return element;
            }

            if(Bounds.Contains(point))
                return this;
            return null;
        }

        /// <summary>
        /// Adds a new item
        /// </summary>
        /// <param name="item"></param>
        public void Add(NodeItem item)
        {
            item.Location = new GraphLocation(Location.X, (Location.Y + HeaderBounds.Height + GetTotalItemsSize().Height) + (ItemsMargin * m_items.Count));
            item.Owner = this;
            m_items.Add(item);
            int totalHeight = (GetTotalItemsSize().Height + (ItemsMargin * m_items.Count) + HeaderBounds.Height);
            if(totalHeight > Bounds.Height)
                Size = new GraphSize(Bounds.Width, totalHeight);
        }

        /// <summary>
        /// Checks if the specified item is contained
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns><see cref="true"/> if the item is contained, <see cref="false"/> otherwise</returns>
        public bool Contains(NodeItem item)
        {
            return m_items.Contains(item);
        }

        /// <summary>
        /// Copy this into a <see cref="NodeItem[]"/>
        /// </summary>
        /// <param name="array">Destination Array</param>
        /// <param name="arrayIndex">Start Index</param>
        public void CopyTo(NodeItem[] array, int arrayIndex)
        {
            m_items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Remove the specified item
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns><see cref="true"/> if the item was removed, <see cref="false"/> otherwise</returns>
        public bool Remove(NodeItem item)
        {
            return m_items.Remove(item);
        }

        /// <summary>
        /// Remove a item at the specified index
        /// </summary>
        /// <param name="index">Index</param>
        public void RemoveAt(int index)
        {
            if(m_items.Count - 1 < index && index > -1)
                m_items.RemoveAt(index);
        }

        /// <summary>
        /// Clear all items
        /// </summary>
        public void Clear()
        {
            foreach(NodeItem item in m_items)
                item.Owner = null;
            m_items.Clear();
        }

        /// <summary>
        /// Return enumerator
        /// </summary>
        /// <returns><see cref="IEnumerator{NodeItem}"/></returns>
        public IEnumerator<NodeItem> GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        /// <summary>
        /// Return enumerator
        /// </summary>
        /// <returns><see cref="IEnumerator{NodeItem}"/></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        /// <summary>
        /// Gets item by index
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns><see cref="NodeItem"/> if a item has been found, <see cref="null"/> otherwise</returns>
        public NodeItem GetItem(int index)
        {
            if(m_items.Count > -1 && m_items.Count < index)
                return m_items[index];
            return null;
        }

        /// <summary>
        /// Get all items of the specified type
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <returns><see cref="NodeItem[]"/> items</returns>
        public T[] GetItems<T>() where T : class
        {
            List<T> items = new List<T>();
            foreach(NodeItem i in m_items)
                if(i.GetType() == typeof(T))
                    items.Add((i as T));
            return items.ToArray();
        }

        /// <summary>
        /// Render node
        /// </summary>
        public virtual void OnRender(ElementRenderEventArgs e)
        {
            if(Selected)
            {
                e.Graphics.FillRectangle(SelectedBackgroundColor.Brush, Bounds);
                e.Graphics.FillRectangle(SelectedHeaderColor.Brush, HeaderBounds);
                e.Graphics.DrawRectangle(SelectedOutlineColor.Pen, Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(BackgroundColor.Brush, Bounds);
                e.Graphics.FillRectangle(HeaderColor.Brush, HeaderBounds);
                e.Graphics.DrawRectangle(OutlineColor.Pen, Bounds);
            }

            if(!string.IsNullOrWhiteSpace(Title))
                e.Graphics.DrawString(Title, TitleFont, TitleColor.Brush, (float)HeaderBounds.X + 5, (float)HeaderBounds.Y + 5);
            if(!string.IsNullOrWhiteSpace(Description))
                e.Graphics.DrawString(Description, DescriptionFont, DescriptionColor.Brush, (float)HeaderBounds.X + 5, (float)HeaderBounds.Y + 17);

            foreach(NodeItem item in m_items)
                item.OnRender(e);
        }
    }
}