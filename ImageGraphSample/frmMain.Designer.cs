namespace ImageGraphSample
{
    partial class frmMain
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PreviewPicturebox = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.MyGraph = new FlowGraph.Graph();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnMirrorDown = new System.Windows.Forms.Button();
            this.btnNewInput = new System.Windows.Forms.Button();
            this.btnMerge = new System.Windows.Forms.Button();
            this.btnIntValue = new System.Windows.Forms.Button();
            this.btnResizeProportionally = new System.Windows.Forms.Button();
            this.btnResize = new System.Windows.Forms.Button();
            this.btnRotateFlip = new System.Windows.Forms.Button();
            this.btnGreyScale = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPicturebox)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1415, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.PreviewPicturebox);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1396, 221);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview";
            // 
            // PreviewPicturebox
            // 
            this.PreviewPicturebox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreviewPicturebox.Location = new System.Drawing.Point(3, 16);
            this.PreviewPicturebox.Name = "PreviewPicturebox";
            this.PreviewPicturebox.Size = new System.Drawing.Size(1390, 202);
            this.PreviewPicturebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PreviewPicturebox.TabIndex = 0;
            this.PreviewPicturebox.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.MyGraph);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 254);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1396, 406);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Flow Graph";
            // 
            // MyGraph
            // 
            this.MyGraph.AlignMargin = 10;
            this.MyGraph.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.MyGraph.DebugFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MyGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MyGraph.FillSelectionRectangle = false;
            this.MyGraph.LargeGridStep = 128F;
            this.MyGraph.LargeGridStepColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.MyGraph.LinkingColor = System.Drawing.Color.Yellow;
            this.MyGraph.Location = new System.Drawing.Point(203, 16);
            this.MyGraph.Name = "MyGraph";
            this.MyGraph.OutlineSelectionColor = System.Drawing.Color.DarkOrange;
            this.MyGraph.SelectedElement = null;
            this.MyGraph.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(128)))), ((int)(((byte)(90)))), ((int)(((byte)(30)))));
            this.MyGraph.ShowDebugInfos = false;
            this.MyGraph.ShowGrid = true;
            this.MyGraph.Size = new System.Drawing.Size(1190, 387);
            this.MyGraph.SmallGridStep = 16F;
            this.MyGraph.SmallGridStepColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.MyGraph.TabIndex = 0;
            this.MyGraph.TimedRedraw = false;
            this.MyGraph.Click += new System.EventHandler(this.MyGraph_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.panel1.Controls.Add(this.btnMirrorDown);
            this.panel1.Controls.Add(this.btnNewInput);
            this.panel1.Controls.Add(this.btnMerge);
            this.panel1.Controls.Add(this.btnIntValue);
            this.panel1.Controls.Add(this.btnResizeProportionally);
            this.panel1.Controls.Add(this.btnResize);
            this.panel1.Controls.Add(this.btnRotateFlip);
            this.panel1.Controls.Add(this.btnGreyScale);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 387);
            this.panel1.TabIndex = 1;
            // 
            // btnMirrorDown
            // 
            this.btnMirrorDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMirrorDown.Location = new System.Drawing.Point(102, 69);
            this.btnMirrorDown.Name = "btnMirrorDown";
            this.btnMirrorDown.Size = new System.Drawing.Size(95, 60);
            this.btnMirrorDown.TabIndex = 8;
            this.btnMirrorDown.Text = "Mirror Down";
            this.btnMirrorDown.UseVisualStyleBackColor = true;
            // 
            // btnNewInput
            // 
            this.btnNewInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewInput.Location = new System.Drawing.Point(102, 323);
            this.btnNewInput.Name = "btnNewInput";
            this.btnNewInput.Size = new System.Drawing.Size(95, 60);
            this.btnNewInput.TabIndex = 7;
            this.btnNewInput.Text = "Input";
            this.btnNewInput.UseVisualStyleBackColor = true;
            // 
            // btnMerge
            // 
            this.btnMerge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMerge.Location = new System.Drawing.Point(3, 135);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(95, 60);
            this.btnMerge.TabIndex = 5;
            this.btnMerge.Text = "Merge";
            this.btnMerge.UseVisualStyleBackColor = true;
            // 
            // btnIntValue
            // 
            this.btnIntValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIntValue.Location = new System.Drawing.Point(3, 359);
            this.btnIntValue.Name = "btnIntValue";
            this.btnIntValue.Size = new System.Drawing.Size(95, 24);
            this.btnIntValue.TabIndex = 4;
            this.btnIntValue.Text = "Int Value";
            this.btnIntValue.UseVisualStyleBackColor = true;
            // 
            // btnResizeProportionally
            // 
            this.btnResizeProportionally.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResizeProportionally.Location = new System.Drawing.Point(102, 135);
            this.btnResizeProportionally.Name = "btnResizeProportionally";
            this.btnResizeProportionally.Size = new System.Drawing.Size(95, 60);
            this.btnResizeProportionally.TabIndex = 3;
            this.btnResizeProportionally.Text = "Resize Proportionally";
            this.btnResizeProportionally.UseVisualStyleBackColor = true;
            // 
            // btnResize
            // 
            this.btnResize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResize.Location = new System.Drawing.Point(102, 3);
            this.btnResize.Name = "btnResize";
            this.btnResize.Size = new System.Drawing.Size(95, 60);
            this.btnResize.TabIndex = 2;
            this.btnResize.Text = "Resize";
            this.btnResize.UseVisualStyleBackColor = true;
            // 
            // btnRotateFlip
            // 
            this.btnRotateFlip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRotateFlip.Location = new System.Drawing.Point(3, 69);
            this.btnRotateFlip.Name = "btnRotateFlip";
            this.btnRotateFlip.Size = new System.Drawing.Size(95, 60);
            this.btnRotateFlip.TabIndex = 1;
            this.btnRotateFlip.Text = "Mirror Right";
            this.btnRotateFlip.UseVisualStyleBackColor = true;
            // 
            // btnGreyScale
            // 
            this.btnGreyScale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGreyScale.Location = new System.Drawing.Point(3, 3);
            this.btnGreyScale.Name = "btnGreyScale";
            this.btnGreyScale.Size = new System.Drawing.Size(95, 60);
            this.btnGreyScale.TabIndex = 0;
            this.btnGreyScale.Text = "Grey Scale Node";
            this.btnGreyScale.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(1415, 665);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPicturebox)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox PreviewPicturebox;
        private FlowGraph.Graph MyGraph;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnGreyScale;
        private System.Windows.Forms.Button btnRotateFlip;
        private System.Windows.Forms.Button btnResize;
        private System.Windows.Forms.Button btnResizeProportionally;
        private System.Windows.Forms.Button btnIntValue;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Button btnNewInput;
        private System.Windows.Forms.Button btnMirrorDown;
    }
}

