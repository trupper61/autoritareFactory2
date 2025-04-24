using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship
{
    public abstract class Fabrikgebeude
    {
        private static int gebeude_ID = 0;
        private int gebeudeID = 0;
        private int positionX;
        private int positionY;
        public int GebeudeID { get { return gebeudeID; } }//ist eine id bei der jedes gebeude eine eigene hat
        public int PositionX { get { return positionX; } }//ist die Position des Feldes oben Links in der Ecke vom gebeude
        public int PositionY { get { return positionY; } }
        public int SizeX { get { return längeInXRichtung; } }
        public int SizeY { get { return längeInYRichtung; } }
        internal int längeInXRichtung;
        public int LängeInXRichtung { get { return längeInXRichtung; } }
        internal int längeInYRichtung;
        public int LängeInYRichtung { get { return längeInYRichtung; } }
        internal int drehung; // ??? wert der Dreung 1: Eingang links, Ausgang rechts und dann im Urzeigersinn bis 4: Engang unten, Ausgang oben
        public int Drehung { get { return drehung; } }

        public Fabrikgebeude(int positionX, int positionY)
        {
            gebeude_ID++;
            gebeudeID = gebeude_ID;
            this.positionX = positionX;
            this.positionY = positionY;
        }
        public Fabrikgebeude(int positionX, int positionY,int drehung)
        {
            gebeude_ID++;
            gebeudeID = gebeude_ID;
            this.positionX = positionX;
            this.positionY = positionY;
            this.drehung = drehung;
        }
        public abstract void Iteration();
    }
}
