using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QyzlAnalysis.Common
{
    public class matchNum
    {
        public static int comAaf(double k1,double k2) {
            double k =Math.Atan(k2) / 3.14 * 180 - Math.Atan(k1) / 3.14 * 180;
            int aaf = 0;
            //if (k < 0) {
            //    k = -k;
            //}
            if ((-45 < k && k<= -41) ||(-49< k && k<= -45)) {
                aaf = 10;
            }
            else if ((-41 < k && k <= -36)||(-53<k && k<= -49)) {
                aaf = 9;
            }
            else if ((-36 < k && k <= -31) || (-57< k && k<= -53))
            {
                aaf = 8;
            }
            else if ((-31 < k && k <= -26) || (-61<k && k<= -57))
            {
                aaf = 7;
            }
            else if ((-26 < k && k <= -21) || (-65< k && k<= -61))
            {
                aaf = 6;
            }
            else if ((-21 < k && k <= -16) || (-69< k && k<= -65))
            {
                aaf = 5;
            }
            else if ((-16 < k && k <= -11) || (-73< k && k<= -69))
            {
                aaf = 4;
            }
            else if (-11 < k && k <= -6 || (-77 < k && k<= -73))
            {
                aaf = 3;
            }
            else if ((-6 < k && k <= -1) || (-81 <k && k<= -77))
            {
                aaf = 2;
            }
            else if ((-1 < k) || (k <= -81))
            {
                aaf = 1;
            }
            return aaf;
        }
        public static double GetB(List<double> list, int i)
        {
            double r1 = list[i];
            double r2 = list[i + 1];
            //double res = Math.Abs(r1 - r2);
            double res = (r1 + r2) / 2;
            double b = 0;
            if (res >= 0 && res <= 0.1)
            {
                b = 0.1;
            }
            if (res > 0.1 && res <= 0.2)
            {
                b = 0.2;
            }
            if (res > 0.2 && res <= 0.3)
            {
                b = 0.3;
            }
            if (res > 0.3 && res <= 0.4)
            {
                b = 0.4;
            }
            if (res > 0.4 && res <= 0.5)
            {
                b = 0.5;
            }
            if (res > 0.5 && res <= 0.6)
            {
                b = 0.6;
            }
            if (res > 0.6 && res <= 0.7)
            {
                b = 0.7;
            }
            if (res > 0.7 && res <= 0.8)
            {
                b = 0.8;
            }
            if (res > 0.8 && res <= 0.9)
            {
                b = 0.9;
            }
            if (res > 0.9 && res <= 1)
            {
                b = 1;
            }
            return b;
        }

    }
}