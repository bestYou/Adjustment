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
    struct NumGps
    {
        public static int knowCoor;//记录已知点的个数
        public static int unKnowCoor;//记录待定点的个数
        public static int inputCoor;//记录待定点输入的个数
        public static int gpsRoute;//记录基线条数     同时用作线路ID
        public static int resultCoor;//记录计算后得到的坐标点数量
        public static int finallyCoor;//记录平差后得到的坐标平差值
    }
    struct PointKnowCoor    ////////////结构体，用于记录已知点的坐标
    {
        //public int id;
        public string name;
        public double x;
        public double y;
        public double z;
    }
    struct PointUnKnowCoor         /////////用于记录计算待定点的信息
    {
        public string startPoint;//起点
        public string endPoint;//终点
        //public int id;
        public double dx;   //待定点的坐标增量
        public double dy;
        public double dz;

        public double row1_col1;   //待定点的基线方差阵
        public double row1_col2;    //第一行
        public double row1_col3;

        public double row2_col1;   //待定点的基线方差阵
        public double row2_col2;    //第二行
        public double row2_col3;

        public double row3_col1;   //待定点的基线方差阵
        public double row3_col2;    //第三行
        public double row3_col3;
    }
    struct PointResultCoor    ////////////结构体，用于记录计算后得到的高程点，是为了得到待定点的近似高程值
    {
        //public int id;  //列立方程时 用于标记方程的参数，还要记录线路名称，确立B矩阵中参数位置

        public string name;
        public double dx;   //待定点的坐标增量
        public double dy;
        public double dz;

        //public string startPoint; //起点   根据起点和终点确定线路，
        //public string endPoint; //终点
    }
    struct PointNameId1
    {
        public int id;  //列立方程时 用于标记方程的参数，还要记录线路名称，确立B矩阵中参数位置
        public string name;
    }
    struct PointNameIdCoor
    {
        public int id;  //列立方程时 用于标记方程的参数，还要记录线路名称，确立B矩阵中参数位置
        public string name;
        public double h;    //近似高程值
        public int hCount;  //待定点出现的次数，用于求待定点的高程平均值
    }
    struct PointFinallyCoor
    {
        public double x;   //存储最后平差得到的坐标平差值    x = 观测高差(Matrix hObservition) + 改正数（V矩阵）
        public double y;
        public double z;
    }


    public partial class FormGPS : Form
    {
        public FormGPS()
        {
            InitializeComponent();
        }

        private void FormGPS_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("将要关闭窗体，是否继续？", "温馨提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                e.Cancel = false;
                FormMain formMain = new FormMain();
                formMain.Show();
            }
            else {
                e.Cancel = true;
            }
        }


        ArrayList pointlistKnow = new ArrayList();//存已知点的坐标
        ArrayList pointlistUnKnow = new ArrayList();//存输入的待定点的坐标
        ArrayList pointlistResultCoor = new ArrayList();//存计算后得到的待定点的坐标信息
        ArrayList pointlistDaiDing = new ArrayList();//存待定点的点名，包含重复的名字，去重后，再存入pointlistNameId中
        ArrayList pointlistNameId = new ArrayList();//存储用于计算B矩阵的点名和对点名进行编号，号码即为在B矩阵中的列数
        ArrayList pointlistNameIdCoor = new ArrayList();//存储用于计算B矩阵的点名和点号，而且添加了近似高程值，用于l矩阵的计算
        ArrayList pointlistFinallyCoor = new ArrayList();//存平差后得到的坐标平差值
        ArrayList pointlistSigma2 = new ArrayList();//存基线方差阵


        private void rTBox_input_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_jiSuan_Click(object sender, EventArgs e)
        {
            string[] sLines = rTBox_input.Lines; //richTextBox中的数据以string数组的形式存在sLines中，每行为一个元素

            if (rTBox_input.Lines.Length < 2)   //至少一行是起算点，一行是待定点信息，共两行
            {
                MessageBox.Show("输入的数据不完整！", "温馨提示");
                return;
            }


            NumGps.knowCoor = 0;
            NumGps.unKnowCoor = 0;
            NumGps.gpsRoute = 0;
            NumGps.resultCoor = 0;
            NumGps.finallyCoor = 0;



            try
            {
                //读取已知数据和观测数据
                /*
                 * @先从richTextBox中读取已知点信息，存入pointlistKnow
                 * 
                 * @再继续读取待定点信息，存入pointlistUnKnow
                 * 
                 */
                //数据格式为：
                /*
                 第一部分：已知点点名，x, y, z
                 第二部分：起点，终点，dx, dy, dz , row1_col1, row1_col2, row1_col3 , row2_col1, row2_col2, row2_col3 , row3_col1, row3_col2, row3_col3
                            也就是：起点，终点，坐标增量，基线方差阵
                 */
                #region 读取 已知数据 和 观测数据
                //Input Know Point Height
                for (int i = 0; i < rTBox_input.Lines.Length; i++)
                {
                    string[] knowPoint = sLines[i].Split(',');
                    PointKnowCoor pointKnowCoor;
                    if (knowPoint.Length == 4)
                    {
                        NumGps.knowCoor++;   //已知点数量增加1
                        pointKnowCoor.name = knowPoint[0];
                        pointKnowCoor.x = double.Parse(knowPoint[1]);
                        pointKnowCoor.y = double.Parse(knowPoint[2]);
                        pointKnowCoor.z = double.Parse(knowPoint[3]);
                        pointlistKnow.Add(pointKnowCoor);
                    }
                    else
                        break;
                }

                //Input unKnow Point Height
                for (int i = NumGps.knowCoor; i < rTBox_input.Lines.Length; i++)
                {
                    string[] unKnowPoint = sLines[i].Split(',');
                    PointUnKnowCoor pointUnKnowCoor;
                    if (unKnowPoint.Length == 14)
                    {
                        NumGps.gpsRoute++;   //基线条数增加1     理解为：每读取一行数据，观测值 +1，也就是B矩阵的行数 + （1 * 3），
                        pointUnKnowCoor.startPoint = unKnowPoint[0];
                        pointUnKnowCoor.endPoint = unKnowPoint[1];
                        pointUnKnowCoor.dx = double.Parse(unKnowPoint[2]);
                        pointUnKnowCoor.dy = double.Parse(unKnowPoint[3]);
                        pointUnKnowCoor.dz = double.Parse(unKnowPoint[4]);

                        pointUnKnowCoor.row1_col1 = double.Parse(unKnowPoint[5]);
                        pointUnKnowCoor.row1_col2 = double.Parse(unKnowPoint[6]);
                        pointUnKnowCoor.row1_col3 = double.Parse(unKnowPoint[7]);

                        pointUnKnowCoor.row2_col1 = double.Parse(unKnowPoint[8]);
                        pointUnKnowCoor.row2_col2 = double.Parse(unKnowPoint[9]);
                        pointUnKnowCoor.row2_col3 = double.Parse(unKnowPoint[10]);

                        pointUnKnowCoor.row3_col1 = double.Parse(unKnowPoint[11]);
                        pointUnKnowCoor.row3_col2 = double.Parse(unKnowPoint[12]);
                        pointUnKnowCoor.row3_col3 = double.Parse(unKnowPoint[13]);

                        pointlistUnKnow.Add(pointUnKnowCoor);
                    }
                    else
                    {
                        MessageBox.Show("请检查数据格式是否正确", "温馨提示");
                        return;
                    }
                }

                /*
                 * 
                 * 筛选待定点
                 * 
                 */
                //第一步，把所有待定点的起点、终点点名 ，去掉已知点点名后存到pointlistDaiDing中，然后，再去重
                for (int i = 0; i < NumGps.gpsRoute; i++)
                {
                    PointUnKnowCoor pointUnKnowCoor_read = (PointUnKnowCoor)pointlistUnKnow[i];
                    for (int j = 0; j < NumGps.knowCoor; j++)
                    {
                        PointKnowCoor pointKnowCoor_read = (PointKnowCoor)pointlistKnow[j];
                        if (pointKnowCoor_read.name != pointUnKnowCoor_read.startPoint && pointKnowCoor_read.name != pointUnKnowCoor_read.endPoint)
                        {   //起点终点均为待定点
                            pointlistDaiDing.Add(pointUnKnowCoor_read.startPoint);
                            pointlistDaiDing.Add(pointUnKnowCoor_read.endPoint);
                        }
                        else if (pointKnowCoor_read.name != pointUnKnowCoor_read.startPoint && pointKnowCoor_read.name == pointUnKnowCoor_read.endPoint)
                        {   //起点为待定点
                            pointlistDaiDing.Add(pointUnKnowCoor_read.startPoint);
                        }
                        else if (pointKnowCoor_read.name == pointUnKnowCoor_read.startPoint && pointKnowCoor_read.name != pointUnKnowCoor_read.endPoint)
                        {   //终点为待定点
                            pointlistDaiDing.Add(pointUnKnowCoor_read.endPoint);
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
                NumGps.unKnowCoor = pointlistDaiDing.Count;   //去重后的待定点数量即为 实际的 待定点数量，也就是  B矩阵的列数 = 待定点数量 * 3

                double[,] arrayB = new double[NumGps.gpsRoute * 3, NumGps.unKnowCoor * 3];
                for(int i =0;i<NumGps.gpsRoute*3;i++){      //为数组赋初值为0
                    for(int j=0;j<NumGps.unKnowCoor*3;j++){
                        arrayB[i,j] = 0;
                    }
                }

                //取点名，对点名进行编号
                for (int i = 0; i < pointlistDaiDing.Count; i++)
                {
                    PointNameId1 pointNameId1;
                    pointNameId1.id = i;
                    pointNameId1.name = (string)pointlistDaiDing[i];     //此处用法待考证：能直接用string进行强制类型转换？
                    pointlistNameId.Add(pointNameId1);       //去重后的数组，保留了点名，点名对应的id，也就是B列数，一个未知点，对应三列
                }

                /*
                    思路： 每条基线对应着连续的三行可以在基线循环时，h1 -> arrayB[3 * i + 0,  3*(待定点id) + 0 ]
                                                                     h1 -> arrayB[3 * i + 1,  3*(待定点id) + 1 ]
                                                                     h1 -> arrayB[3 * i + 2,  3*(待定点id) + 2 ]
                */
                //判断 x^  y^  z^ 系数
                for (int i = 0; i < NumGps.gpsRoute; i++)     //基线数量的循环
                {
                    PointUnKnowCoor pointUnKnowCoor_read = (PointUnKnowCoor)pointlistUnKnow[i];
                    
                    //如果起点和终点都是已知点，那么，不执行操作，因为arrayB在一开始已经赋值为0；只需改变参数位置的1和-1即可
                    for(int j=0;j<NumGps.knowCoor;j++){     
                        PointKnowCoor pointKnowCoor_read = (PointKnowCoor)pointlistKnow[j];
                        if(pointKnowCoor_read.name == pointUnKnowCoor_read.startPoint){
                            //起点和已知点相同，不执行操作
                        }
                        if(pointKnowCoor_read.name == pointUnKnowCoor_read.endPoint){
                            //终点和已知点相同，不执行操作
                        }
                    }

                    //进行待定点的判断
                    for(int j=0;j<NumGps.unKnowCoor;j++){       //NumGps.unKnowCoor存储的是待定点的真实数量
                        PointNameId1 PointNameId1_read = (PointNameId1)pointlistNameId[j];      //取出存在pointlistNameId中的点位信息，包括 点名（name） 和 第几个待定点（ID）， 而参数位置还有个*3的关系
                        if(PointNameId1_read.name == pointUnKnowCoor_read.startPoint /*&& PointNameId1_read.name != pointUnKnowCoor_read.endPoint*/){
                            //在起点，系数为-1                                             又加了个判断，是在起点为待定点而且终点不为待定点
                            arrayB[i * 3 + 0, j * 3 + 0] = -1;  //对应着三行三列的分块矩阵
                            arrayB[i * 3 + 1, j * 3 + 1] = -1;
                            arrayB[i * 3 + 2, j * 3 + 2] = -1;
                        }
                        else if(PointNameId1_read.name == pointUnKnowCoor_read.endPoint /*&& PointNameId1_read.name != pointUnKnowCoor_read.startPoint*/){
                            //在终点，系数为1                                             又加了个判断，是在终点为待定点而且起点不为待定点
                            arrayB[i * 3 + 0, j * 3 + 0] = 1;  //对应着三行三列的分块矩阵
                            arrayB[i * 3 + 1, j * 3 + 1] = 1;
                            arrayB[i * 3 + 2, j * 3 + 2] = 1;
                        }
                        else{
                            //起点和终点都是待定点时，那么系数同时出现 -1 和 1
                           /* for (int k = 0; k < NumGps.unKnowCoor; k++) {
                                PointNameId1 PointNameId1_read2 = (PointNameId1)pointlistNameId[k];
                                if (PointNameId1_read.name == pointUnKnowCoor_read.startPoint && PointNameId1_read2.name == pointUnKnowCoor_read.endPoint)
                                {
                                    arrayB[i * 3 + 0, j * 3 + 0] = -1;  //对应着三行三列的分块矩阵
                                    arrayB[i * 3 + 1, j * 3 + 1] = -1;  //起点
                                    arrayB[i * 3 + 2, j * 3 + 2] = -1;

                                                                  
                                    arrayB[i * 3 + 0, j * 3 + 0] = 1;  //对应着三行三列的分块矩阵
                                    arrayB[i * 3 + 1, j * 3 + 1] = 1;  //终点
                                    arrayB[i * 3 + 2, j * 3 + 2] = 1;
                                }
                            */              //刚刚想到了这种情况，又给排除了。  出现两个待定点时，一个在起点，一个在终点，   在起点时，if语句做了判断，在终点时，else if语句做了判断。   列数由待定点位置（j）判断， 当起点为待定点时，终点也为待定点。这样矛盾吗？  二者，同时出现，j为同一个数，那么，执行完毕后就只剩了 1。有问题
                            
                            }
                        }
                    }
                    //if()    //已知坐标，直接为  0         需要判断是否为已知点，需要代到pointKnow中遍历

                    //if()    //为待定点，在终点，为  1     先判断是否为未知点，然后，再判断是在起点还是终点

                    //if()    //为待定点，在起点，为 -1

                #endregion


                //推算待定点坐标近似值
                /*
                * @先由已知点坐标推算待定点坐标
                * 
                * @再由生成的结果继续进行。
                *      思路为：
                *          让 PointUnKnowCoor 与 PointResultCoor 中的点位高程信息遍历，计算其余待定高程点
                */
                #region     推算待定点坐标近似值
                for (int i = 0; i < NumGps.knowCoor; i++)
                {
                    PointKnowCoor pointKnowCoor_read = (PointKnowCoor)pointlistKnow[i];
                    for (int j = 0; j < NumGps.gpsRoute; j++)
                    {
                        PointUnKnowCoor pointUnKnowCoor_read = (PointUnKnowCoor)pointlistUnKnow[j];
                        if (pointKnowCoor_read.name == pointUnKnowCoor_read.startPoint)
                        {
                            PointResultCoor pointResultCoor;
                            pointResultCoor.name = pointUnKnowCoor_read.endPoint;
                            pointResultCoor.dx = pointKnowCoor_read.x + pointUnKnowCoor_read.dx;
                            pointResultCoor.dy = pointKnowCoor_read.y + pointUnKnowCoor_read.dy;
                            pointResultCoor.dz = pointKnowCoor_read.z + pointUnKnowCoor_read.dz;
                            pointlistResultCoor.Add(pointResultCoor);
                            NumGps.resultCoor++;
                        }
                        if (pointKnowCoor_read.name == pointUnKnowCoor_read.endPoint)
                        {
                            PointResultCoor pointResultCoor;
                            pointResultCoor.name = pointUnKnowCoor_read.endPoint;
                            pointResultCoor.dx = pointKnowCoor_read.x - pointUnKnowCoor_read.dx;
                            pointResultCoor.dy = pointKnowCoor_read.y - pointUnKnowCoor_read.dy;
                            pointResultCoor.dz = pointKnowCoor_read.z - pointUnKnowCoor_read.dz;
                            pointlistResultCoor.Add(pointResultCoor);
                            NumGps.resultCoor++;
                        }
                    }
                }


                //与已知点进行计算具有唯一性，再进行pointResult与pointUnKnowH比对时，可能会出现一次遍历不能达到求出所有待定高程点的近似高程值情况，需要判断 pointResult数量与pointUnKnownH的数量关系，数量相等才能结束循环
                while (NumGps.unKnowCoor != NumGps.resultCoor)
                {

                    int resultCoorshort = 0;   //临时存储pointResultH增加的数量，循环结束后再添加到Num.resultH中
                    for (int i = 0; i < NumGps.unKnowCoor; i++)     //循环条件确定，关键是遍历PointUnKnowH，再匹配PointResultH结果
                    {
                        PointUnKnowCoor pointUnKnowCoor_read = (PointUnKnowCoor)pointlistUnKnow[i];

                        for (int j = 0; j < NumGps.resultCoor; j++)
                        {
                            PointResultCoor pointResultCoor_read = (PointResultCoor)pointlistResultCoor[i];
                            if (pointResultCoor_read.name == pointUnKnowCoor_read.startPoint)
                            {
                                PointResultCoor pointResultCoor;
                                pointResultCoor.name = pointUnKnowCoor_read.endPoint;
                                pointResultCoor.dx = pointResultCoor_read.dx+ pointUnKnowCoor_read.dx;
                                pointResultCoor.dy = pointResultCoor_read.dy + pointUnKnowCoor_read.dy;
                                pointResultCoor.dz = pointResultCoor_read.dz + pointUnKnowCoor_read.dz;
                                pointlistResultCoor.Add(pointResultCoor);
                                resultCoorshort++;
                            }
                            if (pointResultCoor_read.name == pointUnKnowCoor_read.endPoint)
                            {
                                PointResultCoor pointResultCoor;
                                pointResultCoor.name = pointUnKnowCoor_read.startPoint;
                                pointResultCoor.dx = pointResultCoor_read.dx - pointUnKnowCoor_read.dx;
                                pointResultCoor.dy = pointResultCoor_read.dy - pointUnKnowCoor_read.dy;
                                pointResultCoor.dz = pointResultCoor_read.dz - pointUnKnowCoor_read.dz;
                                pointlistResultCoor.Add(pointResultCoor);
                                resultCoorshort++;
                            }
                        }
                    }
                    NumGps.resultCoor += resultCoorshort;
                }

                //待定点近似坐标去重
                //存在多条路线得到同一个待定点的近似坐标情况，需要处理重复的待定点近似坐标      可以考虑 1.直接去重 和 2. 取重复点高程的平均值
                //如果直接去重，pointResult中有name和h两个字段，有些麻烦
                //采用平均值方式去重
                /*
                 实现：
                    遍历pointResult，与pointNameId中的点名比对，如果有重合，那么把值存到另一个pointNameIdH中，记录存储该点时存H的次数，取平均值
                 
                 
                 */
                //算了，研究了下去重算法，还是去重吧。。。
                for (int i = 0; i < pointlistResultCoor.Count; i++)
                {
                    PointResultCoor pointResultCoor_read = (PointResultCoor)pointlistResultCoor[i];
                    for (int j = i + 1; j < pointlistResultCoor.Count; j++)
                    {
                        PointResultCoor pointResultCoor_read2 = (PointResultCoor)pointlistResultCoor[j];
                        if (pointResultCoor_read.name == pointResultCoor_read2.name)
                        {
                            pointlistResultCoor.RemoveAt(j);
                            if (i > 0)
                            {
                                i--;
                            }
                        }
                    }
                }   //得到了待定点近似坐标   于是，要开始矩阵l的构造 
                #endregion

                //构造l矩阵的数据
                double[,] arrayl = new double[NumGps.gpsRoute*3, 1];   // l =    dXij                -   ( Xj^0 - Xi^0 )
                //                                                            在pointlistKnow中         pointlistResult中  终点近似坐标 - 起点近似坐标
                for (int i = 0; i < NumGps.gpsRoute; i++)   //先确定基线是哪条
                {          //          在pointResult中或是在pointKnow中
                    PointUnKnowCoor pointUnKnowCoor_read = (PointUnKnowCoor)pointlistUnKnow[i];
                    for (int j = 0; j < NumGps.knowCoor; j++)
                    {
                        PointKnowCoor pointKnowCoor_read = (PointKnowCoor)pointlistKnow[j];
                        for (int k = 0; k < pointlistResultCoor.Count; k++)
                        {
                            PointResultCoor pointResultCoor_read = (PointResultCoor)pointlistResultCoor[k];
                            if (pointUnKnowCoor_read.endPoint == pointKnowCoor_read.name && pointUnKnowCoor_read.startPoint == pointResultCoor_read.name) {

                                arrayl[i * 3 + 0, 0] = pointUnKnowCoor_read.dx - (pointKnowCoor_read.x - pointResultCoor_read.dx);  //对应着三行一列的分块矩阵
                                arrayl[i * 3 + 1, 0] = pointUnKnowCoor_read.dy - (pointKnowCoor_read.y - pointResultCoor_read.dy);
                                arrayl[i * 3 + 2, 0] = pointUnKnowCoor_read.dz - (pointKnowCoor_read.z - pointResultCoor_read.dz);                            
                            }
                            if (pointUnKnowCoor_read.startPoint == pointKnowCoor_read.name && pointUnKnowCoor_read.endPoint == pointResultCoor_read.name)
                            {
                                arrayl[i * 3 + 0, 0] = pointUnKnowCoor_read.dx - (pointResultCoor_read.dx - pointKnowCoor_read.x);  //对应着三行一列的分块矩阵
                                arrayl[i * 3 + 1, 0] = pointUnKnowCoor_read.dy - (pointResultCoor_read.dy - pointKnowCoor_read.y);
                                arrayl[i * 3 + 2, 0] = pointUnKnowCoor_read.dz - (pointResultCoor_read.dz - pointKnowCoor_read.z); 
                            }
                            for (int l = 0; l < pointlistResultCoor.Count; l++)
                            {
                                PointResultCoor pointResultCoor_read2 = (PointResultCoor)pointlistResultCoor[l];
                                if (pointUnKnowCoor_read.endPoint == pointResultCoor_read.name && pointUnKnowCoor_read.startPoint == pointResultCoor_read2.name)
                                {
                                    arrayl[i * 3 + 0, 0] = pointUnKnowCoor_read.dx - (pointResultCoor_read.dx - pointResultCoor_read2.dx);//对应着三行一列的分块矩阵
                                    arrayl[i * 3 + 1, 0] = pointUnKnowCoor_read.dy - (pointResultCoor_read.dy - pointResultCoor_read2.dy);
                                    arrayl[i * 3 + 2, 0] = pointUnKnowCoor_read.dz - (pointResultCoor_read.dz - pointResultCoor_read2.dz);
                                }
                            }
                        }
                    }
                }

                //构造P矩阵的数据
                double[,] arrayP = new double[NumGps.gpsRoute*3, NumGps.unKnowCoor*3];   // P = 1 / sigma^2
                //先把P矩阵赋初值为0
                for (int i = 0; i < NumGps.gpsRoute * 3; i++) {
                    for (int j = 0; j < NumGps.unKnowCoor * 3;j++ ) {
                        arrayP[i,j] = 0 ;
                    }
                }
                    //把基线方差阵构造出来，
                    //从pointUnKnow中取出数据，转换为Matrix类型，因为在计算P矩阵时，用的是基线方差阵的逆矩阵，所以在存到pointlistSigma2中时，使用Matrix类的Inverse（）方法进行转换
                    for (int i = 0; i < NumGps.gpsRoute; i++)
                    {
                        PointUnKnowCoor pointUnKnowCoor_read = (PointUnKnowCoor)pointlistUnKnow[i];
                        double[,] sigma2 = new double[3, 3];

                        sigma2[0, 0] = pointUnKnowCoor_read.row1_col1;
                        sigma2[0, 1] = pointUnKnowCoor_read.row1_col2;
                        sigma2[0, 2] = pointUnKnowCoor_read.row1_col3;

                        sigma2[1, 0] = pointUnKnowCoor_read.row2_col1;
                        sigma2[1, 1] = pointUnKnowCoor_read.row2_col2;
                        sigma2[1, 2] = pointUnKnowCoor_read.row2_col3;

                        sigma2[2, 0] = pointUnKnowCoor_read.row3_col1;
                        sigma2[2, 1] = pointUnKnowCoor_read.row3_col2;
                        sigma2[2, 2] = pointUnKnowCoor_read.row3_col3;

                        Matrix mtxSigma2 = new Matrix(sigma2);
                        pointlistSigma2.Add(mtxSigma2.Inverse());

                    }
                for (int i = 0; i < NumGps.gpsRoute;i++ ) {
                    Matrix mtxSigma2 = (Matrix)pointlistSigma2[i];  //一条基线对应一个基线方差阵

                        arrayP[i * 3 + 0, i * 3 + 0] = mtxSigma2[0, 0];  //P矩阵的第一行
                        arrayP[i * 3 + 0, i * 3 + 1] = mtxSigma2[0, 1];     
                        arrayP[i * 3 + 0, i * 3 + 2] = mtxSigma2[0, 2];     

                        arrayP[i * 3 + 1, i * 3 + 0] = mtxSigma2[1, 0];  //P矩阵的第二行
                        arrayP[i * 3 + 1, i * 3 + 1] = mtxSigma2[1, 1];     
                        arrayP[i * 3 + 1, i * 3 + 2] = mtxSigma2[1, 2];     

                        arrayP[i * 3 + 2, i * 3 + 0] = mtxSigma2[2, 0];  //P矩阵的第三行
                        arrayP[i * 3 + 2, i * 3 + 1] = mtxSigma2[2, 1];     
                        arrayP[i * 3 + 2, i * 3 + 2] = mtxSigma2[2, 2];     
                  
                }






                //列观测值误差方程
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

                // pointlistResultCoor 存储了待定点的近似坐标值

                //坐标增量观测值数组
                double [,] coorObservation = new double[NumGps.gpsRoute,3];
                for (int i = 0; i < NumGps.gpsRoute; i++)
                {
                    PointUnKnowCoor pointUnKnowCoor_read = (PointUnKnowCoor)pointlistUnKnow[i];
                    coorObservation[i, 0] = pointUnKnowCoor_read.dx;
                    coorObservation[i, 1] = pointUnKnowCoor_read.dy;
                    coorObservation[i, 2] = pointUnKnowCoor_read.dz;
                }

                //平差后的坐标增量数组
                double[,] coorLastPing = new double[NumGps.gpsRoute, 3];
                for (int i = 0; i < NumGps.gpsRoute; i++)
                {
                    for (int j = 0; j < 3;j++ ) {
                        coorLastPing[i, j] = 0;     //先为平差后的坐标增量数组赋初值为0
                    }
                }

                Matrix matCoorObservation = new Matrix(coorObservation);
                Matrix matCoorLastPing = new Matrix(coorLastPing);


                //V矩阵需要处理下，由原来的{ xi, yi, zi, xj, yj, zj, xk, yk, zk}变为 { {xi,yi,zi}, {xj,yj,zj}, {xk,yk,zk} } 
                double[,] arrayV = new double[NumGps.gpsRoute,3];
                for (int i = 0; i < NumGps.gpsRoute;i++ ) {
                    arrayV[i, 0]  = V[i * 3 + 0, 0];
                    arrayV[i, 1]  = V[i * 3 + 1, 0];
                    arrayV[i, 2]  = V[i * 3 + 2, 0];
                }

                //平差后的坐标增量 = 观测得到的坐标增量 + 间接平差得到的坐标增量改正数
                matCoorLastPing = matCoorObservation + V;

                //计算平差后的坐标
                #region     推算待定点平差后的坐标
                for (int i = 0; i < NumGps.knowCoor; i++)
                {
                    PointKnowCoor pointKnowCoor_read = (PointKnowCoor)pointlistKnow[i];
                    for (int j = 0; j < NumGps.gpsRoute; j++)
                    {
                        PointUnKnowCoor pointUnKnowCoor_read = (PointUnKnowCoor)pointlistUnKnow[j];
                        if (pointKnowCoor_read.name == pointUnKnowCoor_read.startPoint)
                        {
                            PointResultCoor pointResultCoor;
                            pointResultCoor.name = pointUnKnowCoor_read.endPoint;
                            pointResultCoor.dx   = pointKnowCoor_read.x + matCoorLastPing[j, 0]/*pointUnKnowCoor_read.dx*/;
                            pointResultCoor.dy   = pointKnowCoor_read.y + matCoorLastPing[j, 1];
                            pointResultCoor.dz   = pointKnowCoor_read.z + matCoorLastPing[j, 2];
                            pointlistFinallyCoor.Add(pointResultCoor);
                            NumGps.finallyCoor++;
                        }
                        if (pointKnowCoor_read.name == pointUnKnowCoor_read.endPoint)
                        {
                            PointResultCoor pointResultCoor;
                            pointResultCoor.name = pointUnKnowCoor_read.startPoint;
                            pointResultCoor.dx   = pointKnowCoor_read.x - matCoorLastPing[j, 0];
                            pointResultCoor.dy   = pointKnowCoor_read.y - matCoorLastPing[j, 1];
                            pointResultCoor.dz   = pointKnowCoor_read.z - matCoorLastPing[j, 2];
                            pointlistFinallyCoor.Add(pointResultCoor);
                            NumGps.finallyCoor++;
                        }
                    }
                }


                //与已知点进行计算具有唯一性，再进行pointFinally与pointUnKnowCoor比对时，可能会出现一次遍历不能达到求出所有待定点的坐标值情况，需要判断 pointFinally数量与pointUnKnownCoor的数量关系，数量相等才能结束循环
                while (NumGps.unKnowCoor != NumGps.finallyCoor)
                {

                    int finallyCoorshort = 0;   //临时存储pointFinallyH增加的数量，循环结束后再添加到NumGps.finallyCoor中
                    for (int i = 0; i < NumGps.gpsRoute; i++)     //循环条件确定，关键是遍历PointUnKnowH，再匹配PointResultH结果
                    {
                        PointUnKnowCoor pointUnKnowCoor_read = (PointUnKnowCoor)pointlistUnKnow[i];

                        for (int j = 0; j < NumGps.finallyCoor; j++)
                        {
                            PointResultCoor pointResultCoor_read = (PointResultCoor)pointlistFinallyCoor[i];
                            if (pointResultCoor_read.name == pointUnKnowCoor_read.startPoint)
                            {
                                PointResultCoor pointResultCoor;
                                pointResultCoor.name = pointUnKnowCoor_read.endPoint;
                                pointResultCoor.dx = pointResultCoor_read.dx + matCoorLastPing[i, 0];
                                pointResultCoor.dy = pointResultCoor_read.dy + matCoorLastPing[i, 1];
                                pointResultCoor.dz = pointResultCoor_read.dz + matCoorLastPing[i, 2];
                                pointlistResultCoor.Add(pointResultCoor);
                                finallyCoorshort++;
                            }
                            if (pointResultCoor_read.name == pointUnKnowCoor_read.endPoint)
                            {
                                PointResultCoor pointResultCoor;
                                pointResultCoor.name = pointUnKnowCoor_read.startPoint;
                                pointResultCoor.dx = pointResultCoor_read.dx - matCoorLastPing[i, 0];
                                pointResultCoor.dy = pointResultCoor_read.dy - matCoorLastPing[i, 1];
                                pointResultCoor.dz = pointResultCoor_read.dz - matCoorLastPing[i, 2];
                                pointlistResultCoor.Add(pointResultCoor);
                                finallyCoorshort++;
                            }
                        }
                    }
                    NumGps.finallyCoor += finallyCoorshort;
                }

                //待定点近似坐标去重
                //存在多条路线得到同一个待定点的近似坐标情况，需要处理重复的待定点近似坐标      可以考虑 1.直接去重 和 2. 取重复点高程的平均值
                //如果直接去重，pointResult中有name和h两个字段，有些麻烦
                //采用平均值方式去重
                /*
                 实现：
                    遍历pointResult，与pointNameId中的点名比对，如果有重合，那么把值存到另一个pointNameIdH中，记录存储该点时存H的次数，取平均值
                 
                 
                 */
                //算了，研究了下去重算法，还是去重吧。。。
                for (int i = 0; i < pointlistFinallyCoor.Count; i++)
                {
                    PointResultCoor pointResultCoor_read = (PointResultCoor)pointlistFinallyCoor[i];
                    for (int j = i + 1; j < pointlistFinallyCoor.Count; j++)
                    {
                        PointResultCoor pointResultCoor_read2 = (PointResultCoor)pointlistFinallyCoor[j];
                        if (pointResultCoor_read.name == pointResultCoor_read2.name)
                        {
                            pointlistFinallyCoor.RemoveAt(j);
                            if (i > 0)
                            {
                                i--;
                            }
                        }
                    }
                }   //得到平差后的待定点坐标值
                #endregion


                //输出结果
                rTBox_output.Text = "点名   " + "坐标 (m) " + System.Environment.NewLine;
                //输出调整后未知点的高程
                for (int i = 0; i < pointlistFinallyCoor.Count; i++)
                {
                    PointResultCoor pointResultCoor_read = (PointResultCoor)pointlistFinallyCoor[i];
                    rTBox_output.AppendText(pointResultCoor_read.name + "   " + pointResultCoor_read.dx + "   " + pointResultCoor_read.dy + "   " + pointResultCoor_read.dz + System.Environment.NewLine);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "请检查数据格式是否正确", "温馨提示");//显示异常信息
            }










































        }
    }
}
