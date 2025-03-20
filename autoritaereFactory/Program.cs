using autoritaereFactory.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autoritaereFactory
{
    internal static class Program
    {
        public static Thread worldIteratorThread;
        public static WorldMap worldMap;
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // the world map iterator
            worldMap = new WorldMap(8,8); // worldsize of currently 8 by 8 chunks
            worldIteratorThread = new Thread(new ThreadStart(WorldMap.theWorld.IterateAll));
            worldIteratorThread.Start();
            // *//
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
