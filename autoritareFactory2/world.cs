using autoritaereFactory.world;
using factordictatorship.drawing;
using factordictatorship.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace factordictatorship
{
    public partial class world : Form
    {
        public WorldMap mapWorld;
        public Dictionary<Keys, bool> keyHit;
        public Timer frameSceduler;
        public WorldDrawer wlrdDrawer;
        public Point lastMousePos;
        long lastTimeTick;
        public Panel uiPanel;
        public Button buildBtn;
        public Button destroyBtn;
        public string aktuellerModus;
        public world()
        {
            InitializeComponent();
            InitUI();
            mapWorld = new WorldMap(8, 8);
            for (int wrdX = 0; wrdX < mapWorld.chunkXcount; wrdX++)
            {
                for (int wrdY = 0; wrdY < mapWorld.chunkXcount; wrdY++)
                {
                    mapWorld.GenerateChunk(wrdX, wrdY);
                }
            }
            SetupTest();
            SettingUpWorldDrawer();
        }
        public void SetupTest()
        {
            Konstrucktor testKonst = new Konstrucktor(4, 5);
            mapWorld.AddEntityAt(testKonst);
        }
        // only use once
        private void SettingUpWorldDrawer()
        {
            // get good delta times
            lastTimeTick = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            wlrdDrawer = new WorldDrawer(this);
            //Controls.Add(wlrdDrawer);
            //KeyDown += OnKeyDown; // this seemed to not work with double buffered Graphics!
            //KeyUp += OnKeyUp;
            // refresh loop
            frameSceduler = new Timer() { Interval = 16, Enabled = true };
            frameSceduler.Tick += RefreshLoop;
            // event handler
            keyHit = new Dictionary<Keys, bool>();
            MouseClick += OnClick;
            MouseMove += OnMouseMove;
            Paint += PaintHandler;
            FormClosed += OnFormClosed;
        }


        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            lastMousePos = e.Location;
        }

        private void OnClick(object sender, MouseEventArgs e)
        {
            lastMousePos = e.Location;
        }

        public void RefreshLoop(object sender, EventArgs e)
        {
            this.Invalidate(DisplayRectangle);
        }
        public void PaintHandler(object sender, PaintEventArgs e)
        {
            long testTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            float deltaMs = (testTime - lastTimeTick) / 1000f;
            lastTimeTick = testTime;
            // draw the world!
            wlrdDrawer.Update(e, deltaMs);
            if (Focused)
            {
                Point worldPoint = wlrdDrawer.TranslateScreen2World(lastMousePos);
                //// draw the hover thing!
                //wlrdDrawer.DrawHover(e,worldPoint);
                // this is really badly optimised... (Who cares)
                Konstrucktor kot = new Konstrucktor(worldPoint.X, worldPoint.Y);
                List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(kot.PositionX, kot.PositionY, kot.SizeX, kot.SizeY);
                if (lffb.Count == 0)
                    wlrdDrawer.DrawPlacableBuilding(e, worldPoint, kot, Color.FromArgb(127, 127, 255, 95));
                else
                    wlrdDrawer.DrawPlacableBuilding(e, worldPoint, kot, Color.FromArgb(127, 255, 64, 16));
            }
        }
        // this is a fix for not working "OnKey"-Events
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);
        public bool IsKeyPressed(Keys k)
        {
            // don't check if not focused
            return ContainsFocus && (GetKeyState((int)k) & 0x8000) > 0;
        }


        // no work if double buffered
        //private void OnKeyUp(object sender, KeyEventArgs e)
        //{
        //    Invalidate();
        //    keyHit[e.KeyCode] = false;
        //}
        //private void OnKeyDown(object sender, KeyEventArgs e)
        //{
        //    Invalidate();
        //    keyHit[e.KeyCode] = true;
        //}

        // free computer-resources (aka Threads9
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            wlrdDrawer.Dispose();
            mapWorld.Dispose();
        }
private void InitUI()
        {
            uiPanel = new Panel
            {
                Size = new Size(this.Width, 50),
                BackColor = Color.Gray,
                Dock = DockStyle.Top
            };
            Controls.Add(uiPanel);

            buildBtn = new Button
            {
                Text = "Build",
                Location = new Point(10, 10)
            };
            buildBtn.Click += (s, e) => aktuellerModus = "Build";
            uiPanel.Controls.Add(buildBtn);

            destroyBtn = new Button
            {
                Text = "Destroy",
                Location = new Point(100, 10)
            };
            destroyBtn.Click += (s, e) => aktuellerModus = "Destroy";
            uiPanel.Controls.Add(destroyBtn);
        }
    }
}
