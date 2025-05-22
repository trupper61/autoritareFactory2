using autoritaereFactory;
using autoritaereFactory.setup;
using autoritaereFactory.world;
using factordictatorship.drawing;
using factordictatorship.formsElement;
using factordictatorship.Resources;
using factordictatorship.setup;
using factordictatorship.setup.BaenderTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
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
        public int rotateState = 1;
        public Panel tutorialPanel;
        public Rezepte Eisenbarren = new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.IronOre, 1, "Eisenbarren", autoritaereFactory.ResourceType.IronIngot, 1, 1000);
        public Rezepte Eisenstange = new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.IronIngot, 1, "Eisenstange", autoritaereFactory.ResourceType.IronStick, 1, 800);
        public Rezepte Eisenplatte = new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.IronIngot, 3, "Eisenstange", autoritaereFactory.ResourceType.IronPlate, 2, 1500);
        public PlayerData player = new PlayerData(0);
        public static Rezepte[] rezepte =
        {
            new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.IronOre, 1, "Eisenbarren", autoritaereFactory.ResourceType.IronIngot, 1, 1000),
            new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.IronIngot, 1, "Eisenstange", autoritaereFactory.ResourceType.IronStick, 1, 800),
            new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.IronIngot, 3, "Eisenplatte", autoritaereFactory.ResourceType.IronPlate, 2, 1500),
            new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.IronStick, 1, "Schrauben", autoritaereFactory.ResourceType.Screw, 4, 1000),
            new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.CopperOre, 1, "Kupferbarren", autoritaereFactory.ResourceType.CopperIngot, 1, 1500),
            new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.CopperIngot, 1, "Kupferdrat", autoritaereFactory.ResourceType.CopperWire, 2, 1300),
            new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.CopperWire, 2, "Kabel", autoritaereFactory.ResourceType.Cable, 1, 1700),
            new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.SteelIngot, 3, "Stahlträger", autoritaereFactory.ResourceType.SteelBeam, 1, 2300),
            new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.SteelIngot, 2, "Stahlrohr", autoritaereFactory.ResourceType.SteelRod, 1, 1900),
            new Rezepte(zugehörigesGebeude.Konstrucktor, autoritaereFactory.ResourceType.limestone, 5, "Betong", autoritaereFactory.ResourceType.Concrete, 3, 8000),

            new Rezepte(zugehörigesGebeude.Fabrikator, autoritaereFactory.ResourceType.IronOre, 5, autoritaereFactory.ResourceType.ColeOre, 5, "Stahlbarren", autoritaereFactory.ResourceType.SteelIngot, 5, 2000),
            new Rezepte(zugehörigesGebeude.Fabrikator, autoritaereFactory.ResourceType.SteelBeam, 5, autoritaereFactory.ResourceType.Concrete, 8, "Stahlbetongträger", autoritaereFactory.ResourceType.SteelConcreteBeam, 1, 5000),
            new Rezepte(zugehörigesGebeude.Fabrikator, autoritaereFactory.ResourceType.Screw, 12, autoritaereFactory.ResourceType.IronStick, 3, "Rotor", autoritaereFactory.ResourceType.Rotor, 1, 15000),
            new Rezepte(zugehörigesGebeude.Fabrikator, autoritaereFactory.ResourceType.CopperWire, 15, autoritaereFactory.ResourceType.SteelRod, 5, "Stator", autoritaereFactory.ResourceType.Stator, 1, 17000),
            new Rezepte(zugehörigesGebeude.Fabrikator, autoritaereFactory.ResourceType.Rotor, 5, autoritaereFactory.ResourceType.Stator, 5, "Motor", autoritaereFactory.ResourceType.Motor, 1, 30000)
        };
        public Resource[] rescourcen =
        {
            new Resource(autoritaereFactory.ResourceType.IronOre),
            new Resource(autoritaereFactory.ResourceType.IronIngot),
            new Resource(autoritaereFactory.ResourceType.IronPlate),
            new Resource(autoritaereFactory.ResourceType.IronStick),
            new Resource(autoritaereFactory.ResourceType.CopperIngot),
            new Resource(autoritaereFactory.ResourceType.CopperOre)
        };
        public bool isDragging = false;
        public Point dragStart;
        public Point beltEnd;
        public Panel konInterface;
        public Panel banInterface;
        public Panel ExporthausInterface;
        public Panel inventoryPanel;
        public Miner aktuellerMiner = null;
        public Konstrucktor aktuellerKon = null;
        public Label resourceCountLabel = null;
        public Label resourceCountLabelKon = null;
        public Fabrikator aktuellerFab = null;
        public Finishinator aktuellerFin = null;
        public Point dragPanelPoint;
        public Panel dragPanel;
        public Panel portPanel;
        public Label inCount1;
        public Label inCount2;
        public Label outCount;
        private List<Label> finishResLabels = new List<Label>();
        public ProgressBar pb;
        public Label percentLabel;
        public ToolStripLabel moneyLb;

        public world()
        {
            InitializeComponent();
            DisplayData();
            InitUI();
            mapWorld = new WorldMap(16, 16);
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
                case GroundResource.ColeOre: return autoritaereFactory.ResourceType.ColeOre;
                case GroundResource.CopperOre: return autoritaereFactory.ResourceType.CopperOre;
                case GroundResource.LimeStone4:
                case GroundResource.LimeStone3:
                case GroundResource.LimeStone2:
                case GroundResource.LimeStone:
                    return autoritaereFactory.ResourceType.limestone;
                default: return (autoritaereFactory.ResourceType)(-1);
            }
        }

        private void OnClick(object sender, MouseEventArgs e)
        {
            lastMousePos = e.Location;
            Point worldPoint = wlrdDrawer.TranslateScreen2World(lastMousePos);
            if (aktuellerModus == "Constructor")
            {

                Konstrucktor kon = new Konstrucktor(worldPoint.X, worldPoint.Y, rotateState);
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
                Miner miner = new Miner(worldPoint.X, worldPoint.Y, rotateState, GetResourceFromGround(resource));
                List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(miner.PositionX, miner.PositionY, miner.SizeX, miner.SizeY);
                if (lffb.Count == 0 && (resource == GroundResource.IronOre || resource == GroundResource.ColeOre || resource == GroundResource.CopperOre || resource == GroundResource.LimeStone || resource == GroundResource.LimeStone2 || resource == GroundResource.LimeStone3 || resource == GroundResource.LimeStone4))
                {
                    mapWorld.AddEntityAt(miner);
                    aktuellerModus = null;
                }
                else
                {
                    MessageBox.Show("Der Platz ist ungültig. Wählen Sie einen anderen Platz.");
                }
            }
            else if (aktuellerModus == "Lager")
            {
                Exporthaus exp = new Exporthaus(worldPoint.X, worldPoint.Y, rotateState, mapWorld);
                List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(exp.PositionX, exp.PositionY, exp.SizeX, exp.SizeY);
                if (lffb.Count == 0)
                {
                    mapWorld.AddEntityAt(exp);
                    aktuellerModus = null;
                }
                else
                    MessageBox.Show("Der Platz ist ungpültig. Wählen Sie einen anderen Platz.");
            }
            else if (aktuellerModus == "Fabricator")
            {
                Fabrikator fab = new Fabrikator(worldPoint.X, worldPoint.Y, rotateState);
                List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(fab.PositionX, fab.PositionY, fab.SizeX, fab.SizeY);
                if(lffb.Count == 0)
                {
                    mapWorld.AddEntityAt(fab);
                    aktuellerModus = null;
                }
                else
                {
                    MessageBox.Show("Der Platz ist ungültig. Wählen Sie einen anderen Platz.");
                }
            }
            else if (aktuellerModus == "Belt Corner")
            {
                CurveBand cb = new CurveBand(rotateState, 0, worldPoint.X, worldPoint.Y, (rotateState % 4) + 1, mapWorld, false); // What rotation status?
                List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(cb.PositionX, cb.PositionY, cb.SizeX, cb.SizeY);
                if (lffb.Count == 0)
                {
                    mapWorld.AddEntityAt(cb);
                    aktuellerModus = null;
                }
                else
                {
                    MessageBox.Show("Der Platz ist ungültig. Wählen Sie einen anderen Platz.");
                }
            }
            else if (aktuellerModus == "Splitter")
            {
                Splitter spl = new Splitter(worldPoint.X, worldPoint.Y, rotateState);
                List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(spl.PositionX, spl.PositionY, spl.SizeX, spl.SizeY);
                if (lffb.Count == 0)
                {
                    mapWorld.AddEntityAt(spl);
                    aktuellerModus = null;
                }
                else
                    MessageBox.Show("Der Platz ist ungültig. Wählen Sie einen anderen Platz.");
            }
            else if (aktuellerModus == "Merger")
            {
                Merger meg = new Merger(worldPoint.X, worldPoint.Y, rotateState);
                List <Fabrikgebeude> lffb = mapWorld.GetEntityInBox(meg.PositionX, meg.PositionY, meg.SizeX, meg.SizeY);
                if (lffb.Count == 0)
                {
                    mapWorld.AddEntityAt(meg);
                    aktuellerModus = null;
                }
                else
                    MessageBox.Show("Der PLatz ist ungültig. Wählen Sie einen anderen PLatz.");
            }
            else if (aktuellerModus == "Finishinator")
            {
                Finishinator fin = new Finishinator(worldPoint.X, worldPoint.Y, rotateState);
                List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(fin.PositionX, fin.PositionY, fin.SizeX, fin.SizeY);
                if (lffb.Count == 0)
                {
                    mapWorld.AddEntityAt(fin);
                    aktuellerModus = null;
                }
                else
                    MessageBox.Show("Der Platz ist ungültig. Wählen Sie einen anderen Platz.");
            }
            else if (aktuellerModus == "Destroy")
            {
                List<Fabrikgebeude> fab = mapWorld.GetEntityInPos(worldPoint.X, worldPoint.Y);
                if (fab.Count == 0)
                {
                    //MessageBox.Show("Nix zum Löschen");
                }
                else
                {
                    foreach (Fabrikgebeude f in fab)
                    {
                        mapWorld.RemoveEntity(f);
                    }
                    //aktuellerModus = null;
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
                    else if (f is Band)
                    {
                        ShowBandInterface(f as Band);
                    }
                    else if (f is Miner)
                    {
                        ShowMinerInterface(f as Miner);
                    }
                    else if(f is Exporthaus) 
                    {
                        ShowExportInterface(f as Exporthaus);
                    }
                    else if(f is Fabrikator)
                    {
                        ShowFabrikatorInterface(f as Fabrikator);
                    }
                    else if(f is Finishinator)
                    {
                        ShowFinishinatorInterface(f as Finishinator);
                    }
                }
            }
        }
        bool rotateButton = false;
        public void RefreshLoop(object sender, EventArgs e)
        {
            this.Invalidate(DisplayRectangle);
            if ((IsKeyPressed(Keys.R) ||IsKeyPressed(Keys.T)) && !rotateButton)
            {
                if (IsKeyPressed(Keys.T)) { rotateState += 2; }
                rotateState %= 4;
                rotateState++;
                rotateButton = true;
            }
            rotateButton = IsKeyPressed(Keys.R) || IsKeyPressed(Keys.T);
            if (konInterface.Visible && aktuellerMiner != null && resourceCountLabel != null)
            {
                resourceCountLabel.Text = $"Gesammelt: {aktuellerMiner.Recurse.Count} / {aktuellerMiner.MaxAnzalRecurse}";
            }
            if (konInterface.Visible && aktuellerKon != null && resourceCountLabelKon != null)
            {
                resourceCountLabelKon.Text = $"Gelagert: {aktuellerKon.ErgebnissRecurse1.Count} / {aktuellerKon.MaxAnzalErgebnissRecurse1}";

            }
            if (portPanel != null && portPanel.Visible)
            {
                if (aktuellerKon != null)
                {
                    inCount1.Text = $"Anzahl: {aktuellerKon.BenotigteRecurse1.Count()}";
                    outCount.Text = $"Anzahl: {aktuellerKon.ErgebnissRecurse1.Count()}";
                }
                else if (aktuellerFab != null)
                {
                    inCount1.Text = $"Anzahl: {aktuellerFab.BenotigteRecurse1.Count()}";
                    inCount2.Text = $"Anzahl: {aktuellerFab.BenotigteRecurse2.Count()}";
                    outCount.Text = $"Anzahl: {aktuellerFab.ErgebnissRecurse1.Count()}";
                }
            }
            if (konInterface.Visible && aktuellerFin != null)
            {
                var resList = new List<(ResourceType typ, int benötigt, int aktuell)>
                {
                    (aktuellerFin.TypBenotigteRecurse1, aktuellerFin.NötigeMengenBenotigteRecurse1, aktuellerFin.AktuelleAnzahlAbgegebeneResource1),
                    (aktuellerFin.TypBenotigteRecurse2, aktuellerFin.NötigeMengenBenotigteRecurse2, aktuellerFin.AktuelleAnzahlAbgegebeneResource2),
                    (aktuellerFin.TypBenotigteRecurse3, aktuellerFin.NötigeMengenBenotigteRecurse3, aktuellerFin.AktuelleAnzahlAbgegebeneResource3),
                    (aktuellerFin.TypBenotigteRecurse4, aktuellerFin.NötigeMengenBenotigteRecurse4, aktuellerFin.AktuelleAnzahlAbgegebeneResource4),
                    (aktuellerFin.TypBenotigteRecurse5, aktuellerFin.NötigeMengenBenotigteRecurse5, aktuellerFin.AktuelleAnzahlAbgegebeneResource5)
                };
                int aktuellGesamt = 0;
                int benötigtGesamt = 0;
                for (int i = 0; i < resList.Count; i++)
                {
                    var (typ, benötigt, aktuell) = resList[i];
                    benötigtGesamt += benötigt;
                    aktuellGesamt += Math.Min(aktuell, benötigt);

                    if (i < finishResLabels.Count)
                    {
                        finishResLabels[i].Text = $"{typ}: {aktuell} / {benötigt}";
                    }
                }

                pb.Maximum = benötigtGesamt > 0 ? benötigtGesamt : 1;
                pb.Value = Math.Min(aktuellGesamt, pb.Maximum);
                percentLabel.Text = $"Gesamtfortschritt: {(int)((double)aktuellGesamt / pb.Maximum * 100)}%";
            }
        }
        public void PaintHandler(object sender, PaintEventArgs e)
        {
            long testTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            float deltaMs = (testTime - lastTimeTick) / 1000f;
            lastTimeTick = testTime;
            // draw the world!
            mapWorld.mut.WaitOne();
            wlrdDrawer.Update(e, deltaMs);
            mapWorld.mut.ReleaseMutex();
            // preview
            if (Focused)
            {
                Point worldPoint = wlrdDrawer.TranslateScreen2World(lastMousePos);
                //// draw the hover thing!
                //wlrdDrawer.DrawHover(e,worldPoint);
                // this is really badly optimised... (Who cares)
                if (aktuellerModus == "Constructor")
                {
                    Konstrucktor kot = new Konstrucktor(worldPoint.X, worldPoint.Y, rotateState);
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
                    Miner miner = new Miner(worldPoint.X, worldPoint.Y, rotateState, GetResourceFromGround(resource));
                    List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(miner.PositionX, miner.PositionY, miner.SizeX, miner.SizeY);
                    if (lffb.Count == 0 && (resource == GroundResource.IronOre || resource == GroundResource.ColeOre || resource == GroundResource.CopperOre || resource == GroundResource.LimeStone || resource == GroundResource.LimeStone2 || resource == GroundResource.LimeStone3 || resource == GroundResource.LimeStone4))
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
                        if (beltLine.Count > 1)
                        {// auto rotate the lines
                            if (beltLine[0].X < beltLine[1].X) rotateState = 1;
                            if (beltLine[0].Y < beltLine[1].Y) rotateState = 2;
                            if (beltLine[0].X > beltLine[1].X) rotateState = 3;
                            if (beltLine[0].Y > beltLine[1].Y) rotateState = 4;
                        }
                        foreach (var pt in beltLine)
                        {
                            Band belt = new Band(rotateState, 20, pt.X, pt.Y, mapWorld);
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
                        Band belt = new Band(rotateState, 20, worldPoint.X, worldPoint.Y, mapWorld);
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
                else if (aktuellerModus == "Belt Corner")
                {
                    CurveBand cb = new CurveBand(rotateState, 0, worldPoint.X, worldPoint.Y, (rotateState % 4) + 1, mapWorld, false); // What rotation status?
                    List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(cb.PositionX, cb.PositionY, cb.SizeX, cb.SizeY);
                    if (lffb.Count == 0)
                    {
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, cb, Color.FromArgb(127, 127, 255, 95));

                    }
                    else
                    {
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, cb, Color.FromArgb(127, 255, 64, 16));
                    }
                }
                else if (aktuellerModus == "Lager")
                {
                    Exporthaus exp = new Exporthaus(worldPoint.X, worldPoint.Y, rotateState, mapWorld);
                    List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(exp.PositionX, exp.PositionY, exp.SizeX, exp.SizeY);
                    if (lffb.Count == 0)
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, exp, Color.FromArgb(127, 127, 255, 95));
                    else
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, exp, Color.FromArgb(127, 255, 64, 16));
                }
                else if (aktuellerModus == "Fabricator")
                {
                    Fabrikator fab = new Fabrikator(worldPoint.X, worldPoint.Y, rotateState);
                    List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(fab.PositionX, fab.PositionY, fab.SizeX, fab.SizeY);
                    if (lffb.Count == 0)
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, fab, Color.FromArgb(127, 127, 255, 95));
                    else
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, fab, Color.FromArgb(127, 255, 64, 16));
                }
                else if (aktuellerModus == "Splitter")
                {
                    Splitter spl = new Splitter(worldPoint.X, worldPoint.Y, rotateState);
                    List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(spl.PositionX, spl.PositionY, spl.SizeX, spl.SizeY);
                    if (lffb.Count == 0)
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, spl, Color.FromArgb(127, 127, 255, 95));
                    else
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, spl, Color.FromArgb(127, 255, 64, 16));
                }
                else if (aktuellerModus == "Merger")
                {
                    Merger meg = new Merger(worldPoint.X, worldPoint.Y, rotateState);
                    List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(meg.PositionX, meg.PositionY, meg.SizeX, meg.SizeY);
                    if (lffb.Count == 0)
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, meg, Color.FromArgb(127, 127, 255, 95));
                    else
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, meg, Color.FromArgb(127, 255, 64, 16));
                }
                else if (aktuellerModus == "Finishinator")
                {
                    Finishinator fin = new Finishinator(worldPoint.X, worldPoint.Y, rotateState);
                    List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(fin.PositionX, fin.PositionY, fin.SizeX, fin.SizeY);
                    if (lffb.Count == 0)
                    {
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, fin, Color.FromArgb(127, 127, 255, 95));
                    }
                    else
                        wlrdDrawer.DrawPlacableBuilding(e, worldPoint, fin, Color.FromArgb(127, 255, 64, 16));
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
            Band belt = new Band(rotateState, 0, x, y, mapWorld);
            List<Fabrikgebeude> lffb = mapWorld.GetEntityInBox(x, y, belt.SizeX, belt.SizeY);
            if (lffb.Count == 0)
            {
                mapWorld.AddEntityAt(belt);
            }
        }
        private void InitUI()
        {
            ToolStrip toolStrip = new ToolStrip();
            toolStrip.Dock = DockStyle.Top;
            toolStrip.MinimumSize= new Size(2000, 50);

            ToolStripButton menuBtn = new ToolStripButton("Menu");
            menuBtn.Image = ResourceHandler.LoadImage("menu\\MenuIcon.png");
            toolStrip.ImageScalingSize = new Size ( 40, 40 );
            menuBtn.Click += (s, e) => ToogleMenuPanel();
            menuBtn.AutoSize = true;
            toolStrip.Items.Add(menuBtn);

            ToolStripButton buildBtn = new ToolStripButton("Build");
            buildBtn.AutoSize = true;
            buildBtn.Image = ResourceHandler.LoadImage("menu\\BuildModeIcon.png");
            buildBtn.Click += (s, e) =>
            {
                if (!buildPanel.Visible)
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
            destroyBtn.Image = ResourceHandler.LoadImage("menu\\Destroy.png");
            destroyBtn.AutoSize = true;
            destroyBtn.Click += (s, e) => aktuellerModus = (aktuellerModus == null || !aktuellerModus.Equals("Destroy")) ? "Destroy" : "";
            toolStrip.Items.Add(destroyBtn);
            ToolStripButton rotateBtn = new ToolStripButton("Rotate");
            rotateBtn.AutoSize = true;
            rotateBtn.Image = ResourceHandler.LoadImage("menu\\Rotate.png");
            rotateBtn.Click += (s, e) =>
            {
                rotateState %= 4;
                rotateState++;
            };
            toolStrip.Items.Add(rotateBtn);

            ToolStripButton inventoryBtn = new ToolStripButton("Inventory");
            inventoryBtn.AutoSize = true;
            inventoryBtn.Image = ResourceHandler.LoadImage("menu\\Inventory.png");
            inventoryBtn.Click += (s, e) => ShowInventory();
            toolStrip.Items.Add(inventoryBtn);
            ToolStripButton tutorialBtn = new ToolStripButton("Tutorial");
            tutorialBtn.AutoSize = true;
            tutorialBtn.Image = ResourceHandler.LoadImage("menu\\tutorial.png");
            tutorialBtn.Click += (s, e) => ToogleTutorialPanel();//TutorialPanel
            toolStrip.Items.Add(tutorialBtn);
            moneyLb = new ToolStripLabel
            {
                AutoSize = true,
                Dock = DockStyle.Right,
                Text = $"Gold: {player.money}"
            };
            toolStrip.Items.Add(moneyLb);
            Controls.Add(toolStrip);
            buildPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Bottom,
                BackColor = Color.LightGray,
                Visible = false,
                AutoScroll = true
            };
            Controls.Add(buildPanel);
            SetupBuildPanel();
            this.Resize += new EventHandler(OnFormResize);

            // Menu Panel
            menuPanel = new Panel
            {
                Size = new Size(200, 170),
                Location = new Point(this.Width / 2 - 100, this.Height / 2 - 75),
                BackColor = Color.LightGray,
                Visible = false
            };
            
            tutorialPanel = new Panel 
            {
                Size = new Size(200, 170),
                Location = new Point(this.Width / 2 - 100, this.Height / 2 - 75),
                BackColor = Color.Aquamarine,
                Visible = false

            };
            Button backBtn = new NoFocusButton
            //Button backBtn = new Button
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
            Button saveBtn = new NoFocusButton
            {
                Text = "Speichern",
                Size = new Size(180, 30),
                Location = new Point(10, 50)
            };
            saveBtn.Click += (s, e) => {
                List<byte> worldData = mapWorld.GetAsBytes();
                worldData.AddRange(player.GetAsBytes());
                FileStream fptr = File.OpenWrite(mapWorld.worldName + ".world");
                fptr.Write(worldData.ToArray(), 0, worldData.Count);
                fptr.Close();
            };
            menuPanel.Controls.Add(saveBtn);
            Button loadBtn = new NoFocusButton
            {
                Text = "Load",
                Size = new Size(180, 30),
                Location = new Point(10, 90)
            };
            loadBtn.Click += (s, e) => {
                //openWorldFile.DefaultExt = ".world";
                openWorldFile.Filter = "WorldFiles (*.world)|*.world";
                openWorldFile.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
                //openWorldFile.CustomPlaces.Add(FileDialogCustomPlace.)
                DialogResult status = openWorldFile.ShowDialog();
                if (status != DialogResult.OK)
                    return;
                Stream worldFptr = openWorldFile.OpenFile();
                byte[] allData = new byte[worldFptr.Length];
                int offset = 0;
                worldFptr.Read(allData, offset, allData.Length);
                WorldMap newMap = WorldMap.FromByteArray(allData, ref offset);
                player = PlayerData.FromByteArray(allData, ref offset);
                worldFptr.Close();
                if (newMap != null)
                {
                    mapWorld.Dispose();
                    mapWorld.AwaitThread();
                    newMap.worldName = openWorldFile.FileName;
                    if (newMap.worldName.EndsWith(".world"))
                        newMap.worldName = newMap.worldName.Substring(0, newMap.worldName.Length - 6);
                    mapWorld = newMap;
                    mapWorld._StartThread();
                }
            };
            menuPanel.Controls.Add(loadBtn);
            Button closeBtn = new NoFocusButton
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
            konInterface.MouseDown += Panel_MouseDown;
            konInterface.MouseMove += Panel_MoseMove;
            konInterface.MouseUp += Panel_MouseUp;
            konInterface.Location = new Point((this.ClientSize.Width - konInterface.Width) / 2, (this.ClientSize.Height - konInterface.Height) / 2);
            Controls.Add(konInterface);

            banInterface = new Panel
            {
                Size = new Size(250, 300),
                Location = new Point(50, 50),
                BackColor = Color.LightGray,
                Visible = false
            };
            Controls.Add(banInterface);
            inventoryPanel = new Panel
            {
                Size = new Size((int)(this.ClientSize.Width * 0.3), (int)(this.ClientSize.Height * 0.6)),
                Location = new Point(5, 5),
                BackColor = Color.LightGray,
                Visible = false
            };
            inventoryPanel.Location = new Point((this.ClientSize.Width - inventoryPanel.Width) / 2, (this.ClientSize.Height - inventoryPanel.Height) / 2);
            inventoryPanel.MouseDown += Panel_MouseDown;
            inventoryPanel.MouseUp += Panel_MouseUp;
            inventoryPanel.MouseMove += Panel_MoseMove;
            Controls.Add(inventoryPanel);
            portPanel = new Panel
            {
                Size = new Size(250, 200),
                Location = new Point((this.ClientSize.Width - 300) / 2, (this.ClientSize.Height - 250) / 2),
                BackColor = Color.LightGray,
                Visible = false
            };
            portPanel.MouseDown += Panel_MouseDown;
            portPanel.MouseMove += Panel_MoseMove;
            portPanel.MouseUp += Panel_MouseUp;
            this.Controls.Add(portPanel);
            ExporthausInterface = new Panel
            {
                Size = new Size(190, 250),
                Location = new Point((this.ClientSize.Width - 190) / 2, (this.ClientSize.Height - 250) / 2),
                BackColor = Color.LightYellow,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };
            ExporthausInterface.MouseDown += Panel_MouseDown;
            ExporthausInterface.MouseMove += Panel_MoseMove;
            ExporthausInterface.MouseUp += Panel_MouseUp;
            this.Controls.Add(ExporthausInterface);
        }

        // BuildPanel Resize Event
        private void OnFormResize(object sender, EventArgs e)
        {
            menuPanel.Location = new Point(this.Width / 2 - 100, this.Height / 2 - 75);

            konInterface.Location = new Point((this.ClientSize.Width - konInterface.Width) / 2, (this.ClientSize.Height - konInterface.Height) / 2);
        }
        // Setup for the Items inside Build Panel
        private void SetupBuildPanel()
        {
            buildPanel.Controls.Clear();

            var buildings = new List<String>
            {
                "Miner",
                "Constructor",
                "Belt",
                "Belt Corner",
                "Splitter",
                "Merger",
                "Lager",
                "Fabricator",
                "Finishinator"
            };
            int x = 20;
            foreach (var name in buildings)
            {
                Button btn = new NoFocusButton
                {
                    Text = name,
                    Size = new Size(110, 35),
                    Location = new Point(x, 10),
                    BackColor = Color.LightSteelBlue
                };
                btn.Click += (s, e) =>
                {
                    aktuellerModus = name;
                    this.Focus();
                };
                buildPanel.Controls.Add(btn);
                x += 115;
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
        private void ToogleTutorialPanel()
        {
            if (tutorialPanel.Visible)
            {
                tutorialPanel.Visible = false;
                this.Focus();
            }
            else
            {
                tutorialPanel.Visible = true;
            }
        }
        public void ShowKonInterface(Konstrucktor kon)
        {
            konInterface.Controls.Clear();
            aktuellerKon = kon;
            ToolTip ttf = new ToolTip();

            int margin = 10;
            int y = margin;
            Label name = new Label()
            {
                Text = "Konstruktor",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(margin, y),
                AutoSize = true
            };
            konInterface.Controls.Add(name);
            y += 30;
            Label productionInfo = null;
            if (aktuellerKon.BenutzesRezept != -1)
            {
                Button portBtn = new Button
                {
                    Text = $"Siehe Ports",
                    Location = new Point(margin, y),
                    Size = new Size(100, 25)
                };
                portBtn.Click += (s, e) => ShowKonstruktorPorts();
                konInterface.Controls.Add(portBtn);
                int perMinute = (kon.MengenErgebnissRecursen1 * 60000) / kon.Produktionsdauer;
                y += 35;
                productionInfo = new Label
                {
                    Text = $"Produziert: {perMinute}x {kon.TypErgebnissRecurse1} pro Minute",
                    Location = new Point(margin, y),
                    AutoSize = false,
                    Size = new Size(270 - 2 * margin, 30),
                    Font = new Font("Arial", 8, FontStyle.Italic)
                };
                konInterface.Controls.Add(productionInfo);
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
            Panel rezeptPanel = new Panel
            {
                Location = new Point(10, y),
                Size = new Size(250, 200),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            int rezeptY = 0;
            foreach (Rezepte rezept in rezepte.Where(r => r.GebeudeTyp == zugehörigesGebeude.Konstrucktor))
            {
                Button rezeptBtn = new Button
                {
                    Text = rezept.RezeptName,
                    Size = new Size(220, 30),
                    Location = new Point(10, rezeptY),
                    BackColor = (aktuellerKon.BenutzesRezept == rezept.rezeptIndex) ? Color.LightGreen : SystemColors.Control
                };
                rezeptBtn.Click += (s, e) =>
                {
                    aktuellerKon.SpeichereRezept(rezept);
                    ShowKonInterface(aktuellerKon); // Neu laden nach Auswahl
                    if (portPanel != null && portPanel.Visible)
                        ShowKonstruktorPorts();
                };
                ttf.SetToolTip(rezeptBtn, $"{rezept.MengenBenotigteRecurse[0]} {rezept.BenotigteRecursen[0]} → {rezept.MengenBenotigteRecurse[0]} {rezept.ErgebnissRecursen[0]}");
                rezeptPanel.Controls.Add(rezeptBtn);
                rezeptY += 35;

            }
            konInterface.Controls.Add(rezeptPanel);

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
                if (portPanel != null)
                    portPanel.Visible = false;
            };
            konInterface.Controls.Add(closeBtn);
            closeBtn.BringToFront();

            int totalHeight = rezeptPanel.Bottom + 20;
            konInterface.Size = new Size(270, totalHeight);
            konInterface.Location = new Point((this.ClientSize.Width - konInterface.Width) / 2, (this.ClientSize.Height - konInterface.Height) / 2);
            //Button closeBtn = new NoFocusButton;
            konInterface.Visible = true;
        }
        public void ShowMinerInterface(Miner miner)
        {
            konInterface.Controls.Clear();
            aktuellerMiner = miner;
            konInterface.AutoSize = false;
            konInterface.Size = new Size(280, 150);
            int margin = 10;
            int y = 10;
            Label name = new Label
            {
                Text = "Miner",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(margin, y),
                AutoSize = true
            };
            konInterface.Controls.Add(name);
            y += 30;
            Label resourceTypelb = new Label
            {
                Text = $"Ressourcentyp: {miner.TypResurce}",
                Location = new Point(margin, y),
                AutoSize = true
            };
            konInterface.Controls.Add(resourceTypelb);
            y += 30;
            PictureBox outcomePb = new PictureBox
            {
                Size = new Size(60, 60),
                BackColor = Color.LightGreen,
                Image = ReturnResourceImage(miner.TypResurce),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(konInterface.Width - 80, y - 20)
            };
            outcomePb.Click += (s, e) =>
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
            ToolTip tt = new ToolTip();
            tt.SetToolTip(outcomePb, $"Klick zum Entnehmen von {miner.TypResurce}");
            konInterface.Controls.Add(outcomePb);
            y += 25;
            resourceCountLabel = new Label
            {
                Text = $"Anzahl: {miner.Recurse.Count} / {miner.MaxAnzalRecurse}",
                Location = new Point(10, y),
                AutoSize = true
            };
            konInterface.Controls.Add(resourceCountLabel);
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
          //  konInterface.Size = new Size(270, Math.Max(y + 10, 150));
            konInterface.Location = new Point((this.ClientSize.Width - konInterface.Width) / 2, (this.ClientSize.Height - konInterface.Height) / 2);
            konInterface.Visible = true;
        }

        public void ShowBandInterface(Band ban)
        {
            banInterface.Visible = true;
            banInterface.Controls.Clear();

            Label name = new Label();
            name.Text = ban.ToString();
            name.Location = new Point(10, 10);
            name.AutoSize = true;

            int y = 50;
            int maxRight = name.Right;

            foreach (Resource resc in rescourcen)
            {
                if(ban.RetWantedRescource(resc.Type, ban) != "0")
                {
                    Label resBand = new Label();

                    switch (resc.Type)
                    {
                        case ResourceType.IronOre:
                            resBand.Text = resc.Type.ToString() + $" {ban.RetWantedRescource(ResourceType.IronOre, ban)}";
                            break;
                        case ResourceType.IronPlate:
                            resBand.Text = resc.Type.ToString() + $" {ban.RetWantedRescource(ResourceType.IronPlate, ban)}";
                            break;
                        case ResourceType.IronStick:
                            resBand.Text = resc.Type.ToString() + $" {ban.RetWantedRescource(ResourceType.IronStick, ban)}";
                            break;
                        case ResourceType.IronIngot:
                            resBand.Text = resc.Type.ToString() + $" {ban.RetWantedRescource(ResourceType.IronIngot, ban)}";
                            break;
                        default:
                            resBand.Text = resc.Type.ToString() + $" {ban.RetWantedRescource(resc.Type, ban)}";
                            break;
                    }
                    resBand.Location = new Point(10, y);
                    resBand.AutoSize = true;

                    banInterface.Controls.Add(resBand);
                    y += 40;
                }
                //if(resc.Type == ResourceType.IronOre) 
                //{
                //    resBand.Text = resc.Type.ToString() + $" {ban.anzahlEisen}";
                //}
                
            }

            banInterface.Controls.Add(name);
            banInterface.Size = new Size(275, Math.Max(y + 10, 150));
            banInterface.Location = new Point((this.ClientSize.Width - konInterface.Width) / 2, (this.ClientSize.Height - konInterface.Height) / 2);

            Button closeBtn = new Button
            {
                Text = "X",
                Size = new Size(30, 30),
                Location = new Point(konInterface.Width - 35, 5),
                BackColor = Color.Red,
                ForeColor = Color.White
            };
            closeBtn.Click += (s, e) => banInterface.Visible = false;
            banInterface.Controls.Add(closeBtn);

            if(ban is CurveBand) 
            {
                CurveBand curveBand = (CurveBand)ban;
                Button rotBtn = new Button
                {
                    Text = "Rotate",
                    Size = new Size(60, 30),
                    Location = new Point(konInterface.Width / 2, 5),
                    BackColor = Color.Red,
                    ForeColor = Color.White
                };
                banInterface.Controls.Add(rotBtn);
                rotBtn.Click += (s, e) => CurveBandVerhalten(curveBand);
            }
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

            int slotsPerRow = 3;
            int usedSlots = player.inventories.Count;
            int slotsize = 45;
            int padding = 8;
            int x = 10;
            int y = 40;
            int column = 0;

            foreach (var inv in player.inventories)
            {
                Panel slotPanel = new Panel
                {
                    Size = new Size(slotsize, slotsize + 20),
                    Location = new Point(x, y)
                };
                PictureBox resourceBox = new PictureBox
                {
                    Size = new Size(slotsize, slotsize),
                    Location = new Point(0, 0),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = inv.Items.Count() > 0 ? Color.LightGreen : Color.LightBlue
                };
                resourceBox.Image = ReturnResourceImage(inv.Type);
                resourceBox.SizeMode = PictureBoxSizeMode.StretchImage;
                resourceBox.Click += (s, e) =>
                {
                    if (aktuellerKon != null)
                    {
                        aktuellerKon.NimmRessourceAusInventar(inv.Items, inv.Type, 5);
                        player.CheckForEmptyInventory();
                        ShowInventory();
                    }
                };
                Label countLabel = new Label
                {
                    Text = inv.Items.Count.ToString(),
                    AutoSize = false,
                    Size = new Size(slotsize, 20),
                    TextAlign = ContentAlignment.TopCenter,
                    Location = new Point(0, slotsize)
                };
                slotPanel.Controls.Add(resourceBox);
                slotPanel.Controls.Add(countLabel);
                inventoryPanel.Controls.Add(slotPanel);

                column++;
                if (column >= slotsPerRow)
                {
                    column = 0;
                    x = 10;
                    y += slotsize + 30;
                }
                else
                {
                    x += slotsize + padding;
                }

            }
            int remainingSlots = player.slotsAvail - usedSlots;
            for (int i = 0; i < remainingSlots; i++)
            {
                PictureBox pb = new PictureBox
                {
                    Size = new Size(slotsize, slotsize),
                    Location = new Point(x, y),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.LightBlue
                };
                inventoryPanel.Controls.Add(pb);

                column++;
                if (column >= slotsPerRow)
                {
                    column = 0;
                    x = 10;
                    y += slotsize + 30;
                }
                else
                {
                    x += slotsize + padding;
                }
            }
        }
        public static Image ReturnResourceImage(ResourceType type)
        {
            if (ResourceHandler.itemSet.ContainsKey(type))
            {
                return ResourceHandler.itemSet[type];
            }
            return null;
            /*
            switch (type)
            {
                case ResourceType.IronOre:
                    return Properties.Resources.iron_ore;
                case ResourceType.IronIngot:
                    return Properties.Resources.IronBarAsItem;
                case ResourceType.IronStick:
                    return Properties.Resources.IronRodAsItem;
                case ResourceType.IronPlate:
                    return Properties.Resources.IronPlateAsItem;
                case ResourceType.Concrete:
                    return Properties.Resources.concrete;
                case ResourceType.CopperIngot:
                    return Properties.Resources.CopperIngot;
                case ResourceType.CopperWire:
                    return Properties.Resources.CopperWire;
                case ResourceType.CopperOre:
                    return Properties.Resources.CopperOre;
                case ResourceType.Motor:
                    return Properties.Resources.Motor;
                case ResourceType.Rotor:
                    return Properties.Resources.Rotor;
                case ResourceType.Cable:
                    return Properties.Resources.CopperCable;
                case ResourceType.limestone:
                    return Properties.Resources.LimeasItem;
                case ResourceType.Screw:
                    return Properties.Resources.screw;
                case ResourceType.Stator:
                    return Properties.Resources.Stator;
                default:
                    return null;
            }*/
        }
        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel panel)
            {
                isDragging = true;
                dragStart = Cursor.Position; // dragStart is Cursor Position
                dragPanelPoint = panel.Location;
                dragPanel = panel;
            }
        }
        private void Panel_MoseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && dragPanel != null)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragStart));
                dragPanel.Location = Point.Add(dragPanelPoint, new Size(diff));
            }
        }
        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            dragPanel = null;
        }
        private void ShowKonstruktorPorts()
        {
            portPanel.Controls.Clear();
            ResourceType inRes = aktuellerKon.TypBenotigteRecurse1;
            ResourceType outRes = aktuellerKon.TypErgebnissRecurse1;
            Label inTxt = new Label
            {
                AutoSize = true,
                Text = "Incomming Port:",
                Font = new Font("Arial", 8, FontStyle.Bold)
            };
            inTxt.Location = new Point((portPanel.Width / 4) - (inTxt.PreferredWidth / 2), 40);
            portPanel.Controls.Add(inTxt);
            PictureBox inPb = new PictureBox
            {
                Size = new Size(45, 45),
                Location = new Point((portPanel.Width / 4) - (45/2), 60),
                BackColor = Color.LightGreen,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = ReturnResourceImage(inRes)
            };
            inPb.Click += (s, e) =>
            {
                foreach (var res in aktuellerKon.BenotigteRecurse1.ToList())
                {
                    player.AddResource(res);
                    aktuellerKon.BenotigteRecurse1.Remove(res);
                }
                if (inventoryPanel.Visible)
                    ShowInventory();
            };
            portPanel.Controls.Add(inPb);
            inCount1 = new Label
            {
                AutoSize = true,
                Location = new Point(inPb.Left, inPb.Bottom + 5),
                Text = $"Anzahl: {aktuellerKon.BenotigteRecurse1.Count()}"
            };
            portPanel.Controls.Add(inCount1);
            Label outTxt = new Label
            {
                AutoSize = true,
                Text = "Outgoing Port:",
                Font = new Font("Arial", 8, FontStyle.Bold)
            };
            outTxt.Location = new Point((3 * portPanel.Width / 4) - (outTxt.PreferredWidth / 2), 40);
            portPanel.Controls.Add(outTxt);
            PictureBox outPb = new PictureBox
            {
                Size = new Size(45, 45),
                Location = new Point((3 * portPanel.Width / 4) - (45 / 2), 60),
                BackColor = Color.LightSteelBlue,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = ReturnResourceImage(outRes)
            };
            outPb.Click += (s, e) =>
            {
                foreach (var res in aktuellerKon.ErgebnissRecurse1.ToList())
                {
                    player.AddResource(res);
                    aktuellerKon.ErgebnissRecurse1.Remove(res);
                }
                if (inventoryPanel.Visible)
                    ShowInventory();
            };
            portPanel.Controls.Add(outPb);
            outCount = new Label
            {
                AutoSize = true,
                Location = new Point(outPb.Left, outPb.Bottom + 5),
                Text = $"Anzahl: {aktuellerKon.ErgebnissRecurse1.Count()}"
            };
            portPanel.Controls.Add(outCount);
            Button closeBtn = new Button
            {
                Size = new Size(30, 30),
                Location = new Point(portPanel.Width - 35, 5),
                Text = "X",
                ForeColor = Color.White,
                BackColor = Color.Red
            };
            closeBtn.Click += (s, e) => portPanel.Visible = false;
            portPanel.Controls.Add(closeBtn);
            ToolTip tt = new ToolTip();
            tt.SetToolTip(inPb, $"Klick zum Entnehmen von {inRes}"); // Shows small PopUp-Window, for UserHelp
            tt.SetToolTip(outPb, $"Klick zum Entnehmen von {outRes}");
            portPanel.BringToFront();
            portPanel.Visible = true;
        }
        public void ShowExportInterface(Exporthaus exp)
        {
            ExporthausInterface.Visible = true;
            ExporthausInterface.Controls.Clear();

            Label name = new Label();
            name.Text = exp.ToString();
            name.Location = new Point(10, 10);
            name.AutoSize = true;
            name.Font = new Font("Arial", 12, FontStyle.Bold);

            ExporthausInterface.Controls.Add(name);

            int y = 50;

            ShowExportHausRes(exp, ExporthausInterface, ref y);

            ExporthausInterface.Size = new Size(275, Math.Max(y + 10, 150));
            ExporthausInterface.Location = new Point(
                (this.ClientSize.Width - ExporthausInterface.Width) / 2,
                (this.ClientSize.Height - ExporthausInterface.Height) / 2);

            Button closeBtn = new Button
            {
                Text = "X",
                Size = new Size(30, 30),
                Location = new Point(ExporthausInterface.Width - 35, 5),
                BackColor = Color.Red,
                ForeColor = Color.White
            };
            closeBtn.Click += (s, e) => ExporthausInterface.Visible = false;
            ExporthausInterface.Controls.Add(closeBtn);
        }
        private void ShowExportHausRes(Exporthaus exporthaus, Panel panel, ref int y)
        {
            int slotsPerRow = 3;
            int usedSlots = exporthaus.inventories.Count;
            int slotSize = 45;
            int padding = 8;
            int x = 10;
            int column = 0;

            foreach (var inv in exporthaus.inventories)
            {
                PictureBox resourceBox = new PictureBox
                {
                    Size = new Size(slotSize, slotSize),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = inv.Items.Count > 0 ? Color.LightGreen : Color.LightBlue,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = ReturnResourceImage(inv.Type)
                };
                resourceBox.Click += (s, e) =>
                {
                    if (inv.Items.Count > 0)
                    {
                        Resource res = inv.Items.First();
                        exporthaus.Verkaufen(res, player);
                        moneyLb.Text = $"Gold: {player.money}";
                        ShowExportInterface(exporthaus);
                    }
                };

                Label countLabel = new Label
                {
                    Text = inv.Items.Count.ToString(),
                    AutoSize = false,
                    Size = new Size(slotSize, 20),
                    TextAlign = ContentAlignment.TopCenter
                };

                // Position setzen
                resourceBox.Location = new Point(x, y);
                countLabel.Location = new Point(x, y + slotSize);

                panel.Controls.Add(resourceBox);
                panel.Controls.Add(countLabel);

                column++;
                if (column >= slotsPerRow)
                {
                    column = 0;
                    x = 10;
                    y += slotSize + 30;
                }
                else
                {
                    x += slotSize + padding;
                }
            }

            // Leere Slots auffüllen
            int remainingSlots = exporthaus.slotsAvail - usedSlots;
            for (int i = 0; i < remainingSlots; i++)
            {
                PictureBox emptySlot = new PictureBox
                {
                    Size = new Size(slotSize, slotSize),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.LightBlue,
                    Location = new Point(x, y)
                };
                panel.Controls.Add(emptySlot);

                column++;
                if (column >= slotsPerRow)
                {
                    column = 0;
                    x = 10;
                    y += slotSize + 30;
                }
                else
                {
                    x += slotSize + padding;
                }
            }
        }
        public void ShowFabrikatorInterface(Fabrikator fab)
        {
            konInterface.Controls.Clear();
            aktuellerFab = fab;
            ToolTip ttf = new ToolTip();

            int margin = 10;
            int y = margin;

            Label name = new Label
            {
                Text = "Fabrikator",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(margin, y),
                AutoSize = true
            };
            konInterface.Controls.Add(name);
            y += 30;

            if (fab.BenutzesRezept != -1)
            {
                Button portBtn = new Button
                {
                    Text = $"Siehe Ports",
                    Location = new Point(margin, y),
                    Size = new Size(100, 25)
                };
                portBtn.Click += (s, e) => ShowFabrikatorPorts();
                konInterface.Controls.Add(portBtn);

                int perMinute = (fab.MengenErgebnissRecursen1 * 60000) / fab.Produktionsdauer;
                y += 35;

                Label productionInfo = new Label
                {
                    Text = $"Produziert: {perMinute}x {fab.TypErgebnissRecurse1} pro Minute",
                    Location = new Point(margin, y),
                    AutoSize = false,
                    Size = new Size(270 - 2 * margin, 30),
                    Font = new Font("Arial", 8, FontStyle.Italic)
                };
                konInterface.Controls.Add(productionInfo);
            }

            y += 40;
            Label rezepteLbl = new Label
            {
                Text = "Rezepte:",
                Location = new Point(margin, y),
                AutoSize = true
            };
            konInterface.Controls.Add(rezepteLbl);
            y += 25;

            Panel rezeptPanel = new Panel
            {
                Location = new Point(margin, y),
                Size = new Size(250, 200),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            int rezeptY = 10;
            foreach (Rezepte rezept in rezepte.Where(r => r.GebeudeTyp == zugehörigesGebeude.Fabrikator))
            {
                Button rezeptBtn = new Button
                {
                    Text = $"{rezept.RezeptName}",
                    Size = new Size(220, 30),
                    Location = new Point(10, rezeptY),
                    BackColor = (fab.BenutzesRezept == rezept.rezeptIndex) ? Color.LightGreen : SystemColors.Control,
                };
                rezeptBtn.Click += (s, e) =>
                {
                    fab.SpeichereRezept(rezept);
                    ShowFabrikatorInterface(fab);
                    if (portPanel != null && portPanel.Visible)
                        ShowFabrikatorPorts();
                };
                ttf.SetToolTip(rezeptBtn, $"{rezept.MengenBenotigteRecurse[0]} {rezept.BenotigteRecursen[0]}, {rezept.MengenBenotigteRecurse[1]} {rezept.BenotigteRecursen[1]} → {rezept.MengenBenotigteRecurse[0]} {rezept.ErgebnissRecursen[0]}");
                rezeptPanel.Controls.Add(rezeptBtn);
                rezeptY += 35;
            }

            konInterface.Controls.Add(rezeptPanel);

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
                aktuellerFab = null;
                if (portPanel != null)
                    portPanel.Visible = false;
            };
            konInterface.Controls.Add(closeBtn);
            closeBtn.BringToFront();

            int totalHeight = rezeptPanel.Bottom + 20;
            konInterface.Size = new Size(270, totalHeight);
            konInterface.Location = new Point((this.ClientSize.Width - konInterface.Width) / 2, (this.ClientSize.Height - konInterface.Height) / 2);
            konInterface.Visible = true;
        }
        private void ShowFabrikatorPorts()
        {
            portPanel.Controls.Clear();
            ResourceType inRes1 = aktuellerFab.TypBenotigteRecurse1;
            ResourceType inRes2 = aktuellerFab.TypBenotigteRecurse2;
            ResourceType outRes = aktuellerFab.TypErgebnissRecurse1;
            Label inTxt1 = new Label
            {
                AutoSize = true,
                Text = "Incomming Port:",
                Font = new Font("Arial", 8, FontStyle.Bold),
                Location = new Point(portPanel.Width / 5 - 30, 40)
            };
            portPanel.Controls.Add(inTxt1);
            PictureBox inPb = new PictureBox
            {
                Size = new Size(45, 45),
                Location = new Point(portPanel.Width / 5 - 22, 60),
                BackColor = Color.LightGreen,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = ReturnResourceImage(inRes1)
            };
            inPb.Click += (s, e) =>
            {
                foreach (var res in aktuellerFab.BenotigteRecurse1.ToList())
                {
                    player.AddResource(res);
                    aktuellerFab.BenotigteRecurse1.Remove(res);
                }
                if (inventoryPanel.Visible)
                    ShowInventory();
            };
            portPanel.Controls.Add(inPb);
            inCount1 = new Label
            {
                AutoSize = true,
                Location = new Point(inPb.Left, inPb.Bottom + 5),
                Text = $"Anzahl: {aktuellerFab.BenotigteRecurse1.Count()}"
            };
            portPanel.Controls.Add(inCount1);
            Label inTxt2 = new Label
            {
                AutoSize = true,
                Text = "Incomming Port 2:",
                Font = new Font("Arial", 8, FontStyle.Bold),
                Location = new Point(2*portPanel.Width / 5 - 30, 40)
            };
            portPanel.Controls.Add(inTxt2);
            PictureBox inPb2 = new PictureBox
            {
                Size = new Size(45, 45),
                Location = new Point(2*portPanel.Width / 5 - 22, 60),
                BackColor = Color.LightGreen,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = ReturnResourceImage(inRes2)
            };
            inPb2.Click += (s, e) =>
            {
                foreach (var res in aktuellerFab.BenotigteRecurse2.ToList())
                {
                    player.AddResource(res);
                    aktuellerFab.BenotigteRecurse2.Remove(res);
                }
                if (inventoryPanel.Visible)
                    ShowInventory();
            };
            portPanel.Controls.Add(inPb2);
            inCount2 = new Label
            {
                AutoSize = true,
                Location = new Point(inPb2.Left, inPb2.Bottom + 5),
                Text = $"Anzahl: {aktuellerFab.BenotigteRecurse2.Count()}"
            };
            portPanel.Controls.Add(inCount2);
            Label outTxt = new Label
            {
                AutoSize = true,
                Text = "Outgoing Port:",
                Font = new Font("Arial", 8, FontStyle.Bold),
                Location = new Point(3*portPanel.Width/5 - 30, 40)
            };
            portPanel.Controls.Add(outTxt);
            PictureBox outPb = new PictureBox
            {
                Size = new Size(45, 45),
                Location = new Point(3*portPanel.Width/5 - 22, 60),
                BackColor = Color.LightSteelBlue,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = ReturnResourceImage(outRes)
            };
            outPb.Click += (s, e) =>
            {
                foreach (var res in aktuellerFab.ErgebnissRecurse1.ToList())
                {
                    player.AddResource(res);
                    aktuellerFab.ErgebnissRecurse1.Remove(res);
                }
                if (inventoryPanel.Visible)
                    ShowInventory();
            };
            portPanel.Controls.Add(outPb);
            outCount = new Label
            {
                AutoSize = true,
                Location = new Point(outPb.Left, outPb.Bottom + 5),
                Text = $"Anzahl: {aktuellerFab.ErgebnissRecurse1.Count()}"
            };
            portPanel.Controls.Add(outCount);
            Button closeBtn = new Button
            {
                Size = new Size(30, 30),
                Location = new Point(portPanel.Width - 35, 5),
                Text = "X",
                ForeColor = Color.White,
                BackColor = Color.Red
            };
            closeBtn.Click += (s, e) => portPanel.Visible = false;
            portPanel.Controls.Add(closeBtn);
            ToolTip tt = new ToolTip();
            tt.SetToolTip(inPb, $"Klick zum Entnehmen von {inRes1}"); // Shows small PopUp-Window, for UserHelp
            tt.SetToolTip(inPb2, $"Klick zum Entnehmen von {inRes2}");
            tt.SetToolTip(outPb, $"Klick zum Entnehmen von {outRes}");
            portPanel.BringToFront();
            portPanel.Visible = true;
        }
        public void ShowFinishinatorInterface(Finishinator fin)
        {
            konInterface.Controls.Clear();
            konInterface.Size = new Size(600, 400);
            konInterface.Location = new Point(100, 100);
            aktuellerFin = fin;
            int spacingY = 50;
            int y = 40;
            int centerX = portPanel.Width / 2;

            Label name = new Label
            {
                Text = "Finishinator",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            konInterface.Controls.Add(name);

            List<(ResourceType typ, int benötigt, int aktuell)> resList = new List<(ResourceType, int, int)>
            {
                (aktuellerFin.TypBenotigteRecurse1, aktuellerFin.NötigeMengenBenotigteRecurse1, aktuellerFin.AktuelleAnzahlAbgegebeneResource1),
                (aktuellerFin.TypBenotigteRecurse2, aktuellerFin.NötigeMengenBenotigteRecurse2, aktuellerFin.AktuelleAnzahlAbgegebeneResource2),
                (aktuellerFin.TypBenotigteRecurse3, aktuellerFin.NötigeMengenBenotigteRecurse3, aktuellerFin.AktuelleAnzahlAbgegebeneResource3),
                (aktuellerFin.TypBenotigteRecurse4, aktuellerFin.NötigeMengenBenotigteRecurse4, aktuellerFin.AktuelleAnzahlAbgegebeneResource4),
                (aktuellerFin.TypBenotigteRecurse5, aktuellerFin.NötigeMengenBenotigteRecurse5, aktuellerFin.AktuelleAnzahlAbgegebeneResource5)
            };

            int benötigtGesamt = 0;
            int aktuellGesamt = 0;
            finishResLabels.Clear();
            for (int i = 0; i < resList.Count; i++)
            {
                var (typ, benöigt, aktuell) = resList[i];
                benötigtGesamt += resList[i].benötigt;
                aktuellGesamt += Math.Min(resList[i].aktuell, resList[i].benötigt);
                PictureBox icon = new PictureBox
                {
                    Size = new Size(40, 40),
                    Location = new Point(centerX - 120, y),
                    BackColor = Color.White,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = ReturnResourceImage(typ)
                };
                konInterface.Controls.Add(icon);
                Label finishLb = new Label
                {
                    AutoSize = true,
                    Location = new Point(centerX - 60, icon.Top + 10),
                    Text = $"{typ}: {aktuell} / {benöigt}"
                };
                konInterface.Controls.Add(finishLb);
                finishResLabels.Add(finishLb);
                y = y + spacingY;
            }
            pb = new ProgressBar
            {
                Width = 200,
                Height = 20,
                Location = new Point(centerX - 100, y),
                Maximum = benötigtGesamt > 0 ? benötigtGesamt : 1,
                Value = aktuellGesamt
            };
            konInterface.Controls.Add(pb);
            y += spacingY;

            percentLabel = new Label
            {
                Text = $"Gesamtfortschritt: {(int)((double)aktuellGesamt / benötigtGesamt * 100)}%",
                AutoSize = true,
                Location = new Point(centerX - 50, y)
            };
            konInterface.Controls.Add(percentLabel);
            y += spacingY;

                Button levelUP = new Button
                {
                    Text = "Stufenaufstieg",
                    Size = new Size(150, 40),
                    Location = new Point(centerX - 75, resList.Count * 65 + 40),
                    BackColor = Color.Gold,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Visible = resList.All(r => r.aktuell >= r.benötigt) ? true : false
                };
                levelUP.Click += (s, e) =>
                {
                    aktuellerFin.SteigeInDerStufeAuf();
                    ShowFinishinatorInterface(fin);
                };
                konInterface.Controls.Add(levelUP);
            
            Button closeBtn = new Button
            {
                Size = new Size(30, 30),
                Location = new Point(konInterface.Width - 35, 5),
                Text = "X",
                ForeColor = Color.White,
                BackColor = Color.Red
            };
            closeBtn.Click += (s, e) =>
            {
                konInterface.Visible = false;
                konInterface.Size = new Size(250, 300);
                konInterface.Location = new Point(50, 50);
            };
            konInterface.Controls.Add(closeBtn);
            konInterface.Visible = true;
        }

        public void CurveBandVerhalten(CurveBand band) 
        {
            if (band.Modus == true)
            {
                band.Modus = false;
                return;
            }
            if (band.Modus == false)
            {
                band.Modus = true;
                return;
            }
        }
    }
}
