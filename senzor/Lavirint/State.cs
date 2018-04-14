using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lavirint
{
    public class State
    {
        public static int[,] lavirint;
        State parent;
        public int markI, markJ; 
        public double cost;
        public Boolean dugme1 = false; 
        public Boolean dugme2 = false;
        public Boolean upaljenAlarm = false;
        private static int[,] steps = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };
        private Hashtable predjenaPoljaSenzor = new Hashtable();

        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;
            foreach (DictionaryEntry hash in this.predjenaPoljaSenzor)
            {
                rez.predjenaPoljaSenzor.Add(hash.Key, null);
            }

            rez.dugme1 = this.dugme1;
            rez.dugme2 = this.dugme2;
            rez.upaljenAlarm = this.upaljenAlarm;

            if (lavirint[markI, markJ] == 5)
            {
                rez.dugme1 = true;
            }
            if (lavirint[markI, markJ] == 6)
            {
                rez.dugme2 = true;
            }

            return rez;
        }

        //
        public List<State> mogucaSledecaStanja()
        {
            List<State> rez = new List<State>();

            // ako je trenutno stanje u regionu senzora
            double rastojanje = Math.Sqrt(Math.Pow(markI - Main.senzorStanje.markI, 2) + Math.Pow(markJ - Main.senzorStanje.markJ, 2));
            Console.WriteLine("rastojanje = " + rastojanje);

            if(rastojanje < 3) // 3 bi trebalo da je 3 kocke
            {
                this.upaljenAlarm = true;
                Console.WriteLine("Alarm upaljen");
                if (!predjenaPoljaSenzor.ContainsKey(10 * markI + markJ))
                {
                    predjenaPoljaSenzor.Add(10 * markI + markJ, null);
                    rez.Add(sledeceStanje(markI, markJ));
                    return rez;
                }
            }

            for (int i = 0; i < steps.GetLength(0); i++)
            {
                int newMarkI = this.markI + steps[i, 0];
                int newMarkJ = this.markJ + steps[i, 1];

                if (isWithinBounds(newMarkI, newMarkJ) && !isWall(newMarkI, newMarkJ))
                {
                    rez.Add(sledeceStanje(newMarkI, newMarkJ));
                }

            }
            return rez;
        }

        //
        private bool isWithinBounds(int newMarkI, int newMarkJ)
        {
            return (newMarkI >= 0 && newMarkI < Main.brojVrsta) && (newMarkJ >= 0 && newMarkJ < Main.brojKolona);
        }

        //
        private bool isWall(int newMarkI, int newMarkJ)
        {
            return (lavirint[newMarkI, newMarkJ] == 1);
        }


        public override int GetHashCode()
        {
            int hcode = 10*markI + markJ;

            if(this.dugme1)
            {
                hcode += 100;
            }
            if (this.dugme2)
            {
                hcode += 1000;
            }
            if (predjenaPoljaSenzor.ContainsKey(10 * markI + markJ))
            {
                hcode = hcode + 10000;
            }
            

            /*int i = 128;
            if(this.dugme1)
            {
                hcode = hcode | i;
            }
            i <<= 1;
            if(this.dugme2)
            {
                hcode = hcode | i;
            }
            i <<= 1;
            if (predjenaPoljaSenzor.ContainsKey(10 * markI + markJ))
            {
                hcode = hcode | i;
            }*/
            return hcode;
        }

        //
        public bool isKrajnjeStanje()
        {
            if(!upaljenAlarm)
                return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ;
            else
                return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ && this.dugme1 && this.dugme2;
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
