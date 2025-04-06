using autoritaereFactory;
using autoritaereFactory.world;
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
        public CurveBand(int bandGe, int itemAnMax, List<Resource> resources, int richtung, int anzEisen, int positionX, int positionY) 
            : base(bandGe, itemAnMax, resources, richtung, anzEisen, positionX, positionY)
        {

        }
    }
}
