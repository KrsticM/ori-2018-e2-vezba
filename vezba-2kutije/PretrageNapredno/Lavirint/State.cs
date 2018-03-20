using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/*
 *  Kutije su iste boje  
 */
namespace Lavirint
{
    public class State
    {
        public static int[,] lavirint;
        State parent;
        public int markI, markJ; //vrsta i kolona
        public double cost;
        public Hashtable boxes = new Hashtable();
        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;
            rez.boxes = new Hashtable(this.boxes);

            bool isBox = lavirint[markI, markJ] == 4;
            if(isBox && !boxes.ContainsKey(markI*100 + markJ))
            {
                rez.boxes.Add(markI * 100 + markJ, null);
            }
            return rez;
        }

        public List<State> mogucaSledecaStanja()
        {
            List<State> rez = new List<State>();
            for (int i = -1; i <= 1; i += 2)
            {
                int newMarkI = markI + i;
                if (newMarkI >= 0 && newMarkI < Main.brojVrsta)
                {
                    if (lavirint[newMarkI, markJ] != 1)
                    {
                        State novo = sledeceStanje(newMarkI, markJ);
                        rez.Add(novo);
                    }
                }
            }

            for (int j = -1; j <= 1; j += 2)
            {
                int newMarkJ = markJ + j;
                if (newMarkJ >= 0 && newMarkJ < Main.brojKolona)
                {
                    if (lavirint[markI, newMarkJ] != 1)
                    {
                        State novo = sledeceStanje(markI, newMarkJ);
                        rez.Add(novo);
                    }
                }
            }

            return rez;
        }

        public override int GetHashCode()
        {
            return ((10000 * this.boxes.Count) + 100*markI + markJ);
        }

        public bool isKrajnjeStanje()
        {
            return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ && this.boxes.Count >= 2;
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
