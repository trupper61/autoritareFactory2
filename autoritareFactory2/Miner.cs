using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship
{
    public class Miner : Fabrikgebeude
    {
        private int Zähler;//beschränkt die anzahl Resurcen pro minute
        private string Recurse;//nur bisher String 
        private int anzahlRecurseVorhanden;
        public int AnzahlRecurseVorhanden { get { return anzahlRecurseVorhanden; } }
        public Miner(int positionX, int positionY, string recurse) : base(positionX, positionY)
        {
            Recurse = recurse;
        }
        public override void Iteration()
        {
            if (anzahlRecurseVorhanden < 100)
            {
                Zähler++;
                if (Zähler <= 10)
                {
                    anzahlRecurseVorhanden++;
                    Zähler = 0;
                }
            }
        }
    }
}
