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
        public Boolean kutija; //0 nije, 1 jeste
        //gore, dole,levo,desno 
        // kretanje kralja
        private static int[,] steps = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };

        //ako ide kao lovac
        //private static int[,] steps = { { 1, 1 }, { 1, -1}, {-1, 1}, {-1, -1} };
        
        //ako ide kao konj
        private static int[,] stepsAfterBox = { { 1, 2}, {-1,2}, {2,1} , {2,-1}, {-2,1}, {-2,-1}, {1,-2}, {-1,-2}};

        // kraljica = kralj + lovac
        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;
            //za kutiju
            rez.kutija = this.kutija;//da bi zapamtio da je pokupio
            /*if(rez.kutija)
            {
                steps = stepsAfterBox;
            }za menjanje kretanja posle kupljenja kutije*/

            if (lavirint[markI, markJ] == 4)
            {
                rez.kutija = true;
            }
                        
            return rez;
        }

        //
        public List<State> mogucaSledecaStanja()
        {
            List<State> rez = new List<State>();

            for(int i = 0; i < steps.GetLength(0); i++)
            {
                int newMarkI = this.markI + steps[i, 0];
                int newMarkJ = this.markJ + steps[i, 1];

                if(isWithinBounds(newMarkI,newMarkJ) && !isWall(newMarkI, newMarkJ))
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
            int hcode = 100*markI + markJ;
            if(this.kutija == true)
            {
                hcode += 1000;
            }
            return hcode;
        }

        public bool isKrajnjeStanje()
        {
            return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ && this.kutija;
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
