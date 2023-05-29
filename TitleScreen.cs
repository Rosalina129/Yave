using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YaveDBLib;

namespace Yave
{
    public partial class TitleScreen : Form
    {
        public static bool isCreated = false;
        public static bool isLoaded = false;
        public static int Difficulty;
        public static int Element;
        public static int Skin;
        public static string CharacterName;

        public TitleScreen()
        {
            InitializeComponent();
        }
        
        private Object[] SkinDB = new object[] {
            global::Yave.Properties.Resources.yave_1,
            global::Yave.Properties.Resources.yave_2,
            global::Yave.Properties.Resources.yave_3,
            global::Yave.Properties.Resources.yave_4,
            global::Yave.Properties.Resources.yave_5,
            global::Yave.Properties.Resources.yave_6,
            global::Yave.Properties.Resources.yave_7,
        };

        private void TitleScreen_Load(object sender, EventArgs e)
        {
            skinListBox.SelectedIndex = 0;
            foreach (string a in Elements.strings)
            {
                comboBox2.Items.Add(a);
            }
            comboBox1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 0;
        }

        private void skinListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Skin = skinListBox.SelectedIndex;
            try
            {
                this.YaveSkin.Image = (Image)SkinDB[skinListBox.SelectedIndex];
                skinnamelabel.Text = (string)ValueData.Player.skinName[skinListBox.SelectedIndex];
            }
            catch (IndexOutOfRangeException) {
                MessageBox.Show("您当前未解锁改皮肤！");
                skinListBox.SelectedIndex = 0;
            }
        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Element = comboBox2.SelectedIndex;
            var a = comboBox2.SelectedIndex;
            label6.Text = Main.Element_Rates(a);
            label7.Text = BuffInit(a);
        }

        private string BuffInit(int Element)
        {
            string a = "影响加成\r\n";
            switch (Element)
            {
                case 0:
                    a += $"经验提升：+{ThreadSystem.ConvertPercent(ValueData.Player.XPBoost)}";
                    break;
                case 1:
                    a += $"最大生命值：+{ThreadSystem.ConvertPercent(ValueData.Player.BaseBoost)}";
                    break;
                case 2:
                    a += $"攻击力：+{ThreadSystem.ConvertPercent(ValueData.Player.BaseBoost)}";
                    break;
                case 3:
                    a += $"防御力：+{ThreadSystem.ConvertPercent(ValueData.Player.BaseBoost)}";
                    break;
                case 4:
                    a += $"各种元素抗性：+{ThreadSystem.ConvertPercent(ValueData.Player.ElementRes)}";
                    break;
                case 5:
                    a += $"元素伤害加成：+{ThreadSystem.ConvertPercent(ValueData.Player.ElementDamage)}";
                    break;
                case 6:
                    a += $"技能能量损耗：-{ThreadSystem.ConvertPercent(ValueData.Player.SkillCost)}";
                    break;
                case 7:
                    a += String.Format("金币加成：{0}\r\n掉落物加成：{0}", ThreadSystem.ConvertPercent(ValueData.Player.RewardBoost));
                    break;
                default:
                    a = null;
                    break;
            }
            return a;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Difficulty = comboBox1.SelectedIndex;
            string c = "选择菜鸟难度，你将不会获得任何成就，也不会解锁新区域，从而无法解锁许多隐藏内容，但该模式会更容易让你通关，您也可以尝试新手模式。是否继续选择菜鸟难度？";
            if (comboBox1.SelectedIndex == 0)
            {
                if (MessageBox.Show(c, this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    label8.Visible = true;
                }
                else
                {
                    comboBox1.SelectedIndex = 1;
                    label8.Visible = false;
                }
            }
            else
            {
                label8.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0)
            {
                saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Yave's Tours 存档文件|*.yts";
                saveFileDialog1.Title = "保存到...";
                saveFileDialog1.DefaultExt = $"{textBox1.Text}.yts";
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (textBox1.Text == "")
                    {
                        CharacterName = "博了";
                    }
                    else
                    {
                        CharacterName = textBox1.Text;
                    }
                    isCreated = true;
                    Main.FileName = saveFileDialog1.FileName;
                    this.Close();
                }
            }
            else
            {
                if (textBox1.Text == "")
                {
                    CharacterName = "博了";
                }
                else
                {
                    CharacterName = textBox1.Text;
                }
                isCreated = true;
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Yave's Tours 存档文件|*.yts";
            openFileDialog1.Title = "读取 Yave's Tours 存档...";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                isLoaded = true;
                Main.FileName = openFileDialog1.FileName;
                this.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Name = textBox1.Text;
        }
    }
}
