using System.Windows.Forms;

namespace FlowGraph
{
    public static class EGripExts
    {
        /// <summary>
        /// Gets the cursor that goes with the specified grip
        /// </summary>
        /// <param name="grip">Grip</param>
        /// <returns>Cursor</returns>
        public static Cursor GetCursor(this EGrip grip)
        {
            switch(grip)
            {
                case EGrip.Bottom:
                    return Cursors.SizeNS;
                case EGrip.BottomRight:
                    return Cursors.SizeNWSE;
                case EGrip.Right:
                    return Cursors.SizeWE;
                case EGrip.Left:
                    return Cursors.SizeWE;
                case EGrip.Top:
                    return Cursors.SizeNS;
                default:
                    return Cursors.Default;
            }
        }
    }
}
