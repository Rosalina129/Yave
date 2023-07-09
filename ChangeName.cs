using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yave
{
    public partial class ChangeName : Form
    {
        public static string CName = "";
        public ChangeName()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CName = textBox1.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}
