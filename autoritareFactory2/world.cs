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
        public Button buildBtn;
        public Button destroyBtn;
        public string aktuellerModus = null;
        public Panel buildPanel;
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

                if (aktuellerModus == "Constructor")
                {
                    Konstrucktor kot = new Konstrucktor(worldPoint.X, worldPoint.Y);
                    List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(kot.PositionX, kot.PositionY, kot.SizeX, kot.SizeY);
                    if (lffb.Count == 0)
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, kot, Color.FromArgb(127, 127, 255, 95));
                    else
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, kot, Color.FromArgb(127, 255, 64, 16));
                }
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
            int panelWidth = (int)(this.Width * 0.7);
            int panelHeight = (int)(this.Height * 0.7);
            buildPanel = new Panel
            {
                Size = new Size(panelWidth, panelHeight),
                Location = new Point((this.Width - panelWidth) / 2, (this.Height - panelHeight)/ 2),
                BackColor = Color.LightGray,
                Visible = false,
            };
            Controls.Add(buildPanel);
            SetupBuildPanel();
            ToolStrip toolStrip = new ToolStrip();
            toolStrip.Dock = DockStyle.Top;

            ToolStripButton buildBtn = new ToolStripButton("Build");
            buildBtn.Click += (s, e) => 
            {
                if (aktuellerModus == null)
                {
                    buildPanel.Visible = true;
                    buildPanel.BringToFront();
                }
                else
                {
                    aktuellerModus = null;
                    buildPanel.Visible = false;
                }   
            };
            toolStrip.Items.Add(buildBtn);

            ToolStripButton destroyBtn = new ToolStripButton("Destroy");
            destroyBtn.Click += (s, e) => aktuellerModus = "Destroy";
            toolStrip.Items.Add(destroyBtn);

            Controls.Add(toolStrip);
        }
        private void SetupBuildPanel()
        {
            buildPanel.Controls.Clear();
            int panelWidth = buildPanel.Width;
            int panelHeight = buildPanel.Height;

            Panel inventoryPanel = new Panel
            {
                Size = new Size(panelWidth / 2, panelHeight),
                Location = new Point(0, 0),
                BackColor = Color.DarkGray
            };
            Label inventoryLabel = new Label
            {
                Text = "Inventar (WIP)",
                AutoSize = false,
                Size = new Size(inventoryPanel.Width, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 20)
            };
            inventoryPanel.Controls.Add(inventoryLabel);
            buildPanel.Controls.Add(inventoryPanel);

            Panel buildOptionsPanel = new Panel
            {
                Size = new Size(panelWidth / 2, panelHeight),
                Location = new Point(panelWidth / 2, 0),
                BackColor = Color.LightBlue
            };
            Label titleLabel = new Label
            {
                Text = "Choose Building",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            buildOptionsPanel.Controls.Add(titleLabel);

            var buildings = new List<String>
            {
                "Miner",
                "Constructor",
                "Belt"
            };
            int y = 50;
            foreach (var name in buildings)
            {
                Button btn = new Button
                {
                    Text = name,
                    Size = new Size(120, 35),
                    Location = new Point(10, y),
                    BackColor = Color.LightSteelBlue
                };
                btn.Click += (s, e) =>
                {
                    aktuellerModus = name;
                    buildPanel.Visible = false;
                    this.Focus();
                };
                buildOptionsPanel.Controls.Add(btn);
                y += 45;
            }
            buildPanel.Controls.Add(buildOptionsPanel);
        }
    }
}
