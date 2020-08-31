using System;
using System.Windows.Forms;

namespace Barcode
{
    public partial class frmMain : Form
    {
        Form1 form1 = new Form1();
        Form2 form2 = new Form2();
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            form2.ShowDialog();
        }
    }
}
