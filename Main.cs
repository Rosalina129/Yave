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

    public partial class Main : Form
    {
        /// <summary>
        /// Program Start
        /// </summary>
        
        // Init Player and more Datas.
        public static int UpgradePoints = 0;
        public static int Difficult = 2;

        public static bool isTouring = false;
        public bool isBattle = false;
        public static int distance = 0;
        private double spawncooldown = 120;

        Character character = new Character();
        MonsterPool monsterpool = new MonsterPool(Difficult);
        Monster monster;
        Random random = new Random();

        public Main()
        {
            InitializeComponent();
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
            double a = random.NextDouble();
            if (a > 0.88)
            {
                SpawnMonster();
                isTouring = false;
                isBattle= true;
            }
        }
        private void SpawnMonster()
        {
            monster = monsterpool.GetRandomMonster();
        }
        
        private void ResultBattle()
        {
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
            energylabel.Text = $"{0}";
            crlabel.Text = ThreadSystem.ConvertPercent(character.Crit_Rate);
            cdlabel.Text = ThreadSystem.ConvertPercent(character.Crit_Damage);

            monnamelabel.Text = monster.GetMonsterName();
            monhealthlabel.Text = monster.GetHealth();
            monatklabel.Text = $"{monster.Attack}";
            mondeflabel.Text = $"{monster.Defense}";
            monreslabel.Text = monster.GetResistance();

            tourdislabel.Text = $"Tour Distance: {ThreadSystem.GetDistance(distance)}";
            monsterpanel.Visible = isBattle;
            button6.Enabled = !isBattle;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            character.Attack = 3;
            character.Defense = 3;
            character.MaxHealth = 100;
            character.Heal();

            monster = monsterpool.GetRandomMonster();
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
            RefreshData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            monster.TakeDamage(character.Attack,character.Crit_Rate,character.Crit_Damage);
            if (!monster.isAlive)
            {
                ResultBattle();
            }
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
                button6.Text = "Go";
                isTouring = false;
            }
            else
            {
                button6.Text = "Stop";
                isTouring = true;
            }
        }
    }
}
