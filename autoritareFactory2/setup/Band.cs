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
        public int ItemAnzahlMax = 10; //Anzahl an Items, die ein Band maximal halten kann.
        public int ItemAnzahlMoment = 0; //Anzahl an Items, die sich gerade auf dem Band befinden.
        public int BandGeschwindigkeit = 200; //Wie schnell es Items von einem auf das andere Band befördern kann. (Item Pro Sekunde) -> 5 Items pro Sekunde
        public int Richtung; //1 -> Links nach Rechts||| 2 -> Oben nach Unten||| 3 -> Rechts nach Links||| 4 -> Unten nach Oben|||
        public int RichtungEingang; //1 -> Eingang Links //2 -> Eingang Oben //3 -> Eingang Rechts // 4 -> Eingang Unten
        public int RichtungAusgang; //1 -> Ausgang Rechts //2 -> Ausgang Unten //3 -> Ausgang Links //4 -> Ausgang Oben
        private System.Windows.Forms.Timer cooldownTimer = new System.Windows.Forms.Timer();

        public Band(int richtung, int itemAnzahlMoment, int positionX, int positionY, WorldMap wrld)
            : base(positionX, positionY)
        {
            itemAnzahlMoment = ItemAnzahlMoment;
            Richtung = richtung;
            längeInXRichtung = 1;
            längeInYRichtung = 1;
            this.wrld = wrld;

            
        }
        public override void Iteration()
        {
            InNaechsteBand(this, wrld);
            UpdateRescourceList();
        }
        public void ErkenneRescourcen()
        {
            if (currentRescourceList != null && ItemAnzahlMoment < ItemAnzahlMax)
            {
                foreach (Resource r in currentRescourceList)
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
            currentRescourceList.Add(r);
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


            switch (band.Richtung)
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

            foreach (Fabrikgebeude v in values)
            {
                if (v is Band)
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
                }
            }
            foreach(Resource resources in band.removedRescources) 
            {
                band.currentRescourceList.Remove(resources);
            }
        }

        public override string ToString()
        {
            return "Band";
        }
    }
}