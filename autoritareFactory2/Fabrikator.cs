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
    public class Fabrikator : Fabrikgebeude
    {
        private ResourceType typBenotigteRecurse1;
        private int benutzesRezept = -1;
        public int BenutzesRezept { get { return benutzesRezept; } }
        public ResourceType TypBenotigteRecurse1 { get { return typBenotigteRecurse1; } }
        private List<Resource> benotigteRecurse1 = new List<Resource>();//Liste mit der ersten für die Produktion nötige Recurse 
        public List<Resource> BenotigteRecurse1 { get { return benotigteRecurse1; } }
        private int nötigeMengenBenotigteRecurse1;//menge der ersten für die Produktion nötige Recurse um zu Produzieren
        public int NötigeMengenBenotigteRecurse1 { get { return nötigeMengenBenotigteRecurse1; } }
        private int maxAnzalBenotigteRecurse1 = 100;
        public int MaxAnzalBenotigteRecurse1 { get { return maxAnzalBenotigteRecurse1; } }

        private ResourceType typBenotigteRecurse2;
        public ResourceType TypBenotigteRecurse2 { get { return typBenotigteRecurse2; } }
        private List<Resource> benotigteRecurse2 = new List<Resource>();//Liste mit der zweiten für die Produktion nötige Recurse 
        public List<Resource> BenotigteRecurse2 { get { return benotigteRecurse2; } }
        private int nötigeMengenBenotigteRecurse2;//menge der zweiten für die Produktion nötige Recurse um zu Produzieren
        public int NötigeMengenBenotigteRecurse2 { get { return nötigeMengenBenotigteRecurse2; } }
        private int maxAnzalBenotigteRecurse2 = 100;
        public int MaxAnzalBenotigteRecurse2 { get { return maxAnzalBenotigteRecurse2; } }

        private ResourceType typErgebnissRecurse1;
        public ResourceType TypErgebnissRecurse1 { get { return typErgebnissRecurse1; } }
        private List<Resource> ergebnissRecurse1 = new List<Resource>();//name der ersten ergebniss Recurse 
        public List<Resource> ErgebnissRecurse1 { get { return ergebnissRecurse1; } }
        private int mengenErgebnissRecursen1;//menge der ersten ergebniss Recurse die bei einer Durchfürung des Produktionsprozesses entsteht
        public int MengenErgebnissRecursen1 { get { return mengenErgebnissRecursen1; } }
        private int maxAnzalErgebnissRecurse1 = 100;
        public int MaxAnzalErgebnissRecurse1 { get { return maxAnzalErgebnissRecurse1; } }

        private int produktionsdauer;//dauer eines Produktionsprozesses in millisekunden
        public int Produktionsdauer { get { return produktionsdauer; } }
        private int verbleibendeProduktionsdauer;//verbleibende dauer des Produktionsprozesses in millisekunden
        public int VerbleibendeProduktionsdauer { get { return verbleibendeProduktionsdauer; } }
        public Fabrikator(int positionX, int positionY, int drehung) : base(positionX, positionY)
        {
            this.drehung = drehung;
            längeInXRichtung = 3;
            längeInYRichtung = 2;
            //PassLängeZUDreungAn(drehung);
        }
        /* in Woyzeck Szene [8] Beim Doktor (S.15) (V.9-10) ersetze Woyzeck mit `WWF4`
        private void PassLängeZUDreungAn(int drehungswert)
        {
            if (drehungswert % 2 == 0)
            {
                int speicherwert = längeInXRichtung;
                längeInXRichtung = längeInYRichtung;
                längeInYRichtung = speicherwert;
            }
        }
        */
        public void SpeichereRezept(Rezepte gewähltesRezept)
        {
            typBenotigteRecurse1 = gewähltesRezept.BenotigteRecursen[0];
            nötigeMengenBenotigteRecurse1 = gewähltesRezept.MengenBenotigteRecurse[0];

            typBenotigteRecurse2 = gewähltesRezept.BenotigteRecursen[1];
            nötigeMengenBenotigteRecurse2 = gewähltesRezept.MengenBenotigteRecurse[1];

            typErgebnissRecurse1 = gewähltesRezept.ErgebnissRecursen[0];
            mengenErgebnissRecursen1 = gewähltesRezept.MengenErgebnissRecursen[0];

            produktionsdauer = gewähltesRezept.Produktionsdauer;
            verbleibendeProduktionsdauer = produktionsdauer;
            benutzesRezept = gewähltesRezept.rezeptIndex;
        }
        public override void Iteration()
        {
            if (typBenotigteRecurse1 != null && typErgebnissRecurse1 != null)
            {
                produziere();
                legAufBand(ergebnissRecurse1, 3, 0);
                nimmVomBand(benotigteRecurse1, -1, 0, typBenotigteRecurse1, maxAnzalBenotigteRecurse1);//heufige wiederholung damit jeder Eingang alle eingangsrecurcen nimmt
                nimmVomBand(benotigteRecurse2, -1, 0, typBenotigteRecurse2, maxAnzalBenotigteRecurse2);
                nimmVomBand(benotigteRecurse1, -1, 1, typBenotigteRecurse1, maxAnzalBenotigteRecurse1);
                nimmVomBand(benotigteRecurse2, -1, 1, typBenotigteRecurse2, maxAnzalBenotigteRecurse2);
            }
        }
        private void produziere()
        {
            if (verbleibendeProduktionsdauer > 0)
            {
                if (benotigteRecurse1.Count >= nötigeMengenBenotigteRecurse1 && benotigteRecurse2.Count >= nötigeMengenBenotigteRecurse2 && (ergebnissRecurse1.Count + mengenErgebnissRecursen1) <= maxAnzalErgebnissRecurse1)
                {
                    verbleibendeProduktionsdauer -= 100;
                }
            }
            else
            {
                for (int i = nötigeMengenBenotigteRecurse1; i > 0; i--)
                {
                    benotigteRecurse1.RemoveAt(0);
                }
                for (int i = nötigeMengenBenotigteRecurse1; i > 0; i--)
                {
                    benotigteRecurse2.RemoveAt(0);
                }
                for (int i = mengenErgebnissRecursen1; i > 0; i--)
                {
                    ergebnissRecurse1.Add(new Resource(TypErgebnissRecurse1));
                }
                verbleibendeProduktionsdauer = produktionsdauer;
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
                    if (band != null && band.GibRichtungEingang() == drehung)
                    {
                        while (band.currentRescourceList.Count < band.ItemAnzahlMax && gebendeRecursenListe.Count > 0)
                        {
                            band.RescourceKommtAufBand(gebendeRecursenListe[0]);
                            gebendeRecursenListe.RemoveAt(0);
                        }
                    }
                }
            }
        }
        private void nimmVomBand(List<Resource> nehmendeRecursenListe, int verschiebungXAchse, int verschiebungYAchse, ResourceType gewolteRecurse, int maxRecursen)//verschiebungXAchse und verschiebungYAchse bezihen sich auf die verschiebung von dem punkt aus der durch positionX/Y beschrieben wird
        {
            if (nehmendeRecursenListe.Count < maxRecursen)
            {
                List<Fabrikgebeude> entitys = WorldMap.theWorld.GetEntityInPos(DrehePAufXAchse(verschiebungXAchse, verschiebungYAchse), DrehePAufYAchse(verschiebungXAchse, verschiebungYAchse));
                if (entitys.Count == 1)
                {
                    if (entitys[0].GetType() != typeof(Band))
                        return;
                    Band band = (Band)entitys[0];
                    if (band != null && band.GibRichtungAusgang() == drehung)
                    {
                        if (nehmendeRecursenListe.Count < maxRecursen && band.currentRescourceList.Count > 0)
                        {
                            for (int i = 0; i < band.currentRescourceList.Count; i++)
                            {
                                if (band.currentRescourceList[i].Type == gewolteRecurse)
                                {
                                    nehmendeRecursenListe.Add(band.NimmRescourceVomBand(i));
                                    break;
                                }
                                //if (i == band.resource.Count - 1)
                                //{
                                //    return;
                                //}
                            }
                        }
                    }
                }
            }
        }
        private int DrehePAufXAchse(int VX, int VY)
        {
            return PositionX + Convert.ToInt32(Math.Sin(drehung * (Math.PI / 2))) * VX + Convert.ToInt32(Math.Cos(drehung * (Math.PI / 2))) * VY + ((drehung / 2) % 2) * (SizeX - 1);
        }
        private int DrehePAufYAchse(int VX, int VY)
        {
            return PositionY - Convert.ToInt32(Math.Cos(drehung * (Math.PI / 2))) * VX + Convert.ToInt32(Math.Sin(drehung * (Math.PI / 2))) * VY + (drehung / 3) * (SizeY - 1);
        }
        public override string ToString()
        {
            return "Fabrikator";
        }
        internal Fabrikator() : base()
        {
            längeInXRichtung = 3;
            längeInYRichtung = 2;
        }
        public override List<byte> GetAsBytes()
        {
            List<byte> bytes = base.GetAsBytes();
            bytes.AddRange(BitConverter.GetBytes((int)benutzesRezept));
            return bytes;
        }
        public static new Fabrikator FromByteArray(byte[] bytes, ref int offset)
        {
            Fabrikator newKonstrucktor = new Fabrikator();
            int benutzesRezept = BitConverter.ToInt32(bytes, offset);
            if (benutzesRezept > -1)
                newKonstrucktor.SpeichereRezept(world.rezepte[benutzesRezept]);
            offset += 4;
            return newKonstrucktor;
        }
    }
}