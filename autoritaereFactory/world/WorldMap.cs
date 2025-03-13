using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using autoritaereFactory.setup;
using static System.Windows.Forms.AxHost;

namespace autoritaereFactory.world
{
    internal class WorldMap
    {
        List<Chunk> chunkList;

        public List<Building> GetEntityInArea(int startX,int startY,int endX,int endY)
        {
            // get the effected chunks!
            int chunkX = startX / Chunk.chunkSize;
            int chunkY = startY / Chunk.chunkSize;
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

            return null;
        }
        public void AddEntityAt(Building build, int posX, int posY)
        {
            build.x = posX;
            build.y = posY;
            AddEntityAt(build);
        }
        public void AddEntityAt(Building build)
        {
            int chunkX = build.x / Chunk.chunkSize;
            int chunkY = build.y / Chunk.chunkSize;

            foreach (Chunk ch in chunkList)
            {
                if (ch.x != chunkX || ch.y != chunkY)
                    continue;
                ch.buildings.Add(build);
            }
        }
        public List<Building> GetEntityInPos(int posX,int posY)
        {
            return GetEntityInArea(posX,posY,posX,posY);
        }
        public BlockState GetBlockState(int posX, int posY)
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
    }
}
