﻿using System;
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
    public partial class Dashboard : Form
    {
        Character ch = Main.character;
        public Dashboard()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ch.AddXP(ch.MaxHP - ch.XP,out string a);
            toolStripStatusLabel1.Text = a;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ch.Heal();
        }
    }
}
