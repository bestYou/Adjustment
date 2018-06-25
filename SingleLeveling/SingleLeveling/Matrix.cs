using System;
using System.Collections.Generic;

namespace SingleLeveling
{
    [Serializable]
    public class Matrix
    {
        public double[] element;
        private int rows = 0;
        private int cols = 0;
        /// <summary>
        /// 获取矩阵行数
        /// </summary>
        public int Rows
        {
            get
            {
                return rows;
            }
        }
        /// <summary>
        /// 获取矩阵列数
        /// </summary>
        public int Cols
        {
            get
            {
                return cols;
            }
        }
        /// <summary>
        /// 获取或设置第i行第j列的元素值
        /// </summary>
        /// <param name="i">第i行</param>
        /// <param name="j">第j列</param>
        /// <returns>返回第i行第j列的元素值</returns>
        public double this[int i, int j]
        {
            get
            {
                if (i < Rows && j < Cols)
                {
                    return element[i * cols + j];
                }
                else
                {
                    throw new Exception("索引越界");
                }
            }
            set
            {
                element[i * cols + j] = value;
            }
        }
        /// <summary>
        /// 用二维数组初始化Matrix
        /// </summary>
        /// <param name="m">二维数组</param>
        public Matrix(double[][] m)
        {
            this.rows = m.GetLength(0);
            this.cols = m.GetLength(1);
            int count = 0;
            this.element = new double[Rows * Cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    element[count++] = m[i][j];
                }
            }
        }
        public Matrix(double[,] m)
        {
            this.rows = m.GetLength(0);
            this.cols = m.GetLength(1);
            this.element = new double[this.rows * this.cols];
            int count = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    element[count++] = m[i, j];
                }
            }
        }
        public Matrix(List<List<double>> m)
        {
            this.rows = m.Count;
            this.cols = m[0].Count;
            this.element = new double[Rows * Cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    this[i, j] = m[i][j];
                }
            }
        }
        #region 矩阵数学运算
        public static Matrix MAbs(Matrix a)
        {
            Matrix _thisCopy = a.DeepCopy();
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    _thisCopy[i, j] = Math.Abs(a[i, j]);
                }
            }
            return _thisCopy;
        }
        /// <summary>
        /// 矩阵相加
        /// </summary>
        /// <param name="a">第一个矩阵,和b矩阵必须同等大小</param>
        /// <param name="b">第二个矩阵</param>
        /// <returns>返回矩阵相加后的结果</returns>
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.cols == b.cols && a.rows == b.rows)
            {
                double[,] res = new double[a.rows, a.cols];
                for (int i = 0; i < a.Rows; i++)
                {
                    for (int j = 0; j < a.Cols; j++)
                    {
                        res[i, j] = a[i, j] + b[i, j];
                    }
                }
                return new Matrix(res);
            }
            else
            {
                throw new Exception("两个矩阵行列不相等");
            }
        }
        /// <summary>
        /// 矩阵相减
        /// </summary>
        /// <param name="a">第一个矩阵,和b矩阵必须同等大小</param>
        /// <param name="b">第二个矩阵</param>
        /// <returns>返回矩阵相减后的结果</returns>
        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.cols == b.cols && a.rows == b.rows)
            {
                double[,] res = new double[a.rows, a.cols];
                for (int i = 0; i < a.Rows; i++)
                {
                    for (int j = 0; j < a.Cols; j++)
                    {
                        res[i, j] = a[i, j] - b[i, j];
                    }
                }
                return new Matrix(res);
            }
            else
            {
                throw new Exception("两个矩阵行列不相等");
            }
        }
        /// <summary>
        /// 对矩阵每个元素取相反数
        /// </summary>
        /// <param name="a">二维矩阵</param>
        /// <returns>得到矩阵的相反数</returns>
        public static Matrix operator -(Matrix a)
        {
            Matrix res = a;
            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < a.cols; j++)
                {
                    res.element[i * a.cols + j] = -res.element[i * a.cols + j];
                }
            }
            return res;
        }
        /// <summary>
        /// 矩阵相乘
        /// </summary>
        /// <param name="a">第一个矩阵</param>
        /// <param name="b">第二个矩阵,这个矩阵的行要与第一个矩阵的列相等</param>
        /// <returns>返回相乘后的一个新的矩阵</returns>
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.cols == b.rows)
            {
                double[,] res = new double[a.rows, b.cols];
                for (int i = 0; i < a.rows; i++)
                {
                    for (int j = 0; j < b.cols; j++)
                    {
                        for (int k = 0; k < a.cols; k++)
                        {
                            res[i, j] += a[i, k] * b[k, j];
                        }
                    }
                }
                return new Matrix(res);
            }
            else
            {
                throw new Exception("两个矩阵行和列不等");
            }
        }
        /// <summary>
        /// 矩阵与数相乘
        /// </summary>
        /// <param name="a">第一个矩阵</param>
        /// <param name="num">一个实数</param>
        /// <returns>返回相乘后的新的矩阵</returns>
        public static Matrix operator *(Matrix a, double num)
        {
            Matrix res = a;
            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < a.cols; j++)
                {
                    res.element[i * a.cols + j] *= num;
                }
            }
            return res;
        }
        /// <summary>
        /// 矩阵转置
        /// </summary>
        /// <returns>返回当前矩阵转置后的新矩阵</returns>
        public Matrix Transpose()
        {
            double[,] res = new double[cols, rows];
            {
                for (int i = 0; i < cols; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        res[i, j] = this[j, i];
                    }
                }
            }
            return new Matrix(res);
        }
        /// <summary>
        /// 矩阵求逆
        /// </summary>
        /// <returns>返回求逆后的新的矩阵</returns>
        public Matrix Inverse()
        {
            //最后原始矩阵并不变，所以需要深拷贝一份
            Matrix _thisCopy = this.DeepCopy();
            if (cols == rows && this.Determinant() != 0)
            {
                //初始化一个同等大小的单位阵
                Matrix res = _thisCopy.EMatrix();
                for (int i = 0; i < rows; i++)
                {
                    //首先找到第i列的绝对值最大的数，并将该行和第i行互换
                    int rowMax = i;
                    double max = Math.Abs(_thisCopy[i, i]);
                    for (int j = i; j < rows; j++)
                    {
                        if (Math.Abs(_thisCopy[j, i]) > max)
                        {
                            rowMax = j;
                            max = Math.Abs(_thisCopy[j, i]);
                        }
                    }
                    //将第i行和找到最大数那一行rowMax交换
                    if (rowMax != i)
                    {
                        _thisCopy.Exchange(i, rowMax);
                        res.Exchange(i, rowMax);

                    }
                    //将第i行做初等行变换，将第一个非0元素化为1
                    double r = 1.0 / _thisCopy[i, i];
                    _thisCopy.Exchange(i, -1, r);
                    res.Exchange(i, -1, r);
                    //消元
                    for (int j = 0; j < rows; j++)
                    {
                        //到本行后跳过
                        if (j == i)
                            continue;
                        else
                        {
                            r = -_thisCopy[j, i];
                            _thisCopy.Exchange(i, j, r);
                            res.Exchange(i, j, r);
                        }
                    }
                }
                return res;
            }
            else
            {
                throw new Exception("矩阵不是方阵无法求逆");
            }
        }
        #region 重载比较运算符
        public static bool operator <(Matrix a, Matrix b)
        {
            bool issmall = true;
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    if (a[i, j] >= b[i, j]) issmall = false;
                }
            }
            return issmall;
        }
        public static bool operator >(Matrix a, Matrix b)
        {
            bool issmall = true;
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    if (a[i, j] <= b[i, j]) issmall = false;
                }
            }
            return issmall;
        }
        public static bool operator <=(Matrix a, Matrix b)
        {
            bool issmall = true;
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    if (a[i, j] > b[i, j]) issmall = false;
                }
            }
            return issmall;
        }
        public static bool operator >=(Matrix a, Matrix b)
        {
            bool issmall = true;
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    if (a[i, j] < b[i, j]) issmall = false;
                }
            }
            return issmall;
        }
        public static bool operator !=(Matrix a, Matrix b)
        {
            bool issmall = true;
            issmall = ReferenceEquals(a, b);
            if (issmall) return issmall;
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    if (a[i, j] == b[i, j]) issmall = false;
                }
            }
            return issmall;
        }
        public static bool operator ==(Matrix a, Matrix b)
        {
            bool issmall = true;
            issmall = ReferenceEquals(a, b);
            if (issmall) return issmall;
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    if (a[i, j] != b[i, j]) issmall = false;
                }
            }
            return issmall;
        }
        public override bool Equals(object obj)
        {
            Matrix b = obj as Matrix;
            return this == b;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
        public double Determinant()
        {
            if (cols == rows)
            {
                Matrix _thisCopy = this.DeepCopy();
                //行列式每次交换行，都需要乘以-1
                double res = 1;
                for (int i = 0; i < rows; i++)
                {
                    //首先找到第i列的绝对值最大的数
                    int rowMax = i;
                    double max = Math.Abs(_thisCopy[i, i]);
                    for (int j = i; j < rows; j++)
                    {
                        if (Math.Abs(_thisCopy[j, i]) > max)
                        {
                            rowMax = j;
                            max = Math.Abs(_thisCopy[j, i]);
                        }
                    }
                    //将第i行和找到最大数那一行rowMax交换,同时将单位阵做相同初等变换
                    if (rowMax != i)
                    {
                        _thisCopy.Exchange(i, rowMax);
                        res *= -1;
                    }
                    //消元
                    for (int j = i + 1; j < rows; j++)
                    {
                        double r = -_thisCopy[j, i] / _thisCopy[i, i];
                        _thisCopy.Exchange(i, j, r);
                    }
                }
                //计算对角线乘积
                for (int i = 0; i < rows; i++)
                {
                    res *= _thisCopy[i, i];
                }
                return res;
            }
            else
            {
                throw new Exception("不是行列式");
            }
        }
        #endregion
        #region 初等变换
        /// <summary>
        /// 初等变换：交换第r1和第r2行
        /// </summary>
        /// <param name="r1">第r1行</param>
        /// <param name="r2">第r2行</param>
        /// <returns>返回交换两行后的新的矩阵</returns>
        public Matrix Exchange(int r1, int r2)
        {
            if (Math.Min(r2, r1) >= 0 && Math.Max(r1, r2) < rows)
            {
                for (int j = 0; j < cols; j++)
                {
                    double temp = this[r1, j];
                    this[r1, j] = this[r2, j];
                    this[r2, j] = temp;
                }
                return this;
            }
            else
            {
                throw new Exception("超出索引");
            }
        }
        /// <summary>
        /// 初等变换：将r1行乘以某个数加到r2行
        /// </summary>
        /// <param name="r1">第r1行乘以num</param>
        /// <param name="r2">加到第r2行，若第r2行为负，则直接将r1乘以num并返回</param>
        /// <param name="num">某行放大的倍数</param>
        /// <returns></returns>
        public Matrix Exchange(int r1, int r2, double num)
        {
            if (Math.Min(r2, r1) >= 0 && Math.Max(r1, r2) < rows)
            {
                for (int j = 0; j < cols; j++)
                {
                    this[r2, j] += this[r1, j] * num;
                }
                return this;
            }
            else if (r2 < 0)
            {
                for (int j = 0; j < cols; j++)
                {
                    this[r1, j] *= num;
                }
                return this;
            }
            else
            {
                throw new Exception("超出索引");
            }
        }
        /// <summary>
        /// 得到一个同等大小的单位矩阵
        /// </summary>
        /// <returns>返回一个同等大小的单位矩阵</returns>
        public Matrix EMatrix()
        {
            if (rows == cols)
            {
                double[,] res = new double[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (i == j)
                            res[i, j] = 1;
                        else
                            res[i, j] = 0;
                    }
                }
                return new Matrix(res);
            }
            else
                throw new Exception("不是方阵，无法得到单位矩阵");
        }
        #endregion
        /// <summary>
        /// 深拷贝，仅仅将值拷贝给一个新的对象
        /// </summary>
        /// <returns>返回深拷贝后的新对象</returns>
        public Matrix DeepCopy()
        {
            double[,] ele = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    ele[i, j] = this[i, j];
                }
            }
            return new Matrix(ele);
        }

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    str += this[i, j].ToString();
                    if (j != Cols - 1)
                        str += " ";
                    else if (i != Rows - 1)
                        str += Environment.NewLine;
                }
            }
            return str;
        }
    }
}