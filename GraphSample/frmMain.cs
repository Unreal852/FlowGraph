using FlowGraph.Nodes;
using FlowGraph.Nodes.Item.Items;
using System;
using System.Windows.Forms;

namespace GraphSample
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            graph1.ShowDebugInfos = true;
            graph1.ShowGrid = true;
            

            Random rnd = new Random();

            for(int i = 1; i != 10; i++)
            {
                Node node = new Node(graph1);
                node.Title = "Test Node";
                node.Description = "Actor Test";
                node.Location = new FlowGraph.GraphLocation(rnd.Next(1, 1000 * i), rnd.Next(1, 1000 * i));
                node.AddItem(new LabelItem() {  Size = new FlowGraph.GraphSize(node.Size.Width, 30)});
                node.AddItem(new LabelItem() { Size = new FlowGraph.GraphSize(node.Size.Width, 30) });
                node.AddItem(new LabelItem() { Size = new FlowGraph.GraphSize(node.Size.Width, 30) });
                graph1.AddElement(node, rnd.Next(0, 10) < 5 ? true : false);
            }
        }
    }
}
