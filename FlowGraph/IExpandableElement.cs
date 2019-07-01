using System.Drawing;

namespace FlowGraph
{
    public interface IExpandableElement : IElement
    {
        /// <summary>
        /// The minimum size of the element
        /// </summary>
        GraphSize MinSize { get; }

        /// <summary>
        /// The maximum size of the element
        /// </summary>
        GraphSize MaxSize { get; }

        /// <summary>
        /// The right grip bounds
        /// </summary>
        Rectangle RightGripBounds { get; }

        /// <summary>
        /// The bottom grip bounds
        /// </summary>
        Rectangle BottomGripBounds { get; }

        /// <summary>
        /// The bottom right corner bounds
        /// </summary>
        Rectangle BottomRightGripBounds { get; }
    }
}