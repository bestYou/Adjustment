using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SingleLeveling
{
    /// <summary>
    /// 线性方程，即Bx+V=0
    /// </summary>
    public class Indirect
    {
        public Matrix B
        {
            get;
            set;
        }
        public Matrix l
        {
            get;
            set;
        }
        public Matrix P
        {
            get;
            set;
        }
        
        /// <summary>
        /// x的维数
        /// </summary>
        public int N
        {
            get
            {
                return B.Rows;
            }
        }

        /// <summary>
        /// 初始化方程
        /// </summary>
        /// <param name="B">系数矩阵</param>
        /// <param name="l">观测值</param>
        /// <param name="P">权阵，若为null，则初始化一个E</param>
        public Indirect(Matrix B, Matrix l, Matrix P = null)
        {
            if (B.Rows != l.Rows)
            {
                throw new Exception("B和V的行数不相等");
            }
            else
            {
                this.B = B;
                this.l = l;
                if (P == null)
                {
                    double[,] E = new double[B.Rows, B.Rows];
                    for (int i = 0; i < B.Rows; i++)
                    {
                        for (int j = 0; j < B.Rows; j++)
                        {
                            if (i == j)
                            {
                                E[i, j] = 1;
                            }
                        }
                    }
                    this.P = new Matrix(E);
                }
                else
                {
                    this.P = P;
                }
            }
        }

        #region 求解方程
        public Matrix GetN()
        {
            return this.B.Transpose() * this.P * this.B;
        }
        public Matrix GetW()
        {
            return this.B.Transpose() * this.P * this.l;
        }
        public Matrix Getx()
        {
            return this.GetN().Inverse() * this.GetW();
        }
        public Matrix GetV()
        {
            return this.B * this.Getx() - this.l;
        }
        public double GetSigma()
        {
            return Math.Sqrt((GetV().Transpose() * P * GetV())[0, 0] / (B.Rows - B.Cols));
        }
        public Matrix GetQvv()
        {
            return P.Inverse() - B * GetN().Inverse() * B.Transpose();
        }
        #endregion
       
        
        public override string ToString()
        {
            Matrix V = this.GetV();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.N; i++)
            {
                for (int j = 0; j < B.Cols; j++)
                    sb.Append(B[i, j] + " ");
                for (int j = 0; j < V.Cols; j++)
                {
                    //是否要加空格
                    string flag = "";
                    if (j != V.Cols - 1) flag = "  ";
                    sb.Append(V[i, j] + flag);
                }
                //判断是否换行
                string lineflag = "";
                if (i != this.N - 1) lineflag = Environment.NewLine;
                sb.Append(lineflag);
            }
            return sb.ToString();
        }
    }
}