using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yave
{
    internal class JSON
    {
        public struct Collection
        {
            public int[] saveVersion;
            public int[] locationData;
            public Player playerData;
        }
        public struct Player
        {
            public int level;
            public string name;
            public Health health;
            public double attack;
            public double defense;
            public double critRate;
            public double critDamage;
            public XP xp;
            public int elementID;
            public List<Skill> Skills;
            public List<Buff> Buffs;
        }
        public struct Health
        {
            public double current;
            public double max;
        }
        public struct XP
        {
            public double current;
            public double max;
        }
        public struct Skill {
            public int ID;
            public int level;
        };
    }
}
