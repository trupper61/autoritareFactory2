using autoritaereFactory;
using autoritaereFactory.world;
using factordictatorship.setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship
{
    public class Exporthaus : Fabrikgebeude
    {
        public List<Resource> rescourcenInLager = new List<Resource>();

        public Exporthaus(int positionX, int positionY, int drehung) 
            : base(positionX, positionY, drehung)
        {
            längeInXRichtung = 1;
            längeInYRichtung = 1;
        }

        public override void Iteration() 
        { 
                
        }

        public void ErkenneBandInNähe(Band band, Exporthaus exporthaus) 
        {
            int wertRotX = 0;
            int wertRotY = 0;

            for(int i = 0; i == 4; i++) 
            {
                switch(i) 
                {
                    case 1:
                        wertRotX = -1;
                        break;
                    case 2:
                        wertRotY = 1;
                        break;
                    case 3:
                        wertRotX = 1;
                        break;
                    case 4:
                        wertRotY -= 1;
                        break;

                }
                List<Fabrikgebeude> values = WorldMap.theWorld.GetEntityInPos(exporthaus.PositionX + wertRotX, exporthaus.PositionY + wertRotY);

                foreach (Fabrikgebeude v in values)
                {
                    if (v is Band)
                    {
                        switch(wertRotX) 
                        {
                            case -1:
                                if(v.drehung == 1) 
                                {
                                    NimmVomBand(band);
                                }
                                break;
                            case 1:
                                if (v.drehung == 3)
                                {
                                    NimmVomBand(band);
                                }
                                break;
                        }
                        switch(wertRotY) 
                        {
                            case -1:
                                if (v.drehung == 4)
                                {
                                    NimmVomBand(band);
                                }
                                break;
                            case 1:
                                if (v.drehung == 2)
                                {
                                    NimmVomBand(band);
                                }
                                break;
                        }
                    }
                }
            }

            
        }

        public void NimmVomBand(Band band) 
        {
            //TBD
        }




    }
}
