using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using autoritaereFactory.setup;
using static System.Windows.Forms.AxHost;

namespace autoritaereFactory.world
{
    public class WorldMap
    {
        public static WorldMap theWorld;
        List<Chunk> chunkList;
        int chunkXcount, chunkYcount;
        public WorldMap(int sizeX,int sizeY)
        {
            theWorld = this;
            chunkXcount = sizeX;
            chunkYcount = sizeY;
            chunkList = new List<Chunk>();
        }
        // from and including start with the size (including)
        public List<Building> GetEntityInBox(int posX,int posY,int width,int height)
        {
            return GetEntityInArea(posX, posY, posX + width, posY + height);
        }
        public List<Building> GetEntityInPos(int posX, int posY)
        {
            return GetEntityInArea(posX, posY, posX, posY);
        }
        // from and including start till and including end
        public List<Building> GetEntityInArea(int startX,int startY,int endX,int endY)
        {
            // get the effected chunks!
            // check upper left chunk to be shoure!
            int chunkX = startX / Chunk.chunkSize - 1;
            int chunkY = startY / Chunk.chunkSize - 1;
            int chunkXe = startX / Chunk.chunkSize;
            int chunkYe = startY / Chunk.chunkSize;
            List<Building> entitys = new List<Building>();
            // build a list of building inside the area
            foreach (Chunk ch in chunkList)
            {
                if (ch.x < chunkX || ch.y < chunkY) continue;
                if (ch.x > chunkXe || ch.y > chunkYe) continue;

                foreach (Building building in ch.buildings)
                {
                    if (building.x > endX) continue;
                    if (building.y > endY) continue;
                    if (building.x + building.width < startX) continue;
                    if (building.y + building.height < startX) continue;
                    entitys.Add(building);
                }
            }

            return entitys; // fix from "null"
        }
        // use AddEntityAt, but override the position of the building
        public void AddEntityAt(Building build, int posX, int posY)
        {
            build.x = posX;
            build.y = posY;
            AddEntityAt(build);
        }
        // return true if succesful and false for a "out of bounce" place...
        public bool AddEntityAt(Building build)
        {
            int chunkX = build.x / Chunk.chunkSize;
            int chunkY = build.y / Chunk.chunkSize;
            // out of bounce checks!
            if (chunkX < 0 || chunkY < 0)
                return false;
            if (chunkX > chunkXcount || chunkY > chunkYcount)
                return false;
            if (!IsBoxInside(build.x, build.y, build.width, build.height))
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
        public void RemoveEntity(Building build)
        {
            int chunkX = build.x / Chunk.chunkSize;
            int chunkY = build.y / Chunk.chunkSize;

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
            foreach(Chunk ch in chunkList)
            {
                foreach(Building bd in ch.buildings)
                {
                    bd.Update();
                }
            }
        }
    }
}
