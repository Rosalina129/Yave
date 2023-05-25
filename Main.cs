using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yave
{

    public partial class Main : Form
    {
        /// <summary>
        /// Program Start
        /// </summary>

        // Init Player and more Datas.
        public static int[] SaveVersion = new int[] {1,0,0};
        public static int UpgradePoints = 0;
        public static int Difficult = 3;
        public static int TalentLevel = 0;

        public static bool isTouring = false;
        public bool isBattle = false;
        public static int distance = 0;
        private double spawncooldown = 125;
        private int comboTime = 0;

        Character character = new Character();
        MonsterPool monsterpool = new MonsterPool(Difficult);
        PlayerSkillPool skillPool = new PlayerSkillPool();
        Monster monster;
        Random random = new Random();

        public Main()
        {
            InitializeComponent();
            totalDamagelabel.Text = "共计伤害 0";
        }
        /// <summary>
        /// Perform refresh data.
        /// </summary>
         
        private void TourRun()
        {
            distance += 1;
            spawncooldown -= 1;
            if (spawncooldown <= 1)
            {
                spawncooldown = 1;
                CalculateSpawns();
            }
        }
        private void CalculateSpawns()
        {
            comboTime = 0;
            double a = random.NextDouble();
            if (a < 0.01)
            {
                clearLog();
                SpawnMonster(random.Next(0,9));
                WritetoLog($"一个野生的 {monster.Name} 出现了!");
                for (int b = 0; b < monster.Skill.Count; b++)
                {
                    monsterSkillsListBox.Items.Add(monster.Skill[b].Name);
                }
                isTouring = false;
                isBattle= true;
            }
        }
        private void SpawnMonster(int index)
        {
            monster = monsterpool.GetMonster(index);
        }
        
        private void ResultBattle()
        {
            spawncooldown = random.NextDouble() * 250 + 185;
            clearLog();
            monsterSkillsListBox.Items.Clear();
            isBattle = false;
            monsterpool = new MonsterPool(Difficult);
        }

        private void RefreshData()
        {
            charNamelabel.Text = character.GetCharacterName();
            xplabel.Text = character.GetXP();
            hplabel.Text = character.GetHealth();
            atklabel.Text = $"{character.Attack}";
            deflabel.Text = $"{character.Defense}";
            levellabel.Text = $"[模式 {TalentLevel}] 等级 {character.Level}";
            energylabel.Text = $"{0}";
            crlabel.Text = ThreadSystem.ConvertPercent(character.Crit_Rate);
            cdlabel.Text = ThreadSystem.ConvertPercent(character.Crit_Damage);

            monnamelabel.Text = monster.GetMonsterName();
            monhealthlabel.Text = monster.GetHealth();
            monatklabel.Text = $"{Math.Round(monster.Attack,0)}";
            mondeflabel.Text = $"{Math.Round(monster.Defense,0)}";
            monreslabel.Text = monster.GetResistance();

            tourdislabel.Text = $"共计旅行距离: {ThreadSystem.GetDistance(distance)}";
            monsterpanel.Visible = isBattle;
            button6.Enabled = !isBattle;
            button6.Text = isTouring ? "Stop" : "Go";
            monsterSkillsListBox.Text = "";

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            character.Attack = 19;
            character.Defense = 30;
            character.MaxHealth = 6000;
            character.Crit_Rate = 0.05;
            character.Crit_Damage = 0.5;
            character.Heal();
            character.Skill = new List<Skill.Skill> { PlayerSkillPool.GetSkill() };
            character.Name = "博了";

            monster = monsterpool.GetMonster(0);
        }
        private void upgradebutton_Click(object sender, EventArgs e)
        {
            // Input Upgrade Points Function Here.
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isTouring)
            {
                TourRun();
            }
            if (!character.isAlive)
            {
                isTouring = false;
                isBattle = false;
                character.isAlive = true;
                distance= 0;
                monsterpool = new MonsterPool(Difficult);
                character.Heal();
                notifyIcon1.Visible = true;
                //MessageBox.Show("You Failed, Tour Progress Reset.");
            }
            RefreshData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Character_UseMainAttack(monster);
            /*
            double a = monster.TakeElementDamage(character.Attack,character.Crit_Rate,character.Crit_Damage,character.ElementID,monster.ElementID);
            clearLog();
            WritetoLog($"Player Used Attack.");
            WritetoLog($"Enemy Taked {a} Damage{(a == 1.0 ? "" : "s")}");
            if (!monster.isAlive)
            {
                ResultBattle();
            }
            else
            {
                double b = character.TakeDamage(monster.Attack, 0, 0);
                WritetoLog($"=========================");
                WritetoLog($"Enemy Attacks.");
                WritetoLog($"Player Taked {b} Damage{(b == 1.0 ? "" : "s")}");
                if (!monster.isAlive)
                {
                    ResultBattle();
                }
            }*/
        }

        private void Character_UseMainAttack(Monster monster)
        {
            double c = 0.0;
            clearLog();
            string e = "";
            c = ThreadSystem.UseSkill(0, character, monster, true, out e);
            WritetoLog(e);
            if (!monster.isAlive)
            {
                ResultBattle();
            }
            else
            {
                int i = random.Next(0, monster.Skill.Count);
                WritetoLog($"=====================");
                ThreadSystem.UseSkill(i, character, monster, false, out e);
                WritetoLog(e);
                if (!monster.isAlive)
                {
                    ResultBattle();
                }
            }
            totalDamagelabel.Text = $"共计伤害 {c}";
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (isTouring)
            {
                isTouring = false;
            }
            else
            {
                isTouring = true;
            }
        }

        private void WritetoLog(string text)
        {
            combatLog.Text += $"{text}\r\n";
        }

        private void clearLog()
        {
            combatLog.Clear();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Collection = string.Empty;
            foreach (var a in monster.Skill)
            {
                Collection += $"{a.Name}\r\n";
                Collection += a.Description + "\r\n";
                Collection += $"数值倍率：";
                for (int i = 0; i < a.Value.Length; i++) {
                    Collection += $"{ThreadSystem.ConvertPercent(a.Value[i])} ";
                }
                Collection += "\r\n\r\n";
            }
            MessageBox.Show(Collection);
        }

        private void Element_Details(object sender, EventArgs e)
        {
            if (sender == this.charNamelabel)
            {
                elementToolTip.ToolTipTitle = $"{Elements.strings[character.ElementID]} | {Elements.strings_English[character.ElementID]}";
                elementToolTip.SetToolTip(this.charNamelabel, Element_Rates(character.ElementID));
            }
            else if (sender == this.monnamelabel)
            {
                elementToolTip.ToolTipTitle = $"{Elements.strings[monster.ElementID]} | {Elements.strings_English[monster.ElementID]}";
                elementToolTip.SetToolTip(this.monnamelabel, Element_Rates(monster.ElementID));
            }
        }

        private string Element_Rates(int Owner_Element_ID)
        {
            string a = "";
            a += $"{Elements.strings_Describe[Owner_Element_ID]}\r\n\r\n元素伤害倍率影响：\r\n";
            for (int b = 0; b < Elements.strings.Length; b++)
            {
                a += $"{Elements.strings[b]} = x{Elements.Values[b,Owner_Element_ID]}\r\n";
            }
            return a;
        }
    }
}
