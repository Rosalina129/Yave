using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yave
{
    internal class Elements
    {
        public static string[] strings_English =
        {
            "Physical","Water","Fire","Ice","Grass","Lumine","Shadow","Void"
        };
        public static string[] strings =
        {
            "物理","水","火","冰","草","光明","暗影","虚无"
        };
        public static string[] strings_Describe =
        {
            "比较擅长硬打格斗，探索中的经验会更容易积攒。",
            "生命万物的根源，对生命值有一定照顾。",
            "焚毁一切的事物，擅长攻击力的提升。",
            "不是零下冰点不是好冰，对防御力情有独钟。",
            "绿色，绿色！可以利用自身较少的抗性去面对困难。",
            "愿星星照耀你，元素加成会有略微提升。",
            "光的对立面，可以用更低的代价去使用技能。",
            "一切皆为0，对探索会有好处。",
        };
        //"Physical","Water","Fire","Ice","Grass","Lumine","Shadow","Void"
        public static double[,] Values = new double[,]
        {
            {1,1,1.25,1.25,1,1,1,1},
            {1,1,0.667,1.75,1.25,1,1,1},
            {0.8,1.5,1,0.85,0.6,1,1,1},
            {0.8,0.571,1.176,1,1,1,1,1},
            {1,0.8,1.667,1,1,1,1,1},
            {1,1,1,1,1,1,1.5,2},
            {1,1,1,1,1,1.5,1,2},
            {1,1,1,1,1,0.5,2,1}
        };
    }
}
