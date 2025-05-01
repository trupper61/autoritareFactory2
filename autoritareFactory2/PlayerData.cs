using autoritaereFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship
{
    public class PlayerData
    {
        public int money;
        public List<Inventory> inventories;

        public PlayerData(int money)
        {
            this.money = money;
            inventories = new List<Inventory>(5);
        }
        public void AddResource(Resource resource)
        {
            Inventory inv = inventories.Where(i => i.Type == resource.Type && i.Items.Count < i.maxStack).FirstOrDefault();
            if (inv != null)
            {
                inv.Add(resource);
            }
            else
            {
                inv = new Inventory(resource.Type, 500); // 500 is max Stack
                inv.Add(resource);
                inventories.Add(inv);
            }
        }
        public void CheckForEmptyInventory()
        {
            foreach(Inventory inv in inventories.ToList())
            {
                if (inv.Items.Count == 0)
                {
                    inventories.Remove(inv);
                }
            }
        }
        public string displayData()
        {
            return money.ToString();
        }
    }
}
