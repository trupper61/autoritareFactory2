using factordictatorship.setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship.world
{
    public class Lagergebaeude : Fabrikgebeude
    {
        public int drehung; //wert der Dreung 1: Eingang links, Ausgang rechts und dann im Urzeigersinn bis 4: Engang unten, Ausgang oben
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

        public void NimmVonBand(Lagergebaeude lager) 
        {
            //Wenn Die Richtung des Eingangs des Lagers gleich die Richtung des Bandes ist, dann rescourcen, die sich auf dem Band befinden nehmen.
            int Richtung;
            int wertRotX = 0;
            int wertRotY = 0;

            switch(drehung) 
            {
                case 1:
                    Richtung = 1;
                    wertRotX = -1;
                    break;
                case 2:
                    Richtung = 2;
                    wertRotY = 1;
                    break;
                case 3:
                    Richtung = 3;
                    wertRotX = 1;
                    break;
                case 4:
                    Richtung = 4;
                    wertRotY = -1;
                    break;
            }

            if (world.GetEntityInPos(lager.PositionX + wertRotX, lager.PositionY + wertRotY).Count > 0) 
            {
                
            }
        }

        //TODO Lagerhaus soll auf Band platzieren.

        public void PlatziereAufBand() 
        {
            
        }
    }
}
