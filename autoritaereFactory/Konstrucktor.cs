using autoritaereFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Fabrik1Console
{
    internal class Konstrucktor : Fabrikgebeude
    {
        private int längeInXRichtung = 2;
        public int LängeInXRichtung { get { return längeInXRichtung; } }
        private int längeInYRichtung = 1;
        public int LängeInYRichtung { get { return längeInYRichtung; } }
        private string benotigteRecurse1;//name der ersten für die Produktion nötige Recurse 
        public string BenotigteRecurse1 { get { return benotigteRecurse1; } }
        private int nötigeMengenBenotigteRecurse1;//menge der ersten für die Produktion nötige Recurse um zu Produzieren
        public int NötigeMengenBenotigteRecurse1 { get { return nötigeMengenBenotigteRecurse1; } }
        private string ergebnissRecurse1;//name der ersten ergebniss Recurse 
        public string ErgebnissRecurse1 { get { return ergebnissRecurse1; } }
        private int mengenErgebnissRecursen1;//menge der ersten ergebniss Recurse die bei einer Durchfürung des Produktionsprozesses entsteht
        public int MengenErgebnissRecursen1 { get { return mengenErgebnissRecursen1; } }
        private int produktionsdauer;//dauer eines Produktionsprozesses in millisekunden
        public int Produktionsdauer { get { return produktionsdauer; } }
        private int verbleibendeProduktionsdauer;//verbleibende dauer des Produktionsprozesses in millisekunden
        public int VerbleibendeProduktionsdauer { get { return verbleibendeProduktionsdauer; } }
        private int anzahlBenotigteRecurse1;//vorhandene Menge der ersten für die Produktion nötige Recurse 
        public int AnzahlBenotigteRecurse1 { get { return anzahlBenotigteRecurse1; } }
        private int anzahlErgebnissRecurse1;//vorhandene Menge der ersten ergebniss Recurse 
        public int AnzahlErgebnissRecurse1 { get { return anzahlErgebnissRecurse1; } }

        public Konstrucktor(int positionX, int positionY) : base(positionX, positionY) { }
        public void SpeichereRezept(Rezepte gewähltesRezept)
        {
            benotigteRecurse1 = gewähltesRezept.BenotigteRecursen[0];
            nötigeMengenBenotigteRecurse1 = gewähltesRezept.MengenBenotigteRecurse[0];
            ergebnissRecurse1 = gewähltesRezept.ErgebnissRecursen[0];
            mengenErgebnissRecursen1 = gewähltesRezept.MengenErgebnissRecursen[0];
            produktionsdauer = gewähltesRezept.Produktionsdauer;
            verbleibendeProduktionsdauer = produktionsdauer;
        }
        public void Iteration()
        {
            if (verbleibendeProduktionsdauer <= 0)
            {
                if (anzahlBenotigteRecurse1 >= nötigeMengenBenotigteRecurse1)
                {
                    verbleibendeProduktionsdauer -= 1000;
                }
            }
            else
            {
                anzahlBenotigteRecurse1 -= nötigeMengenBenotigteRecurse1;
                anzahlErgebnissRecurse1 += mengenErgebnissRecursen1;
                verbleibendeProduktionsdauer = produktionsdauer;
            }
        }

    }
}
