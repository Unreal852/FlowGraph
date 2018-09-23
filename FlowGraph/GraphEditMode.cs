namespace FlowGraph
{
    public enum GraphEditMode
    {
        /// <summary>
        /// Idle
        /// </summary>
        Idle,
        /// <summary>
        /// Moving view
        /// </summary>
        Scrolling,
        /// <summary>
        /// Zooming
        /// </summary>
        Zooming,
        /// <summary>
        /// Selecting elements
        /// </summary>
        Selecting,
        /// <summary>
        /// Selecting multiple elements
        /// </summary>
        SelectingBox,
        /// <summary>
        /// Moving selection
        /// </summary>
        MovingSelection,
        /// <summary>
        /// Linking elements
        /// </summary>
        Linking,
        /// <summary>
        /// Expanding a node group
        /// </summary>
        ExpandingGroup
    }
}
