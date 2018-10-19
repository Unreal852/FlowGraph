namespace FlowGraph
{
    public class GraphSize
    {
        public GraphSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; }

        public static bool operator >(GraphSize size1, GraphSize size2)
        {
            return (size1.Width > size2.Width || size1.Height > size2.Height);
        }

        public static bool operator >=(GraphSize size1, GraphSize size2)
        {
            return (size1.Width >= size2.Width || size1.Height >= size2.Height);
        }

        public static bool operator <(GraphSize size1, GraphSize size2)
        {
            return (size1.Width < size2.Width || size1.Height < size2.Height);
        }

        public static bool operator <=(GraphSize size1, GraphSize size2)
        {
            return (size1.Width <= size2.Width || size1.Height <= size2.Height);
        }
    }
}
