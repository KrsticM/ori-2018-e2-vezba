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
        public Boolean kutijaPlava; //0 nije, 1 jeste
        public Boolean kutijaNara;
        //kretanje kao top
        private static int[,] steps = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };

        private static int[,] stepsNorm = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };
        //kretanje kao konj
        private static int[,] stepsKonj = { { 1, 2 }, { -1, 2 }, { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 }, { 1, -2 }, { -1, -2 } };

        private static int[,] stepsDiag = { { 1, 1 }, { -1, -1 }, { -1, 1 }, { 1, -1 } };

        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;
            rez.kutijaPlava = this.kutijaPlava; //da bi zapamtio da je pokupio kutiju
            rez.kutijaNara = this.kutijaNara;

            //ako zelimo da menjamo kretanje robota posle kupljenja kutije
 
            if (lavirint[markI, markJ] == 4)
            {
                rez.kutijaPlava = true;
            }
            if (lavirint[markI, markJ] == 5 && rez.kutijaPlava)
            {
                rez.kutijaNara = true;
            }
            return rez;
        }


        public List<State> mogucaSledecaStanja()
        {
            //TODO 1: Implementirati metodu tako da odredjuje dozvoljeno kretanje u lavirintu
            //TODO 2: Prosiriti metodu tako da se ne moze prolaziti kroz sive kutije
            List<State> rez = new List<State>();

            if (!kutijaPlava || !kutijaNara)
            {
                // ako nisu pokupljene plava ili narandzasta
                // samo u slucaju kada su pokupljene obe kutije preskacemo ovaj deo
                // proverim da li sam levo ili desno
                if(this.markJ+1 <= Main.brojKolona/2)
                {
                    steps = stepsKonj;
                }
                else
                {
                    steps = stepsNorm;
                }
            }
            else
            {
                steps = stepsDiag;
            }


            for (int i = 0; i < steps.GetLength(0); i++)
            {
                int newMarkI = this.markI + steps[i, 0]; //uzima kretanje za x
                int newMarkJ = this.markJ + steps[i, 1]; //za y

                //ukoliko je u granicama i nije naislo na zid, prelazi se na sledece
                if(isWithinBounds(newMarkI, newMarkJ) && !isWall(newMarkI, newMarkJ))
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
        //
        public override int GetHashCode()
        {
            int hcode = 100 * markI + markJ; //maks 99

            if (this.kutijaNara)
            {
                hcode += 1000;
            }
            if (this.kutijaPlava)
            {
                hcode += 10000;
            }

            return hcode;
        }

        public bool isKrajnjeStanje()
        {
            return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ && this.kutijaNara && this.kutijaPlava;
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
