using Fabrik1Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autoritaereFactory
{
    internal class Miner : Fabrikgebeude
    {
        private string Recurse;
        private int anzahlRecurseVorhanden;
        public int AnzahlRecurseVorhanden { get { return anzahlRecurseVorhanden; } }
        public Miner(int positionX, int positionY, string recurse) : base(positionX, positionY)
        {
            Recurse = recurse;
        }
        public void Iteration()
        {
            if (anzahlRecurseVorhanden < 100)
            {
                anzahlRecurseVorhanden++;
            }
        }
    }
}
