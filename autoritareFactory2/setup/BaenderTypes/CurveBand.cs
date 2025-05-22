using autoritaereFactory;
using autoritaereFactory.world;
using factordictatorship.Resources;

//using factordictatorship.rotation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace factordictatorship.setup.BaenderTypes
{
    //Curved Band Von Links nach Oben
    public class CurveBand : Band
    {
        public int RichtungAusgang; //1 -> Ausgang Rechts //2 -> Ausgang Unten //3 -> Ausgang Links //4 -> Ausgang Oben
        public int RichtungEingang; // Darf nicht selbe und nicht verkehrt herum wie Ausgang sein! Sollte ebenfalls die Richtung sein!.
        public bool Modus; // Richtungsmodus
        private System.Windows.Forms.Timer cooldownTimer = new System.Windows.Forms.Timer();
        public CurveBand(int richtung, int itemAnzahlMoment, int positionX, int positionY, int richtungAusgang, WorldMap world, bool modus)
            : base(richtung, itemAnzahlMoment, positionX, positionY, world)
        {
            RichtungAusgang = richtungAusgang;
            RichtungEingang = richtung;
            Modus = modus;
        }
        public override void Iteration()
        {
            InNaechsteBand(this, wrld);
        }

        public override void ErkenneRescourcen()
        {
            base.ErkenneRescourcen();
        }

        public override void RescourceKommtAufBand(Resource r)
        {
            base.RescourceKommtAufBand(r);
        }

        public override void InNaechsteBand(Band band, WorldMap world)
        {
            Band BandNxt;
            //Schaue alle benachbarten tiles
            //Wenn die Drehung des Ausgangs gleich die Drehung des Eingangs des nächsten Bandes ist, übergebe rescourcen.
            bool moduswurdeBedient = false;
            int wertRotX = 0;
            int wertRotY = 0;
            if(!Modus)
            {
                if(moduswurdeBedient == true) 
                {
                    switch (band.RichtungAusgang)
                    {
                        case 1:
                            band.RichtungAusgang = 3;
                            break;
                        case 2:
                            RichtungAusgang = 4;
                            break;
                        case 3:
                            band.RichtungAusgang = 1;
                            break;
                        case 4:
                            band.RichtungAusgang = 2;
                            break;
                    }
                }
                switch (band.RichtungAusgang)
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
                moduswurdeBedient = false;
            }
            else 
            {
                moduswurdeBedient = true;
                switch (band.RichtungAusgang)
                {
                    case 1:
                        wertRotX = -1;
                        band.RichtungAusgang = 3;
                        break;
                    case 2:
                        wertRotY = -1;
                        RichtungAusgang = 4;
                        break;
                    case 3:
                        wertRotX = 1;
                        band.RichtungAusgang = 1;
                        break;
                    case 4:
                        wertRotY = 1;
                        band.RichtungAusgang = 2;
                        break;
                }
            }

            List<Fabrikgebeude> values = WorldMap.theWorld.GetEntityInPos(band.PositionX + wertRotX, band.PositionY + wertRotY);
            CurveBand curveBandNxt;
            foreach (Fabrikgebeude v in values)
            {
                if (v is Band)
                {
                    if (values.Count == 1)
                    {
                        BandNxt = (Band)v;
                        CurveBand curveBand = (CurveBand) band;
                        curveBand.determineTransfer(band, BandNxt);
                    }
                }
            }
        }

        public override void determineTransfer(Band band, Band BandNxt)
        {
            CurveBand curveBand = (CurveBand)band;
            foreach (Resource resources in band.currentRescourceList)
            {
                if (BandNxt.ItemAnzahlMoment < BandNxt.ItemAnzahlMax)
                {
                    BandNxt.RescourceKommtAufBand(resources);
                    BandNxt.ErkenneRescourcen();
                    
                    band.removedRescources.Add(resources);
                }
            }
            foreach (Resource resources in band.removedRescources)
            {
                band.currentRescourceList.Remove(resources);
                ItemAnzahlMoment = currentRescourceList.Count();
            }
            band.removedRescources.Clear();
        }
        internal CurveBand() : base()
        {
            längeInXRichtung = 1;
            längeInYRichtung = 1;
            ItemAnzahlMoment = currentRescourceList.Count();
        }
        public override List<byte> GetAsBytes()
        {
            List<byte> bytes = base.GetAsBytes();
            bytes.Add((byte)RichtungAusgang);
            return bytes;
        }
        public static new CurveBand FromByteArray(byte[] bytes, ref int offset)
        {
            CurveBand newBand = new CurveBand();
            int resourceCount = BitConverter.ToInt32(bytes, offset + 1);
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
            newBand.RichtungAusgang = bytes[offset++];
            return newBand;
        }

        public void determineTransferCurve(CurveBand band, CurveBand BandNxt) 
        {
            if (BandNxt.Drehung == band.Drehung)
            foreach (Resource resources in band.removedRescources)
            {
                band.currentRescourceList.Remove(resources);
            }
        }
        public override int GibRichtungAusgang()
        {
            return RichtungAusgang;
        }
        public override int GibRichtungEingang()
        {
            return RichtungEingang;
        }
    }
}