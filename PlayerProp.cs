using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YaveDBLib;

namespace Yave
{
    public partial class PlayerProp : Form
    {
        public PlayerProp()
        {
            InitializeComponent();
        }

        private void PlayerProp_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Character a = Main.character;
            skinLabel.Text = ValueData.Player.skinName[a.Skin];
            baseHealthlabel.Text = $"基础生命值 {a.baseMaxHealth}";
            baseATKlabel.Text = $"基础攻击力 {a.baseAttack}";
            baseDEFlabel.Text = $"基础防御力 {a.baseDefense}";
        }
    }
}
