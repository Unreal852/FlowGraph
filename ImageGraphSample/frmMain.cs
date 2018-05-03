using FlowGraph;
using FlowGraph.Nodes;
using ImageGraphSample.Nodes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageGraphSample
{
    public partial class frmMain : Form
    {
        public static frmMain Instance { get; private set; }

        private string m_imagePath;

        public frmMain()
        {
            Instance = this;
            InitializeComponent();

            MyGraph.ShowDebugInfos = true;

            openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
            saveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;

            btnGreyScale.Click += (sender , e) => AddElement(new GrayScaleNode(MyGraph));
            btnRotateFlip.Click += (sender, e) => AddElement(new MirrorNode(MyGraph, RotateFlipType.RotateNoneFlipX));
            btnResize.Click += (sender, e) => AddElement(new ResizeNode(MyGraph));
            btnIntValue.Click += (sender, e) => AddElement(new IntNode(MyGraph));
            btnMerge.Click += (sender, e) => AddElement(new MergeNode(MyGraph));
            btnMirrorDown.Click += (sender, e) => AddElement(new MirrorNode(MyGraph, RotateFlipType.RotateNoneFlipY));
            btnNewInput.Click += (sender, e) => AddElement(new InputNode(Image.FromFile(m_imagePath), MyGraph));
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                MyGraph.ClearElements();

                m_imagePath = diag.FileName;

                Image img = Image.FromFile(diag.FileName);
                PreviewPicturebox.Image = img;

                AddElement(new InputNode(img, MyGraph));
                AddElement(new OutputNode(MyGraph));
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                PreviewPicturebox.Image.Save(diag.FileName + ".png");
            }
        }

        public void SetPreview(Image img)
        {
            PreviewPicturebox.Image = img;
        }

        public void AddElement(IElement element)
        {
            MyGraph.AddElement(element);
            MyGraph.Focus();
        }
    }
}
