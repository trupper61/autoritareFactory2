using autoritaereFactory;
using autoritaereFactory.world;
//using factordictatorship.rotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship.setup.BaenderTypes
{
    //Curved Band Von Links nach Oben
    public class CurveBand : Band
    {
        public int DrehungAusgang;
        public int DrehungEingang; // Darf nicht selbe und nicht verkehrt herum wie Ausgang sein! Sollte ebenfalls die Drehung sein!.
        private System.Windows.Forms.Timer cooldownTimer = new System.Windows.Forms.Timer();
        public CurveBand(int richtung, int itemAnzahlMoment, int positionX, int positionY, int richtungEingang, int richtungAusgang, WorldMap world)
            : base(richtungEingang, itemAnzahlMoment, positionX, positionY, world)
        {
            this.DrehungEingang = richtungEingang;
            this.DrehungAusgang = richtungAusgang;
            //Drehung = DrehungEingang;
        }

        public override void InNaechsteBand(Band band, WorldMap world)
        {
            base.InNaechsteBand(band, world);
            Band BandNxt;
            CurveBand curveBandNxt;
            CurveBand Band = (CurveBand)band;
            //Schaue alle benachbarten tiles
            //Wenn die Drehung des Ausgangs gleich die Drehung des Eingangs des nächsten Bandes ist, übergebe rescourcen.

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
                    //Damit man die Drehung des Bandes nehmen kann, muss man zunächst das Gebäude von der Liste holen.
                    foreach (Band gb in world.GetEntityInPos(band.PositionX + wertRotX, band.PositionY + wertRotY))
                    {
                        BandNxt = gb;
                        if (BandNxt.Drehung != band.Drehung) continue; // Wenn Band Drehung nicht gleich ist mit benachbarte BandDrehung, dann nächste loop
                        if (BandNxt == gb) continue; //Wenn bereits etwas gefunden wurde, alles überspringen.
                        
                        determineTransfer(band, BandNxt);
                    }

                    foreach (CurveBand gb in world.GetEntityInPos(band.PositionX + wertRotX, band.PositionY + wertRotY))
                    {
                        curveBandNxt = gb;
                        if (curveBandNxt.Drehung != curveBandNxt.DrehungAusgang) continue; // Wenn Band Drehung nicht gleich ist mit benachbarte BandDrehung, dann nächste loop
                        if (curveBandNxt == gb) continue; //Wenn bereits etwas gefunden wurde, alles überspringen.

                        determineTransferCurve(Band, curveBandNxt);
                    }

                    /*
                    foreach (Konstrucktor ko in world.GetEntityInBox(band.PositionX + wertRotX, band.PositionY + wertRotY, konstrucktor.längeInXDrehung, konstrucktor.längeInYDrehung))
                    {

                    }
                    */
                }

            }
        }

        public override void determineTransfer(Band band, Band BandNxt)
        {
            base.determineTransfer(band, BandNxt);

            if (BandNxt.Drehung == band.Drehung)
            {
                foreach (Resource resources in band.currentRescourceList)
                {
                    cooldownTimer.Interval = BandGeschwindigkeit;
                    cooldownTimer.Start();

                    if (cooldownTimer.Interval == 0)
                    {
                        BandNxt.RescourceKommtAufBand(resources);
                        band.currentRescourceList.Remove(resources);
                        cooldownTimer.Dispose();
                    }

                }
            }
        }

        public void determineTransferCurve(CurveBand band, CurveBand BandNxt) 
        {
            if (BandNxt.Drehung == band.Drehung)
            {
                foreach (Resource resources in band.currentRescourceList)
                {
                    cooldownTimer.Interval = BandGeschwindigkeit;
                    cooldownTimer.Start();

                    if (cooldownTimer.Interval == 0)
                    {
                        BandNxt.RescourceKommtAufBand(resources);
                        band.currentRescourceList.Remove(resources);
                        cooldownTimer.Dispose();
                    }

                }
            }
        }
    }
}