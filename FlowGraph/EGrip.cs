using System;

namespace FlowGraph
{
    [Flags]
    public enum EGrip
    {
        /// <summary>
        /// None..
        /// </summary>
        None,

        /// <summary>
        /// Left grip
        /// </summary>
        Left,

        /// <summary>
        /// Right grip
        /// </summary>
        Right,

        /// <summary>
        /// Top Grip
        /// </summary>
        Top,

        /// <summary>
        /// Bottom grip
        /// </summary>
        Bottom,

        /// <summary>
        /// Bottom right corner
        /// </summary>
        BottomRight
    }
}