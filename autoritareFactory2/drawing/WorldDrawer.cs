using autoritaereFactory.world;
using factordictatorship.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace factordictatorship.drawing
{
    public class WorldDrawer
    {
        public world mainForms;
        public double cameraX, cameraY; // in tile pos!
        public List<WorldPartImage> chunkPictures;
        public WorldDrawer(world parent)
        {
            mainForms = parent;
            cameraX = cameraY = 0;
            chunkPictures = new List<WorldPartImage>();
        }
        public void Update(PaintEventArgs e, float deltaTime)
        {
            double strength = (mainForms.IsKeyPressed(Keys.ShiftKey) ? 50 : 12);
            double deltaX = (mainForms.IsKeyPressed(Keys.Right) ? strength : 0) - (mainForms.IsKeyPressed(Keys.Left) ? strength : 0);
            double deltaY = (mainForms.IsKeyPressed(Keys.Down) ? strength : 0) - (mainForms.IsKeyPressed(Keys.Up) ? strength : 0);
            cameraX += deltaX * deltaTime;
            cameraY += deltaY * deltaTime;
            if (cameraX < 0) cameraX = 0;
            if (cameraY < 0) cameraY = 0;
            if (cameraX > WorldMap.theWorld.chunkXcount * Chunk.chunkSize) cameraX = WorldMap.theWorld.chunkXcount * Chunk.chunkSize;
            if (cameraY > WorldMap.theWorld.chunkXcount * Chunk.chunkSize) cameraY = WorldMap.theWorld.chunkXcount * Chunk.chunkSize;
            DrawWorld(e);
        }
        public void DrawWorld(PaintEventArgs e)
        {
            Graphics grp = e.Graphics;
            //Graphics grp = mainForms.CreateGraphics();
            List<Chunk> drawChunks = mainForms.mapWorld.GetChuckBox(
                (int)(cameraX), (int)(cameraY),
                (int)Math.Ceiling(mainForms.Width / (float)ResourceHandler.imageSize),
                (int)Math.Ceiling(mainForms.Height / (float)ResourceHandler.imageSize));
            //List<Chunk> drawChunks = mapWorld.GetChuckBox((int)cameraX, (int)cameraY,Width, Height );
            /* // this is slow, but shows how this could work
            foreach (Chunk chunk in drawChunks)
            {
                for (int ptX = 0; ptX < Chunk.chunkSize; ptX++)
                {
                    for (int ptY = 0; ptY < Chunk.chunkSize; ptY++)
                    {
                        grp.DrawImage(
                            ResourceHandler.tileSet[(int)chunk.GetSubChunk(ptX, ptY).ressource],
                            new Point(
                                (int)((chunk.x * Chunk.chunkSize + ptX) * scale + cameraX),
                                (int)((chunk.y * Chunk.chunkSize + ptY) * scale + cameraY))
                            );
                    }
                }
            }*/
            // mark all potential chunks as maybe delete!
            foreach (WorldPartImage wpi in chunkPictures)
            {
                wpi.markeRemove = true;
            }
            // add a new chunk-section
            bool chunkContained;
            foreach (Chunk chunk in drawChunks)
            {
                chunkContained = false;
                foreach (WorldPartImage wpi in chunkPictures)
                {
                    if (wpi.chRef == chunk)
                    {
                        wpi.markeRemove = false;
                        chunkContained = true;
                    }
                }
                if (!chunkContained)
                {
                    // generate new chunks
                    WorldPartImage newWpi = new WorldPartImage(chunk.x, chunk.y);
                    chunkPictures.Add(newWpi);
                }
            }
            // draw it!
            for (int chp = chunkPictures.Count - 1;chp >= 0;chp--)
            {
                WorldPartImage wpi = chunkPictures[chp];
                if (wpi.markeRemove){
                    wpi.Dispose();
                    chunkPictures.RemoveAt(chp);
                    continue;
                }
                grp.DrawImage(
                    wpi.map,
                    new Point(
                        (int)((wpi.chRef.x * Chunk.chunkSize - cameraX) * ResourceHandler.imageSize),
                        (int)((wpi.chRef.y * Chunk.chunkSize - cameraY) * ResourceHandler.imageSize))
                    );
            }
            //grp.Dispose();
        }
        public void DrawHover(PaintEventArgs e,Point tilePos)
        {
            Color funnyColor = Color.FromArgb(127, 123, 45, 67);
            // don't draw out of bounce (oob) of the world!
            if (tilePos.X < 0) return;
            if (tilePos.Y < 0) return;
            if (tilePos.X >= WorldMap.theWorld.chunkXcount * Chunk.chunkSize) return ;
            if (tilePos.Y >= WorldMap.theWorld.chunkXcount * Chunk.chunkSize) return;
            tilePos = TranslateWorld2Screen(tilePos);
            // don't draw out of bounce (oob) of the screen!
            if (tilePos.X < -ResourceHandler.imageSize || tilePos.Y < -ResourceHandler.imageSize)
                return;
            if (tilePos.X > mainForms.Width || tilePos.Y > mainForms.Height)
                return;
            SolidBrush alphaBrush = new SolidBrush(funnyColor);
            Rectangle drawRect = new Rectangle(tilePos, new Size(ResourceHandler.imageSize, ResourceHandler.imageSize));
            e.Graphics.FillRectangle(alphaBrush, drawRect);
            alphaBrush.Dispose();
        }

        // world pos as in tile-position!
        internal Point TranslateScreen2World(Point p)
        {
            double localX,localY;
            localX = p.X / (float)ResourceHandler.imageSize + cameraX;
            localY = p.Y / (float)ResourceHandler.imageSize + cameraY;
            return new Point((int)localX, (int)localY);
        }
        // world pos as in tile-position!
        internal Point TranslateWorld2Screen(Point p)
        {
            double localX, localY;
            localX = (p.X - cameraX) * ResourceHandler.imageSize;
            localY = (p.Y - cameraY) * ResourceHandler.imageSize;
            return new Point((int)localX, (int)localY);
        }
    }
}
