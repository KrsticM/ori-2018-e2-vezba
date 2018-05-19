using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Lavirint
{
    public class State
    {
        public static int[,] lavirint;
        State parent;
        public int markI, markJ; 
        public double cost;
        public int brojPoena;
        public int brojPoenaProtivnik;

        private Hashtable ljPredjene = new Hashtable();
        private Hashtable nPredjene = new Hashtable();

        private static int[,] steps = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };        

        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;

            rez.brojPoena = this.brojPoena;
            rez.brojPoenaProtivnik = this.brojPoenaProtivnik;
            foreach (DictionaryEntry hash in this.ljPredjene)
            {
                rez.ljPredjene.Add(hash.Key, null);
            }
            foreach (DictionaryEntry hash in this.nPredjene)
            {
                rez.nPredjene.Add(hash.Key, null);
            }



            if (lavirint[markI, markJ] == 4) // ljubicasta
            {
                if(!ljPredjene.ContainsKey(10*markI + markJ))
                {
                    rez.brojPoena++;
                    rez.ljPredjene.Add(10 * markI + markJ, null);
                }
            }
            if(lavirint[markI, markJ] == 5)
            {
                if(!nPredjene.ContainsKey(10*markI + markJ))
                {
                    rez.nPredjene.Add(10 * markI + markJ, null);
                    Random rnd = new Random();
                    int br = rnd.Next(1, 100);
                    if (br <= 75)
                    {
                        rez.brojPoena--;
                    }
                    else
                    {
                        int br2 = rnd.Next(1, 100); //celendz
                        if (br2 <= 50)
                        {
                            rez.brojPoena++;
                        }
                        else
                        {
                            rez.brojPoenaProtivnik++;
                        }
                    }
                }

            }
                        
            return rez;
        }

        //
        public List<State> mogucaSledecaStanja()
        {
            List<State> rez = new List<State>();

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
            //TODO 3.6: Promeniti tako da hash code zavisi od broja pokupljenih obaveznih polja
            int key = 10 * markI + markJ; //dve najnize cifre predstavljaju indekse polja na kome se Meda trenutno nalazi
            //to je donjih 7 bita. Počevši od 8og bita svaki bit predstavlja da li je poseceno obavezno polje

            int i = 128; //binarno 1000 0000 (oznaka da je skupljena prva kutija)

            foreach (Point point in Main.ljubicasta) //iterira se kroz listu svih obaveznih
            {
                if (ljPredjene.ContainsKey(point.X * 10 + point.Y) == true) //provera da li je trenutno obavezno polje poseceno
                {
                    key = key | i; //1 se upisuje na bit koji označava da je to obavezno polje poseceno
                }

                i = i << 1; //shift za jedno mesto u levo. npr: 1000 0000 postaje 1 0000 0000 i tako se dobije sledeća obavezno polje
            }

            i = 524288; // ovako moze 12 ljubicastih 2^19 - 7 // 12 narandzastih
            foreach (Point point in Main.narandzasta) //iterira se kroz listu svih obaveznih
            {
                if (nPredjene.ContainsKey(point.X * 10 + point.Y) == true) //provera da li je trenutno obavezno polje poseceno
                {
                    key = key | i; //1 se upisuje na bit koji označava da je to obavezno polje poseceno
                }

                i = i << 1; //shift za jedno mesto u levo. npr: 1000 0000 postaje 1 0000 0000 i tako se dobije sledeća obavezno polje
            }

            return key;
        }

        //
        public bool isKrajnjeStanje()
        {
            if(this.brojPoena < 4 && this.brojPoenaProtivnik < 4)
            {
                return false;
            }
            if(Math.Abs(this.brojPoenaProtivnik - this.brojPoena) < 2)
            {
                return false;
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
