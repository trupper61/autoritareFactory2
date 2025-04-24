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
        public int RichtungAusgang;
        public int RichtungEingang; // Darf nicht selbe und nicht verkehrt herum wie Ausgang sein! Sollte ebenfalls die Richtung sein!.
        private System.Windows.Forms.Timer cooldownTimer = new System.Windows.Forms.Timer();
        public CurveBand(int richtung, int itemAnzahlMoment, int positionX, int positionY, int richtungEingang, int richtungAusgang)
            : base(richtung, itemAnzahlMoment, positionX, positionY)
        {
            richtungEingang = RichtungEingang;
            richtungAusgang = RichtungAusgang;
            richtung = RichtungEingang;
        }

        public override void InNaechsteBand(Band band, Band BandNxt, CurveBand curveBand, WorldMap world, Konstrucktor konstrucktor)
        {
            base.InNaechsteBand(band, BandNxt, curveBand, world, konstrucktor);

            //Schaue alle benachbarten tiles
            //Wenn die Richtung des Ausgangs gleich die Richtung des Eingangs des nächsten Bandes ist, übergebe rescourcen.

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
                        if (BandNxt.drehung != curveBand.RichtungAusgang) continue; // Wenn Band Richtung nicht gleich ist mit benachbarte Bandrichtung, dann nächste loop
                        if (BandNxt == gb) continue; //Wenn bereits etwas gefunden wurde, alles überspringen.
                        BandNxt = gb;
                        //determineTransfer(band, BandNxt);
                    }

                    /*
                    foreach (Konstrucktor ko in world.GetEntityInBox(band.PositionX + wertRotX, band.PositionY + wertRotY, konstrucktor.längeInXRichtung, konstrucktor.längeInYRichtung))
                    {

                    }
                    */
                }

            }
        }

        public override void determineTransfer(Band band, Band BandNxt, CurveBand curveBand)
        {
            base.determineTransfer(band, BandNxt, curveBand);

            if (BandNxt.drehung == curveBand.RichtungAusgang)
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
    }
}