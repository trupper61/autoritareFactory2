﻿using autoritaereFactory;
using autoritaereFactory.setup;
using autoritaereFactory.world;
using factordictatorship.drawing;
using factordictatorship.Resources;
using factordictatorship.setup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using ResourceType = autoritaereFactory.ResourceType;

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
        public string aktuellerModus = "";
        public Panel buildPanel;
        public Panel menuPanel;
        public Rezepte Eisenbarren = new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.IronOre, 1, "Eisenbarren", autoritaereFactory.ResourceType.IronIngot, 1, 1000);
        public Rezepte Eisenstange = new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.IronIngot, 1, "Eisenstange", autoritaereFactory.ResourceType.IronStick, 1, 800);
        public Rezepte Eisenplatte = new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.IronIngot, 3, "Eisenstange", autoritaereFactory.ResourceType.IronPlate, 2, 1500);
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
            Konstrucktor testKonst = new Konstrucktor(4, 5, 1);
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
        public static autoritaereFactory.ResourceType GetResourceFromGround(GroundResource grs)
        {
            switch (grs)
            {
                case GroundResource.Iron:return autoritaereFactory.ResourceType.IronOre;
                default:return (autoritaereFactory.ResourceType)(-1);
            }
        }

        private void OnClick(object sender, MouseEventArgs e)
        {
            lastMousePos = e.Location;
            Point worldPoint = wlrdDrawer.TranslateScreen2World(lastMousePos);
            if (aktuellerModus == "Constructor")
            {

                Konstrucktor kon = new Konstrucktor(worldPoint.X, worldPoint.Y, 1);
                List<Fabrikgebeude> conflictingEntities = mapWorld.GetEntityInBox(kon.PositionX, kon.PositionY, kon.SizeX, kon.SizeY);
                if (conflictingEntities.Count == 0)
                {
                    // Konstruktor platzieren
                    mapWorld.AddEntityAt(kon);
                    aktuellerModus = null; // Konstruktor-Modus deaktivieren, um Bau abzuschließen
                }
                else
                {
                    MessageBox.Show("Der Platz ist ungültig. Wählen Sie einen anderen Platz.");
                }
            }
            else if (aktuellerModus == "Miner")
            {
                GroundResource resource = mapWorld.GetBlockState(worldPoint.X, worldPoint.Y);
                // TODO Miner Resource zu GroundResource ändern
                Miner miner = new Miner(worldPoint.X, worldPoint.Y, 1, GetResourceFromGround(resource));
                List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(miner.PositionX, miner.PositionY, miner.SizeX, miner.SizeY);
                if (lffb.Count == 0 && resource == GroundResource.Iron)
                {
                    mapWorld.AddEntityAt(miner);
                    aktuellerModus = null;
                }
                else
                {
                    MessageBox.Show("Der Platz ist ungültig. Wählen Sie einen anderen Platz.");
                }
            }
            else if (aktuellerModus == "Belt")
            {
                Band belt = new Band(3, 20, worldPoint.X, worldPoint.Y);
                List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(belt.PositionX, belt.PositionY, belt.SizeX, belt.SizeY);
                if (lffb.Count == 0)
                {
                    mapWorld.AddEntityAt(belt);
                    aktuellerModus = null;
                }
                else
                {
                    MessageBox.Show("Der Platz ist ungültig. Wählen Sie einen anderen Platz.");
                }
            }
            else if (aktuellerModus == "Destroy")
            {
                List<Fabrikgebeude> fab = mapWorld.GetEntityInPos(worldPoint.X, worldPoint.Y);
                if (fab.Count == 0)
                {
                    MessageBox.Show("Nix zum Löschen");
                }
                else
                {
                    foreach (Fabrikgebeude f in fab)
                    {
                        mapWorld.RemoveEntity(f);
                    }
                    aktuellerModus = null;
                }
            }
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
            // preview
            if (Focused)
            {
                Point worldPoint = wlrdDrawer.TranslateScreen2World(lastMousePos);
                //// draw the hover thing!
                //wlrdDrawer.DrawHover(e,worldPoint);
                // this is really badly optimised... (Who cares)
                if (aktuellerModus == "Constructor")
                {
                    Konstrucktor kot = new Konstrucktor(worldPoint.X, worldPoint.Y, 1);
                    List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(kot.PositionX, kot.PositionY, kot.SizeX, kot.SizeY);
                    if (lffb.Count == 0)
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, kot, Color.FromArgb(127, 127, 255, 95));
                    else
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, kot, Color.FromArgb(127, 255, 64, 16));

                }
                else if (aktuellerModus == "Miner")
                {
                    GroundResource resource = mapWorld.GetBlockState(worldPoint.X, worldPoint.Y);

                    // TODO: Miner resource to GroundType!
                    Miner miner = new Miner(worldPoint.X, worldPoint.Y, 1, GetResourceFromGround(resource));
                    List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(miner.PositionX, miner.PositionY, miner.SizeX, miner.SizeY);
                    if (lffb.Count == 0 && resource == GroundResource.Iron)
                    {
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, miner, Color.FromArgb(127, 127, 255, 95));
                    }
                    else
                    {
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, miner, Color.FromArgb(127, 255, 64, 16));
                    }
                }
                else if (aktuellerModus == "Belt")
                {
                    Band belt = new Band(3, 20, worldPoint.X, worldPoint.Y);
                    List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(belt.PositionX, belt.PositionY, belt.SizeX, belt.SizeY);
                    if (lffb.Count == 0)
                    {
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, belt, Color.FromArgb(127, 127, 255, 95));
                    }
                    else
                    {
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, belt, Color.FromArgb(127, 255, 64, 16));
                    }
                }
                if (aktuellerModus == "Destroy")
                {
                    List<Fabrikgebeude> lffb = mapWorld.GetEntityInPos(worldPoint.X, worldPoint.Y);
                    if (lffb.Count == 0)
                        wlrdDrawer.DrawHover(e, worldPoint, Color.FromArgb(127, 255, 64, 16));
                    else
                        foreach (var f in lffb)
                            wlrdDrawer.DrawPlacableBuilding(e, new Point(f.PositionX,f.PositionY), f, Color.FromArgb(127, 255, 64, 16));
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
            ToolStripButton menuBtn = new ToolStripButton("Menu");
            menuBtn.Click += (s, e) => ToogleMenuPanel();
            toolStrip.Items.Add(menuBtn);

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
            destroyBtn.Click += (s, e) =>  aktuellerModus = (aktuellerModus is null || aktuellerModus.Equals("Destroy")) ? "" : "Destroy"; 
            toolStrip.Items.Add(destroyBtn);

            Controls.Add(toolStrip);
            this.Resize += new EventHandler(OnFormResize);

            // Menu Panel
            menuPanel = new Panel
            {
                Size = new Size(200, 170),
                Location = new Point(this.Width / 2 - 100, this.Height / 2 - 75),
                BackColor = Color.LightGray,
                Visible = false
            };
            Button backBtn = new Button
            {
                Text = "Back To Game",
                Size = new Size(180, 30),
                Location = new Point(10, 10)
            };
            backBtn.Click += (s, e) =>
            {
                menuPanel.Visible = false;
                this.Focus();
            };
            menuPanel.Controls.Add(backBtn);
            Button saveBtn = new Button
            {
                Text = "Speichern",
                Size = new Size(180, 30),
                Location = new Point(10, 50)
            };
            menuPanel.Controls.Add(saveBtn);
            Button loadBtn = new Button
            {
                Text = "Load",
                Size = new Size(180, 30),
                Location = new Point(10, 90)
            };
            menuPanel.Controls.Add(loadBtn);
            Button closeBtn = new Button
            {
                Text = "Close Game",
                Size = new Size(180, 30),
                Location = new Point(10, 130)
            };
            closeBtn.Click += (s, e) => this.Close();
            menuPanel.Controls.Add(closeBtn);
            Controls.Add(menuPanel);
        }

        // BuildPanel Resize Event
        private void OnFormResize(object sender, EventArgs e)
        {
            int width = (int)(this.Width * 0.7);
            int height = (int)(this.Height * 0.7);
            buildPanel.Size = new Size(width, height);
            buildPanel.Location = new Point((this.Width - width) / 2, (this.Height - height) / 2);

            menuPanel.Location = new Point(this.Width / 2 - 100, this.Height / 2 - 75);

            SetupBuildPanel();
        }
        // Setup for the Items inside Build Panel
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
            Button closeButton = new Button
            {
                Text = "X",
                Size = new Size(30, 30),
                Location = new Point(buildOptionsPanel.Width - 40, 10),
                BackColor = Color.Red,
                ForeColor = Color.White
            };
            closeButton.Click += (s, e) =>
            {
                buildPanel.Visible = false;
                aktuellerModus = null;
                this.Focus();
            };
            buildOptionsPanel.Controls.Add(closeButton);
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
        private void ToogleMenuPanel()
        {
            if (menuPanel.Visible)
            {
                menuPanel.Visible = false;
                this.Focus();
            }
            else
            {
                menuPanel.Visible = true;
            }
        }
    }
}
