using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Masinsko_Ucenje
{
    class DBScan
    {
        /*public List<Point> elementi = new List<Point>();
        public List<Cluster> grupe = new List<Cluster>();
        Random rnd = new Random();

        public void podeliUGrupe(double epsilon)
        { 
            foreach (Point p in elementi)
            {
                Boolean labeliran = false; // Proverim da li je vec dodeljen nekom klasteru
                foreach (Cluster grupa in grupe)
                {
                    foreach (Point point in grupa.elementi)
                    {
                        if (p == point)
                        {
                            labeliran = true;
                            break;
                        }
                    }
                }
                if (!labeliran)
                {
                    // Ako nije vec labeliran
                    // Pravi se nova grupa
                    Cluster cluster = new Cluster();
                    cluster.elementi = new List<Point>();
                    cluster.centar = new Point(0, 0); // Nebitno
                    cluster.elementi.Add(p);         
                    
                    
                    foreach (Point point in elementi)
                    {
                        if (point != p)
                        {
                            //Ako to nije bas taj
                            if (rastojanje(p, point) < epsilon)
                            {
                                cluster.elementi.Add(point);
                            }
                        }
                    }

                    this.grupe.Add(cluster);
                }
                Main.clusteringHistory.Add(Main.DeepClone(this.grupe));
            }
        }
        private double rastojanje(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2));
        }*/
    }
}
