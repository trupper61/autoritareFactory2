using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autoritaereFactory.setup
{
    public enum GroundResource
    {
        Nothing,
        Stone,
        Iron,
    }
    public struct BlockState
    {
        public GroundResource ressource;
        // get the new "resource"
        /*public Resource GenerateNewRessource() {
            return GenerateNewRessource();
        }*/
    }
}
