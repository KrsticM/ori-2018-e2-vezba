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
            Id,MSSubClass,MSZoning,LotFrontage,LotArea,Street,Alley,LotShape,LandContour,Utilities,LotConfig,LandSlope,
            Neighborhood,Condition1,Condition2,BldgType,HouseStyle,OverallQual,OverallCond,YearBuilt,YearRemodAdd,RoofStyle,
            RoofMatl,Exterior1st,Exterior2n,MasVnrType,MasVnrArea,ExterQual,ExterCond,Foundation,BsmtQual,BsmtCond,BsmtExposure,
            BsmtFinType1,BsmtFinSF1,BsmtFinType2,BsmtFinSF2,BsmtUnfSF,TotalBsmtSF,Heating,HeatingQC,CentralAir,Electrical, FstFlrSF, 
            SndFlrSF,LowQualFinSF,GrLivArea,BsmtFullBath,BsmtHalfBath,FullBath,HalfBath,BedroomAbvGr,KitchenAbvGr,
            KitchenQual,TotRmsAbvGrd,Functional,Fireplaces,FireplaceQu,GarageType,GarageYrBlt,GarageFinish,GarageCars,
            GarageArea,GarageQual,GarageCond,PavedDrive,WoodDeckSF,OpenPorchSF,EnclosedPorch,TSsnPorch,ScreenPorch,PoolArea,PoolQC,
            Fence,MiscFeature,MiscVal,MoSold,YrSold,SaleType,SaleCondition,SalePrice

        }

        static void Main(string[] args)
        {
            string[] lines;
            lines = File.ReadAllLines(@"./../../data/train.csv");
            lines = lines.Skip(1).ToArray();

            List<double> neighBour = new List<double>(); // string
            List<double> lotArea = new List<double>();
            List<double> bldgType = new List<double>(); // string
            List<double> salePrice = new List<double>();
            List<double> msSubClass = new List<double>();

            Dictionary<String, double> myMapN = new Dictionary<String, double>();

            Dictionary<String, double> myMapB = new Dictionary<String, double>();

            Dictionary<double, int> testMap = new Dictionary<double, int>(); 

            int count = 0;
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                lotArea.Add(int.Parse(parts[(int)Header.LotArea]));
                salePrice.Add(double.Parse(parts[(int)Header.SalePrice]));
                msSubClass.Add(double.Parse(parts[(int)Header.MSSubClass]));

                String komsiluk = parts[(int)Header.Neighborhood];
                if(!komsiluk.Equals(""))
                {
                    if (!myMapN.ContainsKey(komsiluk))
                    {
                        myMapN.Add(komsiluk, myMapN.Count);
                        neighBour.Add(myMapN.Count);
                    }
                    else
                    {
                        neighBour.Add(myMapN[komsiluk]);
                    }
                }
                else
                {
                    neighBour.Add(-1.00); // nema porodicu
                }

                String bldgTip = parts[(int)Header.BldgType];
                if (!bldgTip.Equals(""))
                {
                    if (!myMapB.ContainsKey(bldgTip))
                    {
                        myMapB.Add(bldgTip, myMapB.Count);
                        bldgType.Add(myMapB.Count);
                    }
                    else
                    {
                        bldgType.Add(myMapB[bldgTip]);
                    }
                }
                else
                {
                    bldgType.Add(-1.00); // nema porodicu
                }

                if (!testMap.ContainsKey(double.Parse(parts[(int)Header.MSSubClass])))
                {
                    testMap.Add(double.Parse(parts[(int)Header.MSSubClass]), testMap.Count);
                    //bldgType.Add(myMapB.Count);
                } 


                count++;
            } 

            normalize(neighBour);
            normalize(lotArea);
            normalize(bldgType);
            normalize(salePrice);

            Console.WriteLine("Razlicito subklasa: " + testMap.Count);

            foreach(int key in testMap.Keys)
            {
                Console.WriteLine(" " + key);
            }

            NeuralNetwork network = new NeuralNetwork();
            network.Add(new NeuralLayer(4, 10, "sigmoid"));
            network.Add(new NeuralLayer(10, 15, "sigmoid")); 

            List<List<double>> X = new List<List<double>>();
            List<List<double>> Y = new List<List<double>>();

            int whereToStop = count * 80 / 100;
            // prolazimo kroz sve likove
            for (int i=0; i< whereToStop; i++)
            {                
                double[] xTemp = { neighBour[i], lotArea[i], bldgType[i], salePrice[i]};
                X.Add(xTemp.ToList());

                if(msSubClass[i] == 60) 
                {
                    double[] yTemp = { 1, 0, 0,0,0,0,0,0,0,0,0,0,0,0,0 };
                    Y.Add(yTemp.ToList());
                }
                else if(msSubClass[i] == 20)
                {
                    double[] yTemp = { 0,1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    Y.Add(yTemp.ToList());
                }                                  
                else if (msSubClass[i] == 70) 
                {
                    double[] yTemp = { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    Y.Add(yTemp.ToList());
                }
                else if (msSubClass[i] == 50)
                {
                    double[] yTemp = { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    Y.Add(yTemp.ToList());
                }
                else if (msSubClass[i] == 190)
                {
                    double[] yTemp = { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    Y.Add(yTemp.ToList());
                }
                else if (msSubClass[i] == 45)
                {
                    double[] yTemp = { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    Y.Add(yTemp.ToList());
                }
                else if (msSubClass[i] == 90)
                {
                    double[] yTemp = { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 };
                    Y.Add(yTemp.ToList());
                }
                else if (msSubClass[i] == 120)
                {
                    double[] yTemp = { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };
                    Y.Add(yTemp.ToList());
                }
                else if (msSubClass[i] == 30)
                {
                    double[] yTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 };
                    Y.Add(yTemp.ToList());
                }
                else if (msSubClass[i] == 85)
                {
                    double[] yTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 };
                    Y.Add(yTemp.ToList());
                }
                else if (msSubClass[i] == 80)
                {
                    double[] yTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 };
                    Y.Add(yTemp.ToList());
                }
                else if (msSubClass[i] == 160)
                {
                    double[] yTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 };
                    Y.Add(yTemp.ToList());
                }
                else if (msSubClass[i] == 75)
                {
                    double[] yTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 };
                    Y.Add(yTemp.ToList());
                }
                else if (msSubClass[i] == 180)
                {
                    double[] yTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 };
                    Y.Add(yTemp.ToList());
                }
                else if (msSubClass[i] == 40)
                {
                    double[] yTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
                    Y.Add(yTemp.ToList());
                }
            }
            Console.WriteLine("Obuka pocela");
            network.fit(X, Y, 0.1, 0.9, 500);
            Console.WriteLine("Gotova obuka");
            int dobrih = 0;
            for (int i = whereToStop; i < count; i++)
            {
                double[] x1 = { neighBour[i], lotArea[i], bldgType[i], salePrice[i] };
                double classa = -1;
                List<double> prediction = network.predict(x1.ToList());
                double max = 0;
                int index = 0;
                for(int j=0; j<prediction.Count; j++)
                {
                    if(prediction[j] > max)
                    {
                        max = prediction[j];
                        index = j;
                    }
                }
                
                if(index == 0)
                {
                    classa = 60;
                }
                else if(index == 1)
                {
                    classa = 20;
                }
                else if (index == 2)
                {
                    classa = 70;
                }
                else if (index == 3)
                {
                    classa = 50;
                }
                else if(index == 4)
                {
                    classa = 190;
                }
                else if (index == 5)
                {
                    classa = 45;
                }
                else if (index == 6)
                {
                    classa = 90;
                }
                else if (index == 7)
                {
                    classa = 120;
                }
                else if (index == 8)
                {
                    classa = 30;
                }
                else if (index == 9)
                {
                    classa = 85;
                }
                else if (index == 10)
                {
                    classa = 80;
                }
                else if (index == 11)
                {
                    classa = 160;
                }
                else if (index == 12)
                {
                    classa = 75;
                }
                else if (index == 13)
                {
                    classa = 180;
                }
                else if (index == 14)
                {
                    classa = 40;
                }

                if (msSubClass[i] == classa)
                {
                    dobrih++;
                }
            }
            Console.WriteLine("Dobrih: " + dobrih + "/{0} odnosno {1} %", count - whereToStop, dobrih*100/ (count - whereToStop));
            Console.ReadLine();
        }
        private static void normalize(List<double> col)
        {
            double min = col[0];
            double max = col[0];

            for (int i = 0; i < col.Count; i++)
            {
                if (col[i] > max)
                {
                    max = col[i];
                }
                if (col[i] < min)
                {
                    min = col[i];
                }
            }

            for (int i = 0; i < col.Count; i++)
            {
                col[i] = (col[i] - min) / (max - min); // 0..1
            }
        }
    }
}
