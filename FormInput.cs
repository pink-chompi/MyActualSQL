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
                MessageBox.Show(formMain.dataGridViews[formMain.selectedTab].Columns[i].HeaderText);
            }
            
        }
    }
}
