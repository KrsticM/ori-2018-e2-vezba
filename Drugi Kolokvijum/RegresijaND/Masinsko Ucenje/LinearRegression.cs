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
            Matrix ymatrica = new Matrix(strY);
            Matrix m = new Matrix(str);
            Matrix transponovana = m.Transpose();
            Matrix pomnozena = m * transponovana;
            Matrix invertovana = pomnozena.Inverse();
            Matrix koeficijenti = invertovana * transponovana * ymatrica;
            Console.WriteLine(koeficijenti.ToString());
        }
    }
}
