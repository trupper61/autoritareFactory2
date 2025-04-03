using autoritaereFactory;
using autoritaereFactory.setup;
using autoritaereFactory.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace factordictatorship.setup
{
    public class Band : Fabrikgebeude
    {
        public List<Resource> resource { get; }
        public int anzahlEisen;
        public int ItemAnzahlMax = 10; //Anzahl an Items, die ein Band maximal halten kann.
        public int ItemAnzahlMoment; //Anzahl an Items, die sich gerade auf dem Band befinden.
        public int BandGeschwindigkeit = 5; //Wie schnell es Items von einem auf das andere Band befördern kann. (Item Pro Sekunde)
        public int Richtung; //1 -> Links nach Rechts||| 2 -> Oben nach Unten||| 3 -> Rechts nach Links||| 4 -> Unten nach Oben|||

        public Band (int bandGe, int itemAnMax, List<Resource> resources, int richtung, int anzEisen, int positionX, int positionY)
            :base(positionX, positionY) 
        {
            anzEisen = anzahlEisen;
            resources = resource;
            richtung = Richtung;
            ItemAnzahlMax = itemAnMax;
        }
        public override void Iteration()
        {
            throw new NotImplementedException();
        }
        public void ErkenneRescourcen() 
        {
            if(resource != null) 
            {
                foreach (Resource r in resource) 
                {
                    if(r.Type == ResourceType.Iron) 
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
        public void InNaechsteBand(Band band, Band BandNxt,WorldMap world) 
        {
            //Schaue alle benachbarten tiles an. Schaue wo ein Band ist. Wenn Band ist nehme die Richtung dieses Bandes. Wenn Bandrichtung gleich ist wie momentaner Band,
            //Transfer rescourcen.
            int wertRotX = 0;
            int wertRotY = 0;

            for(int i = 4; i > 0; i--) 
            {
                switch(i) 
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
                if(world.GetEntityInPos(band.PositionX + wertRotX, band.PositionY + wertRotY).Count == 1) 
                {
                    //Damit man die Richtung des Bandes nehmen kann, muss man zunächst das Gebäude von der Liste holen.
                    foreach(Band gb in world.GetEntityInPos(band.PositionX + wertRotX, band.PositionY + wertRotY)) 
                    {
                        BandNxt = gb;
                    }
                    if(BandNxt.Richtung == band.Richtung) 
                    {
                        foreach(Resource resources in resource) 
                        {
                            BandNxt.RescourceKommtAufBand(resources);
                            band.resource.Remove(resources);
                        }
                    }
                }
                
            }
        }

    }
}
