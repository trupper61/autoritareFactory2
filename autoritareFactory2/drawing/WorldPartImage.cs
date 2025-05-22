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
        // does this do anything?
        public Bitmap map2;
        public Bitmap map3;
        public autoritaereFactory.world.Chunk chRef;
        public int chunkX, chunkY;
        public WorldPartImage(int WorldPosX,int WorldPosY)
        { // SIZE of ? (64 * 32) ** 2 -> 2048 * 2048 // ist das erlaubt?
            chunkX = WorldPosX;
            chunkY = WorldPosY;
            chRef = WorldMap.theWorld.GetSpecificChunk(chunkX, chunkY);
            map = new Bitmap(ResourceHandler.imageSize * Chunk.chunkSize, ResourceHandler.imageSize * Chunk.chunkSize,System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
            // does this do anything?
            map2 = new Bitmap(ResourceHandler.imageSize * Chunk.chunkSize / 2, ResourceHandler.imageSize * Chunk.chunkSize / 2, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
            map3 = new Bitmap(ResourceHandler.imageSize * Chunk.chunkSize / 4, ResourceHandler.imageSize * Chunk.chunkSize / 4, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
            ReGenerate();
            markeRemove = false;
        }
        public void ReGenerate()
        {
            Graphics grp = Graphics.FromImage(map);
            // does this do anything?
            Graphics grp2 = Graphics.FromImage(map2);
            Graphics grp3 = Graphics.FromImage(map3);
            for (int ptX = 0; ptX < Chunk.chunkSize; ptX++)
            {
                for (int ptY = 0; ptY < Chunk.chunkSize; ptY++)
                {
                    grp.DrawImage(
                    ResourceHandler.tileSet[chRef.GetSubChunk(ptX, ptY)],
                    new Rectangle(
                        ptX * ResourceHandler.imageSize, ptY * ResourceHandler.imageSize,
                        ResourceHandler.imageSize, ResourceHandler.imageSize)
                    );
                    // does this do anything?
                    grp2.DrawImage(
                    ResourceHandler.tileSet[chRef.GetSubChunk(ptX, ptY)],
                    new Rectangle(
                        ptX * ResourceHandler.imageSize / 2, ptY * ResourceHandler.imageSize / 2,
                        ResourceHandler.imageSize / 2, ResourceHandler.imageSize / 2)
                    );
                    grp3.DrawImage(
                    ResourceHandler.tileSet[chRef.GetSubChunk(ptX, ptY)],
                    new Rectangle(
                        ptX * ResourceHandler.imageSize / 4, ptY * ResourceHandler.imageSize / 4,
                        ResourceHandler.imageSize / 4, ResourceHandler.imageSize / 4)
                    );
                }
            }
            grp.Dispose();
            // does this do anything?
            grp2.Dispose();
            grp3.Dispose();
        }
        public void Dispose()
        {
            map.Dispose();
            // does this do anything?
            map2.Dispose();
            map3.Dispose();
        }
    }
}
