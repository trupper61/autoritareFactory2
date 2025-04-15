using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship.world
{
    public class Lagergebaeude : Fabrikgebeude
    {
        private int drehung; //wert der Dreung 1: Eingang links, Ausgang rechts und dann im Urzeigersinn bis 4: Engang unten, Ausgang oben
        public Lagergebaeude(int positionX, int positionY, int Drehung) 
            : base(positionX, positionY)
        {
            Drehung = drehung;
        }
        public override void Iteration() 
        {
            throw new NotImplementedException();
        }

        //TODO: Lagerhaus soll von Band nehmen.

        public void NimmVonBand() 
        {
            //Wenn Die Richtung des Eingangs des Lagers gleich die Richtung des Bandes ist, dann rescourcen, die sich auf dem Band befinden nehmen.
        }

        //TODO Lagerhaus soll auf Band platzieren.

        public void PlatziereAufBand() 
        {
            
        }
    }
}
