using autoritaereFactory;
using autoritaereFactory.setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship.setup
{
    public class Band : Building
    {
        public List<Resource> resource { get; } 
        public int ItemAnzahlMax; //Anzahl an Items, die ein Band maximal halten kann.
        public int ItemAnzahlMoment; //Anzahl an Items, die sich gerade auf dem Band befinden.
        public int BandGeschwindigkeit; //Wie schnell es Items von einem auf das andere Band befördern kann. (Item Pro Sekunde)

        public Band(int bandGe, int itemAnMax) 
        {
            BandGeschwindigkeit = bandGe;
            ItemAnzahlMax = itemAnMax;
            ItemAnzahlMoment = 0;
        }

        public void ErkenneRescourcen() 
        {
            if(resource != null) 
            {
                foreach (Resource r in resource) 
                {
                    ItemAnzahlMoment++;
                }
            }
        }

        public void RescourceKommtAufBand(Resource r) 
        {
            resource.Add(r);
        }

    }
}
