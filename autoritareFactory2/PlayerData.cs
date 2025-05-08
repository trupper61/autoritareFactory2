using autoritaereFactory;
using autoritaereFactory.setup;
using autoritaereFactory.world;
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
        public int slotsAvail;

        public PlayerData(int money)
        {
            this.money = money;
            slotsAvail = 4;

            inventories = new List<Inventory>(slotsAvail);
        }
        public void AddResource(Resource resource)
        {
            Inventory inv = inventories.Where(i => i.Type == resource.Type && i.Items.Count < i.maxStack).FirstOrDefault();
            if (inv != null)
            {
                inv.Add(resource);
            }
            else if (inventories.Count() < slotsAvail)
            {
                inv = new Inventory(resource.Type, Inventory.MAX_STACK); // 500 is max Stack
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
        public List<byte> GetAsBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.Add((byte)SavingPackets.PlayerDataPacket);
            bytes.AddRange(BitConverter.GetBytes((int)money));
            bytes.AddRange(BitConverter.GetBytes((int)slotsAvail));
            bytes.AddRange(BitConverter.GetBytes((int)inventories.Count));
            for (int i = 0; i < inventories.Count; i++)
                bytes.AddRange(inventories[i].GetAsBytes());
            return bytes;
        }
        public static PlayerData FromByteArray(byte[] bytes, ref int offset)
        {
            PlayerData nDat = new PlayerData(0);
            if (bytes.Length <= offset || (byte)SavingPackets.PlayerDataPacket != bytes[offset++]) { offset--; return nDat; }
            nDat.money = BitConverter.ToInt32(bytes, offset);
            nDat.slotsAvail = BitConverter.ToInt32(bytes, offset + 4);
            int inventoryCount = BitConverter.ToInt32(bytes, offset + 8);
            offset += 12;
            for (int i = 0; i < inventoryCount; i++)
            {
                nDat.inventories.Add(Inventory.FromByteArray(bytes, ref offset));
            }
            return nDat;
        }
    }
}
