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
        public Boolean rezervoar = false; //0 nije, 1 jeste

        public Boolean lovacKretanje = false;
        //kretanje kao top
        private static int[,] steps = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };

        private static int[,] stepsNorm = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };

        private static int[,] stepsLovac = { { 1, 1 }, { -1, -1 }, { -1, 1 }, { 1, -1 } };

        private Hashtable ugaseneVatre = new Hashtable();
        
        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;
            rez.rezervoar = this.rezervoar; //da bi zapamtio da je pokupio kutiju

            foreach (DictionaryEntry hash in this.ugaseneVatre)
            {
                rez.ugaseneVatre.Add(hash.Key, null);
            }

            if (lavirint[markI, markJ] == 4) // rezervoar
            {
                rez.rezervoar = true;
            }

            if(lavirint[markI, markJ] == 5 && rez.rezervoar && !ugaseneVatre.ContainsKey(markI * 10 + markJ)) //vatre
            {
                rez.ugaseneVatre.Add(markI * 10 + markJ, null);
            }


            return rez;
        }

        
        public List<State> mogucaSledecaStanja()
        {
            //TODO 1: Implementirati metodu tako da odredjuje dozvoljeno kretanje u lavirintu
            //TODO 2: Prosiriti metodu tako da se ne moze prolaziti kroz sive kutije
            List<State> rez = new List<State>();

            if (!rezervoar)
            {
                lovacKretanje = false;
                steps = stepsLovac;
            }
            else
            {
                lovacKretanje = false;
                steps = stepsNorm;
            }


            for (int i = 0; i < steps.GetLength(0); i++)
            {
                if (!lovacKretanje)
                {
                    int newMarkI = this.markI + steps[i, 0]; //uzima kretanje za x
                    int newMarkJ = this.markJ + steps[i, 1]; //za y

                    //ukoliko je u granicama i nije naislo na zid, prelazi se na sledece
                    if (isWithinBounds(newMarkI, newMarkJ) && !isWall(newMarkI, newMarkJ))
                    {
                        rez.Add(sledeceStanje(newMarkI, newMarkJ));
                    }
                }
                else
                {
                    int j = 1;
                    while (true)
                    {
                        int nextI = this.markI + j * steps[i, 0]; //uzima kretanje za x
                        int nextJ = this.markJ + j * steps[i, 1]; //za y
                        ++j;
                        if (!isWithinBounds(nextI, nextJ) || isWall(nextI, nextJ))
                        {
                            break;
                        }
                        rez.Add(sledeceStanje(nextI, nextJ));
                    }
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
        //
        public override int GetHashCode()
        {
            int hcode = 10 * markI + markJ; //maks 99

            int i = 128;
            if (this.rezervoar)
            {
                hcode = hcode | i;
            }
            i <<= 1;
            foreach(Point p in Main.vatre)
            {
                if(ugaseneVatre.ContainsKey(10*p.X+p.Y))
                {
                    hcode = hcode | i;
                }
                i <<= 1;
            }
            return hcode;
        }

        public bool isKrajnjeStanje()
        {
            return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ && this.rezervoar && Main.vatre.Count == ugaseneVatre.Count;
        }//dodali jos proveru da li je kutija pokupljena, ne moze da se zavrsi dok ne pokupi

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
