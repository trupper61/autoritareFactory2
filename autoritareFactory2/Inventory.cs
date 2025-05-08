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
        public const int MAX_STACK = 500;
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

        public List<byte> GetAsBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((int)Type));
            bytes.AddRange(BitConverter.GetBytes((int)Items.Count));
            return bytes;
        }
        internal static Inventory FromByteArray(byte[] bytes, ref int offset)
        {
            Inventory inv = new Inventory((ResourceType)(-1), Inventory.MAX_STACK);
            inv.Type = (ResourceType)BitConverter.ToInt32(bytes, offset);
            int itemCount = BitConverter.ToInt32(bytes, offset + 4);
            for(int i = 0;i < itemCount; i++)
            {
                inv.Items.Add(new Resource(inv.Type));
            }
            offset += 8;
            return inv;
        }
    }
}
