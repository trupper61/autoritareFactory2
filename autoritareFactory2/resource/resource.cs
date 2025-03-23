﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autoritaereFactory
{
    public abstract class Resource
    {
        public string Name { get; private set; }
        public int Amount { get; private set; }

        // spätere ideen:
        //public Point Position { get; set; } Position im Feld
        //public PictureBox Texture { get; set; } Picturebox als Texturfeld

        public Resource(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
