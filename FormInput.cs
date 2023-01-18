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

            int formHeight = 0;
            for (int i = 0; i < formMain.dataGridViews[formMain.selectedTab].Columns.Count; i++)
            {
                Array.Resize(ref labels, labels.Length + 1); Array.Resize(ref textBoxes, textBoxes.Length + 1);
                labels[labels.Length - 1] = new Label(); textBoxes[textBoxes.Length - 1] = new TextBox();
                labels[labels.Length - 1].Location = new Point(15, 15 + (i * 50)); textBoxes[textBoxes.Length - 1].Location = new Point(15, labels[labels.Length - 1].Height + 15 + (i * 50));

                labels[labels.Length - 1].Text = formMain.dataGridViews[formMain.selectedTab].Columns[i].HeaderText;

                textBoxes[textBoxes.Length - 1].Width += 50;

                //labels[labels.Length - 1].ForeColor = Color.Black;

                this.Controls.Add(labels[labels.Length - 1]); this.Controls.Add(textBoxes[textBoxes.Length - 1]);
                //MessageBox.Show(formMain.dataGridViews[formMain.selectedTab].Columns[i].HeaderText);
                formHeight += labels[labels.Length - 1].Height + textBoxes[textBoxes.Length - 1].Height;
            }
            Height = 100 + formHeight;
            Width = 50 + textBoxes[0].Width;
            StartPosition = FormStartPosition.CenterParent;
        }
    }
}
