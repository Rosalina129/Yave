using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yave
{
    public enum ElementName
    {
        Physical,
        Water,
        Fire,
        Ice,
        Grass,
        Lumine,
        Shadow,
        Void
    }
    // Character Class Start
    public class Character
    {
        //Basic Prop
        public int Level { get; set; }
        public string Name { get; set; }
        public double Health { get; set; }
        public double MaxHealth { get; set; }
        public double Attack { get; set; }
        public double Defense { get; set; }
        public double Crit_Rate { get; set; }
        public double Crit_Damage { get; set; }
        public double XP { get; set; }
        public double MaxHP { get; set; }
        public int ElementID { get; set; }

        //Prop
        public bool isAlive { get; set; }


        public Character()
        {
            this.Name = "";
            this.Health = 1;
            this.MaxHealth = 1;
            this.Attack = 0;
            this.Defense = 0;
            Crit_Rate = 0.05;
            Crit_Damage = 0.5;
            XP = 0;
            MaxHP = 100;
            ElementID = 0;
            isAlive = true;
        }


        //Check Controls.
        /// <summary>
        /// Performs XP Check, If more than Max XP, then Upgrade.
        /// </summary>
        private void CheckXP()
        {
            if (XP >= MaxHP)
            {
                Level += 1;
                MaxHP = MaxHP * 1.28;
                XP = 0;
                Main.UpgradePoints += 1;
            }
        }

        /// <summary>
        /// Performs Health Check.
        /// <para>The following may be the case:</para>
        /// <para>If more than Max Health, then return Max Health Limits.</para>
        /// <para>If less than 0, then return Boolean, It means: "The Player is Dead".</para>
        /// </summary>
        /// <returns></returns>
        private bool CheckHealth()
        {
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            if (Health <= 0)
            {
                Health = 0;
                return false;
            }
            else
                return true;
        }

        //Trans String Datas

        /// <summary>
        /// Return Element Name.
        /// </summary>
        /// <param name="elementID"></param>
        /// <returns></returns>

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetCharacterName()
        {
            return $"[{ThreadSystem.GetElementName(ElementID)}] {Name}";
        }

        public string GetHealth()
        {
            return $"{Math.Round(Health, 0)}/{Math.Round(MaxHealth, 0)}";
        }

        public string GetXP()
        {
            return $"{Math.Round(XP, 0)}/{Math.Round(MaxHP, 0)}";
        }

        //Control Functions
        public void AddHealth(double hp)
        {
            Health+= hp;
            CheckHealth();
        }

        public void TakeDamage(double ATKValue)
        {
            Health -= ATKValue;
            if (!CheckHealth())
            {
                isAlive = false;
            }
        }

        public void AddXP(double xp)
        {
            XP += xp;
            CheckXP();
        }

        public void Heal()
        {
            Health = MaxHealth;
            CheckHealth();
        }

    }
    //Chatacter Class End

    //Monster Class Start
    public class Monster
    {
        public string Name;
        public double Health;
        public double MaxHealth;
        public double Attack;
        public double Defense;
        public double Resistance;
        public int ElementID;
        public bool isAlive { get; set; }

        public Monster(string name, double health, double attack, double defense, double resistance, int elementID)
        {
            Name = name;
            Health = health;
            MaxHealth = Health;
            Attack = attack;
            Defense = defense;
            Resistance = resistance;
            ElementID = elementID;
            isAlive= true;
        }

        private bool CheckHealth()
        {
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            if (Math.Round(Health, 0) <= 0)
            {
                Health = 0;
                return false;
            }
            else
                return true;
        }
        public void AddHealth(double hp)
        {
            Health += hp;
            CheckHealth();
        }

        public void TakeElementDamage(double ATKValue, int elementID)
        {

        }

        public void TakeDamage(double ATKValue, double critRate, double critDamage)
        {
            Health -= ThreadSystem.CalculateDamage(ATKValue, Defense,critRate,critDamage,Resistance);
            if (!CheckHealth())
            {
                isAlive = false;
            }
        }

        public string GetHealth()
        {
            return $"{Math.Round(Health,0)}/{Math.Round(MaxHealth,0)}";
        }

        public string GetMonsterName()
        {
            return $"[{ThreadSystem.GetElementName(ElementID)}] {Name}";
        }

        public string GetResistance()
        {
            return $"{ThreadSystem.ConvertPercent(Resistance)}";
        }
    }
    //Monster Class End
    public class MonsterPool
    {
        private List<Monster> monsters;

        public MonsterPool(int Difficulty)
        {
            monsters = new List<Monster>
            {
                new Monster("Slime", ThreadSystem.Difficulty_Control(150, Difficulty), ThreadSystem.Difficulty_Control(4, Difficulty),ThreadSystem.Difficulty_Control(0,Difficulty),0,(int)ElementName.Physical),
                new Monster("Spider", ThreadSystem.Difficulty_Control(12, Difficulty), ThreadSystem.Difficulty_Control(5, Difficulty), ThreadSystem.Difficulty_Control(3, Difficulty), 0.01, (int)ElementName.Grass),
                new Monster("Bat", ThreadSystem.Difficulty_Control(6, Difficulty), ThreadSystem.Difficulty_Control(7, Difficulty), ThreadSystem.Difficulty_Control(1, Difficulty), 0, (int)ElementName.Shadow),
                new Monster("Cockroach", ThreadSystem.Difficulty_Control(12, Difficulty), ThreadSystem.Difficulty_Control(3, Difficulty), ThreadSystem.Difficulty_Control(1, Difficulty), 0.02, (int)ElementName.Grass),
                new Monster("Mosquito", ThreadSystem.Difficulty_Control(15, Difficulty), ThreadSystem.Difficulty_Control(3, Difficulty), ThreadSystem.Difficulty_Control(0, Difficulty), 0, (int)ElementName.Grass),
                new Monster("Ice Slime", ThreadSystem.Difficulty_Control(30, Difficulty), ThreadSystem.Difficulty_Control(8, Difficulty), ThreadSystem.Difficulty_Control(6, Difficulty), 0.03, (int)ElementName.Ice),
                new Monster("Fire Slime", ThreadSystem.Difficulty_Control(30, Difficulty), ThreadSystem.Difficulty_Control(9, Difficulty), ThreadSystem.Difficulty_Control(2, Difficulty), 0.03, (int)ElementName.Fire)
            };
        }

        public Monster GetRandomMonster()
        {
            Random random = new Random();
            int index = random.Next(monsters.Count);
            return monsters[index];
        }
    }

    //Basic Class Start
    //Basic Class End

    //Skill Class Start
    public class Skill
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    //Skill Class End

    //Items Class Start
    public class Items
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
    //Items Class End



    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
    public class ThreadSystem
    {
        public static double Difficulty_Control(double value, int a)
        {
            var Multiply = 0.0;
            switch (a)
            {
                case 1: // Notice
                    Multiply = 1;
                    break;
                case 2: // Pro
                    Multiply = 1.5;
                    break;
                case 3: // Expert
                    Multiply = 2;
                    break;
                case 4: // Master
                    Multiply = 3;
                    break;
                case 5: // HELL
                    Multiply = 5;
                    break;
                default: // Noob
                    Multiply = 0.5;
                    break;
            }
            return value * Multiply;
        }
        public static string ConvertPercent(double percent)
        {
            return $"{Math.Round(percent * 100,2)}%";
        }

        public static string GetElementName(int elementID)
        {
            return Elements.strings[elementID];
        }

        public static double CalculateDamage(double Attack, double Defense, double critRate, double critDamage, double resistance)
        {
            Random r1 = new Random();
            Thread.Sleep(1);
            double c;
            c = r1.NextDouble();
            double DefenseInv = 1 - 1 / (1 + Defense / 10);
            if (c >= 1 - critRate)
                return Attack * (1 - DefenseInv) * (1 + critDamage) * (1 - resistance);
            else
                return Attack * (1 - DefenseInv) * (1 - resistance);
        }

        public static string GetDistance(double distance)
        {
            string unit = "m";
            if (distance > 1000000)
                unit = "km";
            return $"{Math.Round((distance > 100000 ? distance / 100000 : distance/100), 2)} {unit}";
        }
    }
}
