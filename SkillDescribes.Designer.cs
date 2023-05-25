namespace Yave
{
    partial class SkillDescribes
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.skillnamelabel = new System.Windows.Forms.Label();
            this.skilldeslabel = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // skillnamelabel
            // 
            this.skillnamelabel.AutoSize = true;
            this.skillnamelabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.skillnamelabel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.skillnamelabel.Location = new System.Drawing.Point(0, 0);
            this.skillnamelabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.skillnamelabel.Name = "skillnamelabel";
            this.skillnamelabel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.skillnamelabel.Size = new System.Drawing.Size(78, 27);
            this.skillnamelabel.TabIndex = 0;
            this.skillnamelabel.Text = "技能名称";
            // 
            // skilldeslabel
            // 
            this.skilldeslabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skilldeslabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skilldeslabel.Location = new System.Drawing.Point(0, 27);
            this.skilldeslabel.Multiline = true;
            this.skilldeslabel.Name = "skilldeslabel";
            this.skilldeslabel.ReadOnly = true;
            this.skilldeslabel.Size = new System.Drawing.Size(246, 106);
            this.skilldeslabel.TabIndex = 1;
            this.skilldeslabel.Text = "技能介绍";
            // 
            // SkillDescribes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.skilldeslabel);
            this.Controls.Add(this.skillnamelabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SkillDescribes";
            this.Size = new System.Drawing.Size(246, 133);
            this.Load += new System.EventHandler(this.SkillDescribes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label skillnamelabel;
        private System.Windows.Forms.TextBox skilldeslabel;
    }
}
