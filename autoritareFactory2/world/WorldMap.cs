﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using autoritaereFactory.setup;
using factordictatorship;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace autoritaereFactory.world
{
    public class WorldMap : IDisposable
    {
        public Mutex mut;
        public long tickTimer = 0;
        Thread iteratorThread;
        bool shouldStopThread = false;
        int seed = (new Random()).Next();
        public static WorldMap theWorld;
        List<Chunk> chunkList;
        public string worldName;
        public int chunkXcount, chunkYcount;
        private WorldMap()
        {
            mut = new Mutex();
            theWorld = this;
            chunkList = new List<Chunk>();
            iteratorThread = new Thread(IterateAll) { Name = "World-Worker-Thread" };
        }
        public WorldMap(int sizeX, int sizeY)
        {
            mut = new Mutex();
            worldName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            theWorld = this;
            chunkXcount = sizeX;
            chunkYcount = sizeY;
            chunkList = new List<Chunk>();
            iteratorThread = new Thread(IterateAll) { Name = "World-Worker-Thread" };
            _StartThread();
        }
        public void _StartThread()
        {
            shouldStopThread = false;
            iteratorThread.Start();
        }
        // this stops the worker thread
        public void Dispose()
        {
            shouldStopThread = true;
        }
        public void AwaitThread()
        {
            iteratorThread.Join();
        }
        // from and including start with the size (including)
        public List<Fabrikgebeude> GetEntityInBox(int posX, int posY, int width, int height)
        {
            return GetEntityInArea(posX, posY, posX + width - 1, posY + height - 1);
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
            for(int chIndex = 0;chIndex < chunkList.Count;chIndex++)
            {
                Chunk ch = chunkList[chIndex];
                if (ch.x < chunkX || ch.y < chunkY) continue;
                if (ch.x > chunkXe || ch.y > chunkYe) continue;

                for (int bInd = 0; bInd < ch.buildings.Count;bInd++)
                {
                    Fabrikgebeude building = ch.buildings[bInd];
                    if (building.PositionX > endX) continue;
                    if (building.PositionY > endY) continue;
                    if (building.PositionX + building.SizeX <= startX) continue;
                    if (building.PositionY + building.SizeY <= startY) continue;
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
            Chunk newChunk = new Chunk(chunkX, chunkY, seed);
            newChunk.buildings.Add(build);
            chunkList.Add(newChunk);
            return true;
        }
        public bool GenerateChunk(int chunkX,int chunkY)
        {
            if (chunkX < 0 || chunkY < 0)
                return false;
            if (chunkX > chunkXcount || chunkY > chunkYcount)
                return false;
            // find the right chunk
            foreach (Chunk ch in chunkList)
            {
                if (ch.x != chunkX || ch.y != chunkY)
                    continue;
                return false;
            }
            Chunk newChunnk = new Chunk(chunkX, chunkY, seed);
            chunkList.Add(newChunnk);
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

        public GroundResource GetBlockState(int posX, int posY)
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
            return (GroundResource)(-1);
        }
        // iterate over every building in the "loaded" world
        long lastTimeTick;
        public void IterateAll()
        {
            lastTimeTick = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            const int timerDelay = 100; // in ms
            while (!shouldStopThread)
            {
                mut.WaitOne();
                tickTimer++;
                for (int chIndex = 0; chIndex < chunkList.Count;chIndex++)
                {
                    Chunk ch = chunkList[chIndex];
                    for(int bInd = 0;bInd < ch.buildings.Count;bInd++)
                    {
                        Fabrikgebeude bd = ch.buildings[bInd];
                        bd.Iteration();
                    }
                }
                lastTimeTick = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                mut.ReleaseMutex();
                Thread.Sleep(timerDelay - (int)(lastTimeTick % timerDelay));
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
        // 
        public Chunk GetSpecificChunk(int chunkX,int chunkY)
        {
            // build a list of building inside the area
            foreach (Chunk ch in chunkList)
            {
                if (ch.x == chunkX && ch.y == chunkY)
                    return ch;
            }
            return null; // fix from "null"
        }
        private const int SPECIAL_FILE_NUMBER = 4469;
        public List<byte> GetAsBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((int)SPECIAL_FILE_NUMBER));
            bytes.AddRange(BitConverter.GetBytes((int)SPECIAL_FILE_NUMBER));
            //
            bytes.AddRange(BitConverter.GetBytes((int)Chunk.chunkSize)); // check before something bad happens
            bytes.AddRange(BitConverter.GetBytes((int)seed));
            //
            bytes.AddRange(BitConverter.GetBytes((long)tickTimer));
            // this is making someone happy, just to notice, that it won't do anything! (Ha Ha)
            bytes.AddRange(BitConverter.GetBytes((int)chunkXcount));
            bytes.AddRange(BitConverter.GetBytes((int)chunkYcount));
            bytes.AddRange(BitConverter.GetBytes((int)chunkList.Count));
            for (int ch = 0; ch < chunkList.Count;ch++)
            {
                bytes.AddRange(chunkList[ch].GetAsBytes());
            }
            // room for future stuff!
            return bytes;
        }
        public static WorldMap FromByteArray(byte[] bytes, ref int offset)
        {
            WorldMap newMap = new WorldMap();
            if (BitConverter.ToInt32(bytes, offset) != SPECIAL_FILE_NUMBER)
                return null;
            if (BitConverter.ToInt32(bytes, offset + 4) != SPECIAL_FILE_NUMBER)
                return null;
            if (BitConverter.ToInt32(bytes, offset + 8) != Chunk.chunkSize)
                return null;
            offset += 12;
            newMap.seed = BitConverter.ToInt32(bytes, offset);
            newMap.tickTimer = BitConverter.ToInt64(bytes, offset + 4);
            newMap.chunkXcount = BitConverter.ToInt32(bytes, offset + 12);
            newMap.chunkYcount = BitConverter.ToInt32(bytes, offset + 16);
            int chunkCount = BitConverter.ToInt32(bytes, offset + 20);
            offset += 24;
            for (int ch = 0; ch < chunkCount; ch++)
            {
                if (bytes[offset++] == (byte)SavingPackets.ChunkPacket)
                    newMap.chunkList.Add(Chunk.FromByteArray(bytes, ref offset));
                else
                    throw new Exception("this shouldn't bee here!");
            }
            // room for future stuff!
            return newMap;
        }
    }
}
