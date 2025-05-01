using autoritaereFactory;
using autoritaereFactory.setup;
using autoritaereFactory.world;
using factordictatorship.Properties;
using factordictatorship.setup.BaenderTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace factordictatorship.setup
{
    public class Band : Fabrikgebeude
    {
        public List<Resource> resource { get; }
        public int anzahlEisen;
        public int ItemAnzahlMax = 10; //Anzahl an Items, die ein Band maximal halten kann.
        public int ItemAnzahlMoment = 0; //Anzahl an Items, die sich gerade auf dem Band befinden.
        public int BandGeschwindigkeit = 200; //Wie schnell es Items von einem auf das andere Band befördern kann. (Item Pro Sekunde) -> 5 Items pro Sekunde
        private System.Windows.Forms.Timer cooldownTimer = new System.Windows.Forms.Timer();

        internal Band():base()
        {
            längeInXRichtung = 1;
            längeInYRichtung = 1;
            resource = new List<Resource>();
        }
        public Band(int richtung, int itemAnzahlMoment, int positionX, int positionY)
            : base(positionX, positionY,richtung)
        {
            itemAnzahlMoment = ItemAnzahlMoment;
            //drehung = richtung;
            //richtung = Richtung;
            längeInXRichtung = 1;
            längeInYRichtung = 1;
        }
        public override void Iteration()
        {
            //throw new NotImplementedException();
        }
        public void ErkenneRescourcen()
        {
            if (resource != null)
            {
                foreach (Resource r in resource)
                {
                    if (r.Type == ResourceType.IronOre)
                    {
                        anzahlEisen++;
                    }
                    ItemAnzahlMoment++;
                }
            }
        }

        public void RescourceKommtAufBand(Resource r)
        {
            resource.Add(r);
        }
        public Resource NimmRescourceVomBand(int stelleInResourcedieGenommenWerdenSoll)//von Markus ich brauche den zukrifsrecht um auch (List<Resource> resource) zu ändern
        {
            Resource r = resource[stelleInResourcedieGenommenWerdenSoll];
            resource.RemoveAt(stelleInResourcedieGenommenWerdenSoll);
            return r;
        }
        public virtual void InNaechsteBand(Band band, Band BandNxt, CurveBand bandCur, WorldMap world, Konstrucktor konstrucktor)
        {
            //Schaue alle benachbarten tiles an. Schaue wo ein Band ist. Wenn Band ist nehme die Richtung dieses Bandes. Wenn Bandrichtung gleich ist wie momentaner Band,
            //Transfer rescourcen.
            int wertRotX = 0;
            int wertRotY = 0;

            for (int i = 4; i > 0; i--)
            {
                switch (i)
                {
                    case 4:
                        wertRotX = -1;
                        break;
                    case 3:
                        wertRotX = 1;
                        break;
                    case 2:
                        wertRotY = -1;
                        wertRotX = 0;
                        break;
                    case 1:
                        wertRotY = 1;
                        break;
                }
                if (world.GetEntityInPos(band.PositionX + wertRotX, band.PositionY + wertRotY).Count == 1)
                {
                    //Damit man die Richtung des Bandes nehmen kann, muss man zunächst das Gebäude von der Liste holen.
                    foreach (Band gb in world.GetEntityInPos(band.PositionX + wertRotX, band.PositionY + wertRotY))
                    {
                        if (BandNxt.drehung != band.drehung) continue; // Wenn Band Richtung nicht gleich ist mit benachbarte Bandrichtung, dann nächste loop
                        if (BandNxt == gb) continue; //Wenn bereits etwas gefunden wurde, alles überspringen.
                        BandNxt = gb;
                        determineTransfer(band, BandNxt, bandCur);
                    }

                    /*
                    foreach(Konstrucktor ko in world.GetEntityInBox(band.PositionX + wertRotX, band.PositionY + wertRotY, konstrucktor.längeInXRichtung, konstrucktor.längeInYRichtung)) 
                    {
                        
                    }
                    */
                }

            }
        }
        public virtual void determineTransfer(Band band, Band BandNxt, CurveBand curveBand) //Der Prozess, bei dem die Rescourcen in einem zeitlichen Rahmen auf das nächste Band transferiert werden.
        {
            if (BandNxt.drehung == band.drehung)
            {
                foreach (Resource resources in band.resource)
                {
                    cooldownTimer.Interval = BandGeschwindigkeit;
                    cooldownTimer.Start();

                    if (cooldownTimer.Interval == 0)
                    {
                        BandNxt.RescourceKommtAufBand(resources);
                        band.resource.Remove(resources);
                        cooldownTimer.Dispose();
                    }

                }
            }
        }
        public override List<byte> GetAsBytes()
        {
            List<byte> bytes = base.GetAsBytes();
            if (resource == null)
                bytes.AddRange(BitConverter.GetBytes((int)0));
            else
            {
                bytes.AddRange(BitConverter.GetBytes((int)resource.Count));
                for (int i = 0; i < resource.Count; i++)
                {
                    bytes.AddRange(BitConverter.GetBytes((int)resource[i].Type));
                }
            }
            return bytes;
        }
        public static Band FromByteArray(byte[] bytes, ref int offset)
        {
            Band newBand = new Band();
            int resourceCount = BitConverter.ToInt32 (bytes, offset);
            offset += 4;
            for (int i = 0; i < resourceCount; i++)
            {
                newBand.resource.Add(new Resource((ResourceType)BitConverter.ToInt32(bytes,offset)));
                offset+= 4;
            }
            return newBand;
        }
    }
}