using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QyzlAnalysis.Common
{
    public class Calculate
    {
        /// <summary>
        /// 相关度1
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double GetR(double[] x, double[] y)
        {
            int length = x.Length;
            double avgall = 0;
            //  double avgall2=0;
            double avgx = 0;
            double avgy = 0;
            double avgx2 = 0;
            double avgy2 = 0;
            double res = 0;
            for (int i = 0; i < x.Length; i++)
            {
                avgall = x[i] * y[i] + avgall;
                avgx = x[i] + avgx;
                avgy = y[i] + avgy;
                avgx2 = x[i] * x[i] + avgx2;
                avgy2 = y[i] * y[i] + avgy2;
            }
            avgx = avgx / length;
            avgy = avgy / length;
            avgx2 = avgx2 / length;
            avgy2 = avgy2 / length;
            // avgall2=avgx*avgy;
            avgall = avgall / length;
            res = (avgall - avgx * avgy) / (Math.Pow((avgx2 - avgx * avgx) * (avgy2 - avgy * avgy), 0.5));
            return res;
        }
        /// <summary>
        /// 相关度3
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double GetR3(double[] x, double[] y)
        {
            if (x.Length != y.Length)
                throw new Exception("Length of sources is different");
            double avgX = 0;
            double avgY = 0;
            double sumx = 0;
            double sumy = 0;
            double xy = 0;
            double x2 = 0;
            double y2 = 0;
            int len = x.Length;
            for (int i = 0; i < len; i++)
            {
                sumx += x[i];
                sumy += y[i];
                xy += x[i] * y[i];
                x2 += x[i] * x[i];
                y2 += y[i] * y[i];
            }
            avgX = sumx / len;
            avgY = sumy / len;
            double exy = xy / len;
            double exey = avgX * avgY;
            double ex2 = x2 / len;
            double e2x = avgX * avgX;
            double ey2 = y2 / len;
            double e2y = avgY * avgY;
            double fm1 = Math.Sqrt(ex2 - e2x);
            double fm2 = Math.Sqrt(ey2 - e2y);
            return (exy - exey) / (fm1 * fm2);
        }
        /// <summary>
        /// 相关度2
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double GetR2(double[] x, double[] y)
        {
            int length = x.Length;
            double avgx = 0;
            double avgy = 0;
            double par1 = 0;
            double par2 = 0;
            double par3 = 0;
            double res = 0;
            for (int i = 0; i < x.Length; i++)
            {
                avgx = x[i] + avgx;
                avgy = y[i] + avgy;
            }
            avgx = avgx / length;
            avgy = avgy / length;
            for (int i = 0; i < x.Length; i++)
            {
                par1 = (x[i] - avgx) * (y[i] - avgy) + par1;
                par2 = (x[i] - avgx) * (x[i] - avgx) + par2;
                par3 = (y[i] - avgy) * (y[i] - avgy) + par3;
            }
            res = par1 / (Math.Sqrt(par2) * Math.Sqrt(par3));
            return res;
        }
        ///<summary>
        ///用最小二乘法拟合二元多次曲线
        ///</summary>
        ///<param name="arrX">已知点的x坐标集合</param>
        ///<param name="arrY">已知点的y坐标集合</param>
        ///<param name="length">已知点的个数</param>
        ///<param name="dimension">方程的最高次数</param>

        public static double[] MultiLine(double[] arrX, double[] arrY, int length, int dimension)//二元多次线性方程拟合曲线
        {
            int n = dimension + 1;                  //dimension次方程需要求 dimension+1个 系数
            double[,] Guass = new double[n, n + 1];      //高斯矩阵 例如：y=a0+a1*x+a2*x*x
            for (int i = 0; i < n; i++)
            {
                int j;
                for (j = 0; j < n; j++)
                {
                    Guass[i, j] = SumArr(arrX, j + i, length);
                }
                Guass[i, j] = SumArr(arrX, i, arrY, 1, length);
            }
            return ComputGauss(Guass, n);
        }
        public static double SumArr(double[] arr, int n, int length) //求数组的元素的n次方的和
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if (arr[i] != 0 || n != 0)
                    s = s + Math.Pow(arr[i], n);
                else
                    s = s + 1;
            }
            return s;
        }
        public static double SumArr(double[] arr1, int n1, double[] arr2, int n2, int length)
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if ((arr1[i] != 0 || n1 != 0) && (arr2[i] != 0 || n2 != 0))
                    s = s + Math.Pow(arr1[i], n1) * Math.Pow(arr2[i], n2);
                else
                    s = s + 1;
            }
            return s;

        }
        public static double[] ComputGauss(double[,] Guass, int n)
        {
            int i, j;
            int k, m;
            double temp;
            double max;
            double s;
            double[] x = new double[n];

            for (i = 0; i < n; i++) x[i] = 0.0;//初始化


            for (j = 0; j < n; j++)
            {
                max = 0;

                k = j;
                for (i = j; i < n; i++)
                {
                    if (Math.Abs(Guass[i, j]) > max)
                    {
                        max = Guass[i, j];
                        k = i;
                    }
                }
                if (k != j)
                {
                    for (m = j; m < n + 1; m++)
                    {
                        temp = Guass[j, m];
                        Guass[j, m] = Guass[k, m];
                        Guass[k, m] = temp;

                    }
                }

                if (0 == max)
                {
                    // "此线性方程为奇异线性方程" 

                    return x;
                }

                for (i = j + 1; i < n; i++)
                {

                    s = Guass[i, j];
                    for (m = j; m < n + 1; m++)
                    {
                        Guass[i, m] = Guass[i, m] - Guass[j, m] * s / (Guass[j, j]);

                    }
                }


            }//结束for (j=0;j<n;j++)


            for (i = n - 1; i >= 0; i--)
            {
                s = 0;
                for (j = i + 1; j < n; j++)
                {
                    s = s + Guass[i, j] * x[j];
                }

                x[i] = (Guass[i, n] - s) / Guass[i, i];

            }

            return x;
        }//返回值是函数的系数

    }
}