using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yave.Skill;
using Yave.Buff;
using YaveDBLib;
using System.Diagnostics;

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
    public enum Locations
    {
        EmeraldPlains,
        DripForest,
        FrostmeltSnowMountain,
        DolphinShore,
        HarmonicDarkValley,
        KarstCaveMine,
        Shangri_La,
        DeepevilVolcano
    }

    public enum BuffType
    {
        None,
        MaxHealth,
        Attack,
        Defense,
        CritRate,
        CritDamage,
        PhysicalRes,
        WaterRes,
        FireRes,
        IceRes,
        GrassRes,
        LumineRes,
        ShadowRes,
        VoidRes,
        PhysicalDMG,
        WaterDMG,
        FireDMG,
        IceDMG,
        GrassDMG,
        LumineDMG,
        ShadowDMG,
        VoidDMG,
        SkillCost,
        SkillCD,
        ShieldBonus,
        ShieldAbs,
        XPBonus,
        CoinsBonus,
        ItemDropBonus,
        LootBonus
    }
    // Entity Base Class
    public abstract class Entity
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public double Health { get; set; }
        public double MaxHealth { get; set; }
        public double Attack { get; set; }
        public double Defense { get; set; }
        public double Crit_Rate { get; set; }
        public double Crit_Damage { get; set; }
        public int ElementID { get; set; }
        public ElementsPrep EPrep { get; set; }
        public List<Buff.Buff> Buffs { get; set; }

        //Prop
        public bool isAlive { get; set; }

        //Base Datas
        public double baseMaxHealth;
        public double baseAttack;
        public double baseDefense;
        public double baseCritRate;
        public double baseCritDamage;
        /// <summary>
        /// 使用技能
        /// </summary>
        /// <param name="ID">技能的ID</param>
        /// <param name="player">玩家对象</param>
        /// <param name="monster">怪物对象</param>
        /// <param name="target">目标，真为 Monster，假为 Player</param>
        /// <param name="log">输出的日志文本</param>
        public abstract double UseSkill(int ID, Character player, Monster monster, out string log);
    }
    // Character Class Start
    public class Character : Entity
    {
        Random Random = new Random();

        //Basic Prop for Player
        public int Skin { get; set; }
        public int TierLevel { get; set; }
        public double XP { get; set; }
        public int Coins { get; set; }
        public double MaxXP { get; set; }
        public double XPBoost { get; set; }
        public double CoinsBoost { get; set; }
        public double SkillCost { get; set; }
        public double Shield { get; set; }
        public double ShieldMulti { get; set; }
        public List<Skill.PlayerSkill> Skill { get; set; } = new List<Skill.PlayerSkill>();

        public Character()
        {
            Name = "";
            Health = 1;
            MaxHealth = 1;
            Attack = 0;
            Defense = 0;
            Crit_Rate = 0.05;
            Crit_Damage = 0.5;
            XP = 0;
            MaxXP = 50 + ValueData.Upgrade.Need[0];
            ElementID = 0;
            Skill = new List<Skill.PlayerSkill>();
            Buffs = new List<Buff.Buff>();
            isAlive = true;

            baseMaxHealth = MaxHealth;
            baseAttack = Attack;
            baseDefense = Defense;
            baseCritRate = Crit_Rate;
            baseCritDamage = Crit_Damage;

            XPBoost = 1;
            CoinsBoost = 1;
        }

        public void NewCharacter(string name, int elementID, double health, double atk, double defense)
        {
            Name = name;
            ElementID = elementID;
            MaxHealth = health;
            Attack = atk;
            Defense = defense;
        }

        public void Update()
        {
            baseMaxHealth = MaxHealth;
            baseAttack = Attack;
            baseDefense = Defense;
            baseCritRate = Crit_Rate;
            baseCritDamage = Crit_Damage;
        }
        //Check Controls.
        /// <summary>
        /// Performs XP Check, If more than Max XP, then Upgrade.
        /// </summary>
        private string CheckXP()
        {
            string e = "";
            var a = new double[]
            {
                Random.Next((int)ValueData.Upgrade.Health[ElementID, TierLevel, 0], (int)ValueData.Upgrade.Health[ElementID, TierLevel, 1] + 1),
                Random.Next((int)ValueData.Upgrade.ATK[ElementID, TierLevel, 0], (int)ValueData.Upgrade.ATK[ElementID, TierLevel, 1] + 1),
                Random.Next((int)ValueData.Upgrade.DEF[ElementID, TierLevel, 0], (int)ValueData.Upgrade.DEF[ElementID, TierLevel, 1] + 1)
            };
            if (XP >= MaxXP)
            {
                if (Level < 100)
                {
                    Level += 1;
                    XP -= MaxXP;
                    MaxXP = MaxXP + LevelXPNeed(Level);
                    TierLVLUpgrade(Level);
                    baseMaxHealth += a[0];
                    baseAttack += a[1];
                    baseDefense += a[2];
                    MaxHealth = baseMaxHealth;
                    Attack = baseAttack;
                    Defense = baseDefense;
                    Main.UpgradePoints += 1;
                    e += $"角色升级！+{a[0]} 生命, +{a[1]} 攻击力, +{a[2]} 防御力。";
                }
                else if (Level == 100 && XP == MaxXP)
                {
                    Coins += (int)(XP - MaxXP);
                    XP = MaxXP;
                }
            }
            return e;
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
        private void BuffCalculate()
        {

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
            return $"{Math.Round(XP, 0)}/{Math.Round(MaxXP, 0)}";
        }

        //Control Functions
        public void AddHealth(double hp)
        {
            Health+= hp;
            CheckHealth();
        }

        public void AddBuff(Buff.Buff buff)
        {
            this.Buffs.Add(buff);
            Sync();
        }

        public void RemoveBuff(int a)
        {
            this.Buffs.Remove(this.Buffs[a]);
            Sync();
        }

        public double TakeElementDamage(double ATKValue, double critRate, double critDamage, int elementA, int elementB, bool isAntiDefense = false)
        {
            double a = 0.0;
            if (isAntiDefense)
            {
                a = Math.Round(ThreadSystem.CalculateDamage(ATKValue, 0, critRate, critDamage, 0, elementA, elementB), 0);
            }
            else
            {
                a = Math.Round(ThreadSystem.CalculateDamage(ATKValue, Defense, critRate, critDamage, 0, elementA, elementB), 0);
            }
            Health -= a <= 1 ? 1 : a;
            if (!CheckHealth())
            {
                isAlive = false;
            }
            return a;
        }

        public void AddXP(double xp, out string e)
        {
            XP += xp;
            e = CheckXP();
        }

        public void TierLVLUpgrade(int level)
        {
            if (level == 20 ||
                level == 40 ||
                level == 60 ||
                level == 70 ||
                level == 80 ||
                level == 90)
            {
                this.TierLevel++;
            }
        }

        public double LevelXPNeed(int level)
        {
            int a = 0;
            if (level <= 4)
            {
                a = 0;
            }
            else if (level > 4 && level <= 9)
            {
                a = 1;
            }
            else if (level > 9 && level <= 14)
            {
                a = 2;
            }
            else if (level > 14 && level <= 19)
            {
                a = 3;
            }
            else if (level > 19 && level <= 24)
            {
                a = 4;
            }
            else if (level > 24 && level <= 29)
            {
                a = 5;
            }
            else if (level > 29 && level <= 34)
            {
                a = 6;
            }
            else if (level > 34 && level <= 39)
            {
                a = 7;
            }
            else if (level > 39 && level <= 45)
            {
                a = 8;
            }
            else if (level > 45 && level <= 52)
            {
                a = 9;
            }
            else if (level > 52 && level <= 59)
            {
                a = 10;
            }
            else if (level > 59 && level <= 62)
            {
                a = 11;
            }
            else if (level > 62 && level <= 64)
            {
                a = 12;
            }
            else if (level > 64 && level <= 66)
            {
                a = 13;
            }
            else if (level > 66 && level <= 69)
            {
                a = 14;
            }
            else if (level > 69 && level <= 74)
            {
                a = 15;
            }
            else if (level > 74 && level <= 79)
            {
                a = 16;
            }
            else if (level > 79 && level <= 84)
            {
                a = 17;
            }
            else if (level > 84 && level <= 89)
            {
                a = 18;
            }
            else if (level > 89 && level <= 92)
            {
                a = 19;
            }
            else if (level > 92 && level <= 95)
            {
                a = 20;
            }
            else if (level > 95)
            {
                a = 21;
            }
            return ValueData.Upgrade.Need[a];
        }

        public void Heal()
        {
            Health = MaxHealth;
            isAlive = CheckHealth();
        }

        public void LevelUP()
        {
            baseMaxHealth += 20;
        }

        public void SyncBuffPer()
        {
            for (int i = 0; i < this.Buffs.Count; i++)
            {
                this.Buffs[i].Cycles--;
                if (this.Buffs[i].Cycles == 0)
                {
                    RemoveBuff(i);
                }
            }
            Sync();
        }

        public void Sync()
        {
            int length = Buff.Buff.buffType.Length;
            // Base Health, ATK, DEF, CritRate, CritDamage.
            double[] relativePercent = new double[length];
            for (int i = 0;i < relativePercent.Length; i++)
            {
                relativePercent[i] = 1.0;
            }
            double[] absoluteValues = new double[length];
            foreach (var a in this.Buffs)
            {
                if (a.Relative)
                {
                    relativePercent[a.ID] += a.Values;
                }
                absoluteValues[a.ID] += a.Values;
            }
            this.MaxHealth = this.baseMaxHealth * (relativePercent[0]) + absoluteValues[0];
            this.Attack = this.baseAttack * (relativePercent[1]) + absoluteValues[1] ;
            this.Defense = this.baseDefense * (relativePercent[2]) + absoluteValues[2];
            this.Crit_Rate = this.baseCritRate * (relativePercent[3]) + absoluteValues[3];
            this.Crit_Damage = this.baseCritDamage * (relativePercent[4]) + absoluteValues[4];
        }

        public override double UseSkill(int ID, Character player, Monster monster, out string log)
        {
            double damageTotal = 0.0;
            log = "";
            log += $"{player.Name} 使用了 {player.Skill[ID].Name}.\r\n";
            switch (player.Skill[ID].SkillID)
            {
                case 0:
                    damageTotal += monster.TakeElementDamage(player.Skill[0].Value[player.Skill[0].Level] * player.Attack, player.Crit_Rate, player.Crit_Damage, player.ElementID, monster.ElementID);
                    log += $"敌方受到了 {damageTotal} 点伤害\r\n";
                    break;
            }
            return damageTotal;
            throw new NotImplementedException();
        }
    }
    //Chatacter Class End

    public class Reward
    {
        public double Coins { get; set; }
        public double XP { get; set; }
    }
    //Monster Class Start
    public class Monster : Entity
    {
        public double Resistance { get; set; }
        public List<MonsterSkill> Skill { get; set; }
        public Reward Rewards { get; set; }

        public Monster(string name, double health, double attack, double defense, double resistance, int elementID, List<Skill.MonsterSkill> skill, Reward rewards)
        {
            Name = name;
            Health = health;
            MaxHealth = Health;
            Attack = attack;
            Defense = defense;
            Resistance = resistance;
            ElementID = elementID;
            isAlive = true;
            Skill = skill;
            Rewards = rewards;

            baseMaxHealth = MaxHealth;
            baseAttack = attack;
            baseDefense = defense;
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

        public double TakeElementDamage(double ATKValue, double critRate, double critDamage, int elementA, int elementB)
        {
            double a = Math.Round(ThreadSystem.CalculateDamage(ATKValue, Defense, critRate, critDamage, Resistance,elementA,elementB), 0);
            Health -= a <= 1 ? 1 : a;
            if (!CheckHealth())
            {
                isAlive = false;
            }
            return a;
        }

        public double TakeDamage(double ATKValue, double critRate, double critDamage)
        {
            double a = Math.Round(ThreadSystem.CalculateDamage(ATKValue, Defense, critRate, critDamage, Resistance),0);
            Health -= a <= 1 ? 1 : a;
            if (!CheckHealth())
            {
                isAlive = false;
            }
            return a;
        }

        public string GetHealth()
        {
            return $"{ThreadSystem.ConvertPercent(Health / MaxHealth)} [{Health}/{MaxHealth}]";
        }

        public string GetMonsterName()
        {
            return $"[{ThreadSystem.GetElementName(ElementID)}] {Name}";
        }

        public string GetResistance()
        {
            return $"{ThreadSystem.ConvertPercent(Resistance)}";
        }

        public override double UseSkill(int ID, Character player, Monster monster, out string log)
        {
            double damageTotal = 0.0;
            log = "";
            log += $"{monster.Name} 使用了 {monster.Skill[ID].Name}.\r\n";
            switch (monster.Skill[ID].SkillID)
            {
                case 0:
                case 1:
                    damageTotal = player.TakeElementDamage(monster.Skill[ID].Value[0] * monster.Attack, 0, 0, player.ElementID, monster.ElementID);
                    log += $"玩家受到了 {damageTotal} 点伤害\r\n";
                    break;
                case 2:
                    for (int a = 0; a < monster.Skill[ID].Value.Length; a++)
                    {
                        damageTotal = player.TakeElementDamage(monster.Skill[ID].Value[a] * monster.Attack, 0, 0, player.ElementID, monster.ElementID);
                        log += $"玩家受到了 {damageTotal} 点伤害\r\n";
                    }
                    break;
                case 3:
                    damageTotal = player.TakeElementDamage(monster.Skill[ID].Value[0] * monster.Attack, 0, 0, player.ElementID, monster.ElementID, true);
                    log += $"玩家受到了 {damageTotal} 点伤害\r\n";
                    break;
                case 4:
                    damageTotal = player.TakeElementDamage(monster.Skill[ID].Value[0] * monster.Attack, 0, 0, player.ElementID, monster.ElementID);
                    log += $"玩家受到了 {damageTotal} 点伤害\r\n";
                    monster.Health += monster.Skill[ID].Value[0] * damageTotal;
                    log += $"敌方回复了 {damageTotal} HP\r\n";
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
            }
            return damageTotal;
            throw new NotImplementedException();
        }
    }
    //Monster Class End
    public class MonsterPool
    {
        private List<Monster> monsters;
        MonsterSkillPool monsterSkillPool = new MonsterSkillPool();
        public MonsterPool(int Difficulty)
        {
            monsters = new List<Monster>
            {
                // Region 1 Monsters
                // Tier 1
                new Monster(
                    "Slime",
                    ThreadSystem.Difficulty_Control(132,Difficulty),
                    ThreadSystem.Difficulty_Control(18,Difficulty),
                    ThreadSystem.Difficulty_Control(10,Difficulty),
                    0,
                    (int)ElementName.Grass,
                    new List<MonsterSkill>
                    {
                        MonsterSkillPool.GetSkill()
                    },
                    new Reward
                    {
                        Coins = 8,
                        XP = 13
                    }
                ),
                new Monster(
                    "Red Slime",
                    ThreadSystem.Difficulty_Control(169,Difficulty),
                    ThreadSystem.Difficulty_Control(25,Difficulty),
                    ThreadSystem.Difficulty_Control(12,Difficulty),
                    0,
                    (int)ElementName.Physical,
                    new List<MonsterSkill>
                    {
                        MonsterSkillPool.GetSkill(),
                        MonsterSkillPool.GetSkill(1)
                    },
                    new Reward
                    {
                        Coins = 9,
                        XP = 16
                    }
                ),
                new Monster(
                    "Orange Slime",
                    ThreadSystem.Difficulty_Control(148,Difficulty),
                    ThreadSystem.Difficulty_Control(30,Difficulty),
                    ThreadSystem.Difficulty_Control(7,Difficulty),
                    0,
                    (int)ElementName.Fire,
                    new List<MonsterSkill>
                    {
                        MonsterSkillPool.GetSkill(),
                    },
                    new Reward
                    {
                        Coins = 12,
                        XP = 23
                    }
                ),
                new Monster(
                    "Yellow Slime",
                    ThreadSystem.Difficulty_Control(163,Difficulty),
                    ThreadSystem.Difficulty_Control(38,Difficulty),
                    ThreadSystem.Difficulty_Control(12,Difficulty),
                    0,
                    (int)ElementName.Lumine,
                    new List<MonsterSkill>
                    {
                        MonsterSkillPool.GetSkill(),
                        MonsterSkillPool.GetSkill(2)
                    },
                    new Reward
                    {
                        Coins = 23,
                        XP = 14
                    }
                ),
                new Monster(
                    "Green Slime",
                    ThreadSystem.Difficulty_Control(172,Difficulty),
                    ThreadSystem.Difficulty_Control(38,Difficulty),
                    ThreadSystem.Difficulty_Control(13,Difficulty),
                    0,
                    (int)ElementName.Grass,
                    new List<MonsterSkill>
                    {
                        MonsterSkillPool.GetSkill(),
                        MonsterSkillPool.GetSkill(3)
                    },
                    new Reward
                    {
                        Coins = 7,
                        XP = 19
                    }
                ),
                new Monster(
                    "Cyan Slime",
                    ThreadSystem.Difficulty_Control(210,Difficulty),
                    ThreadSystem.Difficulty_Control(20,Difficulty),
                    ThreadSystem.Difficulty_Control(19,Difficulty),
                    0,
                    (int)ElementName.Ice,
                    new List<MonsterSkill>
                    {
                        MonsterSkillPool.GetSkill(),
                        MonsterSkillPool.GetSkill(4)
                    },
                    new Reward
                    {
                        Coins = 8,
                        XP = 10
                    }
                ),
                new Monster(
                    "Blue Slime",
                    ThreadSystem.Difficulty_Control(289,Difficulty),
                    ThreadSystem.Difficulty_Control(18,Difficulty),
                    ThreadSystem.Difficulty_Control(12,Difficulty),
                    0,
                    (int)ElementName.Water,
                    new List<MonsterSkill>
                    {
                        MonsterSkillPool.GetSkill(),
                        MonsterSkillPool.GetSkill(1),
                        MonsterSkillPool.GetSkill(5)
                    },
                    new Reward
                    {
                        Coins = 10,
                        XP = 14
                    }
                ),
                new Monster(
                    "Purple Slime",
                    ThreadSystem.Difficulty_Control(230,Difficulty),
                    ThreadSystem.Difficulty_Control(23,Difficulty),
                    ThreadSystem.Difficulty_Control(19,Difficulty),
                    0,
                    (int)ElementName.Shadow,
                    new List<MonsterSkill>
                    {
                        MonsterSkillPool.GetSkill(),
                        MonsterSkillPool.GetSkill(1),
                        MonsterSkillPool.GetSkill(5)
                    },
                    new Reward
                    {
                        Coins = 11,
                        XP = 15
                    }
                ),
                new Monster(
                    "Black Slime",
                    ThreadSystem.Difficulty_Control(245,Difficulty),
                    ThreadSystem.Difficulty_Control(23,Difficulty),
                    ThreadSystem.Difficulty_Control(20,Difficulty),
                    0,
                    (int)ElementName.Void,
                    new List<MonsterSkill>
                    {
                        MonsterSkillPool.GetSkill(),
                        MonsterSkillPool.GetSkill(3),
                        MonsterSkillPool.GetSkill(6)
                    },
                    new Reward
                    {
                        Coins = 7,
                        XP = 17
                    }
                ),
            };
        }

        public Monster GetMonster(int index)
        {
            return monsters[index];
        }
    }

    //Basic Class Start
    //Basic Class End

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
            return value * Multiply * ((Main.character.TierLevel + 1) * (Main.character.TierLevel + 1) * 0.25);
        }
        public static string ConvertPercent(double percent)
        {
            return $"{Math.Round(percent * 100,2)}%";
        }

        public static string GetElementName(int elementID)
        {
            return Elements.strings[elementID];
        }

        public static double CalculateDamage(double Attack, double Defense, double critRate, double critDamage, double resistance, int elementA = 0, int elementB = 0)
        {
            Random r1 = new Random();
            Thread.Sleep(1);
            double c;
            c = r1.NextDouble();
            if (c >= 1 - critRate)
                return (Attack * Attack / (Attack + Defense)) * (1 + critDamage) * (1 - resistance) * GetElementRates(elementA, elementB);
            else
                return (Attack * Attack / (Attack + Defense)) * (1 - resistance) * GetElementRates(elementA,elementB);
        }

        public static string GetDistance(double distance)
        {
            string unit = "m";
            if (distance > 1000000)
                unit = "km";
            return $"{Math.Round((distance > 100000 ? distance / 100000 : distance/100), 2)} {unit}";
        }

        public static double GetElementRates(int elementA, int elementB)
        {
            return Elements.Values[elementA,elementB];
        }

    }
    public struct ElementsPrep
    {
        public double[] ERes;
        public double[] EDMGBonus;
    }
}
