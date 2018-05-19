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
        public static int[,] konj = { { -1, -2 }, { 1, -2 }, { -1, 2 }, { 1, 2 }, { -2, -1 }, { 2, -1 }, { -2, 1 }, { 2, 1 } };
        public static int[,] lovac = { { -1, -1 }, { 1, -1 }, { -1, 1 }, { 1, 1 } };
        public static int[,] top = { { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 } };
        State parent;
       
        public int markI, markJ; //vrsta i kolona
        public double cost;
        private Hashtable LevePosecene = new Hashtable();
        private Hashtable DesnePosecene = new Hashtable();

        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;

            foreach (DictionaryEntry hash in this.LevePosecene)
            {
                rez.LevePosecene.Add(hash.Key, null);
            }
            foreach (DictionaryEntry hash in this.DesnePosecene)
            {
                rez.DesnePosecene.Add(hash.Key, null);
            }

            if (markJ + 1 < Main.brojKolona/2) // ako je levo
            {
                if (lavirint[markI, markJ] == 4 && !LevePosecene.ContainsKey(markI * 10 + markJ))
                {
                    rez.LevePosecene.Add(markI * 10 + markJ, null);
                }
            }
            else // ako je desno
            {
                if (lavirint[markI, markJ] == 4 && !DesnePosecene.ContainsKey(markI * 10 + markJ))
                {
                    if (Main.boxesLeft.Count == LevePosecene.Count)
                    {
                        rez.DesnePosecene.Add(markI * 10 + markJ, null);
                    }
                }
            }
            
          

            return rez;
        }

        public bool isWall(int markI, int markJ)
        {
            bool yes = false;
            if (lavirint[markI, markJ] == 1)
            {
                yes = true;
            }
            return yes;
        }

        public bool isEndOfMap(int newMarkI, int newMarkJ)
        {
            return (newMarkI >= 0 && newMarkI < Main.brojVrsta) && (newMarkJ > 0 && newMarkJ < Main.brojKolona);
        }


        public List<State> mogucaSledecaStanja()
        {
            //TODO1: Implementirati metodu tako da odredjuje dozvoljeno kretanje u lavirintu
            //TODO2: Prosiriti metodu tako da se ne moze prolaziti kroz sive kutije
            List<State> rez = new List<State>();
            //kretnja kao konj

            for (int i = 0; i < top.GetLength(0); i++)
            {
                int newMarkI = markI + top[i, 0];
                int newMarkJ = markJ + top[i, 1];

                if (isEndOfMap(newMarkI, newMarkJ) && !isWall(newMarkI, newMarkJ))
                {

                    State novo = sledeceStanje(newMarkI, newMarkJ);
                    rez.Add(novo);
                }


            }

            return rez;
        }


        public override int GetHashCode()
        {
            int key = 10 * markI + markJ; //dve najnize cifre predstavljaju indekse polja na kome se Meda trenutno nalazi
            //to je donjih 7 bita. Počevši od 8og bita svaki bit predstavlja da li je poseceno obavezno polje

            int i = 128; //binarno 1000 0000 (oznaka da je skupljena prva kutija)

            foreach (Point point in Main.boxesLeft) //iterira se kroz listu svih obaveznih
            {
                if (LevePosecene.ContainsKey(point.X * 10 + point.Y) == true) //provera da li je trenutno obavezno polje poseceno
                {
                    key = key | i; //1 se upisuje na bit koji označava da je to obavezno polje poseceno
                }
                i = i << 1; //shift za jedno mesto u levo. npr: 1000 0000 postaje 1 0000 0000 i tako se dobije sledeća obavezno polje
            }

            i = 524288; // 2^19

            foreach (Point point in Main.boxesRight) //iterira se kroz listu svih obaveznih
            {
                if (DesnePosecene.ContainsKey(point.X * 10 + point.Y) == true) //provera da li je trenutno obavezno polje poseceno
                {
                    key = key | i; //1 se upisuje na bit koji označava da je to obavezno polje poseceno
                }
                i = i << 1; //shift za jedno mesto u levo. npr: 1000 0000 postaje 1 0000 0000 i tako se dobije sledeća obavezno polje
            }
            return key;
        }
    

        public bool isKrajnjeStanje()
        {
            Console.WriteLine("isKrajnjeStanje - Potrebno levih: " + Main.boxesLeft.Count + " Poseceno levih " + this.LevePosecene.Count);
            Console.WriteLine("isKrajnjeStanje - Potrebno desnih: " + Main.boxesRight.Count + " Poseceno desnih " + this.DesnePosecene.Count);
            if (this.LevePosecene.Count != Main.boxesLeft.Count || this.DesnePosecene.Count != Main.boxesRight.Count)
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
