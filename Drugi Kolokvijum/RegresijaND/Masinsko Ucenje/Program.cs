using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Masinsko_Ucenje
{
    static class Program
    {
        static void Main(string[] args)
        {
            LinearRegression regression =  new LinearRegression();

            string[] lines;
            lines = File.ReadAllLines(@"./../../data/train.csv");
            lines = lines.Skip(1).ToArray(); // skip header row (Indeks, Bodovi)

            List<List<double>> koordinate = new List<List<double>>();

            List<double> Y = new List<double>();

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                List<double> temp = new List<double>();
                for (int i = 0; i < parts.Length; i++)
                {
                    if (i != parts.Length - 1)
                    {
                        temp.Add(double.Parse(parts[i])); 
                    }
                    else
                    {
                        Y.Add(double.Parse(parts[i]));

                    }
                }
                koordinate.Add(temp);
            }
            Console.WriteLine("Dalje neces moci");

            /*for(int j=0; j<lines.Length; j++)
            {
                string line = lines[j];
                string[] parts = line.Split(',');
                for(int i=0; i < parts.Length; i++)
                {
                    if(i != parts.Length-1)
                    {
                        koordinate[j].Add(1.0); //double.Parse(parts[i])
                    }
                    else
                    {
                        Y.Add(double.Parse(parts[i]));

                    }
                }
            }*/

            regression.fit(koordinate, Y);
            Console.WriteLine("Dalje neces moci2");

            lines = File.ReadAllLines(@"./../../data/test.csv");
            lines = lines.Skip(1).ToArray(); // skip header row (Indeks, Bodovi)

            List<List<double>> koordinateNove = new List<List<double>>();

            List<double> YNovo = new List<double>();

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                List<double> temp = new List<double>();
                for (int i = 0; i < parts.Length; i++)
                {
                    if (i != parts.Length - 1)
                    {
                        temp.Add(double.Parse(parts[i]));
                    }
                    else
                    {
                        YNovo.Add(double.Parse(parts[i]));

                    }
                }
                koordinateNove.Add(temp);
            }
            for (int i=0; i<koordinateNove.Count; i++)
            {
                Console.WriteLine("Tacno = " + YNovo[i]);

                Console.WriteLine("Pogodjeno = " + regression.predict(koordinateNove[i]));


            }


            //Console.ReadLine();
        }
    }
}

