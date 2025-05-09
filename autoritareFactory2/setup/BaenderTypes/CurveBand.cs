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
        public int RichtungAusgang; //1 -> Ausgang Rechts //2 -> Ausgang Unten //3 -> Ausgang Links //4 -> Ausgang Oben
        public int RichtungEingang; // Darf nicht selbe und nicht verkehrt herum wie Ausgang sein! Sollte ebenfalls die Richtung sein!.
        private System.Windows.Forms.Timer cooldownTimer = new System.Windows.Forms.Timer();
        public CurveBand(int richtung, int itemAnzahlMoment, int positionX, int positionY, int richtungAusgang, WorldMap world)
            : base(richtung, itemAnzahlMoment, positionX, positionY, world)
        {
            RichtungAusgang = richtungAusgang;
            RichtungEingang = richtung;
        }
        public override void Iteration()
        {
            InNaechsteBand(this, wrld);
            UpdateRescourceList();
        }

        public override void InNaechsteBand(Band band, WorldMap world)
        {
            base.InNaechsteBand(band, world);
            Band BandNxt;
            //Schaue alle benachbarten tiles
            //Wenn die Drehung des Ausgangs gleich die Drehung des Eingangs des nächsten Bandes ist, übergebe rescourcen.

            int wertRotX = 0;
            int wertRotY = 0;

            switch (band.RichtungAusgang)
            {
                case 1:
                    wertRotX = 1;
                    break;
                case 2:
                    wertRotY = -1;
                    break;
                case 3:
                    wertRotX = -1;
                    break;
                case 4:
                    wertRotY = 1;
                    break;
            }

            List<Fabrikgebeude> values = world.GetEntityInPos(band.PositionX + wertRotX, band.PositionY + wertRotY);
            CurveBand curveBandNxt;
            foreach (Fabrikgebeude v in values)
            {
                if (v is Band)
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
                        if (curveBandNxt.Drehung != curveBandNxt.Drehung) continue; // Wenn Band Drehung nicht gleich ist mit benachbarte BandDrehung, dann nächste loop // Merge: curveBandNxt.DrehungAusgang?
                        if (curveBandNxt == gb) continue; //Wenn bereits etwas gefunden wurde, alles überspringen.

                        determineTransferCurve(gb, curveBandNxt); // Needs second curveBand variable.
                    }
                }
            }
        }

                    /*
                    foreach (Konstrucktor ko in world.GetEntityInBox(band.PositionX + wertRotX, band.PositionY + wertRotY, konstrucktor.längeInXDrehung, konstrucktor.längeInYDrehung))
                    {
                    if (values.Count == 1)
                    {
                        BandNxt = (Band)v;
                        determineTransfer(band, BandNxt);
                        //Damit man die Richtung des Bandes nehmen kann, muss man zunächst das Gebäude von der Liste holen.
                        foreach (Band gb in world.GetEntityInPos(band.PositionX + wertRotX, band.PositionY + wertRotY))
                        {
                            //BandNxt = gb;
                            //if (BandNxt.Richtung != band.Richtung) continue; // Wenn Band Richtung nicht gleich ist mit benachbarte Bandrichtung, dann nächste loop


                        }
                    }
                }
            }
        }*/

        public override void determineTransfer(Band band, Band BandNxt)
        {
            //base.determineTransfer(band, BandNxt);

            if (BandNxt.Drehung == band.Drehung)
            foreach (Resource resources in band.currentRescourceList)
            {
                if (BandNxt.ItemAnzahlMoment < BandNxt.ItemAnzahlMax)
                {
                    BandNxt.RescourceKommtAufBand(resources);
                    BandNxt.ErkenneRescourcen();

                    band.removedRescources.Add(resources);
                }
            }
        }

        public void determineTransferCurve(CurveBand band, CurveBand BandNxt) 
        {
            if (BandNxt.Drehung == band.Drehung)
            foreach (Resource resources in band.removedRescources)
            {
                band.currentRescourceList.Remove(resources);
            }
        }
    }
}