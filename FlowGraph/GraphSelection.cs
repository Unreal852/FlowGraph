using System.Collections.Generic;

namespace FlowGraph
{
    public class GraphSelection
    {
        private List<IElement> m_selectedElements = new List<IElement>();

        public GraphSelection()
        {

        }

        /// <summary>
        /// Gets the specified element by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>IElement</returns>
        public IElement this[int index] => m_selectedElements[index];

        /// <summary>
        /// Gets all selected elements
        /// </summary>
        public ICollection<IElement> Elements => m_selectedElements;

        /// <summary>
        /// Gets the selected elements count
        /// </summary>
        public int Count => m_selectedElements.Count;

        /// <summary>
        /// Add new element
        /// </summary>
        /// <param name="element">Element</param>
        public void AddElement(IElement element)
        {
            if (!element.CanBeSelected)
                return;
            m_selectedElements.Add(element);
            if (!element.Selected)
                element.Selected = true;
        }

        /// <summary>
        /// Add new elements
        /// </summary>
        /// <param name="elements">Elements</param>
        public void AddElements(ICollection<IElement> elements)
        {
            foreach (IElement element in elements)
                AddElement(element);
        }

        /// <summary>
        /// Verify if the specified element is selected
        /// </summary>
        /// <param name="element">Element</param>
        /// <returns>TRUE if selected otherwise FALSE</returns>
        public bool IsSelected(IElement element)
        {
            return m_selectedElements.Contains(element);
        }

        /// <summary>
        /// Verify if the specified elements are selected
        /// </summary>
        /// <param name="elements">Elements</param>
        /// <returns> <see cref="(IElement element, bool Selected)"/> for each specified elements</returns>
        public IEnumerable<(IElement Element, bool Selected)> AreSelected(IEnumerable<IElement> elements)
        {
            foreach (IElement element in elements)
                yield return (element, m_selectedElements.Contains(element));
        }

        /// <summary>
        /// Unselect all currently selected elements
        /// </summary>
        public void UnselectAll()
        {
            m_selectedElements.ForEach((IElement element) => element.Selected = false);
            m_selectedElements.Clear();
        }
    }
}
