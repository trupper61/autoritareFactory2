using autoritaereFactory.world;
using factordictatorship.Resources;
using factordictatorship.setup;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace factordictatorship.drawing
{
    public class WorldDrawer
    {
        public readonly Color highlightColor = Color.FromArgb(127, 123, 45, 67);
        public static int scale = 1; // 1 to 3 | 1 zoomed in, 3 zoomed out
        public static int GetRealScale { get { return ResourceHandler.imageSize / scale; } }
        bool prevDown = false;
        public world mainForms;
        public double cameraX, cameraY; // in tile pos!
        public List<WorldPartImage> chunkPictures;
        public WorldDrawer(world parent)
        {

            mainForms = parent;
            cameraX = cameraY = 0;
            chunkPictures = new List<WorldPartImage>();
        }
        public void Dispose()
        {
            foreach (WorldPartImage wpi in chunkPictures)
            {
                wpi.Dispose();
            }
        }
        public void Update(PaintEventArgs e, float deltaTime)
        {
            if (mainForms.IsKeyPressed(Keys.Add) && !prevDown)
            {
                scale--;
                if (scale < 1) scale = 1;
            }
            else if (mainForms.IsKeyPressed(Keys.Subtract) && !prevDown)
            {
                scale++;
                if (scale > 3) scale = 3;
            }
            prevDown = mainForms.IsKeyPressed(Keys.Add) || mainForms.IsKeyPressed(Keys.Subtract);
            double strength = (mainForms.IsKeyPressed(Keys.ShiftKey) ? 50 : 12);
            double deltaX = ((mainForms.IsKeyPressed(Keys.Right) || mainForms.IsKeyPressed(Keys.D)) ? strength : 0);
            deltaX -= ((mainForms.IsKeyPressed(Keys.Left) || mainForms.IsKeyPressed(Keys.A)) ? strength : 0);
            double deltaY = ((mainForms.IsKeyPressed(Keys.Down) || mainForms.IsKeyPressed(Keys.S)) ? strength : 0);
            deltaY -= ((mainForms.IsKeyPressed(Keys.Up) || mainForms.IsKeyPressed(Keys.W)) ? strength : 0);
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
                (int)Math.Ceiling(mainForms.Width / (float)GetRealScale),
                (int)Math.Ceiling(mainForms.Height / (float)GetRealScale));
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
                    // generate new chunks (yes this is expencive)
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
                    new Rectangle( // note to self DON'T USE RectangleF
                        (int)Math.Round((wpi.chRef.x * Chunk.chunkSize - cameraX) * GetRealScale),
                        (int)Math.Round((wpi.chRef.y * Chunk.chunkSize - cameraY) * GetRealScale),
                        Chunk.chunkSize * GetRealScale, Chunk.chunkSize * GetRealScale)
                    );
            }
            foreach(WorldPartImage wpi in chunkPictures)
            {
                foreach(Fabrikgebeude fbu in wpi.chRef.buildings)
                {
                    Image drawImg = ResourceHandler.buildingSet[fbu.GetType().GetHashCode() + fbu.Drehung - 1];
                    //Rectangle pos = new Rectangle(TranslateWorld2Screen(new Point(fbu.PositionX, fbu.PositionY)), drawImg.Size);
                    Rectangle pos = new Rectangle(
                        TranslateWorld2Screen(new Point(fbu.PositionX, fbu.PositionY)),
                        new Size(fbu.SizeX * GetRealScale, fbu.SizeY * GetRealScale));
                    grp.DrawImage(  drawImg,pos);

                    // try draw content of the Band
                    if (fbu.GetType() != typeof(Band))
                        continue;
                    Band bandFbu = (Band)fbu;
                    if (bandFbu.currentRescourceList.Count == 0)
                        continue;
                    if(ResourceHandler.itemSet.ContainsKey(bandFbu.currentRescourceList[0].Type))
                        grp.DrawImage(ResourceHandler.itemSet[bandFbu.currentRescourceList[0].Type],pos);
                }
            }
            //grp.Dispose();
        }
        public void DrawHover(PaintEventArgs e, Point tilePos)
        {
            DrawHover(e, tilePos, highlightColor);
        }
        public void DrawHover(PaintEventArgs e, Point tilePos, Color highColor)
        {
            // don't draw out of bounce (oob) of the world!
            if (tilePos.X < 0) return;
            if (tilePos.Y < 0) return;
            if (tilePos.X >= WorldMap.theWorld.chunkXcount * Chunk.chunkSize) return;
            if (tilePos.Y >= WorldMap.theWorld.chunkXcount * Chunk.chunkSize) return;
            tilePos = TranslateWorld2Screen(tilePos);
            // don't draw out of bounce (oob) of the screen!
            if (tilePos.X < -GetRealScale || tilePos.Y < -GetRealScale)
                return;
            if (tilePos.X > mainForms.Width || tilePos.Y > mainForms.Height)
                return;
            SolidBrush alphaBrush = new SolidBrush(highColor);
            Rectangle drawRect = new Rectangle(tilePos, new Size(GetRealScale, GetRealScale));
            e.Graphics.FillRectangle(alphaBrush, drawRect);
            alphaBrush.Dispose();
        }
        public void DrawPlacableBuilding(PaintEventArgs e, Point tilePos,Fabrikgebeude fbu,Color drawColor)
        {
            // don't draw out of bounce (oob) of the world!
            if (tilePos.X < 0) return;
            if (tilePos.Y < 0) return;
            if (tilePos.X >= WorldMap.theWorld.chunkXcount * Chunk.chunkSize) return;
            if (tilePos.Y >= WorldMap.theWorld.chunkXcount * Chunk.chunkSize) return;
            tilePos = TranslateWorld2Screen(tilePos);
            // don't draw out of bounce (oob) of the screen!
            if (tilePos.X < -GetRealScale || tilePos.Y < -GetRealScale)
                return;
            if (tilePos.X > mainForms.Width || tilePos.Y > mainForms.Height)
                return;
            // get image
            Image factoryBuilding = ResourceHandler.buildingSet[fbu.GetType().GetHashCode() + fbu.Drehung- 1];

            // https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/how-to-use-a-color-matrix-to-set-alpha-values-in-images?view=netframeworkdesktop-4.8
            // Initialize the color matrix.
            // Note the value 0.8 in row 4, column 4.
            float[][] matrixItems ={
   new float[] { drawColor.R / 256f, 0, 0, 0, 0}, // red channel
   new float[] {0, drawColor.G / 256f, 0, 0, 0}, // green channel
   new float[] {0, 0, drawColor.B / 256f, 0, 0}, // blue channel
   new float[] {0, 0, 0, drawColor.A / 256f, 0}, // alpha channel
   new float[] {0, 0, 0, 0, 1}}; // bias? channel
            ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

            // Create an ImageAttributes object and set its color matrix.
            ImageAttributes imageAtt = new ImageAttributes();
            imageAtt.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            // drawing
            Rectangle drawRect = new Rectangle(tilePos, new Size(fbu.SizeX * GetRealScale, fbu.SizeY * GetRealScale));
            e.Graphics.DrawImage(
                factoryBuilding,drawRect,
                0,0,factoryBuilding.Size.Width,
                factoryBuilding.Size.Height,
                GraphicsUnit.Pixel,imageAtt);
        }

        // world pos as in tile-position!
        internal Point TranslateScreen2World(Point p)
        {
            double localX,localY;
            localX = p.X / (float)GetRealScale + cameraX;
            localY = p.Y / (float)GetRealScale + cameraY;
            return new Point((int)localX, (int)localY);
        }
        // world pos as in tile-position!
        internal Point TranslateWorld2Screen(Point p)
        {
            double localX, localY;
            localX = (p.X - cameraX) * GetRealScale;
            localY = (p.Y - cameraY) * GetRealScale;
            localX = Math.Round(localX);
            localY = Math.Round(localY);
            return new Point((int)localX, (int)localY);
        }
    }
}
