using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SingleLeveling
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void 水准网ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //将formMain窗体设为父窗体
            this.IsMdiContainer = true;
            //隐藏父窗体上的标题文本
            this.label_mainTitle.Visible = false;
            //隐藏父窗体上的修饰图片
            this.pictureBox_main.Visible = false;
            //隐藏父窗体上的版权文本
            this.labeltop.Visible = false;

            FormLevel formLevel = new FormLevel();
            formLevel.MdiParent = this;
            formLevel.Show();

        }

        private void 平面网ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void gPS网ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //将formMain窗体设为父窗体
            this.IsMdiContainer = true;
            //隐藏父窗体上的标题文本
            this.label_mainTitle.Visible = false;
            //隐藏父窗体上的修饰图片
            this.pictureBox_main.Visible = false;
            //隐藏父窗体上的版权文本
            this.labeltop.Visible = false;

            FormGPS formGPS = new FormGPS();
            formGPS.MdiParent = this;
            formGPS.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void 关于系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //退出系统
            Application.Exit();
        }
    }
}
