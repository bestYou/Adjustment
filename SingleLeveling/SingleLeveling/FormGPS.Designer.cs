namespace SingleLeveling
{
    partial class FormGPS
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
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.rTBox_input = new System.Windows.Forms.RichTextBox();
            this.rTBox_output = new System.Windows.Forms.RichTextBox();
            this.button_jiSuan = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(657, 563);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // rTBox_input
            // 
            this.rTBox_input.Location = new System.Drawing.Point(12, 12);
            this.rTBox_input.Name = "rTBox_input";
            this.rTBox_input.Size = new System.Drawing.Size(636, 490);
            this.rTBox_input.TabIndex = 1;
            this.rTBox_input.Text = "";
            this.rTBox_input.TextChanged += new System.EventHandler(this.rTBox_input_TextChanged);
            // 
            // rTBox_output
            // 
            this.rTBox_output.Location = new System.Drawing.Point(663, 12);
            this.rTBox_output.Name = "rTBox_output";
            this.rTBox_output.Size = new System.Drawing.Size(396, 490);
            this.rTBox_output.TabIndex = 2;
            this.rTBox_output.Tag = "";
            this.rTBox_output.Text = "结果显示区域";
            // 
            // button_jiSuan
            // 
            this.button_jiSuan.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_jiSuan.Location = new System.Drawing.Point(497, 508);
            this.button_jiSuan.Name = "button_jiSuan";
            this.button_jiSuan.Size = new System.Drawing.Size(105, 44);
            this.button_jiSuan.TabIndex = 3;
            this.button_jiSuan.Text = "计算";
            this.button_jiSuan.UseVisualStyleBackColor = true;
            this.button_jiSuan.Click += new System.EventHandler(this.button_jiSuan_Click);
            // 
            // button_save
            // 
            this.button_save.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_save.Location = new System.Drawing.Point(743, 508);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(105, 44);
            this.button_save.TabIndex = 4;
            this.button_save.Text = "保存";
            this.button_save.UseVisualStyleBackColor = true;
            // 
            // FormGPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 563);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.button_jiSuan);
            this.Controls.Add(this.rTBox_output);
            this.Controls.Add(this.rTBox_input);
            this.Controls.Add(this.splitter1);
            this.Name = "FormGPS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GPS网间接平差";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGPS_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.RichTextBox rTBox_input;
        private System.Windows.Forms.RichTextBox rTBox_output;
        private System.Windows.Forms.Button button_jiSuan;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;

    }
}