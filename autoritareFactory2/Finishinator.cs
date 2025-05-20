using autoritaereFactory;
using autoritaereFactory.world;
using factordictatorship.setup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace factordictatorship
{
    public class Finishinator : Fabrikgebeude
    {
        private int levle = 1;
        public int Levle { get { return levle; } }

        private ResourceType typBenotigteRecurse1;
        public ResourceType TypBenotigteRecurse1 { get { return typBenotigteRecurse1; } }
        private int nötigeMengenBenotigteRecurse1;
        public int NötigeMengenBenotigteRecurse1 { get { return nötigeMengenBenotigteRecurse1; } }
        private int aktuelleAnzahlAbgegebeneResource1;
        public int AktuelleAnzahlAbgegebeneResource1 { get { return aktuelleAnzahlAbgegebeneResource1; } }

        private ResourceType typBenotigteRecurse2;
        public ResourceType TypBenotigteRecurse2 { get { return typBenotigteRecurse2; } }
        private int nötigeMengenBenotigteRecurse2;
        public int NötigeMengenBenotigteRecurse2 { get { return nötigeMengenBenotigteRecurse2; } }
        private int aktuelleAnzahlAbgegebeneResource2;
        public int AktuelleAnzahlAbgegebeneResource2 { get { return aktuelleAnzahlAbgegebeneResource2; } }

        private ResourceType typBenotigteRecurse3;
        public ResourceType TypBenotigteRecurse3 { get { return typBenotigteRecurse3; } }
        private int nötigeMengenBenotigteRecurse3;
        public int NötigeMengenBenotigteRecurse3 { get { return nötigeMengenBenotigteRecurse3; } }
        private int aktuelleAnzahlAbgegebeneResource3;
        public int AktuelleAnzahlAbgegebeneResource3 { get { return aktuelleAnzahlAbgegebeneResource3; } }

        private ResourceType typBenotigteRecurse4;
        public ResourceType TypBenotigteRecurse4 { get { return typBenotigteRecurse4; } }
        private int nötigeMengenBenotigteRecurse4;
        public int NötigeMengenBenotigteRecurse4 { get { return nötigeMengenBenotigteRecurse4; } }
        private int aktuelleAnzahlAbgegebeneResource4;
        public int AktuelleAnzahlAbgegebeneResource4 { get { return aktuelleAnzahlAbgegebeneResource4; } }

        private ResourceType typBenotigteRecurse5;
        public ResourceType TypBenotigteRecurse5 { get { return typBenotigteRecurse5; } }
        private int nötigeMengenBenotigteRecurse5;
        public int NötigeMengenBenotigteRecurse5 { get { return nötigeMengenBenotigteRecurse5; } }
        private int aktuelleAnzahlAbgegebeneResource5;
        public int AktuelleAnzahlAbgegebeneResource5 { get { return aktuelleAnzahlAbgegebeneResource5; } }
        public Finishinator(int positionX, int positionY, int drehung) : base(positionX, positionY, drehung)
        {
            längeInXRichtung = 8;
            längeInYRichtung = 8;

            typBenotigteRecurse1 = ResourceType.Screw;
            nötigeMengenBenotigteRecurse1 = 1000;

            typBenotigteRecurse2 = ResourceType.Cable;
            nötigeMengenBenotigteRecurse2 = 300;

            typBenotigteRecurse3 = ResourceType.CopperWire;
            nötigeMengenBenotigteRecurse3 = 800;

            typBenotigteRecurse4 = ResourceType.IronPlate;
            nötigeMengenBenotigteRecurse4 = 200;

            typBenotigteRecurse5 = ResourceType.Concrete;
            nötigeMengenBenotigteRecurse5 = 500;
        }
        public override void Iteration()
        {
            nimmVonAllenBändern(aktuelleAnzahlAbgegebeneResource1, typBenotigteRecurse1, nötigeMengenBenotigteRecurse1);
            nimmVonAllenBändern(aktuelleAnzahlAbgegebeneResource2, typBenotigteRecurse2, nötigeMengenBenotigteRecurse2);
            nimmVonAllenBändern(aktuelleAnzahlAbgegebeneResource3, typBenotigteRecurse3, nötigeMengenBenotigteRecurse3);
            nimmVonAllenBändern(aktuelleAnzahlAbgegebeneResource4, typBenotigteRecurse4, nötigeMengenBenotigteRecurse4);
            nimmVonAllenBändern(aktuelleAnzahlAbgegebeneResource5, typBenotigteRecurse5, nötigeMengenBenotigteRecurse5);
            TesteAufstiegserlaubniss();
        }
        private void nimmVonAllenBändern(int aktuelleAnzahlAbgegebeneResource, ResourceType gewolteRecurse, int nötigeMengenBenotigteRecurse)
        {
            for (int i = 0; i < 8; i++)
            {
                nimmVomBand(aktuelleAnzahlAbgegebeneResource, -1, i, gewolteRecurse, nötigeMengenBenotigteRecurse);
                nimmVomBand(aktuelleAnzahlAbgegebeneResource, 8, i, gewolteRecurse, nötigeMengenBenotigteRecurse);
                nimmVomBand(aktuelleAnzahlAbgegebeneResource, i, -1, gewolteRecurse, nötigeMengenBenotigteRecurse);
                nimmVomBand(aktuelleAnzahlAbgegebeneResource, i, 8, gewolteRecurse, nötigeMengenBenotigteRecurse);
            }
        }
        private void TesteAufstiegserlaubniss()
        {
            if (aktuelleAnzahlAbgegebeneResource1 == nötigeMengenBenotigteRecurse1 && aktuelleAnzahlAbgegebeneResource2 == nötigeMengenBenotigteRecurse2 && aktuelleAnzahlAbgegebeneResource3 == nötigeMengenBenotigteRecurse3 && aktuelleAnzahlAbgegebeneResource4 == nötigeMengenBenotigteRecurse4 && aktuelleAnzahlAbgegebeneResource5 == nötigeMengenBenotigteRecurse5)
            {
                //Rupert oder wer immer für das interface des Finishinators verantwortlich ist an dieser stell einen Button oder so etwas erscheinen lassen der beim Drücken SteigeInDerStufeAuf() durchführt
            }
        }
        public void SteigeInDerStufeAuf()
        {
            //den in TesteAufstiegserlaubniss() zugänglich gemachten Button wieder verschwinden lassen
            levle++;
            if (levle == 2)
            {
                typBenotigteRecurse1 = ResourceType.Motor;
                nötigeMengenBenotigteRecurse1 = 100;

                typBenotigteRecurse2 = ResourceType.SteelRod;
                nötigeMengenBenotigteRecurse2 = 300;

                typBenotigteRecurse3 = ResourceType.SteelConcreteBeam;
                nötigeMengenBenotigteRecurse3 = 250;

                typBenotigteRecurse4 = ResourceType.Rotor;
                nötigeMengenBenotigteRecurse4 = 50;

                typBenotigteRecurse5 = ResourceType.Stator;
                nötigeMengenBenotigteRecurse5 = 50;
            }
            if (levle == 3)
            {
                File.Delete(WorldMap.theWorld.worldName);
                long ticks = WorldMap.theWorld.tickTimer;
                MessageBox.Show($"You have finished the Game with:\n{ticks / 600} min {ticks / 10 % 60}.{ticks % 10} sec ");
                // should throw new error?
            }
        }
        private void nimmVomBand(int aktuelleAnzahlAbgegebeneResource, int verschiebungXAchse, int verschiebungYAchse, ResourceType gewolteRecurse, int nötigeMengenBenotigteRecurse)//verschiebungXAchse und verschiebungYAchse bezihen sich auf die verschiebung von dem punkt aus der durch positionX/Y beschrieben wird
        {
            if (aktuelleAnzahlAbgegebeneResource < nötigeMengenBenotigteRecurse)
            {
                List<Fabrikgebeude> entitys = WorldMap.theWorld.GetEntityInPos(verschiebungXAchse, verschiebungYAchse);
                if (entitys.Count == 1)
                {
                    if (entitys[0].GetType() != typeof(Band))
                        return;
                    Band band = (Band)entitys[0];
                    if (band != null && band.GibRichtungAusgang() == drehung)
                    {
                        band.ErkenneRescourcen();
                        while (aktuelleAnzahlAbgegebeneResource < nötigeMengenBenotigteRecurse && band.currentRescourceList.Count > 0)
                        {
                            for (int i = 0; i < band.currentRescourceList.Count; i++)
                            {
                                if (band.currentRescourceList[i].Type == gewolteRecurse)
                                {
                                    aktuelleAnzahlAbgegebeneResource++;
                                    band.NimmRescourceVomBand(i);
                                    break;
                                }
                                if (i == band.currentRescourceList.Count - 1)
                                {
                                    return;
                                }
                            }
                            band.ErkenneRescourcen();
                        }
                    }
                }
            }
        }
        internal Finishinator() : base()
        {
            längeInXRichtung = 8;
            längeInYRichtung = 8;

            typBenotigteRecurse1 = ResourceType.Screw;
            nötigeMengenBenotigteRecurse1 = 1000;

            typBenotigteRecurse2 = ResourceType.Cable;
            nötigeMengenBenotigteRecurse2 = 300;

            typBenotigteRecurse3 = ResourceType.CopperWire;
            nötigeMengenBenotigteRecurse3 = 800;

            typBenotigteRecurse4 = ResourceType.IronPlate;
            nötigeMengenBenotigteRecurse4 = 200;

            typBenotigteRecurse5 = ResourceType.Concrete;
            nötigeMengenBenotigteRecurse5 = 500;
        }
        public override List<byte> GetAsBytes()
        {
            List<byte> bytes = base.GetAsBytes();
            bytes.AddRange(BitConverter.GetBytes((int)levle));
            return bytes;
        }
        public static new Finishinator FromByteArray(byte[] bytes, ref int offset)
        {
            Finishinator newFinishinator = new Finishinator();
            newFinishinator.levle = BitConverter.ToInt32(bytes, offset);
            offset += 4;
            return newFinishinator;
        }
        public override string ToString()
        {
            return "Finishinator";
        }
    }
}