using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autoritaereFactory
{
    internal class Rezepte
    {
        private string zugehörigesGebeude;
        public string ZugehörigesGebeude {  get { return zugehörigesGebeude; } }
        private List<string> benotigteRecursen = new List<string>();
        public List<string> BenotigteRecursen { get {  return benotigteRecursen; } }
        private List<string> ergebnissRecursen = new List<string>();
        public List<string> ErgebnissRecursen { get { return ergebnissRecursen; } }
        private List<int> mengenBenotigteRecurse = new List<int>();
        public List<int> MengenBenotigteRecurse { get { return mengenBenotigteRecurse; } }
        private List<int> mengenErgebnissRecursen = new List<int>();
        public List<int> MengenErgebnissRecursen { get { return mengenErgebnissRecursen; } }
        private int produktionsdauer;
        public int Produktionsdauer { get {  return produktionsdauer; } }
        private string rezeptName;
        public string RezeptName {  get { return rezeptName; } }
        public Rezepte(string zugehörigesGebeude, string benotigteRecurse1, int mengenBenotigteRecurse1, string rezeptName, string ergebnissRecurse1, int mengenErgebnissRecursen1, int produktionsdauer)
        {
            this.zugehörigesGebeude = zugehörigesGebeude;
            this.produktionsdauer = produktionsdauer;
            this.rezeptName = rezeptName;
            benotigteRecursen.Add(benotigteRecurse1);
            mengenBenotigteRecurse.Add(mengenBenotigteRecurse1);
            ergebnissRecursen.Add(ergebnissRecurse1);
            mengenErgebnissRecursen.Add(mengenErgebnissRecursen1);
        }
    }
}
