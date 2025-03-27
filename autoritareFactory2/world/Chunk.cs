using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using autoritaereFactory.setup;
using factordictatorship;

namespace autoritaereFactory.world
{
    public class Chunk
    {
        public const int chunkSize = 32;
        public int x, y; // chunk position not world
        public List<Fabrikgebeude> buildings;
        readonly GroundResource[,] blockState;
        public Chunk()
        {
            blockState = new GroundResource[chunkSize, chunkSize];
        }
        public Chunk(int posX, int posY)
        {
            Random rng = new Random();
            blockState = new GroundResource[chunkSize, chunkSize];
            for (int ptX = 0; ptX < chunkSize; ptX++)
            {
                for (int ptY = 0; ptY < chunkSize; ptY++)
                {
                    blockState[ptX, ptY] = (GroundResource)rng.Next(0, 2);
                }
            }
            x = posX;
            y = posY;
        }
        public GroundResource GetSubChunk(int innerX, int innerY)
        {
            return blockState[innerX, innerY];
        }
    }
}
