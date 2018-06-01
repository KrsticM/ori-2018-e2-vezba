using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Masinsko_Ucenje
{
    public class LinearRegression
    {
        public double k { get; set; }
        public double n { get; set; }

	    public void fit(double[] x, double[] y) {
            // TODO 2: implementirati fit funkciju koja odredjuje parametre k i n
            // y = kx + n
            double xy_sum = 0;
            double x_sum = 0;
            double y_sum = 0;
            double xx_sum = 0;
            double count = x.Length;

            for (int i = 0; i < x.Length; ++i)
            {
                xy_sum += x[i] * y[i];
                x_sum += x[i];
                y_sum += y[i];
                xx_sum += x[i] * x[i];

            }

            k = (count * xy_sum - x_sum * y_sum) / (count * xx_sum - x_sum * x_sum);
            n = (y_sum - k * x_sum) / count;
        }

        public double predict(double x)
        {
            // TODO 3: Implementirati funkciju predict koja na osnovu x vrednosti vraca
            // predvinjenu vrednost y
            return k * x + n;
        }
    }
}
