using autoritaereFactory.setup;
using factordictatorship.setup;
using factordictatorship.setup.BaenderTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public int SizeX { get { return drehung % 2 == 1 ? längeInXRichtung : längeInYRichtung; } }
        public int SizeY { get { return drehung % 2 == 0 ? längeInXRichtung : längeInYRichtung; } }
        internal int längeInXRichtung;
        public int LängeInXRichtung { get { return längeInXRichtung; } }
        internal int längeInYRichtung;
        public int LängeInYRichtung { get { return längeInYRichtung; } }
        internal int drehung; // ??? wert der Dreung 1: Eingang links, Ausgang rechts und dann im Urzeigersinn bis 4: Engang unten, Ausgang oben
        public int Drehung { get { return drehung; } }

        internal Fabrikgebeude() { }
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
        public virtual List<byte> GetAsBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(this.GetType().GetHashCode()));
            bytes.AddRange(BitConverter.GetBytes(gebeudeID));
            bytes.AddRange(BitConverter.GetBytes(positionX));
            bytes.AddRange(BitConverter.GetBytes(positionY));
            bytes.Add((byte)drehung);
            return bytes;
        }
        public abstract void Iteration();

        internal static Fabrikgebeude FromByteArray(byte[] bytes, ref int offset)
        {
            int classHash = BitConverter.ToInt32(bytes, offset);
            int gebId = BitConverter.ToInt32(bytes, offset + 4);
            int positionX = BitConverter.ToInt32(bytes, offset + 8);
            int positionY = BitConverter.ToInt32(bytes, offset + 12);
            int drehung = bytes[offset + 16];
            offset += 17;
            Fabrikgebeude newFbu = null;
            if (classHash == typeof(Band).GetHashCode())
            {
                newFbu = Band.FromByteArray(bytes, ref offset);
                ((Band)newFbu).Richtung = drehung;
            }
            else if (classHash == typeof(CurveBand).GetHashCode())
                newFbu = CurveBand.FromByteArray(bytes, ref offset);
            else if (classHash == typeof(Konstrucktor).GetHashCode())
                newFbu = Konstrucktor.FromByteArray(bytes, ref offset);
            else if (classHash == typeof(Miner).GetHashCode())
                newFbu = Miner.FromByteArray(bytes, ref offset);
            else if (classHash == typeof(Merger).GetHashCode())
                newFbu = Merger.FromByteArray(bytes, ref offset);
            else if (classHash == typeof(Splitter).GetHashCode())
                newFbu = Splitter.FromByteArray(bytes, ref offset);
            else if (classHash == typeof(Exporthaus).GetHashCode())
                newFbu = Exporthaus.FromByteArray(bytes, ref offset);

            else if (classHash == typeof(Fabrikator).GetHashCode())
                newFbu = Fabrikator.FromByteArray(bytes, ref offset);
            else if (classHash == typeof(Finishinator).GetHashCode())
                newFbu = Finishinator.FromByteArray(bytes, ref offset);
            else
            {
                throw new Exception("Could not load fabrikgebäude!");
            }
            newFbu.gebeudeID = gebId;
            newFbu.positionX = positionX;
            newFbu.positionY = positionY;
            newFbu.drehung = drehung;
            return newFbu;
        }
    }
}
