using autoritaereFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship
{
    public enum zugehörigesGebeude
    {
        Konstrucktor,
        Fabrikator
    }

    public class Rezepte
    {
        private zugehörigesGebeude gebeudeTyp;
        public zugehörigesGebeude GebeudeTyp { get { return gebeudeTyp; } }
        private List<ResourceType> benotigteRecursen = new List<ResourceType>();
        public List<ResourceType> BenotigteRecursen { get { return benotigteRecursen; } }
        private List<int> mengenBenotigteRecurse = new List<int>();
        public List<int> MengenBenotigteRecurse { get { return mengenBenotigteRecurse; } }
        private List<ResourceType> ergebnissRecursen = new List<ResourceType>();
        public List<ResourceType> ErgebnissRecursen { get { return ergebnissRecursen; } }
        private List<int> mengenErgebnissRecursen = new List<int>();
        public List<int> MengenErgebnissRecursen { get { return mengenErgebnissRecursen; } }
        private int produktionsdauer;
        public int Produktionsdauer { get { return produktionsdauer; } }
        private string rezeptName;
        public string RezeptName { get { return rezeptName; } }

        public Rezepte(zugehörigesGebeude gebeudeTyp, ResourceType benotigteRecurse1, int anzahlbenotigteRecursen1, string rezeptName, ResourceType ergebnissRecurse1, int anzahlergebnissRecursen, int produktionsdauer)
        {
            this.gebeudeTyp = gebeudeTyp;
            this.produktionsdauer = produktionsdauer;
            this.rezeptName = rezeptName;
            benotigteRecursen.Add(benotigteRecurse1);
            mengenBenotigteRecurse.Add(anzahlbenotigteRecursen1);
            ergebnissRecursen.Add(ergebnissRecurse1);
            mengenErgebnissRecursen.Add(anzahlergebnissRecursen);
        }

        public Rezepte(zugehörigesGebeude gebeudeTyp, ResourceType benotigteRecurse1, int anzahlbenotigteRecursen1, ResourceType benotigteRecurse2, int anzahlbenotigteRecursen2, string rezeptName, ResourceType ergebnissRecurse1, int anzahlergebnissRecursen, int produktionsdauer)
        {
            this.gebeudeTyp = gebeudeTyp;
            this.produktionsdauer = produktionsdauer;
            this.rezeptName = rezeptName;
            benotigteRecursen.Add(benotigteRecurse1);
            mengenBenotigteRecurse.Add(anzahlbenotigteRecursen1);
            benotigteRecursen.Add(benotigteRecurse2);
            mengenBenotigteRecurse.Add(anzahlbenotigteRecursen2);
            ergebnissRecursen.Add(ergebnissRecurse1);
            mengenErgebnissRecursen.Add(anzahlergebnissRecursen);
        }
    }
}
