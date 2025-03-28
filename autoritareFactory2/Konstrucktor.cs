﻿using autoritaereFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship
{
    public class Konstrucktor : Fabrikgebeude
    {
        private ResourceType typBenotigteRecurse1;
        public ResourceType TypBenotigteRecurse1 { get { return typBenotigteRecurse1; } }
        private List<Resource> benotigteRecurse1;//Liste mit der ersten für die Produktion nötige Recurse 
        public List<Resource> BenotigteRecurse1 { get { return benotigteRecurse1; } }//nur bisher String 
        private int nötigeMengenBenotigteRecurse1;//menge der ersten für die Produktion nötige Recurse um zu Produzieren
        public int NötigeMengenBenotigteRecurse1 { get { return nötigeMengenBenotigteRecurse1; } }
        private ResourceType typErgebnissRecurse1;
        public ResourceType TypErgebnissRecurse1 { get { return typErgebnissRecurse1; } }
        private List<Resource> ergebnissRecurse1;//name der ersten ergebniss Recurse 
        public List<Resource> ErgebnissRecurse1 { get { return ergebnissRecurse1; } }//nur bisher String 
        private int mengenErgebnissRecursen1;//menge der ersten ergebniss Recurse die bei einer Durchfürung des Produktionsprozesses entsteht
        public int MengenErgebnissRecursen1 { get { return mengenErgebnissRecursen1; } }
        private int produktionsdauer;//dauer eines Produktionsprozesses in millisekunden
        public int Produktionsdauer { get { return produktionsdauer; } }
        private int verbleibendeProduktionsdauer;//verbleibende dauer des Produktionsprozesses in millisekunden
        public int VerbleibendeProduktionsdauer { get { return verbleibendeProduktionsdauer; } }

        public Konstrucktor(int positionX, int positionY) : base(positionX, positionY)
        {
            längeInXRichtung = 2;
            längeInYRichtung = 1;
        }
        public void SpeichereRezept(Rezepte gewähltesRezept)
        {
            typBenotigteRecurse1 = gewähltesRezept.BenotigteRecursen[0];
            nötigeMengenBenotigteRecurse1 = gewähltesRezept.MengenBenotigteRecurse[0];
            typErgebnissRecurse1 = gewähltesRezept.ErgebnissRecursen[0];
            mengenErgebnissRecursen1 = gewähltesRezept.MengenErgebnissRecursen[0];
            produktionsdauer = gewähltesRezept.Produktionsdauer;
            verbleibendeProduktionsdauer = produktionsdauer;
        }
        public override void Iteration()
        {
            //if (verbleibendeProduktionsdauer <= 0)
            //{
            //    if (anzahlBenotigteRecurse1 >= nötigeMengenBenotigteRecurse1) 
            //    {
            //        verbleibendeProduktionsdauer -= 100;
            //    }
            //}
            //else
            //{
            //    anzahlBenotigteRecurse1 -= nötigeMengenBenotigteRecurse1;
            //    anzahlErgebnissRecurse1 += mengenErgebnissRecursen1;
            //    verbleibendeProduktionsdauer = produktionsdauer;
            //}
        }

    }
}
