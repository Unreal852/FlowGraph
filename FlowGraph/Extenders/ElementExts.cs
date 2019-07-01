using System.Drawing;

namespace FlowGraph
{
    public static class ElementExts
    {
        /// <summary>
        /// Cast the specified element with the specified type
        /// </summary>
        /// <typeparam name="T">Type to cast</typeparam>
        /// <param name="element">Element</param>
        /// <returns><see cref="{T}"/> if successfull cast, <see cref="null"/> otherwise</returns>
        public static T As<T>(this IElement element) where T : class //For nullable return
        {
            if(element is T casted)
                return casted;
            return null;
        }

        public static bool IsInAnyGrip(this IExpandableElement element, Point point)
        {
            return GetCurrentGrip(element, point) != EGrip.None;
        }

        public static EGrip GetCurrentGrip(this IExpandableElement element, Point point)
        {
            if(element.BottomGripBounds != null && element.BottomGripBounds.Contains(point))
                return EGrip.Bottom;
            else if(element.RightGripBounds != null && element.RightGripBounds.Contains(point))
                return EGrip.Right;
            else if(element.BottomRightGripBounds != null && element.BottomRightGripBounds.Contains(point))
                return EGrip.BottomRight;
            return EGrip.None;
        }
    }
}