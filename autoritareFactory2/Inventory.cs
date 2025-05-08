using autoritaereFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship
{
    public class Inventory
    {
        public List<Resource> Items { get; private set; }
        public int maxStack { get; private set; }
        public ResourceType Type { get; private set; }
        public Inventory(ResourceType type, int maxStack)
        {
            Items = new List<Resource>();
            Type = type;
            this.maxStack = maxStack;
        }
        public void Add(Resource resource)
        {
            if (Items.Count > maxStack)
                return;
            Items.Add(resource);
        }
        public List<Resource> GetItems()
        {
            return Items;
        }
    }
}
