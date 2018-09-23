using FlowGraph.Events;
using System.Drawing;

namespace FlowGraph
{
    public interface IElement
    {
        IElement Owner { get; }

        bool CanBeSelected { get; }
        bool Selected { get; set; }

        GraphLocation Location { get; set; }

        GraphSize Size { get; set; }

        Rectangle Bounds { get; }


        IElement FindElementAt(Point point);
        void OnRender(ElementRenderEventArgs e);
    }
}
