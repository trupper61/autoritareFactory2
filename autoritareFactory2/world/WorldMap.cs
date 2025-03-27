using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using autoritaereFactory.setup;
using factordictatorship;
using static System.Windows.Forms.AxHost;

namespace autoritaereFactory.world
{
    public class WorldMap
    {
        public static WorldMap theWorld;
        List<Chunk> chunkList;
        int chunkXcount, chunkYcount;
        public WorldMap(int sizeX, int sizeY)
        {
            theWorld = this;
            chunkXcount = sizeX;
            chunkYcount = sizeY;
            chunkList = new List<Chunk>();
        }
        // from and including start with the size (including)
        public List<Fabrikgebeude> GetEntityInBox(int posX, int posY, int width, int height)
        {
            return GetEntityInArea(posX, posY, posX + width, posY + height);
        }
        public List<Fabrikgebeude> GetEntityInPos(int posX, int posY)
        {
            return GetEntityInArea(posX, posY, posX, posY);
        }
        // from and including start till and including end
        public List<Fabrikgebeude> GetEntityInArea(int startX, int startY, int endX, int endY)
        {
            // get the effected chunks!
            // check upper left chunk to be shoure!
            int chunkX = startX / Chunk.chunkSize - 1;
            int chunkY = startY / Chunk.chunkSize - 1;
            int chunkXe = endX / Chunk.chunkSize;
            int chunkYe = endY / Chunk.chunkSize;
            List<Fabrikgebeude> entitys = new List<Fabrikgebeude>();
            // build a list of building inside the area
            foreach (Chunk ch in chunkList)
            {
                if (ch.x < chunkX || ch.y < chunkY) continue;
                if (ch.x > chunkXe || ch.y > chunkYe) continue;

                foreach (Fabrikgebeude building in ch.buildings)
                {
                    if (building.PositionX > endX) continue;
                    if (building.PositionY > endY) continue;
                    if (building.PositionX + building.SizeX < startX) continue;
                    if (building.PositionY + building.SizeY < startY) continue;
                    entitys.Add(building);
                }
            }

            return entitys; // fix from "null"
        }
        /* / use AddEntityAt, but override the position of the building
        public void AddEntityAt(Fabrikgebeude build, int posX, int posY)
        {
            build.x = posX;
            build.y = posY;
            AddEntityAt(build);
        }*/
        // return true if succesful and false for a "out of bounce" place...
        public bool AddEntityAt(Fabrikgebeude build)
        {
            int chunkX = build.PositionX / Chunk.chunkSize;
            int chunkY = build.PositionY / Chunk.chunkSize;
            // out of bounce checks!
            if (chunkX < 0 || chunkY < 0)
                return false;
            if (chunkX > chunkXcount || chunkY > chunkYcount)
                return false;
            if (!IsBoxInside(build.PositionX, build.PositionY, build.SizeX, build.SizeY))
                return false;
            // find the right chunk
            foreach (Chunk ch in chunkList)
            {
                if (ch.x != chunkX || ch.y != chunkY)
                    continue;
                ch.buildings.Add(build);
                return true;
            }
            // if there is no right chunk...
            Chunk newChunk = new Chunk(chunkX, chunkY);
            newChunk.buildings.Add(build);
            chunkList.Add(newChunk);
            return true;
        }
        // check for out of bounce!
        public bool IsBoxInside(int posX, int posY, int width, int height)
        {
            return IsAreaInside(posX, posY, posX + width, posY + height);
        }
        // check for out of bounce!
        public bool IsAreaInside(int startX, int startY, int endX, int endY)
        {
            if (startX < 0 || startY < 0) return false;
            if (
                endX / Chunk.chunkSize > chunkXcount ||
                endY / Chunk.chunkSize > chunkYcount
                )
                return false;
            return true;
        }
        public void RemoveEntity(Fabrikgebeude build)
        {
            int chunkX = build.PositionX / Chunk.chunkSize;
            int chunkY = build.PositionY / Chunk.chunkSize;

            foreach (Chunk ch in chunkList)
            {
                if (ch.x != chunkX || ch.y != chunkY)
                    continue;
                ch.buildings.Remove(build);
            }
        }

        public BlockState? GetBlockState(int posX, int posY)
        {
            int chunkX = posX / Chunk.chunkSize;
            int chunkY = posY / Chunk.chunkSize;
            int subChunkX = posX % Chunk.chunkSize;
            int subChunkY = posY % Chunk.chunkSize;
            foreach (Chunk ch in chunkList)
            {
                if (ch.x != chunkX || ch.y != chunkY)
                    continue;
                return ch.GetSubChunk(subChunkX, subChunkY);
            }
            return null;
        }
        // iterate over every building in the "loaded" world
        public void IterateAll()
        {
            const int timerDelay = 100; // in ms
            foreach (Chunk ch in chunkList)
            {
                foreach (Fabrikgebeude bd in ch.buildings)
                {
                    bd.Iteration();
                }
            }
        }
        public List<Chunk> GetChuckBox(int posX, int posY, int sizeX, int sizeY)
        {
            return GetChuckArea(posX, posY, posX + sizeX, posY + sizeY);
        }
        public List<Chunk> GetChuckArea(int startX, int startY, int endX, int endY)
        {
            // get the effected chunks!
            // check upper left chunk to be shoure!
            int chunkX = startX / Chunk.chunkSize - 1;
            int chunkY = startY / Chunk.chunkSize - 1;
            int chunkXe = endX / Chunk.chunkSize;
            int chunkYe = endY / Chunk.chunkSize;
            List<Chunk> chunks = new List<Chunk>();
            // build a list of building inside the area
            foreach (Chunk ch in chunkList)
            {
                if (ch.x < chunkX || ch.y < chunkY) continue;
                if (ch.x > chunkXe || ch.y > chunkYe) continue;

                chunks.Add(ch);
            }

            return chunks; // fix from "null"
        }
    }
}
