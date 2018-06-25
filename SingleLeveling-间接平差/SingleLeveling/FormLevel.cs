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
        public static int knowH;//记录已知高程点的个数
        public static int unKnowH;//记录待定高程点的个数
        public static int input;//记录待定高程点输入的个数
        public static int levelRoute;//记录水准路线条数     同时用作线路ID
        public static int resultH;//记录计算后得到的高程点数量
        public static int finallyH;//记录平差后得到的高程平差值
    }
    struct PointKnowH    ////////////结构体，用于记录已知高程点
    {
        //public int id;
        public string name;
        public double h;
    }
    struct PointUnKnowH         /////////用于记录计算待定高程点的信息
    {
        public string startPoint;//后视点，起点
        public string endPoint;//前视点，终点
        //public int id;
        public double dh;//高差
        public double ds;//距离
        //public int levelRoute;//线路ID
    }
    struct PointResultH    ////////////结构体，用于记录计算后得到的高程点，是为了得到待定点的近似高程值
    {
        //public int id;  //列立方程时 用于标记方程的参数，还要记录线路名称，确立B矩阵中参数位置

        public string name;
        public double h;

        //public string startPoint; //起点   根据起点和终点确定线路，
        //public string endPoint; //终点
    }
    struct PointNameId {
        public int id;  //列立方程时 用于标记方程的参数，还要记录线路名称，确立B矩阵中参数位置
        public string name;
    }
    struct PointNameIdH
    {
        public int id;  //列立方程时 用于标记方程的参数，还要记录线路名称，确立B矩阵中参数位置
        public string name;
        public double h;    //近似高程值
        public int hCount;  //待定点出现的次数，用于求待定点的高程平均值
    }
    struct PointFinally{
        public double h;    //存储最后平差得到的高差平差值    h = 观测高差(Matrix hObservition) + 改正数（V矩阵）
    }

    public partial class FormLevel : Form
    {
        public FormLevel()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        ArrayList pointlistKnow = new ArrayList();//存已知点的高程
        ArrayList pointlistUnKnow = new ArrayList();//存输入的待定高程点的信息
        ArrayList pointlistResultH = new ArrayList();//存计算后得到的高程点的信息
        ArrayList pointlistDaiDing = new ArrayList();//存待定点的点名，包含重复的名字，去重后，再存入pointlistNameId中
        ArrayList pointlistNameId = new ArrayList();//存储用于计算B矩阵的点名和对点名进行编号，号码即为在B矩阵中的列数
        ArrayList pointlistNameIdH = new ArrayList();//存储用于计算B矩阵的点名和点号，而且添加了近似高程值，用于l矩阵的计算
        ArrayList pointlistFinallyH = new ArrayList();//存平差后得到的高程平差值

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        int iObsCount ;     //= rTextBox_input.Lines.Length - 3;//观测值个数

        private void button_jisuan_Click(object sender, EventArgs e)
        {
            string[] sLines = rTextBox_input.Lines; //richTextBox中的数据以string数组的形式存在sLines中，每行为一个元素

            if (rTextBox_input.Lines.Length < 2 )   //至少一行是起算点，一行是待定高程点，共两行
            {
                MessageBox.Show("输入的数据不完整！", "温馨提示");
                return;
            }

            Num.knowH = 0;
            Num.unKnowH = 0;
            Num.levelRoute = 0;
            Num.resultH = 0;
            Num.finallyH = 0;
              
            try
            {
                //读取已知数据和观测数据
                /*
                 * @先从richTextBox中读取已知点高程，存入pointlistKnow
                 * 
                 * @再继续读取待定点高程，存入pointlistUnKnow
                 * 
                 */
                #region 读取 已知数据 和 观测数据
                //Input Know Point Height
                for (int i = 0; i < rTextBox_input.Lines.Length; i++) {
                    string[] knowPoint = sLines[i].Split(',');
                    PointKnowH pointKnowH;
                    if (knowPoint.Length == 2)
                    {
                        Num.knowH++;   //已知高程点数量增加1
                        pointKnowH.name = knowPoint[0];
                        pointKnowH.h = double.Parse(knowPoint[1]);
                        pointlistKnow.Add(pointKnowH);
                    }
                    else
                        break;
                }
                  
                //Input unKnow Point Height
                for (int i = Num.knowH; i < rTextBox_input.Lines.Length ; i++)
                {
                    string[] unKnowPoint = sLines[i].Split(',');
                    PointUnKnowH pointUnKnowH;
                    if (unKnowPoint.Length == 4)
                    {
                        //Num.unKnowH++;   //待定高程点数量增加1     处理：判断重复点，去重
                        Num.levelRoute++;   //水准路线条数增加1     理解为：每读取一行数据，观测值 +1，也就是B矩阵的行数 +1，
                        pointUnKnowH.startPoint = unKnowPoint[0];
                        pointUnKnowH.endPoint = unKnowPoint[1];
                        pointUnKnowH.dh = double.Parse(unKnowPoint[2]);
                        pointUnKnowH.ds = double.Parse(unKnowPoint[3]);
                        pointlistUnKnow.Add(pointUnKnowH);
                    }
                    else {
                        MessageBox.Show("请检查数据格式是否正确", "温馨提示");
                        return;
                    }
                }

                /*
                 * 
                 * 筛选待定高程点
                 * 
                 */
                //第一步，把所有待定高程点的起点、终点点名 ，去掉已知点点名后存到pointlistDaiDing中，然后，再去重
                for (int i = 0; i < Num.levelRoute; i++) {      
                    PointUnKnowH pointUnKnowH_read = (PointUnKnowH)pointlistUnKnow[i];
                    for (int j = 0; j < Num.knowH; j++) {
                        PointKnowH pointKnowH_read = (PointKnowH)pointlistKnow[j];
                        if (pointKnowH_read.name != pointUnKnowH_read.startPoint && pointKnowH_read.name != pointUnKnowH_read.endPoint)
                        {   //起点终点均为待定点
                            pointlistDaiDing.Add(pointUnKnowH_read.startPoint);
                            pointlistDaiDing.Add(pointUnKnowH_read.endPoint);
                        }
                        else if (pointKnowH_read.name != pointUnKnowH_read.startPoint && pointKnowH_read.name == pointUnKnowH_read.endPoint)
                        {   //起点为待定点
                            pointlistDaiDing.Add(pointUnKnowH_read.startPoint);
                        }
                        else if (pointKnowH_read.name == pointUnKnowH_read.startPoint && pointKnowH_read.name != pointUnKnowH_read.endPoint)
                        {   //终点为待定点
                            pointlistDaiDing.Add(pointUnKnowH_read.endPoint);
                        }
                        else
                            continue;   //假如两个点都是已知点的话，继续执行 几乎没有这种情况吧？
                    }
                }
                //删除重复数据
                for (int i = 0; i < pointlistDaiDing.Count; i++)
                {
                    for (int j = i + 1; j < pointlistDaiDing.Count; j++)
                    {
                        if (pointlistDaiDing[i].Equals(pointlistDaiDing[j]))
                        {
                            pointlistDaiDing.RemoveAt(j);
                            if (i > 0)
                            {
                                i--;
                            }
                        }
                    }
                }
                Num.unKnowH = pointlistDaiDing.Count;   //去重后的待定高程点数量即为 待定点数量，也就是B矩阵的列数

                double [,] arrayB = new double[Num.levelRoute,Num.unKnowH];

                //取点名，对点名进行编号
                for (int i = 0; i < pointlistDaiDing.Count; i++) {
                    PointNameId pointNameId;
                    pointNameId.id = i;
                    pointNameId.name = (string)pointlistDaiDing[i];     //此处用法待考证：能直接用string进行强制类型转换？
                    pointlistNameId.Add(pointNameId);
                }

                //判断x^系数
                for (int i = 0; i < Num.levelRoute;i++ )
                {
                    PointUnKnowH pointUnKnowH_read = (PointUnKnowH)pointlistUnKnow[i];
                    for (int j = 0; j < Num.unKnowH; j++) {
                        PointNameId pointNameId_read = (PointNameId)pointlistNameId[j];
                        if (pointNameId_read.name == pointUnKnowH_read.startPoint) {
                            arrayB[i, j] = -1;
                        }
                        else if (pointNameId_read.name == pointUnKnowH_read.endPoint)
                        {
                            arrayB[i, j] = 1;
                        }
                        else {
                            arrayB[i, j] = 0;
                        }
                    }
                }
                #endregion


                //推算待定点高程近似值
                /*
                * @先由已知点高程推算待定点高程
                * 
                * @再由生成的结果继续进行。
                *      思路为：
                *          让 PointUnKnowH 与 PointResultH 中的点位高程信息遍历，计算其余待定高程点
                */
                #region     推算待定点高程近似值
                for (int i = 0; i < Num.knowH; i++)
                {
                    PointKnowH pointKnowH_read = (PointKnowH)pointlistKnow[i];
                    for (int j = 0; j < Num.unKnowH; j++)
                    {
                        PointUnKnowH pointUnKnowH_read = (PointUnKnowH)pointlistUnKnow[j];
                        if (pointKnowH_read.name == pointUnKnowH_read.startPoint)
                        {
                            double Hresult = pointKnowH_read.h + pointUnKnowH_read.dh;
                            PointResultH pointResultH;
                            pointResultH.name = pointUnKnowH_read.endPoint;
                            pointResultH.h = Hresult;
                            pointlistResultH.Add(pointResultH);     //推算待定点的近似高程值是不是要考虑 去重呢？    尝试使用平均值来达到去重效果
                            Num.resultH++;
                        }
                        if (pointKnowH_read.name == pointUnKnowH_read.endPoint)
                        {
                            double Hresult = pointKnowH_read.h - pointUnKnowH_read.dh;
                            PointResultH pointResultH;
                            pointResultH.name = pointUnKnowH_read.startPoint;
                            pointResultH.h = Hresult;
                            pointlistResultH.Add(pointResultH);
                            Num.resultH++;
                        }
                    }
                }


                //与已知点进行计算具有唯一性，再进行pointResult与pointUnKnowH比对时，可能会出现一次遍历不能达到求出所有待定高程点的近似高程值情况，需要判断 pointResult数量与pointUnKnownH的数量关系，数量相等才能结束循环
                while(Num.unKnowH!=Num.resultH){

                    int resultHshort = 0;   //临时存储pointResultH增加的数量，循环结束后再添加到Num.resultH中
                    for (int i = 0; i < Num.unKnowH; i++)     //循环条件确定，关键是遍历PointUnKnowH，再匹配PointResultH结果
                    {
                        PointUnKnowH pointUnKnowH_read = (PointUnKnowH)pointlistUnKnow[i];      

                        for (int j = 0; j < Num.resultH; j++)
                        {
                            PointResultH pointResultH_read = (PointResultH)pointlistResultH[i];
                            if (pointResultH_read.name == pointUnKnowH_read.startPoint)
                            {
                                double Hresult = pointResultH_read.h + pointUnKnowH_read.dh;
                                PointResultH pointResultH;
                                pointResultH.name = pointUnKnowH_read.endPoint;
                                pointResultH.h = Hresult;
                                pointlistResultH.Add(pointResultH);
                                resultHshort++;
                            }
                            if (pointResultH_read.name == pointUnKnowH_read.endPoint)
                            {
                                double Hresult = pointResultH_read.h - pointUnKnowH_read.dh;
                                PointResultH pointResultH;
                                pointResultH.name = pointUnKnowH_read.startPoint;
                                pointResultH.h = Hresult;
                                pointlistResultH.Add(pointResultH);
                                resultHshort++;
                            }
                        }
                    }
                    Num.resultH += resultHshort;
                
                }

                //待定点近似高程值去重
                //存在多条路线得到同一个待定点的近似高程情况，需要处理重复的待定点近似高程      可以考虑 1.直接去重 和 2. 取重复点高程的平均值
                //如果直接去重，pointResult中有name和h两个字段，有些麻烦
                //采用平均值方式去重
                /*
                 实现：
                    遍历pointResult，与pointNameId中的点名比对，如果有重合，那么把值存到另一个pointNameIdH中，记录存储该点时存H的次数，取平均值
                 
                 
                 */
                 //算了，研究了下去重算法，还是去重吧。。。
                for (int i = 0; i < pointlistResultH.Count; i++)
                {
                    PointResultH pointResultH_read = (PointResultH)pointlistResultH[i];
                    for (int j = i + 1; j < pointlistResultH.Count; j++)
                    {
                        PointResultH pointResultH_read2 = (PointResultH)pointlistResultH[j];
                        if (pointResultH_read.name == pointResultH_read2.name)
                        {
                            pointlistResultH.RemoveAt(j);
                            if (i > 0)
                            {
                                i--;
                            }
                        }
                    }
                }   //终于，待定点近似高程值得到了。   于是，要开始矩阵l的构造 
                #endregion

                //构造l矩阵的数据
                double[,] arrayl = new double[Num.levelRoute, 1];   // l = dh - endPointH + startPointH;
                for (int i = 0; i < Num.levelRoute; i++) {          //          在pointResult中或是在pointKnow中
                    PointUnKnowH pointUnKnowH_read = (PointUnKnowH)pointlistUnKnow[i];
                    for(int j =0;j<Num.knowH;j++){
                        PointKnowH pointKnowH_read = (PointKnowH)pointlistKnow[j];
                        for (int k = 0; k < pointlistResultH.Count; k++) {
                            PointResultH pointResultH_read = (PointResultH)pointlistResultH[k];
                            if (pointUnKnowH_read.endPoint == pointKnowH_read.name && pointUnKnowH_read.startPoint == pointResultH_read.name)
                            {
                                arrayl[i, 0] = pointUnKnowH_read.dh - pointKnowH_read.h + pointResultH_read.h;
                            }
                            if (pointUnKnowH_read.startPoint == pointKnowH_read.name && pointUnKnowH_read.endPoint == pointResultH_read.name)
                            {
                                arrayl[i, 0] = pointUnKnowH_read.dh - pointResultH_read.h + pointKnowH_read.h;
                            }
                            for (int l = 0; l < pointlistResultH.Count; l++) {
                                PointResultH pointResultH_read2 = (PointResultH)pointlistResultH[l];
                                if (pointUnKnowH_read.endPoint == pointResultH_read.name && pointUnKnowH_read.startPoint == pointResultH_read2.name)
                                {
                                    arrayl[i, 0] = pointUnKnowH_read.dh - pointResultH_read.h + pointResultH_read2.h;
                                }
                            }
                        }
                    }
                }

                //构造P矩阵的数据
                double[,] arrayP = new double[Num.levelRoute, Num.levelRoute];   // P = 1 / S[i]
                for (int i = 0; i < Num.levelRoute; i++ ) {
                    PointUnKnowH pointUnKnowH_read = (PointUnKnowH)pointlistUnKnow[i];
                    arrayP[i, i] = 1 / pointUnKnowH_read.ds;
                }






                    //列高差观测值误差方程
                    /*
                     * @原理：
                     *      每个待定高程点对应着一个误差方程
                     *      在间接平差中，待定高程点的数量对应着误差方程的数量
                     * 
                     * 
                     * @实现：
                     *      思路为：
                     *      
                     *          Num.resultH 记录了待定高程点高程近似值 的数量，
                     *                      这个，也就是误差方程的个数了
                     *          
                     *          PointResultH 记录了计算后得到的 待定高程点高程近似值信息，
                     *              包括：点名（name）、高程（h）
                     *        
                     * @ Matrix B:      记录误差方程系数阵
                     *                  如果有参数，也就是有待定点，有两种情况：
                     *                      在起点，那么其系数为 -1
                     *                      在终点，那么其系数为 1
                     *                  如果没有参数，那么其系数为 0
                     * 
                     * @ Matrix l:      记录自由项阵l，其值全部为常数
                     *                  公式为：v1 = x^ - ( h1 - X1^0 +Ha)
                     *                          l = hi - Xi^0 + H0
                     *                          l = 两站间高差 - 终点高程 + 起点高程
                     *                          
                     * debug：思路有问题，
                     *      按陶老师讲的，
                     *          观测值个数，对应着B矩阵的行数
                     *          待定点的个数，对应着B矩阵的列数
                     *                          
                     */
                Matrix matrixB = new Matrix(arrayB);
                Matrix matrixP = new Matrix(arrayP);
                Matrix matrixl = new Matrix(arrayl);

                //Indirect indirect = new Indirect(matrixB,matrixl,matrixP);

                Matrix Nbb = matrixB.Transpose() * matrixP * matrixB;
                Matrix W = matrixB.Transpose() * matrixP * matrixl;
                Matrix x = Nbb.Inverse() * W;
                Matrix V = matrixB * x - matrixl;

                //高差观测值数组
                double[,] hObservation = new double[Num.levelRoute,1];
                for(int i=0;i<Num.levelRoute;i++){
                    PointUnKnowH pointUnKnowH_read = (PointUnKnowH)pointlistUnKnow[i];
                    hObservation[i,0] = pointUnKnowH_read.dh;
                }

                //平差后的高差数组
                double[,] hLastPing = new double[Num.levelRoute,1];
                for(int i=0;i<Num.levelRoute;i++){
                    hLastPing[i,0] = 0;     //先为平差后的高差数组赋初值为0
                }

                Matrix matHObservation = new Matrix(hObservation);
                Matrix matHLastPing = new Matrix(hLastPing);

                matHLastPing = matHObservation + V;



                double a = matHLastPing[1,0];


                #region     推算平差后的高程平差值
                for (int i = 0; i < Num.knowH; i++)
                {
                    PointKnowH pointKnowH_read = (PointKnowH)pointlistKnow[i];
                    for (int j = 0; j < Num.unKnowH; j++)
                    {
                        PointUnKnowH pointUnKnowH_read = (PointUnKnowH)pointlistUnKnow[j];
                        if (pointKnowH_read.name == pointUnKnowH_read.startPoint)
                        {
                            double Hresult = pointKnowH_read.h + matHLastPing[j,0];
                            PointResultH pointResultH;
                            pointResultH.name = pointUnKnowH_read.endPoint;
                            pointResultH.h = Hresult;
                            pointlistFinallyH.Add(pointResultH);     //推算待定点的近似高程值是不是要考虑 去重呢？    尝试使用平均值来达到去重效果
                            Num.finallyH++;
                        }
                        if (pointKnowH_read.name == pointUnKnowH_read.endPoint)
                        {
                            double Hresult = pointKnowH_read.h - matHLastPing[j,0];
                            PointResultH pointResultH;
                            pointResultH.name = pointUnKnowH_read.startPoint;
                            pointResultH.h = Hresult;
                            pointlistFinallyH.Add(pointResultH);
                            Num.finallyH++;
                        }
                    }
                }


                //与已知点进行计算具有唯一性，再进行pointResult与pointUnKnowH比对时，可能会出现一次遍历不能达到求出所有待定高程点的近似高程值情况，需要判断 pointResult数量与pointUnKnownH的数量关系，数量相等才能结束循环
                while(Num.unKnowH!=Num.resultH){

                    int resultHshort = 0;   //临时存储pointResultH增加的数量，循环结束后再添加到Num.resultH中
                    for (int i = 0; i < Num.unKnowH; i++)     //循环条件确定，关键是遍历PointUnKnowH，再匹配PointResultH结果
                    {
                        PointUnKnowH pointUnKnowH_read = (PointUnKnowH)pointlistUnKnow[i];      

                        for (int j = 0; j < Num.resultH; j++)
                        {
                            PointResultH pointResultH_read = (PointResultH)pointlistResultH[j];
                            if (pointResultH_read.name == pointUnKnowH_read.startPoint)
                            {
                                double Hresult = pointResultH_read.h + matHLastPing[j,0];
                                PointResultH pointResultH;
                                pointResultH.name = pointUnKnowH_read.endPoint;
                                pointResultH.h = Hresult;
                                pointlistFinallyH.Add(pointResultH);
                                resultHshort++;
                            }
                            if (pointResultH_read.name == pointUnKnowH_read.endPoint)
                            {
                                double Hresult = pointResultH_read.h - matHLastPing[j,0];
                                PointResultH pointResultH;
                                pointResultH.name = pointUnKnowH_read.startPoint;
                                pointResultH.h = Hresult;
                                pointlistFinallyH.Add(pointResultH);
                                resultHshort++;
                            }
                        }
                    }
                    Num.finallyH += resultHshort;
                
                }

                //高程平差值去重，得到高程平差值及其对应的点名
                for (int i = 0; i < pointlistFinallyH.Count; i++)
                {
                    PointResultH pointResultH_read = (PointResultH)pointlistFinallyH[i];
                    for (int j = i + 1; j < pointlistResultH.Count; j++)
                    {
                        PointResultH pointResultH_read2 = (PointResultH)pointlistFinallyH[j];
                        if (pointResultH_read.name == pointResultH_read2.name)
                        {
                            pointlistFinallyH.RemoveAt(j);
                            if (i > 0)
                            {
                                i--;
                            }
                        }
                    }
                }   //得到高程平差值 
                #endregion



	                //输出结果
                rTextBox_output.Text = "点名   " + "调整后H (m) " + System.Environment.NewLine;
                //输出调整后未知点的高程
                for (int i = 0; i < pointlistFinallyH.Count; i++)
                {
                    PointResultH pointResultH_read = (PointResultH)pointlistFinallyH[i];
                    rTextBox_output.AppendText(pointResultH_read.name + "   " + pointResultH_read.h + System.Environment.NewLine);
                }




                /*
                    rTextBox_output.Text = "闭合差：" + dFh*1000 + System.Environment.NewLine +  
                                           "水准路线总长度：" + dSumD + System.Environment.NewLine +
                                           "每公里高差改正数：" + dFh / dSumD * 1000 + System.Environment.NewLine +
                                           "序号   " + "调整后H (m) " + System.Environment.NewLine;

	                //输出调整后未知点的高程
	                for(int i=0;i<iObsCount-1;i++)
	                {
                        rTextBox_output.AppendText(i + 1 + "   " + dH[i] + System.Environment.NewLine);
	                }
                 */



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

        private void FormLevel_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MessageBox.Show("将要关闭窗体，是否继续？", "温馨提示", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                FormMain formMain = new FormMain();
                formMain.Show();
            }
        }

       

    }
}



/*
 Input:
 
已知点点号，已知点高程	
测站起点，测站终点，高差，距离

A,212.126
E,212.308
A,B,  -0.2696,    0.3571
B,C,   0.1137,    0.4976
C,D,   0.0760,    0.3535
D,E,   0.2662,    0.1901
 
 
 Output:
 
                 高差闭合差计算结果

线路号:1
线路点号:       E         D         C         B         A  

高差闭合差:       -4.3000      (MM)
总长度:                 1.3983       (KM)
 * 
 * 
                                                           高程控制网平差成果表
                                                           ━━━━━━━━━━ 

                    网名:水准测量高程控制网
 
                    等级:二等
 
                    测量单位:山东科技大学 测绘工程15-1
 
                    测量时间:2017.6.28
 
                    测量人员:尤超帅
 
                    仪器:
 
                    平差参考系:
 
                    平差类型:
 
                    高差观测值总数:4

                    多余观测数(自由度):1

                    先验每公里高程测量高差中误差:
 
                    后验每公里高程测量高差中误差:3.636

 
 

                                                 高差观测值平差成果表
                                                 --------------------
 
             ┏━━━━┯━━━━┯━━━━━━━┯━━━━┯━━━━━━━┯━━━━┯━━━━━┓
             ┃  起点  │ 终点   │   观测高差   │ 改正数 │    平差值    │  精度  │   距离   ┃
             ┃────┼────┼───────┼────┼───────┼────┼─────┨
             ┃  N1    │   N2   │    Dh(米)    │Vh(毫米)│    DH^(米)   │Mh(毫米)│  S(公里) ┃
             ┣━━━━┿━━━━┿━━━━━━━┿━━━━┿━━━━━━━┿━━━━┿━━━━━┫
             ┃      A │      B │    -0.2696   │ -1.10  │    -0.2707   │  1.88  │   0.357  ┃
             ┠────┼────┼───────┼────┼───────┼────┼─────┨
             ┃      B │      C │     0.1137   │ -1.53  │     0.1122   │  2.06  │   0.498  ┃
             ┠────┼────┼───────┼────┼───────┼────┼─────┨
             ┃      C │      D │     0.0760   │ -1.09  │     0.0749   │  1.87  │   0.353  ┃
             ┠────┼────┼───────┼────┼───────┼────┼─────┨
             ┃      D │      E │     0.2662   │ -0.58  │     0.2656   │  1.47  │   0.190  ┃
             ┗━━━━┷━━━━┷━━━━━━━┷━━━━┷━━━━━━━┷━━━━┷━━━━━┛


                                          高程平差值和精度成果表
                                          ----------------------
 
              ┏━━━━━━━━━┯━━━━┯━━━━━━━┯━━━━━┯━━━━━━━━━┓
              ┃      点 名       │ 点 号  │  高  程 (米) │精度(毫米)│     备  注       ┃
              ┣━━━━━━━━━┿━━━━┿━━━━━━━┿━━━━━┿━━━━━━━━━┫
              ┃              A   │      A │    212.1260  │   0.00   │                  ┃
              ┠─────────┼────┼───────┼─────┼─────────┨
              ┃              E   │      E │    212.3080  │   0.00   │                  ┃
              ┠─────────┼────┼───────┼─────┼─────────┨
              ┃              B   │      B │    211.8553  │   1.88   │                  ┃
              ┠─────────┼────┼───────┼─────┼─────────┨
              ┃              C   │      C │    211.9675  │   2.10   │                  ┃
              ┠─────────┼────┼───────┼─────┼─────────┨
              ┃              D   │      D │    212.0424  │   1.47   │                  ┃
              ┗━━━━━━━━━┷━━━━┷━━━━━━━┷━━━━━┷━━━━━━━━━┛


 
 
 
 
 */