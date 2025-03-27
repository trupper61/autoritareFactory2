using autoritaereFactory.world;
using factordictatorship.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace factordictatorship.drawing
{
    public class WorldPartImage
    {
        public bool markeRemove;
        public Bitmap map;
        public autoritaereFactory.world.Chunk chRef;
        public int chunkX, chunkY;
        public WorldPartImage(int WorldPosX,int WorldPosY)
        { // SIZE of ? (64 * 32) ** 2 -> 2048 * 2048 // ist das erlaubt?
            chunkX = WorldPosX;
            chunkY = WorldPosY;
            chRef = WorldMap.theWorld.GetSpecificChunk(chunkX, chunkY);
            map = new Bitmap(ResourceHandler.imageSize * Chunk.chunkSize, ResourceHandler.imageSize * Chunk.chunkSize);
            ReGenerate();
            markeRemove = false;
        }
        public void ReGenerate()
        {
            Graphics grp = Graphics.FromImage(map);
            for (int ptX = 0; ptX < Chunk.chunkSize; ptX++)
            {
                for (int ptY = 0; ptY < Chunk.chunkSize; ptY++)
                {
                    grp.DrawImage(
                    ResourceHandler.tileSet[(int)chRef.GetSubChunk(ptX, ptY)],
                    new Point(ptX * ResourceHandler.imageSize, ptY * ResourceHandler.imageSize)
                    );
                }
            }
            grp.Dispose();
        }
        public void Dispose()
        {
            map.Dispose();
        }
    }
}
