using CSML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Masinsko_Ucenje
{
    public class LinearRegression
    {

        List<double> koeficijenti = new List<double>();

	    public void fit(List<List<double>> koordinate, List<double> Y) 
        {
            string str = "";
            for (int i=0; i<koordinate.Count; i++)
            {
                for(int j=0; j < koordinate[0].Count; j++)
                {
                    str += koordinate[i][j].ToString();
                    if(j != koordinate[0].Count-1)
                    {
                        str += ",";
                    }
                }
                if(i != koordinate.Count-1)
                {
                    str += ";";
                }
            }
            string strY = "";
            for(int i=0; i<Y.Count; i++)
            {
                strY += Y[i].ToString();
                if (i != Y.Count - 1)
                {
                    strY += ";";
                }
            }
            Console.WriteLine("Matrice " + str);
            Console.WriteLine("Matrice " + strY);
            Matrix ymatrica = new Matrix(strY);
            Console.WriteLine("nrapvipo prvi");
            Matrix m = new Matrix(str);
            Console.WriteLine(m.ToString());
            Console.WriteLine("nrapvipo drugi");
            Matrix transponovana = m.Transpose();
            Console.WriteLine(transponovana.ToString());
            Console.WriteLine("transponovao");
            Matrix pomnozena = m * transponovana;
            Console.WriteLine("pomnozio");
            Console.WriteLine(pomnozena.ToString());
            Matrix invertovana = pomnozena.Inverse();
            Console.WriteLine(invertovana.ToString());
            Console.WriteLine("invertovano");
            Matrix koeficijenti = transponovana * ymatrica;
            koeficijenti = koeficijenti * invertovana;
            Console.WriteLine(koeficijenti.ToString());
        }
        public double predict(List<double> x)
        {
            double retVal = 0;
            for(int i=0; i< koeficijenti.Count; i++)
            {
                retVal += koeficijenti[i] * Math.Pow(x[i], i);
            }
            return retVal;
        }
    }
}
