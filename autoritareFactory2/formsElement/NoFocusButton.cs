using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace factordictatorship.formsElement
{
    public class NoFocusButton : Button
    {

        public NoFocusButton() : base()
        {
            SetStyle(ControlStyles.Selectable, false);
        }
    }
}
