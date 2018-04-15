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
        //public Boolean kutija; //0 nije, 1 jeste

        //kretanje kao top
        private static int[,] steps = { { 0, 1}, { 0, -1 }, { -1, 0 }, { 1, 0 } };
        //kretanje kao 
        private static int[,] stepsBeforeSix = { { 1, 2 }, { -1, 2 }, { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 }, { 1, -2 }, { -1, -2 } };
        private static int[,] stepsAfterSix = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 }, { 2, 0 }, { 0, -2 }, { -2, 0 }, { 0, 2 } };
        private static int[,] stepsCikCak = { { 1, 1 }, { -1, -1 }, { 1, -1 }, { -1, 1 } , { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };


        private Hashtable posecenePlave = new Hashtable();
        private Hashtable poseceneZute = new Hashtable();
        private int poeni = 0;


        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;
          //  rez.kutija = this.kutija; //da bi zapamtio da je pokupio kutiju
            rez.poeni = this.poeni;


            //preuzimamo plave
            foreach (DictionaryEntry hash in this.posecenePlave)
            {
                rez.posecenePlave.Add(hash.Key, null);
            }

            foreach (DictionaryEntry hash in this.poseceneZute)
            {
                rez.poseceneZute.Add(hash.Key, null);
            }

            if (lavirint[markI, markJ] == 4 && !posecenePlave.ContainsKey(markI * 10 + markJ) && poeni < 11) //plave
            {
                rez.posecenePlave.Add(markI * 10 + markJ, null);
                rez.poeni+= 1;
            }

            if (lavirint[markI, markJ] == 5 && !poseceneZute.ContainsKey(markI * 10 + markJ) && poeni < 8) //zute
            {
                rez.poseceneZute.Add(markI * 10 + markJ, null);
                rez.poeni += 3;
            }

            return rez;
        }

        
        public List<State> mogucaSledecaStanja()
        {
            //TODO 1: Implementirati metodu tako da odredjuje dozvoljeno kretanje u lavirintu
            //TODO 2: Prosiriti metodu tako da se ne moze prolaziti kroz sive kutije
            List<State> rez = new List<State>();

            if(poeni <= 6)
            {
                steps = stepsBeforeSix;
            }
            else if(poeni > 6 && poeni < 11)
            {
                steps = stepsAfterSix;
            }
            else
            {
                steps = stepsCikCak;
            }

            for(int i = 0; i < steps.GetLength(0); i++)
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

            i = 524288;

            foreach (Point point in Main.zute) //iterira se kroz listu svih obaveznih
            {
                if (poseceneZute.ContainsKey(point.X * 10 + point.Y) == true) //provera da li je trenutno obavezno polje poseceno
                {
                    key = key | i; //1 se upisuje na bit koji označava da je to obavezno polje poseceno
                }
                i = i << 1; //shift za jedno mesto u levo. npr: 1000 0000 postaje 1 0000 0000 i tako se dobije sledeća obavezno polje
            }

            return key;



        }

        public bool isKrajnjeStanje()
        {
            Console.WriteLine("Krajnje stanje - poeni " + this.poeni);
            return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ && (this.poeni == 11);
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
