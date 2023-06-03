using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YaveDBLib;
using Yave.Skill;
using System.Security.Policy;

namespace Yave
{

    public partial class Main : Form
    {
        /// <summary>
        /// Program Start
        /// </summary>
        public static string FileName = "";
        // Init Player and more Datas.
        public static int[] SaveVersion = new int[] {1,0,0};
        public static int[] SoftwareVersion = new int[] {1,0,20230603,0};
        public static int UpgradePoints = 0;
        public static int Difficult = 3;

        public static bool isTouring = false;
        public bool isBattle = false;
        public static int[] distance = new int[8];
        public static int location = (int)Locations.EmeraldPlains;
        private double spawncooldown = 125;
        private int saveCooldown = 65 * 180;
        private int cacheData_a = 0;


        public static Character character = new Character();
        MonsterPool monsterpool = new MonsterPool(Difficult);
        PlayerSkillPool skillPool = new PlayerSkillPool();
        Monster monster;
        Random random = new Random();

        public Main()
        {
            InitializeComponent();
            totalDamagelabel.Text = "共计伤害 0";
            rewardResults.Text = "";
        }
        /// <summary>
        /// Perform refresh data.
        /// </summary>
         
        private void TourRun()
        {
            distance[0] += 1;
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
            var addxpValue = monster.Rewards.XP * character.XPBoost * Difficulty_Control_Player(Difficult) * (character.TierLevel * character.TierLevel+1) * (random.NextDouble() + 1.2);
            var addCoinsValue = monster.Rewards.Coins * character.CoinsBoost * Difficulty_Control_Player(Difficult) * (character.TierLevel * character.TierLevel + 1) * (random.NextDouble() + 1.2);
            if (character.Level == 100 && character.XP >= character.MaxXP)
            {
                rewardResults.Text = $"获得 {Math.Round(addCoinsValue, 0)} (溢出部分 +{Math.Round(addxpValue, 0)}) 金币。";
            }
            else
            {
                rewardResults.Text = $"获得 {Math.Round(addxpValue, 0)} XP，获得 {Math.Round(addCoinsValue, 0)} 金币。";
            }
            character.AddXP(addxpValue,out string a);
            character.Coins += (int)addCoinsValue;
            rewardResults.Text += $"{a}";
            spawncooldown = random.NextDouble() * 250 + 185;
            clearLog();
            monsterSkillsListBox.Items.Clear();
            isBattle = false;
            monsterpool = new MonsterPool(Difficult);
        }

        private void NewSaveInit()
        {
            Difficult = TitleScreen.Difficulty;
            character.Name = TitleScreen.CharacterName;
            character.MaxHealth = ValueData.Player.Health * Difficulty_Control_Player(Difficult);
            character.Attack = ValueData.Player.Attack * Difficulty_Control_Player(Difficult);
            character.Defense = ValueData.Player.Defense * Difficulty_Control_Player(Difficult);
            character.Crit_Rate = ValueData.Player.CritRate;
            character.Crit_Damage = ValueData.Player.CritDamage;
            character.Skill = new List<PlayerSkill> { PlayerSkillPool.GetSkill() };
            character.ElementID = TitleScreen.Element;
            ElementBuffs(character.ElementID);
            character.Update();
            character.Heal();
            if (Difficult!= 0)  SaveData();
        }

        private void RefreshData()
        {
            charNamelabel.Text = character.GetCharacterName();
            xplabel.Text = character.GetXP();
            hplabel.Text = character.GetHealth();
            atklabel.Text = $"{Math.Round(character.Attack, 0)}";
            deflabel.Text = $"{Math.Round(character.Defense, 0)}";
            levellabel.Text = $"[阶级 {character.TierLevel}] 等级 {character.Level}";
            energylabel.Text = $"{0}";
            crlabel.Text = ThreadSystem.ConvertPercent(character.Crit_Rate);
            cdlabel.Text = ThreadSystem.ConvertPercent(character.Crit_Damage);

            monnamelabel.Text = monster.GetMonsterName();
            monhealthlabel.Text = monster.GetHealth();
            monatklabel.Text = $"{Math.Round(monster.Attack,0)}";
            mondeflabel.Text = $"{Math.Round(monster.Defense,0)}";
            monreslabel.Text = monster.GetResistance();

            tourdislabel.Text = $"共计旅行距离: {ThreadSystem.GetDistance(distance[location])}";
            monsterpanel.Visible = isBattle;
            button6.Enabled = !isBattle;
            button6.Text = isTouring ? "Stop" : "Go";
            monsterSkillsListBox.Text = "";

            CoinsStatus.Text = $"{character.Coins}";

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            monster = monsterpool.GetMonster(0);
            TitleScreen titleScreen = new TitleScreen();
            titleScreen.ShowDialog();
            if (!TitleScreen.isCreated && !TitleScreen.isLoaded)
            {
                this.Close();
            }
            else
            {
                if (TitleScreen.isCreated)
                {
                    //MessageBox.Show(FileName);
                    NewSaveInit();
                }
                if (TitleScreen.isLoaded)
                {
                    LoadData();
                }
            }
        }
        private void upgradebutton_Click(object sender, EventArgs e)
        {
            // Input Upgrade Points Function Here.
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            saveCooldown--;
            if (saveCooldown < 0)
            {
                saveCooldown = 65*180;
                SaveData();
            }
            if (isTouring)
            {
                TourRun();
            }
            if (!character.isAlive)
            {
                rewardResults.Text = $"你已倒下，已自动退回开始的地方。";
                isTouring = false;
                isBattle = false;
                character.isAlive = true;
                distance[location]= 0;
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
            c = character.UseSkill(0, character, monster, out e);
            WritetoLog(e);
            if (!monster.isAlive)
            {
                ResultBattle();
            }
            else
            {
                int i = random.Next(0, monster.Skill.Count);
                WritetoLog($"=====================");
                monster.UseSkill(i, character, monster, out e);
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

        public static string Element_Rates(int Owner_Element_ID)
        {
            string a = "";
            a += $"{Elements.strings_Describe[Owner_Element_ID]}\r\n\r\n元素伤害倍率影响：\r\n";
            for (int b = 0; b < Elements.strings.Length; b++)
            {
                a += $"{Elements.strings[b]} = x{Elements.Values[b,Owner_Element_ID]}\r\n";
            }
            return a;
        }

        private double Difficulty_Control_Player(int Difficulty)
        {
            double Multiply = 1;
            switch (Difficulty)
            {
                case 0:
                    Multiply = 5;
                    break;
                case 1:
                    Multiply = 1;
                    break;
                case 2:
                    Multiply = 0.94;
                    break;
                case 3:
                    Multiply = 0.89;
                    break;
                case 4:
                    Multiply = 0.84;
                    break;
                case 5:
                    Multiply = 0.75;
                    break;
            }
            return Multiply;
        }

        private void ElementBuffs(int Element_ID)
        {
            var a = character;
            switch (Element_ID)
            {
                case 0:
                    a.XPBoost += ValueData.Player.XPBoost;
                    break;
                case 1:
                    a.MaxHealth *= (ValueData.Player.BaseBoost + 1);
                    break;
                case 2:
                    a.Attack *= (ValueData.Player.BaseBoost + 1);
                    break;
                case 3:
                    a.Defense *= (ValueData.Player.BaseBoost + 1);
                    break;
                case 4:
                    var b = ValueData.Player.ElementRes;
                    a.EPrep.ERes[(int)ElementName.Physical] = b;
                    a.EPrep.ERes[(int)ElementName.Water] = b;
                    a.EPrep.ERes[(int)ElementName.Fire] = b;
                    a.EPrep.ERes[(int)ElementName.Ice] = b;
                    a.EPrep.ERes[(int)ElementName.Grass] = b;
                    a.EPrep.ERes[(int)ElementName.Lumine] = b;
                    a.EPrep.ERes[(int)ElementName.Shadow] = b;
                    a.EPrep.ERes[(int)ElementName.Void] = b;
                    break;
                case 5:
                    var c = ValueData.Player.ElementDamage;
                    a.EPrep.EDMGBonus[(int)ElementName.Physical] = c;
                    a.EPrep.EDMGBonus[(int)ElementName.Water] = c;
                    a.EPrep.EDMGBonus[(int)ElementName.Fire] = c;
                    a.EPrep.EDMGBonus[(int)ElementName.Ice] = c;
                    a.EPrep.EDMGBonus[(int)ElementName.Grass] = c;
                    a.EPrep.EDMGBonus[(int)ElementName.Lumine] = c;
                    a.EPrep.EDMGBonus[(int)ElementName.Shadow] = c;
                    a.EPrep.EDMGBonus[(int)ElementName.Void] = c;
                    break;
                case 6:
                    a.SkillCost = ValueData.Player.SkillCost;
                    break;
                case 7:
                    a.CoinsBoost += ValueData.Player.RewardBoost;
                    break;
            }
        }

        private void SaveData()
        {
            if (Difficult != 0)
            {
                //CheckFileExists();
                JSON.Collection JsonData = new JSON.Collection();
                FileStream fs = new FileStream(FileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                JsonData.saveVersion = SaveVersion;
                JsonData.locationData = new int[8];
                for (int i = 0; i < distance.Length; i++)
                {
                    JsonData.locationData[i] = distance[i];
                }
                JsonData.playerData.level = character.Level;
                JsonData.playerData.name = character.Name;
                JsonData.playerData.health.current = character.Health;
                JsonData.playerData.health.max = character.MaxHealth;
                JsonData.playerData.attack = character.Attack;
                JsonData.playerData.defense = character.Defense;
                JsonData.playerData.critRate = character.Crit_Rate;
                JsonData.playerData.critDamage = character.Crit_Damage;
                JsonData.playerData.xp.current = character.XP;
                JsonData.playerData.xp.max = character.MaxXP;
                JsonData.playerData.elementID = character.ElementID;
                JsonData.playerData.Skills = new List<JSON.Skill>();
                for (int i = 0; i < character.Skill.Count; i++)
                {
                    JsonData.playerData.Skills.Add(
                        new JSON.Skill
                        {
                            ID = character.Skill[i].SkillID,
                            level = character.Skill[i].Level
                        }
                    );
                }
                JsonData.playerData.Buffs = character.Buffs;
                string JsonDataString = JsonConvert.SerializeObject(JsonData, Formatting.Indented);
                sw.WriteLine(JsonDataString);
                sw.Close();
                fs.Close();
                saveCooldown = 65 * 180;
                timer2.Enabled = true;
            }
        }

        private void LoadData()
        {
            Character a = character;
            JSON.Collection b = new JSON.Collection();
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string JsonDataString = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            b = JsonConvert.DeserializeObject<JSON.Collection>(JsonDataString);
            SaveVersion = b.saveVersion;
            for (int i = 0; i < distance.Length; i++)
            {
                distance[i] = b.locationData[i];
            }
            a.Level = b.playerData.level;
            a.Name = b.playerData.name;
            a.Health = b.playerData.health.current;
            a.MaxHealth = b.playerData.health.max;
            a.Attack = b.playerData.attack;
            a.Defense = b.playerData.defense;
            a.Crit_Rate = b.playerData.critRate;
            a.Crit_Damage = b.playerData.critDamage;
            a.XP = b.playerData.xp.current;
            a.MaxXP = b.playerData.xp.max;
            a.ElementID = b.playerData.elementID;
            for (int i = 0; i < b.playerData.Skills.Count; i++)
            {
                a.Skill.Add(
                    PlayerSkillPool.GetSkill(b.playerData.Skills[i].ID)
                );
            }
            a.Buffs = b.playerData.Buffs;
            saveCooldown = 65 * 120;
            a.Update();
        }
        /*
        private bool CheckFileExists()
        {
            var result = false;
            if (!File.Exists(FileName))
            {
                File.Create(FileName);
                result = true;
            }
            return result;
        }
        */
        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Difficult == 0)
            {
                MessageBox.Show("你当前为菜鸟模式，无法保存！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SaveData();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (Difficult == 0)
            {
                MessageBox.Show("你当前为菜鸟模式，无法保存！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileName = saveFileDialog1.FileName;
                    SaveData();
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            cacheData_a++;
            if (cacheData_a== 1)
            {
                rewardResults.Text = $"保存成功！";
            }
            else if (cacheData_a== 180)
            {
                rewardResults.Text = $"";
                cacheData_a= 0;
                timer2.Enabled = false;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PlayerProp playerProp = new PlayerProp();
            playerProp.Show();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileName = openFileDialog1.FileName;
                LoadData();
            }
        }
    }
}
