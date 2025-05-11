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
    internal class Merger : Fabrikgebeude
    {
        private List<Resource> listResource = new List<Resource>();//Liste mit der ersten für die Produktion nötige Recurse 
        public List<Resource> ListResource { get { return listResource; } }
        private int maxAnzalRecurse1 = 20;
        public int MaxAnzalRecurse1 { get { return maxAnzalRecurse1; } }
        public Merger(int positionX, int positionY, int drehung) : base(positionX, positionY, drehung)
        {
            längeInXRichtung = 1;
            längeInYRichtung = 1;
        }
        public override void Iteration()
        {
            for (int i = 0; i < 4; i++)
            {
                nimmVomBand(listResource, 0, 1, maxAnzalRecurse1);//Eingang: unten, links, und oben. Ausgang: Rechts
                nimmVomBand(listResource, -1, 0, maxAnzalRecurse1);
                nimmVomBand(listResource, 0, -1, maxAnzalRecurse1);
                legAufBand(listResource, 1, 0);
                legAufBand(listResource, 1, 0);
                legAufBand(listResource, 1, 0);
            }
        }
        private void legAufBand(List<Resource> gebendeRecursenListe, int verschiebungXAchse, int verschiebungYAchse)//verschiebungXAchse und verschiebungYAchse bezihen sich auf die verschiebung von dem punkt aus der durch positionX/Y beschrieben wird
        {
            if (gebendeRecursenListe.Count > 0)
            {
                List<Fabrikgebeude> entitys = WorldMap.theWorld.GetEntityInPos(DrehePAufXAchse(verschiebungXAchse, verschiebungYAchse), DrehePAufYAchse(verschiebungXAchse, verschiebungYAchse));
                if (entitys.Count == 1)
                {
                    if (entitys[0].GetType() != typeof(Band))
                        return;
                    Band band = (Band)entitys[0];
                    if (band != null)
                    {
                        band.ErkenneRescourcen();
                        if (band.ItemAnzahlMoment < band.ItemAnzahlMax && gebendeRecursenListe.Count > 0)
                        {
                            band.RescourceKommtAufBand(gebendeRecursenListe[0]);
                            gebendeRecursenListe.RemoveAt(0);
                        }
                    }
                }
            }
        }
        private void nimmVomBand(List<Resource> nehmendeRecursenListe, int verschiebungXAchse, int verschiebungYAchse, int maxRecursen)//verschiebungXAchse und verschiebungYAchse bezihen sich auf die verschiebung von dem punkt aus der durch positionX/Y beschrieben wird
        {
            if (nehmendeRecursenListe.Count < maxRecursen)
            {
                List<Fabrikgebeude> entitys = WorldMap.theWorld.GetEntityInPos(DrehePAufXAchse(verschiebungXAchse, verschiebungYAchse), DrehePAufYAchse(verschiebungXAchse, verschiebungYAchse));
                if (entitys.Count == 1)
                {
                    if (entitys[0].GetType() != typeof(Band))
                        return;
                    Band band = (Band)entitys[0];
                    if (band != null)
                    {
                        band.ErkenneRescourcen();
                        if (nehmendeRecursenListe.Count < maxRecursen && band.currentRescourceList.Count > 0)
                        {
                            nehmendeRecursenListe.Add(band.NimmRescourceVomBand(0));
                        }
                    }
                }
            }
        }
        private int DrehePAufXAchse(int VX, int VY)
        {
            return PositionX + Convert.ToInt32(Math.Sin(drehung * (Math.PI / 2))) * VX + Convert.ToInt32(Math.Cos(drehung * (Math.PI / 2))) * VY;
        }
        private int DrehePAufYAchse(int VX, int VY)
        {
            return PositionY - Convert.ToInt32(Math.Cos(drehung * (Math.PI / 2))) * VX + Convert.ToInt32(Math.Sin(drehung * (Math.PI / 2))) * VY;
        }
    }
}