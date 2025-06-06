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
    internal class Splitter : Fabrikgebeude
    {
        private List<Resource> listResource = new List<Resource>();//Liste mit der ersten für die Produktion nötige Recurse 
        public List<Resource> ListResource { get { return listResource; } }
        private int maxAnzalRecurse1 = 20;
        public int MaxAnzalRecurse1 { get { return maxAnzalRecurse1; } }
        private int Rechenwert = 0;
        public Splitter(int positionX, int positionY, int drehung) : base(positionX, positionY, drehung)
        {
            längeInXRichtung = 1;
            längeInYRichtung = 1;
        }
        public override void Iteration()
        {
            for (int i = 0; i < 3; i++)
             {
                nimmVomBand(listResource, -1, 0, maxAnzalRecurse1, 0);//Eingang: Links. Ausgang Oben, Rechts und Unten
                nimmVomBand(listResource, -1, 0, maxAnzalRecurse1, 0);
                nimmVomBand(listResource, -1, 0, maxAnzalRecurse1, 0);
                /*legAufBand(listResource, 0, -1, 3);
                legAufBand(listResource, 1, 0, 0);
                legAufBand(listResource, 0, 1, 1);*/
                Verteile();
                Verteile();
                Verteile();
            }
        }
        private void Verteile()
        {
            Rechenwert = (Rechenwert + 1) % 3;
            if(Rechenwert == 0)
            {
                legAufBand(listResource, 0, -1, 3);
            }
            if(Rechenwert == 1)
            {
                legAufBand(listResource, 1, 0, 0);
            }
            if (Rechenwert == 2)
            {
                legAufBand(listResource, 0, 1, 1);
            }
        }
        private void legAufBand(List<Resource> gebendeRecursenListe, int verschiebungXAchse, int verschiebungYAchse, int extradrehung)//verschiebungXAchse und verschiebungYAchse bezihen sich auf die verschiebung von dem punkt aus der durch positionX/Y beschrieben wird
        {
            if (gebendeRecursenListe.Count > 0)
            {
                List<Fabrikgebeude> entitys = WorldMap.theWorld.GetEntityInPos(DrehePAufXAchse(verschiebungXAchse, verschiebungYAchse), DrehePAufYAchse(verschiebungXAchse, verschiebungYAchse));
                if (entitys.Count == 1)
                {
                    if (entitys[0].GetType() != typeof(Band))
                        return;
                    Band band = (Band)entitys[0];
                    if (band != null && band.GibRichtungEingang() == ((drehung + extradrehung) % 5))
                    {
                        if (band.currentRescourceList.Count < band.ItemAnzahlMax && gebendeRecursenListe.Count > 0)
                        {
                            band.RescourceKommtAufBand(gebendeRecursenListe[0]);
                            gebendeRecursenListe.RemoveAt(0);
                        }
                    }
                }
            }
        }
        private void nimmVomBand(List<Resource> nehmendeRecursenListe, int verschiebungXAchse, int verschiebungYAchse, int maxRecursen, int extradrehung)//verschiebungXAchse und verschiebungYAchse bezihen sich auf die verschiebung von dem punkt aus der durch positionX/Y beschrieben wird
        {
            if (nehmendeRecursenListe.Count < maxRecursen)
            {
                List<Fabrikgebeude> entitys = WorldMap.theWorld.GetEntityInPos(DrehePAufXAchse(verschiebungXAchse, verschiebungYAchse), DrehePAufYAchse(verschiebungXAchse, verschiebungYAchse));
                if (entitys.Count == 1)
                {
                    if (entitys[0].GetType() != typeof(Band))
                        return;
                    Band band = (Band)entitys[0];
                    if (band != null && band.GibRichtungAusgang() == ((drehung + extradrehung) % 5))
                    {
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
        internal Splitter() : base()
        {
            längeInXRichtung = 1;
            längeInYRichtung = 1;
        }
        public override List<byte> GetAsBytes()
        {
            List<byte> bytes = base.GetAsBytes();
            bytes.AddRange(BitConverter.GetBytes((int)listResource.Count));
            for (int i = 0; i < listResource.Count; i++)
            {
                bytes.AddRange(BitConverter.GetBytes((int)listResource[i].Type));
            }
            return bytes;
        }
        public static new Splitter FromByteArray(byte[] bytes, ref int offset)
        {
            Splitter newSplitter = new Splitter();
            int resourceCount = BitConverter.ToInt32(bytes, offset);
            offset += 4;
            for (int i = 0; i < resourceCount; i++)
            {
                newSplitter.listResource.Add(new Resource((ResourceType)BitConverter.ToInt32(bytes, offset)));
                offset += 4;
            }
            return newSplitter;
        }
    }
}