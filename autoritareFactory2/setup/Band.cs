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
        public int Richtung; //0 -> Rechts||| 1 -> Unten||| 2 -> Links||| 3 -> Oben|||

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

        //Relevantes gebäude übergibt sich selbst. Diese Methode muss in der Klasse Konstruktor ausgeführt werden
        public void NehmeResourcenVonBand(Konstrucktor gebauede, Band band) 
        {
            //Wenn Gebäudeeingang rechts vom Förderband, und das Gebäude noch nicht voll (Max 100 Resscourcen), dann

            if (null == null) //Platzhalter | Wenn die Anzahl der Rescourcen in der Liste nicht über oder gleich 100, weiter 
            {
                //Wenn die Rescourcen auf dem Band relevant für das Gebäude sind, weiter.
                if (null == null) //Platzhalter
                {
                    foreach (Resource r in band.resource)
                    {
                        if (r.Type == gebauede.TypBenotigteRecurse1)
                        {
                            band.anzahlEisen--;
                            
                        }
                        band.ItemAnzahlMoment--;
                    }
                }
            }


            //entnehme Rescource und Füge sie in die Liste des Konstruktorgebäude-Objektes 'benötigteResscourcen' hinzu.
            //Entferne die jeweiligen Rescourcen aus der Liste des Förderbandes
        }

    }
}
