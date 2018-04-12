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
        public int markI, markJ; //vrsta i kolona
        public double cost;
        public static Boolean redosled = false; // 0 naizmenicno, 1 prvo plave
        //gore, dole,levo,desno 
        // kretanje kralja
        private static int[,] steps = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };

        //ako ide kao lovac
        //private static int[,] steps = { { 1, 1 }, { 1, -1}, {-1, 1}, {-1, -1} };
        
        //ako ide kao konj
        //private static int[,] stepsAfterBox = { { 1, 2}, {-1,2}, {2,1} , {2,-1}, {-2,1}, {-2,-1}, {1,-2}, {-1,-2}};

        // kraljica = kralj + lovac

        private Hashtable posecenePlave = new Hashtable();
        private Hashtable poseceneNarandzaste = new Hashtable();


        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;

            foreach(DictionaryEntry hash in this.posecenePlave)
            {
                rez.posecenePlave.Add(hash.Key, null);
            }
            foreach (DictionaryEntry hash in this.poseceneNarandzaste)
            {
                rez.poseceneNarandzaste.Add(hash.Key, null);
            }
            if(redosled) // prvo plave
            {
                if(lavirint[markI,markJ] == 4 && !posecenePlave.ContainsKey(markI*10+markJ))
                {
                    rez.posecenePlave.Add(markI * 10 + markJ, null);
                }
                if(lavirint[markI, markJ] == 5 && !poseceneNarandzaste.ContainsKey(markI * 10 + markJ))
                {
                    if(Main.plave.Count == posecenePlave.Count)
                    {
                        rez.poseceneNarandzaste.Add(markI * 10 + markJ, null);
                    }
                }
            }
            else // naizmenicno
            {
                if (lavirint[markI, markJ] == 4 && !posecenePlave.ContainsKey(markI * 10 + markJ))
                {
                    if(posecenePlave.Count == 0 && poseceneNarandzaste.Count == 0)
                    {
                        rez.posecenePlave.Add(markI * 10 + markJ, null);
                    }                    
                    else if(posecenePlave.Count <= poseceneNarandzaste.Count)
                    {
                        rez.posecenePlave.Add(markI * 10 + markJ, null);
                    }
                }
                if (lavirint[markI, markJ] == 5 && !poseceneNarandzaste.ContainsKey(markI * 10 + markJ))
                {
                    if (posecenePlave.Count == 0 && poseceneNarandzaste.Count == 0)
                    {
                        rez.poseceneNarandzaste.Add(markI * 10 + markJ, null);
                    }
                    else if (poseceneNarandzaste.Count <= posecenePlave.Count)
                    {
                        rez.poseceneNarandzaste.Add(markI * 10 + markJ, null);
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

            foreach (Point point in Main.plave) //iterira se kroz listu svih obaveznih
            {
                if (posecenePlave.ContainsKey(point.X * 10 + point.Y) == true) //provera da li je trenutno obavezno polje poseceno
                {
                    key = key | i; //1 se upisuje na bit koji označava da je to obavezno polje poseceno
                }
                i = i << 1; //shift za jedno mesto u levo. npr: 1000 0000 postaje 1 0000 0000 i tako se dobije sledeća obavezno polje
            }

            i = 2048;

            foreach (Point point in Main.narandzaste) //iterira se kroz listu svih obaveznih
            {
                if (poseceneNarandzaste.ContainsKey(point.X * 10 + point.Y) == true) //provera da li je trenutno obavezno polje poseceno
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
            if (poseceneNarandzaste.Count != Main.narandzaste.Count || posecenePlave.Count != Main.plave.Count)
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
