using autoritaereFactory;
using autoritaereFactory.setup;
using factordictatorship.setup;
using factordictatorship.setup.BaenderTypes;
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
            { GroundResource.Grass0 , LoadImage("ground\\Grassland0.png") },
            { GroundResource.Grass1 , LoadImage("ground\\Grassland1.png") },
            { GroundResource.Grass2 , LoadImage("ground\\Grassland2.png") },
            { GroundResource.Grass3 , LoadImage("ground\\Grassland3.png") },
            { GroundResource.Desert , LoadImage("ground\\IndustrialWasteland.png") },
            { GroundResource.Desert2, LoadImage("ground\\IndustrialWasteland2.png") },
            { GroundResource.ColeOre, LoadImage("ground\\ColeOre.png") },
            { GroundResource.IronOre, LoadImage("ground\\iron.png") },
            { GroundResource.CopperOre, LoadImage("ground\\CopperOre.png") },
            { GroundResource.LimeStone, LoadImage("ground\\LimeStone.png") },
        };
        public static Dictionary<int, Image> buildingSet = new Dictionary<int, Image>(){
            {typeof(Band).GetHashCode(),LoadImage("building\\belt.png") },
            {typeof(Konstrucktor).GetHashCode(),LoadImage("building\\factory.png") },
            {typeof(Miner).GetHashCode(),LoadImage("building\\miner.png") },
            {typeof(Fabrikator).GetHashCode(),LoadImage("building\\fabricator.png") },
            {typeof(CurveBand).GetHashCode(),LoadImage("building\\beltCorner.png") }
        };
        public static Dictionary<ResourceType, Image> itemSet = new Dictionary<ResourceType, Image>(){
            {ResourceType.IronOre,LoadImage("item\\iron-ore.png") },
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
