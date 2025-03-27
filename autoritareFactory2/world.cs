using autoritaereFactory.world;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace factordictatorship
{
    public partial class world : Form
    {
        public world()
        {
            InitializeComponent();
            WorldMap wrld = new WorldMap(8,8);
        }
    }
}
