using System;
using System.Collections.Generic;
using System.Text;

namespace Lavirint
{
    public class State
    {   
        public static int[,] lavirint;
        State parent;
        public int markI, markJ; //vrsta i kolona
        public double cost;
        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;
            return rez;
        }

        
        public List<State> mogucaSledecaStanja()
        {
            //TODO1: Implementirati metodu tako da odredjuje dozvoljeno kretanje u lavirintu
            //TODO2: Prosiriti metodu tako da se ne moze prolaziti kroz sive kutije
            List<State> rez = new List<State>();

            for(int i=-1; i<=1; i+=2)
            {
                //Console.WriteLine("i {0}", i);
                int newMarkI = markI + i;
                if(newMarkI >= 0 && newMarkI < Main.brojVrsta )
                {
                    if(lavirint[newMarkI,markJ] != 1)
                    {
                        State novo = sledeceStanje(newMarkI, markJ);
                        rez.Add(novo);
                        //Console.WriteLine("I: " + markI + "J: " + markJ);
                    }
                }
            }

            for (int j = -1; j <= 1; j += 2)
            {
                //Console.WriteLine("j {0}", j);
                int newMarkJ = markJ + j;
                if (newMarkJ >= 0 && newMarkJ < Main.brojKolona)
                {
                    if (lavirint[markI, newMarkJ] != 1)
                    {
                        State novo = sledeceStanje(markI, newMarkJ);
                        rez.Add(novo);
                       // Console.WriteLine("I: " + markI + "J: " + markJ);
                    }
                }
            }
            //proveri da li smo naisli na teleport
            for (int i = 0; i < Main.teleport.Count; i++)
            {
                if (markI == Main.teleport[i].markI && markJ == Main.teleport[i].markJ) // imamo teleport tu
                {
                    State novo = bestExit(markI,markJ,i);
                    rez.Add(novo);
                }
            }
            return rez;
        }
        //najbolji izlaz-> najmanje rastojanje do cilja
        public State bestExit(int markI, int markJ,int index)
        {
            int pamtii = 0;
            double min = Math.Sqrt(Math.Pow(Main.teleport[0].markI - Main.krajnjeStanje.markI, 2) + Math.Pow(Main.teleport[0].markJ - Main.krajnjeStanje.markJ, 2));
            //prodji kroz sve ulaze i proveri da li je najblizi cilju
              for (int i=1;i<Main.teleport.Count;i++)
            {//racunamo euklidsko rastojanje cilj->teleport
                double temp= Math.Sqrt(Math.Pow(Main.teleport[i].markI - Main.krajnjeStanje.markI, 2) + Math.Pow(Main.teleport[i].markJ - Main.krajnjeStanje.markJ, 2));
                if (temp<min && index!=i)
                {
                    //congrats
                    min = temp;
                    pamtii = i;

                }
            }
            State s = sledeceStanje(Main.teleport[pamtii].markI, Main.teleport[pamtii].markJ);
            return s;

        }

        public override int GetHashCode()
        {
            return 100*markI + markJ;
        }

        public bool isKrajnjeStanje()
        {
            return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ;
        }

        public List<State> path()
        {
            List<State> putanja = new List<State>();
            State tt = this;
            while (tt != null)
            {
                putanja.Insert(0, tt);
                tt = tt.parent;
            }
            return putanja;
        }
    }
}
