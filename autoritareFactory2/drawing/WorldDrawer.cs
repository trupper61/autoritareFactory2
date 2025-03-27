using autoritaereFactory.world;
using factordictatorship.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace factordictatorship.drawing
{
    public class WorldDrawer : Control
    {
        public world mainForms;
        public float cameraX, cameraY;
        public List<WorldPartImage> chunkPictures;
        public WorldDrawer(world parent)
        {
            mainForms = parent;
            cameraX = cameraY = 0;
            chunkPictures = new List<WorldPartImage>();
        }
        public void Update(object sender, PaintEventArgs e)
        {
            float strength = (mainForms.IsKeyPressed(Keys.ShiftKey) ? 5 : 1);
            float deltaX = (mainForms.IsKeyPressed(Keys.Right) ? strength : 0) - (mainForms.IsKeyPressed(Keys.Left) ? strength : 0);
            float deltaY = (mainForms.IsKeyPressed(Keys.Down) ? strength : 0) - (mainForms.IsKeyPressed(Keys.Up) ? strength : 0);
            cameraX += deltaX;// + (float)Math.Sin(DateTime.Now.Second * 0.1f) ;
            cameraY += deltaY;
            if (cameraX < 0) cameraX = 0;
            if (cameraY < 0) cameraY = 0;
            DrawWorld(e);
        }
        public void DrawWorld(PaintEventArgs e)
        {
            Graphics grp = e.Graphics;
            //Graphics grp = mainForms.CreateGraphics();
            List<Chunk> drawChunks = mainForms.mapWorld.GetChuckBox(
                (int)(cameraX / (float)ResourceHandler.imageSize), (int)(cameraY / (float)ResourceHandler.imageSize),
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
                        (int)(wpi.chRef.x * Chunk.chunkSize * ResourceHandler.imageSize - cameraX),
                        (int)(wpi.chRef.y * Chunk.chunkSize * ResourceHandler.imageSize - cameraY))
                    );
            }
            //grp.Dispose();
        }
    }
}
