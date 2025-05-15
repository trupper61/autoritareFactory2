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
        public List<Resource> resource { get; } = new List<Resource>();
        public List<Resource> currentRescourceList = new List<Resource>();
        public List<Resource> removedRescources = new List<Resource>();
        public WorldMap wrld;
        public int anzahlEisen;
        public int anzahlCopper;
        public int anzahlKohle;
        public int ItemAnzahlMax = 10; //Anzahl an Items, die ein Band maximal halten kann.
        public int ItemAnzahlMoment = 0; //Anzahl an Items, die sich gerade auf dem Band befinden.
        public int BandGeschwindigkeit = 200; //Wie schnell es Items von einem auf das andere Band befördern kann. (Item Pro Sekunde) -> 5 Items pro Sekunde
        internal Band() : base()
        {
            längeInXRichtung = 1;
            längeInYRichtung = 1;
            ItemAnzahlMoment = currentRescourceList.Count();
        }

        public int RichtungEingang; //1 -> Eingang Links //2 -> Eingang Oben //3 -> Eingang Rechts // 4 -> Eingang Unten
        public int RichtungAusgang; //1 -> Ausgang Rechts //2 -> Ausgang Unten //3 -> Ausgang Links //4 -> Ausgang Oben
        private System.Windows.Forms.Timer cooldownTimer = new System.Windows.Forms.Timer();
        public int Richtung; // For Merge-Fix

        public Band(int richtung, int itemAnzahlMoment, int positionX, int positionY, WorldMap wrld)
            : base(positionX, positionY,richtung)
        {
            itemAnzahlMoment = ItemAnzahlMoment;
            Richtung = richtung;
            RichtungAusgang = richtung;
            längeInXRichtung = 1;
            längeInYRichtung = 1;
            this.wrld = wrld;

            
        }
        public override void Iteration()
        {
            InNaechsteBand(this, wrld);
            ItemAnzahlMoment = currentRescourceList.Count();
            UpdateRescourceList();
        }
        public virtual void ErkenneRescourcen()
        {
            if (currentRescourceList != null && ItemAnzahlMoment < ItemAnzahlMax)
            {
                foreach (Resource r in currentRescourceList)
                {
                    if (r.Type == ResourceType.IronOre)
                    {
                        anzahlEisen++;
                    }
                    else if (r.Type == ResourceType.CopperOre)
                    {
                        anzahlCopper++;
                    }
                    else if (r.Type == ResourceType.ColeOre)
                    {
                        anzahlCopper++;
                    }
                }
            }
        }

        public virtual void RescourceKommtAufBand(Resource r)
        {
            currentRescourceList.Add(r);
            ItemAnzahlMoment++;
        }
        public void UpdateRescourceList()
        {
            //currentRescourceList = resource;
        }
        public Resource NimmRescourceVomBand(int stelleInResourcedieGenommenWerdenSoll)//von Markus ich brauche den zukrifsrecht um auch (List<Resource> resource) zu ändern
        {
            Resource r = currentRescourceList[stelleInResourcedieGenommenWerdenSoll];
            currentRescourceList.RemoveAt(stelleInResourcedieGenommenWerdenSoll);
            return r;
        }
        public virtual void InNaechsteBand(Band band, WorldMap world)
        {
            Band BandNxt;
            //Schaue alle benachbarten tiles an. Schaue wo ein Band ist. Wenn Band ist nehme die Richtung dieses Bandes. Wenn Bandrichtung gleich ist wie momentaner Band,
            //Transfer rescourcen.
            int wertRotX = 0;
            int wertRotY = 0;


            switch (band.Drehung)
            {
                case 1:
                    wertRotX = 1;
                    break;
                case 2:
                    wertRotY = 1;
                    break;
                case 3:
                    wertRotX = -1;
                    break;
                case 4:
                    wertRotY = -1;
                    break;
            }

            List<Fabrikgebeude> values = WorldMap.theWorld.GetEntityInPos(band.PositionX + wertRotX, band.PositionY + wertRotY);

            foreach (Fabrikgebeude v in values)
            {
                if (v is Band)
                {
                    if (values.Count == 1)
                    {
                        BandNxt = (Band)v;
                        determineTransfer(band, BandNxt);
                        //Damit man die Richtung des Bandes nehmen kann, muss man zunächst das Gebäude von der Liste holen.
                        foreach (Band gb in WorldMap.theWorld.GetEntityInPos(band.PositionX + wertRotX, band.PositionY + wertRotY))
                        {
                            //BandNxt = gb;
                            //if (BandNxt.Richtung != band.Richtung) continue; // Wenn Band Richtung nicht gleich ist mit benachbarte Bandrichtung, dann nächste loop
                        }
                    }
                }
            }
        }
        public virtual void determineTransfer(Band band, Band BandNxt) //Der Prozess, bei dem die Rescourcen in einem zeitlichen Rahmen auf das nächste Band transferiert werden.
        {
            foreach (Resource resources in band.currentRescourceList)
            {
                if(BandNxt.ItemAnzahlMoment < BandNxt.ItemAnzahlMax) 
                {
                    BandNxt.RescourceKommtAufBand(resources);
                    BandNxt.ErkenneRescourcen();
                    
                    band.removedRescources.Add(resources);
                    //band.ItemAnzahlMoment--;
                }
            }
            foreach(Resource resources in band.removedRescources) 
            {
                band.currentRescourceList.Remove(resources);
                ItemAnzahlMoment = currentRescourceList.Count();
            }
            band.removedRescources.Clear();
        }

        public override string ToString()
        {
            return "Band";
        }
        public override List<byte> GetAsBytes()
        {
            List<byte> bytes = base.GetAsBytes();
            bytes.AddRange(BitConverter.GetBytes((int)removedRescources.Count));
            for (int i = 0; i < removedRescources.Count; i++)
            {
                bytes.AddRange(BitConverter.GetBytes((int)removedRescources[i].Type));
            }
            bytes.AddRange(BitConverter.GetBytes((int)currentRescourceList.Count));
            for (int i = 0; i < currentRescourceList.Count; i++)
            {
                bytes.AddRange(BitConverter.GetBytes((int)currentRescourceList[i].Type));
            }
            return bytes;
        }
        public static Band FromByteArray(byte[] bytes, ref int offset)
        {
            Band newBand = new Band();
            int resourceCount = BitConverter.ToInt32(bytes, offset);
            offset += 4;
            for (int i = 0; i < resourceCount; i++)
            {
                newBand.removedRescources.Add(new Resource((ResourceType)BitConverter.ToInt32(bytes, offset)));
                offset += 4;
            }
            resourceCount = BitConverter.ToInt32(bytes, offset);
            offset += 4;
            for (int i = 0; i < resourceCount; i++)
            {
                newBand.currentRescourceList.Add(new Resource((ResourceType)BitConverter.ToInt32(bytes, offset)));
                offset += 4;
            }
            return newBand;
        }

        public string RetWantedRescource(ResourceType type, Band band) 
        {
            int zaehler = 0;
            foreach(Resource res in band.currentRescourceList) 
            {
                if(res.Type == type) 
                {
                    zaehler++;
                }
            }

            return zaehler.ToString();
        }
        public virtual int GibRichtungAusgang()
        {
            return Richtung;
        }
        public virtual int GibRichtungEingang()
        {
            return Richtung;
        }
    }
}