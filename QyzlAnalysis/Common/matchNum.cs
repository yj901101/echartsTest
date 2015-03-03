using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QyzlAnalysis.Common
{
    public class matchNum
    {
        public static int comAaf(double k1,double k2) {
            double k = k1 - k2;
            int aaf = 0;
            if (k < 0) {
                k = -k;
            }
            if (0 <= k && k<=0.3) {
                aaf = 10;
            }
            else if (0.3 < k && k <= 0.6) {
                aaf = 9;
            }
            else if (0.6 < k && k <= 0.9)
            {
                aaf = 8;
            }
            else if (0.9 < k && k <= 1.2)
            {
                aaf = 7;
            }
            else if (1.2 < k && k <= 1.5)
            {
                aaf = 6;
            }
            else if (1.5 < k && k <= 1.8)
            {
                aaf = 5;
            }
            else if (1.8 < k && k <= 2.1)
            {
                aaf = 4;
            }
            else if (2.1 < k && k <= 2.4)
            {
                aaf = 3;
            }
            else if (2.4 < k && k <= 2.7)
            {
                aaf = 2;
            }
            else if (2.7 < k && k <= 3)
            {
                aaf = 1;
            }
            return aaf;
        }
        public static double GetB(List<double> list, int i)
        {
            double r1 = list[i];
            double r2 = list[i + 1];
            double res = Math.Abs(r1 - r2);
            double b = 0;
            if (res >= 0 && res <= 0.1)
            {
                b = 1;
            }
            if (res > 0.1 && res <= 0.2)
            {
                b = 0.9;
            }
            if (res > 0.2 && res <= 0.3)
            {
                b = 0.8;
            }
            if (res > 0.3 && res <= 0.4)
            {
                b = 0.7;
            }
            if (res > 0.4 && res <= 0.5)
            {
                b = 0.6;
            }
            if (res > 0.5 && res <= 0.6)
            {
                b = 0.5;
            }
            if (res > 0.6 && res <= 0.7)
            {
                b = 0.4;
            }
            if (res > 0.7 && res <= 0.8)
            {
                b = 0.3;
            }
            if (res > 0.8 && res <= 0.9)
            {
                b = 0.2;
            }
            if (res > 0.9 && res <= 1)
            {
                b = 0.1;
            }
            return b;
        }

    }
}