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
    public partial class SkillDescribes : UserControl
    {
        private string skillname = string.Empty;
        private string skilldescription = string.Empty;

        public string Skillname { get => skillname; set => skillname = value; }
        public string Skilldescription { get => skilldescription; set => skilldescription = value; }

        public SkillDescribes()
        {
            InitializeComponent();
        }

        private void SkillDescribes_Load(object sender, EventArgs e)
        {
            skillnamelabel.Text = Skillname;
            skilldeslabel.Text = Skilldescription;
        }
    }
}
