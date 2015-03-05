using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QyzlAnalysis.Common
{
    public class matchNum
    {
        public static int comAaf(double k1,double k2) {
            double k = Math.Atan(k1) / 3.14 * 180 - Math.Atan(k2) / 3.14 * 180;
            int aaf = 0;
            //if (k < 0) {
            //    k = -k;
            //}
            if ((-24 < k && k<= -20) ||(-28< k && k<-25)) {
                aaf = 10;
            }
            else if ((-20 < k && k <= -15)||(-32<k && k<= -28)) {
                aaf = 9;
            }
            else if ((-15 < k && k <= -10) || (-36< k && k<= -32))
            {
                aaf = 8;
            }
            else if ((-10 < k && k <= -5) || (-40<k && k<= -36))
            {
                aaf = 7;
            }
            else if ((-5 < k && k <= -1) || (-44< k && k<= -40))
            {
                aaf = 6;
            }
            else if ((-1 < k && k <= 5) || (-48< k && k<= -44))
            {
                aaf = 5;
            }
            else if ((5 < k && k <= 10) || (-52< k && k<= -48))
            {
                aaf = 4;
            }
            else if (10 < k && k <= 15 || (-56 < k && k<= -52))
            {
                aaf = 3;
            }
            else if ((15 < k && k <= 20) || (-60 <k && k<= -56))
            {
                aaf = 2;
            }
            else if ((20 < k) || (k<-60))
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