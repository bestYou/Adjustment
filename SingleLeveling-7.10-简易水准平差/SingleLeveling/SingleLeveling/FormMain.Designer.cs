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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox_main
            // 
            this.pictureBox_main.InitialImage = global::SingleLeveling.Properties.Resources._20080122_f0ed7b443d4e87785dcaAJ9wGEaAu1O4;
            this.pictureBox_main.Location = new System.Drawing.Point(296, 98);
            this.pictureBox_main.Name = "pictureBox_main";
            this.pictureBox_main.Size = new System.Drawing.Size(478, 359);
            this.pictureBox_main.TabIndex = 0;
            this.pictureBox_main.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.平差类型ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1116, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 平差类型ToolStripMenuItem
            // 
            this.平差类型ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.水准网ToolStripMenuItem,
            this.平面网ToolStripMenuItem});
            this.平差类型ToolStripMenuItem.Name = "平差类型ToolStripMenuItem";
            this.平差类型ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.平差类型ToolStripMenuItem.Text = "平差类型";
            // 
            // 水准网ToolStripMenuItem
            // 
            this.水准网ToolStripMenuItem.Name = "水准网ToolStripMenuItem";
            this.水准网ToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.水准网ToolStripMenuItem.Text = "水准网";
            // 
            // 平面网ToolStripMenuItem
            // 
            this.平面网ToolStripMenuItem.Name = "平面网ToolStripMenuItem";
            this.平面网ToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.平面网ToolStripMenuItem.Text = "平面网";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1116, 643);
            this.Controls.Add(this.pictureBox_main);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "FormMain";
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
    }
}