using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        private Dictionary<Point, bool> reqStates = new Dictionary<Point, bool>();


        public State()
        {
            foreach (Point point in Main.obaveznaStanja)
            {
                reqStates.Add(point, false);
            }
        }

        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;

            foreach (KeyValuePair<Point, bool> entry in this.reqStates)
            {
                rez.reqStates[entry.Key] = entry.Value;
            }
            
            if (lavirint[markI, markJ] == 4)
            {
                rez.reqStates[new Point(markI, markJ)] = true;
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
            //Console.WriteLine("Hashcode: " + (1000 * this.kutija + 100 * markI + markJ));
            int i = 1000;
            int key = 100 * this.markI + this.markJ;
            foreach (KeyValuePair<Point, bool> entry in this.reqStates)
            {
                if (entry.Value == true)
                {
                    key = key + i;
                }
                i *= 10;
            }
            return  key;
        }

        public bool isKrajnjeStanje()
        {
            foreach (KeyValuePair<Point, bool> entry in this.reqStates)
            {
                if (this.reqStates[entry.Key] == false)
                {
                    return false; //nismo pokupili neke sastojke => stanje nije krajnje
                }
            }
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
