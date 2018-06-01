using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Masinsko_Ucenje
{
    public class KMeans
    {
        public List<Point> elementi = new List<Point>();
        public List<Cluster> grupe = new List<Cluster>();
        public int brojGrupa = 0;
        Random rnd = new Random();

        public void podeliUGrupe(int brojGrupa, double errT)
        {
            this.brojGrupa = brojGrupa;
            if (brojGrupa == 0) return;
            //------------  inicijalizacija -------------
            for (int i = 0; i < brojGrupa; i++)
            {
                // TODO 5: na slucajan nacin inicijalizovati centre grupa
                int idx = rnd.Next(0, this.elementi.Count());

                Cluster cluster = new Cluster();
                Point rndPoint = this.elementi[idx];
                cluster.centar = new Point(rndPoint.x, rndPoint.y);
                this.grupe.Add(cluster);
            }
            //------------- iterativno racunanje centara ---
            for (int it = 0; it < 1000; it++)
            {
                foreach (Cluster grupa in grupe)
                    grupa.elementi = new List<Point>();

                foreach (Point cc in elementi)
                {
                    int najblizaGrupa = 0;
                    for (int i = 0; i < brojGrupa; i++)
                    {
                        if (grupe[najblizaGrupa].rastojanje(cc) >
                            grupe[i].rastojanje(cc))
                        {
                            najblizaGrupa = i;
                        }
                    }
                    grupe[najblizaGrupa].elementi.Add(cc);
                }
                double err = 0;
                for (int i = 0; i < brojGrupa; i++)
                {
                    err += grupe[i].pomeriCentar();
                }
                if (err < errT)
                    break;

                Main.clusteringHistory.Add(Main.DeepClone(this.grupe));
            }
        }
        public void printPercent()
        {
            int ukupnoUSvimKnjigama = 0;
            foreach(Cluster c in grupe)
            {
                foreach (Point p in c.elementi)
                {
                    if (p.y == 5)
                    {
                        ukupnoUSvimKnjigama++;
                    }
                }
            }

            for(int i=0; i<grupe.Count; i++)
            {
                int klasterPojava = 0;
                foreach(Point p in grupe[i].elementi)
                {
                    if(p.y == 5)
                    {
                        klasterPojava++;
                    }
                }
                double procenat = (double)klasterPojava / (double)ukupnoUSvimKnjigama;
                Console.WriteLine("Klaster: " + i + "ima: " + procenat*100 + " posto");

            }
        }
    }
    
    [Serializable]
    public class Cluster
    {
        public Point centar = new Point(0,0);
        public List<Point> elementi = new List<Point>();

        public double rastojanje(Point c)
        {   // TODO 6: implementirati funkciju rastojanja
           return Math.Sqrt(Math.Pow(c.x - centar.x, 2) + Math.Pow(c.y - centar.y, 2));
        }

        public double pomeriCentar()
        {   // TODO 7: implemenitrati funkciju koja pomera centre klastera
            double sX = 0;
            double sY = 0;
            double retVal = 0;

            sX = this.elementi.Average(pt => pt.x);
            sY = this.elementi.Average(pt => pt.y);

            Point newCenter = new Point(sX, sY);
            retVal = this.rastojanje(newCenter);
            this.centar = newCenter;

            return retVal;
        }
    }
}
