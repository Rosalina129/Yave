using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yave.Buff;

namespace Yave
{
    public partial class Dashboard : Form
    {
        Character ch = Main.character;
        int buffID = 0;
        public Dashboard()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ch.AddXP(ch.MaxXP - ch.XP,out string a);
            toolStripStatusLabel1.Text = a;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ch.Heal();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            SyncBuff();
        }
        private void SyncBuff()
        {
            listBox1.Items.Clear();
            for (int i = 0; i < ch.Buffs.Count; i++)
            {
                listBox1.Items.Add($"{Buff.Buff.buffType[ch.Buffs[i].ID]}: {Buff.Buff.GetRelativeMode(ch.Buffs[i].Values, ch.Buffs[i].Relative)}");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            buffID = comboBox1.SelectedIndex;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;
            string a = "";
            a += $"Buff 序列 {i}\r\n";
            a += $"类型：{Buff.Buff.GetBuffType(ch.Buffs[i].ID)}\r\n";
            a += $"增益：{Buff.Buff.GetRelativeMode(ch.Buffs[i].Values, ch.Buffs[i].Relative)}\r\n";
            if (ch.Buffs[i].Cycles >= 0)
            {
                a += $"轮数：{ch.Buffs[i].Cycles}\r\n";
            }
            else
            {
                a += $"轮数：无限\r\n";
            }
            label7.Text = a;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (Regex.IsMatch(textBox1.Text, @"^(\-|\+)?\d+(\.\d+)?$"))
            {
                Buff.Buff buff = new Buff.Buff(
                    comboBox1.SelectedIndex,
                    Convert.ToDouble(textBox1.Text),
                    new string[] { "Debug" },
                    checkBox1.Checked,
                    (int)numericUpDown1.Value
                );
                Main.character.AddBuff(buff);
            }
            else
            {
                MessageBox.Show("请输入数字。");
            }
            SyncBuff();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ch.SyncBuffPer();
            SyncBuff();
        }
    }
}
