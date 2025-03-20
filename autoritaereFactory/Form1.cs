using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using autoritaereFactory.world;

namespace autoritaereFactory
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            WorldMap wrld = new WorldMap(6,6);
        }
    }
}
