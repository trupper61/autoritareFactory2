using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autoritaereFactory.setup
{
    public enum GroundResource
    {
        AnyTile = -1,
        Grass0,
        Grass1,
        Grass2,
        Grass3,
        // insert Grass Tiles here!
        GrassUpperBound,
        Desert,
        Desert2,
        // insert desert tiles here!
        DesertUpperBound,
        // Default Ground End
        // Ore Start
        IronOre,
        ColeOre,
        CopperOre,
       
        LimeStone,
        LimeStone2,
        LimeStone3,
        LimeStone4,
        LimeStoneUpperBound,
        // always on the end!
        UpperBound
    }

    public enum SavingPackets
    {
        ChunkPacket,
        EntityPacket,
        PlayerDataPacket,
    }
}
