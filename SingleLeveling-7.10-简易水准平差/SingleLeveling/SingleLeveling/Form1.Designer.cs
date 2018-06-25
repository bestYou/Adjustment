namespace SingleLeveling
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rTextBox_output = new System.Windows.Forms.RichTextBox();
            this.rTextBox_input = new System.Windows.Forms.RichTextBox();
            this.button_jisuan = new System.Windows.Forms.Button();
            this.button_ceshi = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button_jisuan);
            this.splitContainer1.Panel1.Controls.Add(this.rTextBox_input);
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.button_ceshi);
            this.splitContainer1.Panel2.Controls.Add(this.rTextBox_output);
            this.splitContainer1.Size = new System.Drawing.Size(883, 533);
            this.splitContainer1.SplitterDistance = 426;
            this.splitContainer1.TabIndex = 0;
            // 
            // rTextBox_output
            // 
            this.rTextBox_output.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rTextBox_output.Location = new System.Drawing.Point(41, 25);
            this.rTextBox_output.Name = "rTextBox_output";
            this.rTextBox_output.Size = new System.Drawing.Size(380, 404);
            this.rTextBox_output.TabIndex = 1;
            this.rTextBox_output.Text = "";
            this.rTextBox_output.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // rTextBox_input
            // 
            this.rTextBox_input.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rTextBox_input.Location = new System.Drawing.Point(27, 25);
            this.rTextBox_input.Name = "rTextBox_input";
            this.rTextBox_input.Size = new System.Drawing.Size(371, 404);
            this.rTextBox_input.TabIndex = 2;
            this.rTextBox_input.Text = "";
            this.rTextBox_input.TextChanged += new System.EventHandler(this.richTextBox2_TextChanged);
            // 
            // button_jisuan
            // 
            this.button_jisuan.Location = new System.Drawing.Point(313, 463);
            this.button_jisuan.Name = "button_jisuan";
            this.button_jisuan.Size = new System.Drawing.Size(85, 37);
            this.button_jisuan.TabIndex = 3;
            this.button_jisuan.Text = "计算";
            this.button_jisuan.UseVisualStyleBackColor = true;
            this.button_jisuan.Click += new System.EventHandler(this.button_jisuan_Click);
            // 
            // button_ceshi
            // 
            this.button_ceshi.Location = new System.Drawing.Point(41, 463);
            this.button_ceshi.Name = "button_ceshi";
            this.button_ceshi.Size = new System.Drawing.Size(85, 37);
            this.button_ceshi.TabIndex = 4;
            this.button_ceshi.Text = "测试";
            this.button_ceshi.UseVisualStyleBackColor = true;
            this.button_ceshi.Click += new System.EventHandler(this.button_ceshi_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 533);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "单一水准计算";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox rTextBox_output;
        private System.Windows.Forms.Button button_jisuan;
        private System.Windows.Forms.RichTextBox rTextBox_input;
        private System.Windows.Forms.Button button_ceshi;
    }
}

