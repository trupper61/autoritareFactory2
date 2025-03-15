using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fabrik1Console
{
    public abstract class Fabrikgebeude
    {
        private static int gebeude_ID = 0;
        private int gebeudeID = 0;
        public int GebeudeID { get { return gebeudeID; } }//ist eine id bei der jedes gebeude eine eigene hat
        private int positionX;
        public int PosiX { get { return positionX; } }//ist die Position des Feldes oben Links in der Ecke vom gebeude
        private int positionY;
        public int PositionY { get { return positionY; } }
        /*private int längeInXRichtung;
        public int LängeInXRichtung { get { return längeInXRichtung; } }
        private int längeInYRichtung;
        public int LängeInYRichtung { get { return längeInYRichtung; } }*/
        public Fabrikgebeude(int positionX, int positionY)
        {
            gebeude_ID++;
            gebeudeID = gebeude_ID;
            this.positionX = positionX;
            this.positionY = positionY;
        }
    }
}
