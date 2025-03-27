using autoritaereFactory;
using autoritaereFactory.setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship.setup
{
    public class Band : Fabrikgebeude
    {
        public List<Resource> resource { get; }
        public int anzahlEisen;
        public int ItemAnzahlMax = 10; //Anzahl an Items, die ein Band maximal halten kann.
        public int ItemAnzahlMoment; //Anzahl an Items, die sich gerade auf dem Band befinden.
        public int BandGeschwindigkeit = 5; //Wie schnell es Items von einem auf das andere Band befördern kann. (Item Pro Sekunde)

        public Band (int bandGe, int itemAnMax, List<Resource> resources, int anzEisen,int positionX, int positionY)
            :base(positionX, positionY) 
        {
            anzEisen = anzahlEisen;
            resources = resource;
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

    }
}
