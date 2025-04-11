﻿using autoritaereFactory;
using autoritaereFactory.world;
using factordictatorship.setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship
{
    public class Miner : Fabrikgebeude
    {
        private int Zähler;//beschränkt die anzahl Resurcen pro minute
        private ResourceType typResurce;
        public ResourceType TypResurce { get { return typResurce; } }
        private List<Resource> recurse = new List<Resource>();//Liste mit der Recurse, die Abgebaut wird
        public List<Resource> Recurse { get { return recurse; } }
        private int maxAnzalRecurse = 100;
        public int MaxAnzalRecurse { get { return maxAnzalRecurse; } }
        private int drehung;//wert der Dreung 1: Ausgang rechts und dann im Urzeigersinn bis 4: Ausgang oben
        public int Drehung { get { return drehung; } }
        public Miner(int positionX, int positionY, int drehung, ResourceType typResurce) : base(positionX, positionY)
        {
            this.typResurce = typResurce;
            längeInXRichtung = 1;
            längeInYRichtung = 1;
            this.drehung = drehung;
        }
        public override void Iteration()
        {
            mine();
            legAufBand(recurse, 1, 0);
        }
        private void mine()
        {
            if (recurse.Count < maxAnzalRecurse)
            {
                Zähler++;
                if (Zähler <= 10)
                {
                    recurse.Add(new Resource(TypResurce));
                    Zähler = 0;
                }
            }
        }
        private void legAufBand(List<Resource> gebendeRecursenListe, int verschiebungXAchse, int verschiebungYAchse)//verschiebungXAchse und verschiebungYAchse bezihen sich auf die verschiebung von dem punkt aus der durch positionX/Y beschrieben wird
        {
            if (gebendeRecursenListe.Count > 0)
            {
                List<Fabrikgebeude> entitys = WorldMap.theWorld.GetEntityInPos(DrehePAufXAchse(verschiebungXAchse, verschiebungYAchse), DrehePAufYAchse(verschiebungXAchse, verschiebungYAchse));
                if (entitys.Count == 1)
                {
                    Band band = (Band)entitys[0];
                    if (band != null)
                    {
                        band.ErkenneRescourcen();
                        while (band.ItemAnzahlMoment < band.ItemAnzahlMax & gebendeRecursenListe.Count > 0)
                        {
                            band.RescourceKommtAufBand(gebendeRecursenListe[0]);
                            gebendeRecursenListe.RemoveAt(0);
                            band.ErkenneRescourcen();
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
