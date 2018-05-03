using System.Windows.Forms;
using System.Linq;

namespace ImageGraphSample
{
    public partial class frmDropDownDialog : Form
    {
        public frmDropDownDialog(string title, params object[] items)
        {
            InitializeComponent();
            Text = title;
            cmbValues.Items.AddRange(items);

            btnOk.Click += (sender, e) =>
            {
                if (cmbValues.SelectedIndex > -1)
                {
                    Selected = cmbValues.Items[cmbValues.SelectedIndex];
                    DialogResult = DialogResult.OK;
                }
                Close();
            };
        }

        /// <summary>
        /// Selected object from the combobox
        /// </summary>
        public object Selected { get; private set; }
    }
}
