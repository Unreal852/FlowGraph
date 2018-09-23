namespace GraphSample
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
            this.graph1 = new FlowGraph.Graph();
            this.SuspendLayout();
            // 
            // graph1
            // 
            this.graph1.AlignMargin = 10;
            this.graph1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.graph1.DebugFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.graph1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graph1.FillSelectionRectangle = false;
            this.graph1.LargeGridStep = 128F;
            this.graph1.LargeGridStepColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.graph1.LinkingColor = System.Drawing.Color.Yellow;
            this.graph1.Location = new System.Drawing.Point(0, 0);
            this.graph1.Name = "graph1";
            this.graph1.OutlineSelectionColor = System.Drawing.Color.DarkOrange;
            this.graph1.SelectedElement = null;
            this.graph1.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(128)))), ((int)(((byte)(90)))), ((int)(((byte)(30)))));
            this.graph1.ShowDebugInfos = false;
            this.graph1.ShowGrid = true;
            this.graph1.Size = new System.Drawing.Size(1249, 506);
            this.graph1.SmallGridStep = 16F;
            this.graph1.SmallGridStepColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.graph1.TabIndex = 1;
            this.graph1.Text = "graph1";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1249, 506);
            this.Controls.Add(this.graph1);
            this.Name = "frmMain";
            this.Text = "Graph Sample";
            this.ResumeLayout(false);

        }

        #endregion
        private FlowGraph.Graph graph1;
    }
}

