﻿using System;
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
        // yes this function is slow, but who cares anyways.
        private static float lowbias32(int seed, uint x)
        {
            x += (uint)seed;
            x ^= x >> 16;
            x *= 0x7feb352dU;
            x ^= x >> 15;
            x *= 0x846ca68bU;
            x ^= x >> 16;
            return x / (float)UInt32.MaxValue;
        }
        private static float interpelate(float a, float b, float t)
        {
            float choose = -2 * t * t * t + 3 * t * t;
            return b * choose + a * (1 - choose);
        }

        private static float GenerateNoiseValue(int seed, float x,float y)
        {
            int posX = (int)x;
            int posY = (int)y;
            float fracX = x - posX;
            float fracY = y - posY;
            /* // triangle(aka Simplex) value noise
            float valA = lowbias32(seed, (uint)(posX + (posY << 8)));
            float valB = lowbias32(seed, (uint)(1 + posX + ((1+ posY) << 8)));
            float edge = interpelate(valA,valB, fracX);
            float valC;if (fracX > fracY)
            {
                valC = lowbias32(seed, (uint)(1 + posX + ((posY) << 8)));
                return interpelate(edge, valC, fracX * (1- fracY) * (fracX - fracY));
            }
            else
            {
                valC = lowbias32(seed, (uint)(posX + ((posY + 1) << 8)));
                return interpelate(edge, valC, fracY * (1 - fracX) * (fracY - fracX));
            }/*/ // normal value noise
            float valA = lowbias32(seed, (uint)(posX + (posY << 8)));
            float valB = lowbias32(seed, (uint)(1 + posX + ((posY) << 8)));
            float edge = interpelate(valA, valB, fracX);
            float valC = lowbias32(seed, (uint)(posX + ((1+ posY) << 8)));
            float valD = lowbias32(seed, (uint)(1 + posX + ((1 + posY) << 8)));
            float edgeB = interpelate(valC, valD, fracX);
            return interpelate(edge, edgeB, fracY);
            // */
            //return (float)(Math.Sin(x) + Math.Sin(y)) / 4.0f + 0.5f;
        }
        private static float TestBiomeType(int seed, float x,float y,float expTem,float expHum)
        {
            float tem = GenerateNoiseValue(seed, x * 0.01f, y * 0.01f);
            float hum = GenerateNoiseValue(seed, x * 0.1f - 10, y * 0.1f - 3);
            return tem * expTem + hum * expHum;
        }
        private void GenerateBlobGround(
            int seed, GroundResource state,int randStrength, 
            float expTem,float expHum,float cutoff)
        {
            // maybe fill the tiles with the resource
            for (int i = 0; i < chunkSize; i++)
            {
                for (int j = 0; j < chunkSize; j++)
                {
                    // test if outside the "circle"
                    if (TestBiomeType(seed, i + x * chunkSize, j + y * chunkSize, expTem, expHum) < cutoff)
                        continue;
                    // put
                    blockState[i,j] = state;
                }
            }
        }
        private void GenerateCircleRessource(int seed, GroundResource state, float radius, int randStrength, float existenceChance,
            float expTem,float expHum,float cutoff)
        {
            // explorer the neighbours with this funny loop
            for (int neb = 0; neb < 9; neb++)
            {
                // create an fitting random function
                Random rng = new Random((x + neb / 3 - 1) * 256 + (y + neb % 3 - 1) + seed);
                rng.Next();// more random!
                if (existenceChance <= rng.NextDouble())
                    continue;
                // get the chosen position
                int selectedX = rng.Next(chunkSize) + (neb / 3 - 1) * chunkSize;
                int selectedY = rng.Next(chunkSize) + (neb % 3 - 1) * chunkSize;
                if (cutoff > -5f && TestBiomeType(seed,selectedX + x * chunkSize, selectedY + y * chunkSize, expTem, expHum) < cutoff)
                    continue;
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
        private void GenerateCircleRessource(int seed, GroundResource state, float radius, int randStrength, float existenceChance)
        {
            GenerateCircleRessource(seed, state, radius, randStrength, existenceChance, 0, 0, -10.0f);
        }
        public Chunk(int posX, int posY,int seed)
        {
            x = posX;
            y = posY;
            buildings = new List<Fabrikgebeude>();
            Random rng = new Random(posX * 512 + posY * 4 + seed);
            blockState = new GroundResource[chunkSize, chunkSize];
            // you can put more to place more resources!
            GenerateBlobGround(seed - 2, GroundResource.Desert,5, 1, 0,0.5f);
            GenerateCircleRessource(seed, GroundResource.ColeOre, 4.5f, 16, 1f,0,0.5f,0.25f); // evl Kohle
            GenerateCircleRessource(seed+2, GroundResource.IronOre, 7f, 40, 0.25f); // evl hochwertiges
            // fill with random grass!
            for (int ptX = 0; ptX < chunkSize; ptX++)
            {
                for (int ptY = 0; ptY < chunkSize; ptY++)
                {
                    if (blockState[ptX, ptY] == GroundResource.Grass)
                        blockState[ptX, ptY] = (GroundResource)rng.Next(
                            (int)GroundResource.Grass, (int)GroundResource.GrassUpperBound); // Grass 1 && Grass 2
                    if (blockState[ptX, ptY] == GroundResource.Desert)
                        blockState[ptX, ptY] = (GroundResource)rng.Next(
                            (int)GroundResource.Desert, (int)GroundResource.DesertUpperBound); // Desert 1 && Desert 2
                }
            }
        }
        public GroundResource GetSubChunk(int innerX, int innerY)
        {
            return blockState[innerX, innerY];
        }
    }
}
