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
        public Boolean kutija;
        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;
            rez.kutija = this.kutija;
            if(lavirint[markI,markJ] == 4)
            {
                rez.kutija = true;
                Console.WriteLine("Kutija u blizini!");
            }
            return rez;
        }

        
        public List<State> mogucaSledecaStanja()
        {
            //TODO1: Implementirati metodu tako da odredjuje dozvoljeno kretanje u lavirintu
            //TODO2: Prosiriti metodu tako da se ne moze prolaziti kroz sive kutije
            List<State> rez = new List<State>();

            for(int i=-2; i<=2; i+=4)
            {
                //Console.WriteLine("i {0}", i);
                int newMarkI = markI + i;
                for(int j=-1; j<=1; j+=2)
                {
                    int newMarkJ = markJ + j;
                    if (newMarkI >= 0 && newMarkI < Main.brojVrsta)
                    {
                        if (newMarkJ >= 0 && newMarkJ < Main.brojKolona)
                        {
                            if (lavirint[newMarkI, newMarkJ] != 1)
                            {
                                State novo = sledeceStanje(newMarkI, newMarkJ);
                                rez.Add(novo);
                                //Console.WriteLine("I: " + markI + "J: " + markJ);
                            }
                        }
                    }
                }                
            }

            for (int j = -2; j <= 2; j += 4)
            {
                //Console.WriteLine("i {0}", i);
                int newMarkJ = markJ + j;
                for (int i = -1; i <= 1; i += 2)
                {
                    int newMarkI = markI + i;
                    if (newMarkI >= 0 && newMarkI < Main.brojVrsta)
                    {
                        if (newMarkJ >= 0 && newMarkJ < Main.brojKolona)
                        {
                            if (lavirint[newMarkI, newMarkJ] != 1)
                            {
                                State novo = sledeceStanje(newMarkI, newMarkJ);
                                rez.Add(novo);
                                //Console.WriteLine("I: " + markI + "J: " + markJ);
                            }
                        }
                    }
                }
            }
            return rez;
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(kutija) * 10000 + 100*markI + markJ;
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
