namespace FlowGraph
{
    public enum EGraphEditMode
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
        /// Expanding a expendable element to the bottom
        /// </summary>
        ExpandingBottom,

        /// <summary>
        /// Expanding a expendable element to the right
        /// </summary>
        ExpandingRight,

        /// <summary>
        /// Expanding a expendable element to the bottom right
        /// </summary>
        ExpandingBottomRight,
    }
}