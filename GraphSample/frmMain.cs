using FlowGraph.Nodes;
using FlowGraph.Nodes.Item.Items;
using System;
using System.Windows.Forms;

namespace GraphSample
{
    public partial class frmMain : Form
    {
        private Timer m_timer;

        public frmMain()
        {
            InitializeComponent();

            graph1.ShowDebugInfos = true;
            graph1.ShowGrid = true;
            graph1.TimedRedraw = true;

            Random rnd = new Random();

            int nodeY = 20;
            for (int i = 0; i < 3; i++)
            {
                nodeY += 20;
                Node node = new Node(graph1) { Location = new FlowGraph.GraphLocation(0, nodeY), Title = "Test" };
                node.Add(new LabelItem() { Size = new FlowGraph.GraphSize(node.Size.Width, 30) });
                graph1.AddElement(node);
            }

            NodeGroup group = new NodeGroup(graph1);
            group.Location = new FlowGraph.GraphLocation(0, 0);
            group.Size = new FlowGraph.GraphSize(1000, 700);

            graph1.AddElement(group);
        }
    }
}
