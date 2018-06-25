namespace SingleLeveling
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox_main = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.平差类型ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.水准网ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.平面网ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gPS网ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label_mainTitle = new System.Windows.Forms.Label();
            this.labeltop = new System.Windows.Forms.Label();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.作者信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox_main
            // 
            this.pictureBox_main.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox_main.Image = global::SingleLeveling.Properties.Resources._20080122_f0ed7b443d4e87785dcaAJ9wGEaAu1O4;
            this.pictureBox_main.InitialImage = global::SingleLeveling.Properties.Resources._20080122_f0ed7b443d4e87785dcaAJ9wGEaAu1O4;
            this.pictureBox_main.Location = new System.Drawing.Point(377, 193);
            this.pictureBox_main.Name = "pictureBox_main";
            this.pictureBox_main.Size = new System.Drawing.Size(256, 256);
            this.pictureBox_main.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_main.TabIndex = 0;
            this.pictureBox_main.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.平差类型ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1031, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出XToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.文件ToolStripMenuItem.Text = "文件 (&F)";
            // 
            // 平差类型ToolStripMenuItem
            // 
            this.平差类型ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.水准网ToolStripMenuItem,
            this.平面网ToolStripMenuItem,
            this.gPS网ToolStripMenuItem});
            this.平差类型ToolStripMenuItem.Name = "平差类型ToolStripMenuItem";
            this.平差类型ToolStripMenuItem.Size = new System.Drawing.Size(104, 24);
            this.平差类型ToolStripMenuItem.Text = "平差类型 (&T)";
            // 
            // 水准网ToolStripMenuItem
            // 
            this.水准网ToolStripMenuItem.Name = "水准网ToolStripMenuItem";
            this.水准网ToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.水准网ToolStripMenuItem.Text = "水准网";
            this.水准网ToolStripMenuItem.Click += new System.EventHandler(this.水准网ToolStripMenuItem_Click);
            // 
            // 平面网ToolStripMenuItem
            // 
            this.平面网ToolStripMenuItem.Name = "平面网ToolStripMenuItem";
            this.平面网ToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.平面网ToolStripMenuItem.Text = "平面网";
            this.平面网ToolStripMenuItem.Click += new System.EventHandler(this.平面网ToolStripMenuItem_Click);
            // 
            // gPS网ToolStripMenuItem
            // 
            this.gPS网ToolStripMenuItem.Name = "gPS网ToolStripMenuItem";
            this.gPS网ToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.gPS网ToolStripMenuItem.Text = "GPS网";
            this.gPS网ToolStripMenuItem.Click += new System.EventHandler(this.gPS网ToolStripMenuItem_Click);
            // 
            // label_mainTitle
            // 
            this.label_mainTitle.AutoSize = true;
            this.label_mainTitle.Font = new System.Drawing.Font("苏新诗卵石体", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_mainTitle.Location = new System.Drawing.Point(266, 80);
            this.label_mainTitle.Name = "label_mainTitle";
            this.label_mainTitle.Size = new System.Drawing.Size(509, 80);
            this.label_mainTitle.TabIndex = 2;
            this.label_mainTitle.Text = "平差小王子";
            this.label_mainTitle.Click += new System.EventHandler(this.label1_Click);
            // 
            // labeltop
            // 
            this.labeltop.AutoSize = true;
            this.labeltop.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labeltop.Location = new System.Drawing.Point(324, 517);
            this.labeltop.Name = "labeltop";
            this.labeltop.Size = new System.Drawing.Size(393, 20);
            this.labeltop.TabIndex = 3;
            this.labeltop.Text = "版权所有 2016-2017 @平差小王子编组委";
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于系统ToolStripMenuItem,
            this.作者信息ToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(77, 24);
            this.帮助ToolStripMenuItem.Text = "帮助 (&H)";
            // 
            // 关于系统ToolStripMenuItem
            // 
            this.关于系统ToolStripMenuItem.Name = "关于系统ToolStripMenuItem";
            this.关于系统ToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.关于系统ToolStripMenuItem.Text = "关于系统 (&A)";
            this.关于系统ToolStripMenuItem.Click += new System.EventHandler(this.关于系统ToolStripMenuItem_Click);
            // 
            // 作者信息ToolStripMenuItem
            // 
            this.作者信息ToolStripMenuItem.Name = "作者信息ToolStripMenuItem";
            this.作者信息ToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.作者信息ToolStripMenuItem.Text = "作者信息";
            // 
            // 退出XToolStripMenuItem
            // 
            this.退出XToolStripMenuItem.Name = "退出XToolStripMenuItem";
            this.退出XToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.退出XToolStripMenuItem.Text = "退出 (&X)";
            this.退出XToolStripMenuItem.Click += new System.EventHandler(this.退出XToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1031, 649);
            this.Controls.Add(this.labeltop);
            this.Controls.Add(this.label_mainTitle);
            this.Controls.Add(this.pictureBox_main);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "测量平差课程设计";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_main;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 平差类型ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 水准网ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 平面网ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gPS网ToolStripMenuItem;
        private System.Windows.Forms.Label label_mainTitle;
        private System.Windows.Forms.Label labeltop;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于系统ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 作者信息ToolStripMenuItem;
    }
}