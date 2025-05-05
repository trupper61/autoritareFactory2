using factordictatorship.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autoritaereFactory
{
    public class Resource
    {
        public ResourceType Type { get; private set; }
        public Resource(ResourceType type)
        {
            Type = type;
        }
    }
    public enum ResourceType
    {
        IronIngot,
        IronOre,
        IronPlate,
        IronStick
    }
}
