using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ComputationalGraph
{
    class Program
    {
        enum Header
        {
            Id, Col_1, Col_2, Col_3, Col_4, Col_5
        }

        static void Main(string[] args)
        {
            NeuralNetwork network = new NeuralNetwork();
            network.Add(new NeuralLayer(4, 4, "sigmoid"));
            network.Add(new NeuralLayer(4, 3, "sigmoid"));
            network.Add(new NeuralLayer(3, 3, "sigmoid"));

            string[] lines;
            lines = File.ReadAllLines(@"./../../data/train.csv");
            lines = lines.Skip(1).ToArray();

            List<double> col_1 = new List<double>();
            List<double> col_2 = new List<double>();
            List<double> col_3 = new List<double>();
            List<double> col_4 = new List<double>();
            List<double> col_5 = new List<double>();

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                col_1.Add(double.Parse(parts[(int)Header.Col_1]));
                col_2.Add(double.Parse(parts[(int)Header.Col_2]));
                col_3.Add(double.Parse(parts[(int)Header.Col_3]));
                col_4.Add(double.Parse(parts[(int)Header.Col_4]));

                double y; // inicijalizuj
                if (parts[(int)Header.Col_5].Equals("type_1"))
                {
                    y = 0;
                }
                else if (parts[(int)Header.Col_5].Equals("type_2"))
                {
                    y = 0.5;
                }
                else if (parts[(int)Header.Col_5].Equals("type_3"))
                {
                    y = 1;
                }
                else
                {
                    y = -1.0; // ne bi trebalo nikada da se desi
                }
                Console.WriteLine("TiP: " + y);
                col_5.Add(y);

            }

            List<List<double>> X = new List<List<double>>();
            List<List<double>> Y = new List<List<double>>();

            normalize(col_1);
            normalize(col_2);
            normalize(col_3);
            normalize(col_4);

            for (int i=0; i<col_1.Count; i++)
            {

                double[] xTemp = {(double)col_1[i], (double)col_2[i] , (double)col_3[i], (double)col_4[i]};
                X.Add(xTemp.ToList());


                double[] yTemp = { col_5[i]==0?0:1, col_5[i] == 0.5 ? 0 : 1, col_5[i] == 1 ? 0 : 1 };
                Y.Add(yTemp.ToList());
            }          
            

            Console.WriteLine("Training started..");
            network.fit(X, Y, 0.1, 0.9, 500);

            Console.WriteLine("Training done.");

           // double[] x1 = { 5.1, 262.5, 1.4, 0.2 }; // tip 3

            Console.WriteLine(network.predict(X[50].ToList())[0]);
            Console.WriteLine(network.predict(X[50].ToList())[1]);
            Console.WriteLine(network.predict(X[50].ToList())[2]);
            Console.ReadKey();
        }
        public static void normalize(List<double> lista)
        {
            double max = lista[0];
            double min = lista[0];
            for (int i = 1; i < lista.Count; i++)
            {
                if (lista[i] > max)
                {
                    max = lista[i];

                }
                if (lista[i] < min)
                {
                    min = lista[i];
                }
            }
            for (int i = 1; i < lista.Count; i++)
            {
                lista[i] = (lista[i] - min) / (max - min);
            }
        }
    }
}
