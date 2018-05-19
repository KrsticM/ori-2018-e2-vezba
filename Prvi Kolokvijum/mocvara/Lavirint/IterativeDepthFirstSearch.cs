using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace Lavirint
{
    class IterativeDeepFirstSeach
    {
        public State search(State state, int maxDepth) //prosledjeni su pocetno stanje i maksimalna dubina do koje ide pretraga
        {
            //TODO 7: Implementirati iterativnu prvi u dubinu pretragu
            //Slepa pretraga

            //Pretrazuje se nivo po nivo -> stanja u istom nivou imaju jednak put do sebe
            for (int i = 0; i < maxDepth; ++i) // i - trenutni nivo koji se pretrazuje
            {
                List<State> stanjaZaObradu = new List<State>();
                stanjaZaObradu.Add(state);

                while (stanjaZaObradu.Count > 0)
                {
                    State trenutnoStanje = stanjaZaObradu[0];
                    stanjaZaObradu.Remove(trenutnoStanje);

                    if (trenutnoStanje.cost < i) //ako je na manjem nivou dubine, razvija se
                    {
                        stanjaZaObradu.InsertRange(0, trenutnoStanje.mogucaSledecaStanja());
                    }
                    else if (trenutnoStanje.cost > i) //ako je na vecem nivou dubine, odbacuje se
                    {
                        continue;
                    }
                    else if (trenutnoStanje.cost == 1)//ako je na nivou i, proverava se
                    {
                        Main.allSearchStates.Add(trenutnoStanje); //za prikaz u debug
                        if (trenutnoStanje.isKrajnjeStanje() == true)
                        {
                            return trenutnoStanje;
                        }
                    }

                }

            }

            return null;
        }
    }
}