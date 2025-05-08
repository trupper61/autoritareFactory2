using autoritaereFactory.setup;
using factordictatorship.setup;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factordictatorship.Resources
{
    public class ResourceHandler
    {
        public static Dictionary<GroundResource, Image> tileSet = new Dictionary<GroundResource, Image>(){
            { GroundResource.Grass , LoadImage("grass.png") },
            { GroundResource.Grass2 , LoadImage("grass2.png") },
            { GroundResource.Desert , LoadImage("IndustrialWasteland.png") },
            { GroundResource.Desert2, LoadImage("IndustrialWasteland2.png") },
            { GroundResource.ColeOre, LoadImage("ColeOre.png") },
            { GroundResource.IronOre, LoadImage("iron.png") } };
        public static Dictionary<int, Image> buildingSet = new Dictionary<int, Image>(){
            {typeof(Band).GetHashCode(),LoadImage("belt.png") },
            {typeof(Konstrucktor).GetHashCode(),LoadImage("factory.png") },
            {typeof(Miner).GetHashCode(),LoadImage("miner.png") } 
        };
        public const int imageSize = 32;
        private static Image LoadImage(string path)
        {
            path = "Resources\\" + path;
            for (int i = 0; i < 3; i++)
            {
                if (File.Exists(path))
                {
                    Image image = Image.FromFile(path);
                    return image;
                }
                path = "..\\" + path;
            }
            return null;
        }
        internal static void SetupBuilding()
        {
            List<int> keys = buildingSet.Keys.ToList();
            foreach (int key in keys)
            {
                Image img = buildingSet[key];
                //
                for (int i = 1; i <= 3; i++)
                {
                    img = (Image)img.Clone();
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    buildingSet.Add(key + i, img);
                }
            }
        }
    }
}
