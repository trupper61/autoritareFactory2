using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autoritaereFactory
{
    public class Iron : Resource
    {
        public Iron(int initAmount) : base("Iron", initAmount) { } // initAmount als wie viel an der Quelle ist
    }
}
