using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship
{
    public class PlayerData
    {
        public int money;

        public PlayerData(int money)
        {
            this.money = money;
        }

        public string displayData()
        {
            return money.ToString();
        }
    }
}
