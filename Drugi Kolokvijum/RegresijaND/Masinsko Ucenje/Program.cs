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
            lines = File.ReadAllLines(@"./../../data/bodovi.csv");
            lines = lines.Skip(1).ToArray(); // skip header row (Indeks, Bodovi)

            List<List<double>> koordinate = new List<List<double>>();

            List<double> Y = new List<double>();

            // 1,1,1,1
            // 2,2,2,2


            // 1 1 1 1
            // 2 2 2 2

            for(int j=0; j<lines.Length; j++)
            {
                string line = lines[j];
                string[] parts = line.Split(',');
                for(int i=0; i < parts.Length; i++)
                {
                    if(i != parts.Length-1)
                    {
                        koordinate[j].Add(double.Parse(parts[i]));
                    }
                    else
                    {
                        Y.Add(double.Parse(parts[i]));

                    }
                }
            }

            regression.fit(koordinate, Y);
            Console.ReadLine();
        }
    }
}

