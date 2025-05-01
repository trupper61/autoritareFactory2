using autoritaereFactory;
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
using System.Reflection;
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
        public Rezepte[] rezepte =
        {
            new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.IronOre, 1, "Eisenbarren", autoritaereFactory.ResourceType.IronIngot, 1, 1000),
            new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.IronIngot, 1, "Eisenstange", autoritaereFactory.ResourceType.IronStick, 1, 800),
            new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.IronIngot, 3, "Eisenplatte", autoritaereFactory.ResourceType.IronPlate, 2, 1500)
        };
        public bool isDragging = false;
        public Point dragStart;
        public Point beltEnd;
        public Panel konInterface;
        public PlayerData player = new PlayerData(0);
        public Panel inventoryPanel;
        public Miner aktuellerMiner = null;
        public Konstrucktor aktuellerKon = null;
        public Label resourceCountLabel = null;
        public Label resourceCountLabelKon = null;
        public Point dragPanelPoint;

        public world()
        {
            InitializeComponent();
            DisplayData();
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
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
        }
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (aktuellerModus == "Belt" && e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragStart = wlrdDrawer.TranslateScreen2World(e.Location);
            }
        }
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (aktuellerModus == "Belt" && isDragging)
            {
                isDragging = false;
                beltEnd = wlrdDrawer.TranslateScreen2World(e.Location);
                PlaceBeltLine(dragStart, beltEnd);
                aktuellerModus = null;
            }
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            lastMousePos = e.Location;
        }
        public static autoritaereFactory.ResourceType GetResourceFromGround(GroundResource grs)
        {
            switch (grs)
            {
                case GroundResource.IronOre: return autoritaereFactory.ResourceType.IronOre;
                default: return (autoritaereFactory.ResourceType)(-1);
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
                if (lffb.Count == 0 && resource == GroundResource.IronOre)
                {
                    mapWorld.AddEntityAt(miner);
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
            else
            {
                List<Fabrikgebeude> fab = mapWorld.GetEntityInPos(worldPoint.X, worldPoint.Y);
                if (fab.Count == 0)
                    return;
                foreach (Fabrikgebeude f in fab)
                {
                    if (f is Konstrucktor)
                    {
                        ShowKonInterface(f as Konstrucktor);
                    }
                    else if (f is Miner)
                    {
                        ShowMinerInterface(f as Miner);
                    }
                }
            }
        }
        public void RefreshLoop(object sender, EventArgs e)
        {
            this.Invalidate(DisplayRectangle);
            if (konInterface.Visible  && aktuellerMiner != null && resourceCountLabel != null)
            {
                resourceCountLabel.Text = $"Gesammelt: {aktuellerMiner.Recurse.Count} / {aktuellerMiner.MaxAnzalRecurse}";
            }
            if (konInterface.Visible && aktuellerKon != null && resourceCountLabelKon != null)
            {
                resourceCountLabelKon.Text = $"Gelagert: {aktuellerKon.ErgebnissRecurse1.Count} / {aktuellerKon.MaxAnzalErgebnissRecurse1}";
                
            }
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
                    if (lffb.Count == 0 && resource == GroundResource.IronOre)
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
                    if (isDragging)
                    {
                        List<Point> beltLine = GetLinePoints(dragStart, worldPoint);
                        foreach (var pt in beltLine)
                        {
                            Band belt = new Band(3, 20, pt.X, pt.Y);
                            List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(belt.PositionX, belt.PositionY, belt.SizeX, belt.SizeY);
                            if (lffb.Count == 0)
                            {
                                wlrdDrawer.DrawPlacableBuilding(e, pt, belt, Color.FromArgb(127, 127, 255, 95));
                            }
                            else
                            {
                                wlrdDrawer.DrawPlacableBuilding(e, pt, belt, Color.FromArgb(127, 255, 64, 16));
                            }
                        }
                    }
                    else
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
                }
                if (aktuellerModus == "Destroy")
                {
                    List<Fabrikgebeude> lffb = mapWorld.GetEntityInPos(worldPoint.X, worldPoint.Y);
                    if (lffb.Count == 0)
                        wlrdDrawer.DrawHover(e, worldPoint, Color.FromArgb(127, 255, 64, 16));
                    else
                        foreach (var f in lffb)
                            wlrdDrawer.DrawPlacableBuilding(e, new Point(f.PositionX, f.PositionY), f, Color.FromArgb(127, 255, 64, 16));
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
        private List<Point> GetLinePoints(Point start, Point end)
        {
            List<Point> points = new List<Point>();
            if (start.X != end.X && start.Y != end.Y) // Only straight lines
                return points;
            // Richtung der Linie in X- und Y-Richtung bestimmen (-1, 0, 1)
            int dx = Math.Sign(end.X - start.X);
            int dy = Math.Sign(end.Y - start.Y);

            while (start.X != end.X || start.Y != end.Y)
            {
                points.Add(new Point(start.X, start.Y));
                start.X += dx;
                start.Y += dy;
            }
            points.Add(end);
            return points;
        }
        private void PlaceBeltLine(Point start, Point end)
        {
            if (start.X != end.X && start.Y != end.Y)
            {
                // Only straight Lines
                MessageBox.Show("Nur gerade Linien für die Transportbänder");
                return;
            }
            int dx = Math.Sign(end.X - start.X);
            int dy = Math.Sign(end.Y - start.Y);
            while (start.X != end.X || start.Y != end.Y)
            {
                TryPlaceBeltAt(start.X, start.Y);
                start.X += dx;
                start.Y += dy;
            }
            TryPlaceBeltAt(end.X, end.Y);
        }
        private void TryPlaceBeltAt(int x, int y)
        {
            Band belt = new Band(3, 20, x, y);
            List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(x, y, belt.SizeX, belt.SizeY);
            if (lffb.Count == 0)
            {
                mapWorld.AddEntityAt(belt);
            }
        }
        private void InitUI()
        {
            int panelWidth = (int)(this.Width * 0.7);
            int panelHeight = (int)(this.Height * 0.7);
            buildPanel = new Panel
            {
                Size = new Size(panelWidth, panelHeight),
                Location = new Point((this.Width - panelWidth) / 2, (this.Height - panelHeight) / 2),
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
            destroyBtn.Click += (s, e) => aktuellerModus = (aktuellerModus is null || aktuellerModus.Equals("Destroy")) ? "" : "Destroy";
            toolStrip.Items.Add(destroyBtn);

            ToolStripButton inventoryBtn = new ToolStripButton("Inventory");
            inventoryBtn.Click += (s, e) => ShowInventory();
            toolStrip.Items.Add(inventoryBtn);
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

            konInterface = new Panel
            {
                Size = new Size(250, 300),
                Location = new Point(50, 50),
                BackColor = Color.LightGray,
                Visible = false
            };
            konInterface.Location = new Point((this.ClientSize.Width - konInterface.Width) / 2, (this.ClientSize.Height - konInterface.Height) / 2);
            Controls.Add(konInterface);
            inventoryPanel = new Panel
            {
                Size = new Size((int)(this.ClientSize.Width * 0.3), (int)(this.ClientSize.Height * 0.6)),
                Location = new Point(5, 5),
                BackColor = Color.LightGray,
                Visible = false
            };
            inventoryPanel.Location = new Point((this.ClientSize.Width - inventoryPanel.Width) / 2, (this.ClientSize.Height - inventoryPanel.Height) / 2);
            inventoryPanel.MouseDown += InventoryPanel_MouseDown;
            inventoryPanel.MouseUp += InventoryPanel_MouseUp;
            inventoryPanel.MouseMove += InventoryPanel_MoseMove;
            Controls.Add(inventoryPanel);
        }

        // BuildPanel Resize Event
        private void OnFormResize(object sender, EventArgs e)
        {
            int width = (int)(this.Width * 0.7);
            int height = (int)(this.Height * 0.7);
            buildPanel.Size = new Size(width, height);
            buildPanel.Location = new Point((this.Width - width) / 2, (this.Height - height) / 2);

            menuPanel.Location = new Point(this.Width / 2 - 100, this.Height / 2 - 75);

            konInterface.Location = new Point((this.ClientSize.Width - konInterface.Width) / 2, (this.ClientSize.Height - konInterface.Height) / 2);

            SetupBuildPanel();
        }
        // Setup for the Items inside Build Panel
        private void SetupBuildPanel()
        {
            buildPanel.Controls.Clear();

            Button closeButton = new Button
            {
                Text = "X",
                Size = new Size(30, 30),
                Location = new Point(buildPanel.Width - 40, 10),
                BackColor = Color.Red,
                ForeColor = Color.White
            };
            closeButton.Click += (s, e) =>
            {
                buildPanel.Visible = false;
                aktuellerModus = null;
                this.Focus();
            };
            buildPanel.Controls.Add(closeButton);
            Label titleLabel = new Label
            {
                Text = "Choose Building",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            buildPanel.Controls.Add(titleLabel);

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
                buildPanel.Controls.Add(btn);
                y += 45;
            }
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
        public void ShowKonInterface(Konstrucktor kon)
        {
            konInterface.Visible = true;
            konInterface.Controls.Clear();
            aktuellerKon = kon;
            Label name = new Label()
            {
                Text = "Konstruktor",
                Location = new Point(10, 10),
                AutoSize = true
            };
            konInterface.Controls.Add(name);
            int y = 40;
            if (kon.TypErgebnissRecurse1 != null)
            {
                Label productInfo = new Label
                {
                    Text = $"Produces: {kon.MengenErgebnissRecursen1} x {kon.TypErgebnissRecurse1}",
                    Location = new Point(10, y),
                    AutoSize = true
                };
                konInterface.Controls.Add(productInfo);
                y += 25;
                resourceCountLabelKon = new Label
                {
                    Text = $"Gelagert: {kon.ErgebnissRecurse1.Count} / {kon.MaxAnzalErgebnissRecurse1}",
                    Location = new Point(10, y),
                    AutoSize = true
                };
                konInterface.Controls.Add(resourceCountLabelKon);
                y += 30;
                Button productBtn = new Button
                {
                    Text = $"Produkt nehmen",
                    Size = new Size(200, 30),
                    Location = new Point(10, y)
                };
                productBtn.Click += (s, e) =>
                {
                    foreach (var res in kon.ErgebnissRecurse1.ToList())
                    {
                        player.AddResource(res);
                        kon.ErgebnissRecurse1.Remove(res);
                    }
                    if (inventoryPanel.Visible)
                        ShowInventory();
                };
                konInterface.Controls.Add(productBtn);
            }
            y += 40;
            Label rezepteLbl = new Label
            {
                Text = "Rezepte:",
                Location = new Point(10, y),
                AutoSize = true
            };
            konInterface.Controls.Add(rezepteLbl);
            y += 25;
            foreach (Rezepte rezept in rezepte)
            {
                Button rezeptBtn = new Button
                {
                    Text = rezept.RezeptName + $" ({rezept.MengenBenotigteRecurse[0]} {rezept.BenotigteRecursen[0]} → {rezept.MengenErgebnissRecursen[0]} {rezept.ErgebnissRecursen[0]})",
                    Size = new Size(240, 30),
                    Location = new Point(10, y)
                };
                rezeptBtn.Click += (s, e) =>
                {
                    kon.SpeichereRezept(rezept);
                    ShowKonInterface(kon); // Neu laden nach Auswahl
                };
                konInterface.Controls.Add(rezeptBtn);
                y += 35;

            }
            Button closeBtn = new Button
            {
                Text = "X",
                Size = new Size(30, 30),
                Location = new Point(konInterface.Width - 35, 5),
                BackColor = Color.Red,
                ForeColor = Color.White
            };
            closeBtn.Click += (s, e) =>
            {
                konInterface.Visible = false;
                aktuellerKon = null;
            };
            konInterface.Controls.Add(closeBtn);
            closeBtn.BringToFront();

            konInterface.Size = new Size(270, Math.Max(y + 10, 200));
            konInterface.Location = new Point((this.ClientSize.Width - konInterface.Width) / 2, (this.ClientSize.Height - konInterface.Height) / 2);
        }
        public void ShowMinerInterface(Miner miner)
        {
            konInterface.Visible = true;
            konInterface.Controls.Clear();
            aktuellerMiner = miner;
            Label name = new Label
            {
                Text = "Miner",
                Location = new Point(10, 10),
                AutoSize = true
            };
            konInterface.Controls.Add(name);

            int y = 40;
            Label resourceTypelb = new Label
            {
                Text = $"Ressourcentyp: {miner.TypResurce}",
                Location = new Point(10, y),
                AutoSize = true
            };
            konInterface.Controls.Add(resourceTypelb);
            y += 25;
            resourceCountLabel = new Label
            {
                Text = $"Gesammelt: {miner.Recurse.Count} / {miner.MaxAnzalRecurse}",
                Location = new Point(10, y),
                AutoSize = true
            };
            konInterface.Controls.Add(resourceCountLabel);
            y += 35;
            Button takeBtn = new Button
            {
                Text = "Take resource",
                Location = new Point(10, y),
                Size = new Size(150, 30)
            };         
            takeBtn.Click += (s, e) =>
            {
                foreach (var res in miner.Recurse.ToList())
                {
                    player.AddResource(res);
                    miner.Recurse.Remove(res);
                }
                ShowMinerInterface(miner);
                if (inventoryPanel.Visible)
                    ShowInventory();
            };
            konInterface.Controls.Add(takeBtn);
            y += 40;
            Button closeBtn = new Button
            {
                Text = "X",
                Size = new Size(30, 30),
                Location = new Point(konInterface.Width - 35, 5),
                BackColor = Color.Red,
                ForeColor = Color.White
            };
            closeBtn.Click += (s, e) => konInterface.Visible = false;
            konInterface.Controls.Add(closeBtn);
            closeBtn.BringToFront();
            konInterface.Size = new Size(270, Math.Max(y + 10, 150));
            konInterface.Location = new Point((this.ClientSize.Width - konInterface.Width) / 2, (this.ClientSize.Height - konInterface.Height) / 2);
        }
        public void DisplayData()
        {
            moneyAmount.Text = player.displayData();
        }
        public void ShowInventory()
        {
            inventoryPanel.Visible = true;
            inventoryPanel.Controls.Clear();
            Label title = new Label
            {
                Text = "Inventory",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            Button inventoryCloseBtn = new Button
            {
                Text = "X",
                Size = new Size(30, 30),
                Location = new Point(inventoryPanel.Width - 35, 5),
                BackColor = Color.Red,
                ForeColor = Color.White
            };
            inventoryCloseBtn.Click += (s, e) => inventoryPanel.Visible = false;
            inventoryPanel.Controls.Add(inventoryCloseBtn);
            inventoryPanel.Controls.Add(title);
            int y = 40;
            foreach (var inv in player.inventories)
            {
                int totalQuantity = inv.Items.Count();
                Label entry = new Label
                {
                    Text = $"{inv.Type}: {totalQuantity} / {inv.maxStack}",
                    Location = new Point(10, y),
                    AutoSize = true
                };
                inventoryPanel.Controls.Add(entry);

                Button giveBtn = new Button
                {
                    Text = "→",
                    Location = new Point(100, y - 3),
                    Size = new Size(30, 23)
                };
                giveBtn.Click += (s, e) =>
                {
                    if (aktuellerKon != null)
                    {
                        aktuellerKon.NimmRessourceAusInventar(inv.Items, inv.Type, 5); // oder beliebige Anzahl
                        player.CheckForEmptyInventory();
                        ShowInventory();
                    }
                };
                inventoryPanel.Controls.Add(giveBtn);
                giveBtn.BringToFront();
                y += 20;
            }
            inventoryPanel.Visible = true;
        }
        private void InventoryPanel_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            dragStart = Cursor.Position; // dragStart is Cursor Position
            dragPanelPoint = inventoryPanel.Location;
        }
        private void InventoryPanel_MoseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragStart));
                inventoryPanel.Location = Point.Add(dragPanelPoint, new Size(diff));
            }
        }
        private void InventoryPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
    }
} 

