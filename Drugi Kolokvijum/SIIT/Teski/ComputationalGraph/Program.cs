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
            network.Add(new NeuralLayer(4, 3, "sigmoid"));
            network.Add(new NeuralLayer(3, 2, "sigmoid"));
            network.Add(new NeuralLayer(2, 1, "sigmoid"));

            string[] lines;
            lines = File.ReadAllLines(@"./../../data/train.csv");
            lines = lines.Skip(1).ToArray();

            List<double> col_1 = new List<double>();
            List<double> col_2 = new List<double>();
            List<double> col_3 = new List<double>();
            List<double> col_4 = new List<double>();
            List<double> col_5 = new List<double>();

            int trainNo = 0;
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
                col_5.Add(y);
                trainNo++;
            }

            //string[] lines;
            lines = File.ReadAllLines(@"./../../data/test.csv");
            lines = lines.Skip(1).ToArray();

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
                col_5.Add(y);

            }

            normalize(col_1);
            normalize(col_2);
            normalize(col_3);
            normalize(col_4);


            List<List<double>> X = new List<List<double>>();
            List<List<double>> Y = new List<List<double>>();


            for (int i=0; i<trainNo; i++)
            {

                double[] xTemp = {(double)col_1[i], (double)col_2[i] , (double)col_3[i], (double)col_4[i]};
                X.Add(xTemp.ToList());

                double[] yTemp = { (double)col_5[i] };
                Y.Add(yTemp.ToList());
            }
            
            

            

            Console.WriteLine("Training started..");
            network.fit(X, Y, 0.1, 0.9, 1500);

            Console.WriteLine("Training done.");

            int pogodjenih = 0;
            for (int i=trainNo; i<col_1.Count; i++)
            {
               
                double[] temp = { col_1[i], col_2[i], col_3[i], col_4[i] };
                double predicted = network.predict(temp.ToList())[0]; // 0   0.5   1
                double tip = -1;
                if (predicted <= 0.33)
                {
                    tip = 0;
                }
                else if (predicted > 0.33 && predicted < 0.66)
                {
                    tip = 0.5;
                }
                else
                {
                    tip = 1;
                }
                if(tip == col_5[i])
                {
                    pogodjenih++;
                }
            }

            Console.WriteLine("Pogodjenih {0} / {1} odnosno za Jelenu {2}%", pogodjenih, col_1.Count - trainNo, pogodjenih*100/(col_1.Count - trainNo));

            Console.ReadKey();
        }

        private static void normalize(List<double> col)
        {
            double min = col[0];
            double max = col[0];

            for(int i=0; i< col.Count; i++)
            {
                if(col[i] > max)
                {
                    max = col[i];                
                }
                if(col[i] < min)
                {
                    min = col[i];
                }
            }

            for(int i=0; i<col.Count; i++)
            {
                col[i] = (col[i] - min) / (max - min); // 0..1
            }
        }
    }
}
