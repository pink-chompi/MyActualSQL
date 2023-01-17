using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActualSQL
{
    public partial class FormInput : Form
    {
        Label[] labels = new Label[0];
        TextBox[] textBoxes = new TextBox[0];

        FormMain formMain;
        public FormInput(FormMain f)
        {
            InitializeComponent();  
            formMain = f;
        }

        private void FormInput_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < formMain.dataGridViews[formMain.selectedTab].Columns.Count; i++)
            {
                Array.Resize(ref labels, labels.Length + 1);
                labels[labels.Length - 1] = new Label();
                labels[labels.Length - 1].Location = new Point(50, 50);
                labels[labels.Length - 1].Text = "24241251512512"; //formMain.dataGridViews[formMain.selectedTab].Columns[i].HeaderText;
                labels[labels.Length - 1].Visible = true;
                labels[labels.Length - 1].Enabled = true;
                labels[labels.Length - 1].ForeColor = Color.Black;
                labels[labels.Length - 1].Height = 50;
                labels[labels.Length - 1].Width = 50;
                labels[labels.Length - 1].BorderStyle = BorderStyle.FixedSingle;
                labels[labels.Length - 1].Name = "label" + i.ToString();
                labels[labels.Length - 1].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                this.Controls.Add(labels[labels.Length - 1]);
                //MessageBox.Show(formMain.dataGridViews[formMain.selectedTab].Columns[i].HeaderText);
            }
            
        }
    }
}
