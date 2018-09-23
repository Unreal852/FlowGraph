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
            if (element is T casted)
                return casted;
            return null;
        }
    }
}
