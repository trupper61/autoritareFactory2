using autoritaereFactory;
using autoritaereFactory.world;
using factordictatorship.setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace factordictatorship
{
    public class Exporthaus : Fabrikgebeude
    {
        public List<Resource> rescourcenInLager = new List<Resource>();
        public WorldMap wrld;
        public Exporthaus(int positionX, int positionY, int drehung, WorldMap wrld) 
            : base(positionX, positionY, drehung)
        {
            längeInXRichtung = 1;
            längeInYRichtung = 1;
            this.wrld = wrld;
        }

        public override void Iteration() 
        {
            ErkenneBandInNähe(wrld, this);
        }

        public void ErkenneBandInNähe(WorldMap wrld, Exporthaus exporthaus) 
        {
            int wertRotX = 0;
            int wertRotY = 0;

            for(int i = 0; i == 4; i++) 
            {
                switch(i) 
                {
                    case 1:
                        wertRotX = 1;
                        break;
                    case 2:
                        wertRotY = 1;
                        break;
                    case 3:
                        wertRotX = -1;
                        break;
                    case 4:
                        wertRotY -= 1;
                        break;

                }
                List<Fabrikgebeude> values = WorldMap.theWorld.GetEntityInPos(exporthaus.PositionX + wertRotX, exporthaus.PositionY + wertRotY);

                foreach (Fabrikgebeude v in values)
                {
                    if (v is factordictatorship.setup.Band)
                    {
                        switch(wertRotX) 
                        {
                            case -1:
                                if(v.drehung == 1) 
                                {
                                    nimmVomBand(wrld, wertRotX, values, this);
                                }
                                break;
                            case 1:
                                if (v.drehung == 3)
                                {
                                    nimmVomBand(wrld, wertRotX, values, this);
                                }
                                break;
                        }
                        switch(wertRotY) 
                        {
                            case -1:
                                if (v.drehung == 4)
                                {
                                    nimmVomBand(wrld, wertRotY, values, this);
                                }
                                break;
                            case 1:
                                if (v.drehung == 2)
                                {
                                    nimmVomBand(wrld, wertRotY, values, this);
                                }
                                break;
                        }
                    }
                }
            }

            
        }

        private void nimmVomBand(WorldMap wrld, int Pos, List<Fabrikgebeude> gebauede, Exporthaus exp) 
        {
            factordictatorship.setup.Band band;

            int bandPosX = 0;
            int bandPosY = 0;
            foreach (Fabrikgebeude baender in gebauede) 
            {
                bandPosX = baender.PositionX;
                bandPosY = baender.PositionY;
                band = (factordictatorship.setup.Band)baender;

                foreach (Resource resources in band.currentRescourceList)
                {
                    band.removedRescources.Add(resources);
                    exp.rescourcenInLager.Add(resources);
                }
                foreach (Resource resources in band.removedRescources)
                {
                    band.currentRescourceList.Remove(resources);
                    band.ItemAnzahlMoment = band.currentRescourceList.Count();
                }
                band.removedRescources.Clear();
            }
        }
    }
}
