using System;
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
        public static Image[] tileSet = { LoadImage("grass.png") , LoadImage("iron.png") };
        public const int imageSize = 16;
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
    }
}
