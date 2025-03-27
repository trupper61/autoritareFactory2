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
        // yes this function is slow, but who cares anyways.
        private void GenerateCircleRessource(int seed, GroundResource state, float radius,int randStrength)
        {
            // explorer the neighbours with this funny loop
            for (int neb = 0; neb < 9; neb++)
            {
                // create an fitting random function
                Random rng = new Random((x + neb / 3 - 1) * 256 + (y + neb % 3 - 1) + seed);
                // get the chosen position
                int selectedX = rng.Next(chunkSize) + (neb / 3 - 1) * chunkSize;
                int selectedY = rng.Next(chunkSize) + (neb % 3 - 1) * chunkSize;
                int bigSize = (int)Math.Ceiling(radius);
                // maybe fill the tiles with the resource
                for (int i = -bigSize; i <= bigSize; i++)
                {
                    for (int j = -bigSize; j <= bigSize; j++)
                    {
                        // check if outside this chunk
                        if (selectedX + i < 0 || selectedY + j < 0)
                            continue;
                        if (selectedX + i >= chunkSize || selectedY + j >= chunkSize)
                            continue;
                        // test if outside the "circle"
                        if (i * i + j * j + rng.Next(0, randStrength) > radius * radius)
                            continue;
                        // put
                        blockState[selectedX + i, selectedY + j] = state;
                    }
                }
            }
        }
        public Chunk(int posX, int posY,int seed)
        {
            x = posX;
            y = posY;
            Random rng = new Random(posX * 256 + posY + seed);
            blockState = new GroundResource[chunkSize, chunkSize];
            // fill with random grass!
            for (int ptX = 0; ptX < chunkSize; ptX++)
            {
                for (int ptY = 0; ptY < chunkSize; ptY++)
                {
                    blockState[ptX, ptY] = (GroundResource)rng.Next(0, 2); // Grass 1 && Grass 2
                }
            }
            // you can put more to place more resources!
            GenerateCircleRessource(seed, GroundResource.Iron, 4.5f,16); // evl Kohle
        }
        public GroundResource GetSubChunk(int innerX, int innerY)
        {
            return blockState[innerX, innerY];
        }
    }
}
