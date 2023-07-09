using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yave
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Rosalina129/Yave");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void About_Load(object sender, EventArgs e)
        {
            label2.Text = $"版本: {Main.SoftwareVersion[0]}.{Main.SoftwareVersion[1]}.{Main.SoftwareVersion[2]} (Beta 1)";
        }
    }
}
