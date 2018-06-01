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
            SNO, Actual, Pred, Alive, Plod, Name, Title, Male, Culture, dateOfBirth, dateOfDeath, mother, father, heir, house, spouse, book1, book2,
            book3, book4, book5, isAliveMother, isAliveFather, isAliveHeir, isAliveSpouse, isMarried, isNobleD, age, numDeathRelations, boolDeathRelations, isPopular, Popularity, isAliveD
        }

        static void Main(string[] args)
        {
            string[] lines;
            lines = File.ReadAllLines(@"./../../data/dataset.csv");
            lines = lines.Skip(1).ToArray();

            List<int> pol = new List<int>();
            List<double> popularnost = new List<double>();
            List<int> brojPojavljivanja = new List<int>();
            List<int> isNoble = new List<int>();
            List<int> ubijenoUOkolini = new List<int>();
            List<int> pripadaPorodici = new List<int>();
            List<int> isAlive = new List<int>();

            Dictionary<String, int> myMap = new Dictionary<String, int>();

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                pol.Add(int.Parse(parts[(int)Header.Male]));
                popularnost.Add(double.Parse(parts[(int)Header.Popularity]));

                int firstBook = int.Parse((parts[(int)Header.book1]));
                int secondBook = int.Parse((parts[(int)Header.book2]));
                int thirdBook = int.Parse((parts[(int)Header.book3]));
                int fourthBook = int.Parse((parts[(int)Header.book4]));
                int fifthBook = int.Parse((parts[(int)Header.book5]));

                int brojPojava = firstBook + secondBook + thirdBook + fourthBook + fifthBook;

                brojPojavljivanja.Add(brojPojava);

                isNoble.Add(int.Parse(parts[(int)Header.isNobleD]));
                ubijenoUOkolini.Add(int.Parse(parts[(int)Header.numDeathRelations]));

                String porodica = parts[(int)Header.house];
                if(!porodica.Equals(""))
                {
                    if (!myMap.ContainsKey(porodica))
                    {
                        myMap.Add(porodica, myMap.Count);
                        pripadaPorodici.Add(myMap.Count);
                    }
                    else
                    {
                        pripadaPorodici.Add(myMap[porodica]);
                    }
                }
                else
                {
                    pripadaPorodici.Add(-1); // nema porodicu
                }            
                isAlive.Add(int.Parse((parts[(int)Header.isAliveD])));

            }
            //Console.WriteLine("Broj porodica: " + myMap.Count);

            //foreach (var item in myMap)
            //{
            //    Console.WriteLine("Naziv: " + item.Key +  " id: " + item.Value);
            //}


            NeuralNetwork network = new NeuralNetwork();
            network.Add(new NeuralLayer(6, 6, "sigmoid"));
            network.Add(new NeuralLayer(6, 1, "sigmoid"));

            List<List<double>> X = new List<List<double>>();

            List<List<double>> Y = new List<List<double>>();
            // prolazimo kroz sve likove
            for (int i=0; i<pol.Count; i++)
            {
                
                double[] xTemp = { (double)pol[i], (double)popularnost[i], (double)brojPojavljivanja[i], (double)isNoble[i], (double)ubijenoUOkolini[i], (double)pripadaPorodici[i]};
                X.Add(xTemp.ToList());


                double[] yTemp = { (double)isAlive[i] };
                Y.Add(yTemp.ToList());

               

                if (i >= 1500)
                {
                    break;
                }

   
            }
            Console.WriteLine("Obuka pocela");
            network.fit(X, Y, 0.1, 0.9, 10000);
            Console.WriteLine("Gotova obuka");


          

            double[] x1 = { pol[1903], popularnost[1903], brojPojavljivanja[1903], isNoble[1903], ubijenoUOkolini[1903], pripadaPorodici[1903] };
            Console.WriteLine(network.predict(x1.ToList())[0]);

            Console.ReadLine();
        }
    }
}
