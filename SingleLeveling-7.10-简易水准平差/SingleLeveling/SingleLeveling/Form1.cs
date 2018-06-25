using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;


namespace SingleLeveling
{

    struct Num
    {
        public static int yh;//记录已知高程点的个数
        public static int nh;//记录待定高程点的个数
        public static int inputNum;//记录待定高程点输入的个数
    }
    struct Point    ////////////结构体，用于记录已知高程点
    {
        //public int id;
        public string name;
        public double h;
    }
    struct Point1         /////////用于记录计算待定高程点的信息
    {
        public string startPoint;//后视点，起点
        public string endPoint;//前视点，终点
        public int id;
        public double dh;//高差
        public double ds;//距离
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        ArrayList pointlist = new ArrayList();//存已知点的高程
        ArrayList pointlist1 = new ArrayList();//存输入的待定高程点的信息

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        int nLevelType;     //单一水准类型,1=附合，0=闭合

        int iObsCount ;     //= rTextBox_input.Lines.Length - 3;//观测值个数
        double Ha, Hb;      //已知点高程
    //  double* hObs = new double[iObsCount];//高差观测值
    //  double* dObs = new double[iObsCount];//水准路线长度




        private void button_jisuan_Click(object sender, EventArgs e)
        {
            string[] sLines = rTextBox_input.Lines; //richTextBox中的数据以string数组的形式存在sLines中，每行为一个元素

            if (rTextBox_input.Lines.Length < 4 )
            {
                MessageBox.Show("输入的数据不完整！", "温馨提示");
                return;
            }

            Num.inputNum = 0;
            
            try{
                nLevelType = int.Parse(sLines[0]);
                if(nLevelType==1){      // 1 为附合水准
                    Num.yh = 2;
                    iObsCount = rTextBox_input.Lines.Length - 3;//观测值个数，rTextBox_input.Lines.Length表示的是richTextBox中的文本行数

                    //第二行数据
                    string[] a = sLines[1].Split(',');
                    Point p1;
                    p1.name = a[0];
                    p1.h = double.Parse(a[1].ToString());
                    pointlist.Add(p1);
                    Ha = double.Parse(a[1].ToString());

                    //第三行数据
                    string[] a1 = sLines[2].Split(',');
                    Point p2;
                    p2.name = a1[0];
                    p2.h = double.Parse(a1[1].ToString());
                    pointlist.Add(p2);
                    Hb = double.Parse(a1[1].ToString());

                    //逐行用Split函数分离，获取观测数据
                    for (int i = 0; i < iObsCount; i++)
                    {
                        //第四行数据
                        //观测数据，格式为：起点，终点，高差，距离
                        /*
1
A, 45.286
B, 49.579
A,1,2.331,1.6
1,2,2.813,2.1
2,3,-2.244,1.7
3,4,1.430,2.0                                            
*/
                        string[] a2 = sLines[i+3].Split(',');
                        Point1 p3;
                        p3.startPoint = a2[0];
                        p3.endPoint = a2[1];
                        p3.id = i;
                        p3.dh = double.Parse(a2[2].ToString());
                        p3.ds = double.Parse(a2[3].ToString());
                        pointlist1.Add(p3);
                        Num.inputNum++;     //输入的待定高程点数量增加1
                    }

                    double dFh=0;//高差闭合差
	                double dSumD=0;//路线总长度

	                for(int i=0;i<iObsCount;i++)
	                {
                        Point1 point1_read = (Point1)pointlist1[i];
		                dFh = dFh + point1_read.dh;    //计算高差观测值之和
		                dSumD = dSumD + point1_read.ds;//计算水准路线总长度
	                }

	                dFh = dFh - (Hb - Ha); //计算高差闭合差

	                for( int i=0;i<iObsCount;i++)//按路线长度分配闭合差
	                {
                        Point1 point1_read = (Point1)pointlist1[i];
                        point1_read.dh = point1_read.dh - dFh * point1_read.ds / dSumD;
	                }

	                double[] dH=new double[iObsCount-1];//未知点个数为nObsCount-1个

	                //推算未知点高程
                    Point1 point1_read1 = (Point1)pointlist1[0];
	                dH[0] = Ha + point1_read1.dh;
	                for(int i=0;i<iObsCount-2;i++)
	                {
                        Point1 point1_read = (Point1)pointlist1[i+1];
		                dH[i + 1] = dH[i] + point1_read.dh;
	                }

	                //输出结果
                    rTextBox_output.Text = "闭合差：" + dFh*1000 + System.Environment.NewLine +  
                                           "水准路线总长度：" + dSumD + System.Environment.NewLine +
                                           "每公里高差改正数：" + dFh / dSumD * 1000 + System.Environment.NewLine +
                                           "序号   " + "调整后H (m) " + System.Environment.NewLine;

	                //输出调整后未知点的高程
	                for(int i=0;i<iObsCount-1;i++)
	                {
                        rTextBox_output.AppendText(i + 1 + "   " + dH[i] + System.Environment.NewLine);
	                }


                }
                else if (nLevelType == 0)      // 0 为闭合水准
                {

                }
                else {
                    MessageBox.Show("请检查数据格式是否正确", "温馨提示");
                }
            }
            catch(Exception ex){
                    MessageBox.Show(ex.Message+"请检查数据格式是否正确","温馨提示");//显示异常信息
                }

            //rTextBox_output.Text = nLevelType.ToString() + System.Environment.NewLine+iObsCount.ToString();
        }

        private void button_ceshi_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //点名转换为点号
        /*
         * 方法名：getStationNumber()
         * 
         * 实现把 Sting 类型的 点名 转换为 Int 类型的 点号
         * 
         * 点号可直接代入数组使用
         * 
         */
        public int getStationNumber(string pointName) {




            return 0;
        }


    }
}



/*
 Input:
 
1
A, 45.286
B, 49.579
1,2.331,1.6
2,2.813,2.1
3,-2.244,1.7
4,1.430,2.0
 
 
 Output:
 
闭合差：37.0mm
水准路线总长度：7.4km
每公里高差改正数：5.0mm
序号   调整后H (m) 
1   47.6090
2   50.4115
3   48.1590
 
 
 
 
 */