using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEBluePrintIO
{
    public interface IShipGridModel
    {
        BasicBlock[] Neighbors(int x, int y, int z);
        BasicBlock[,,] LocalCube(int x, int y, int z);
    }

    public class ShipGridModel : IShipGridModel
    {
        readonly int maxX = 0;
        readonly int maxY = 0;
        readonly int maxZ = 0;

        private readonly BasicBlock[,,] blockDecks = null;
        private readonly List<BasicBlock> allBlocks = new();

        private int OffSetX { get; set; }
        private int OffSetY { get; set; }
        private int OffSetZ { get; set; }

        public ShipGridModel(List<List<string[]>> decks)
        {
            foreach (var lines in decks)
            {
                foreach (var line in lines)
                {
                    maxX = Math.Max(maxX, line.Length);
                }
                maxZ = Math.Max(maxZ, lines.Count);
            }
            maxY = decks.Count;
            OffSetZ = -maxZ / 2;
            OffSetY = -maxY / 2;
            OffSetX = -maxX / 2;

            blockDecks = new BasicBlock[maxX, maxY, maxZ];

            MakeBlocks(decks);

            //Optimize Armor
            allBlocks.ForEach(x => x.Optimize(this));
        }

        private void MakeBlocks(List<List<string[]>> decks)
        {
            int y = 0;
            foreach (var deck in decks)
            {
                int z = 0;
                foreach (var line in deck)
                {
                    int x = 0;
                    foreach (var item in line)
                    {
                        BasicBlock block = null;
                        if (!String.IsNullOrWhiteSpace(item) && blockDecks[x, y, z] == null)
                        {
                            var args = item.Split(':');
                            var arg1 = args.Length > 1 ? args[1] : "F";
                            var arg2 = args.Length > 2 ? args[2] : null;
                            if (args[0] == "CJ") block = new BasicBlock() { Type = "MyObjectBuilder_Conveyor", Subtype = "LargeBlockConveyor" };
                            else if (args[0] == "A") block = new ArmorBlock() { Type = "MyObjectBuilder_CubeBlock", Subtype = "LargeBlockArmorBlock" };
                            //Weird AP
                            else if (args[0] == "AP") block = new BasicBlock(arg1, arg2) { Type = "MyObjectBuilder_CubeBlock", Subtype = "LargeArmorPanelLight" };
                            else if (args[0] == "AV") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_AirVent", Subtype = "" };
                            else if (args[0] == "B") block = new BasicBlock() { Type = "MyObjectBuilder_BatteryBlock", Subtype = "LargeBlockBatteryBlock" };
                            //Weird BED
                            else if (args[0] == "BED") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_CryoChamber", Subtype = "LargeBlockBed" };
                            else if (args[0] == "C") block = new OutwardFacing(arg1, arg2) { Type = "MyObjectBuilder_ShipConnector", Subtype = "Connector" };
                            else if (args[0] == "Cam") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_CameraBlock", Subtype = "LargeCameraBlock" };
                            else if (args[0] == "Cockpit") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_Cockpit", Subtype = "LargeBlockCockpitSeat" };
                            else if (args[0] == "CS") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_Cockpit", Subtype = "CockpitOpen" };
                            else if (args[0] == "CW") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_CubeBlock", Subtype = "Catwalk" };
                            else if (args[0] == "D") block = new OutwardFacing(arg1) { Type = "MyObjectBuilder_AirtightSlideDoor", Subtype = "LargeBlockSlideDoor" };
                            else if (args[0] == "GG") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_GravityGenerator", Subtype = "" };
                            else if (args[0] == "GY") block = new BasicBlock(arg1, arg2) { Type = "MyObjectBuilder_Gyro", Subtype = "LargeBlockGyro" };
                            else if (args[0] == "HTS") block = new OutwardFacing(arg1) { Type = "MyObjectBuilder_Thrust", Subtype = "LargeBlockSmallHydrogenThrust" };
                            //Weird JB
                            else if (args[0] == "JB") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_Jukebox", Subtype = "Jukebox" };
                            else if (args[0] == "LDR") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_Ladder2", Subtype = "" };
                            else if (args[0] == "LCD") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_TextPanel", Subtype = "LargeLCDPanel" };
                            else if (args[0] == "Li") block = new BasicBlock(arg1, arg2) { Type = "MyObjectBuilder_InteriorLight", Subtype = "SmallLight" };
                            else if (args[0] == "LiS") block = new BasicBlock(arg1, arg2) { Type = "MyObjectBuilder_ReflectorLight", Subtype = "LargeBlockFrontLight" };
                            else if (args[0] == "PB") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_MyProgrammableBlock", Subtype = "LargeProgrammableBlock" };
                            else if (args[0] == "PS") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_Cockpit", Subtype = "PassengerSeatLarge" };
                            else if (args[0] == "Proj") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_Projector", Subtype = "LargeProjector" };
                            else if (args[0] == "RC") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_RemoteControl", Subtype = "LargeBlockRemoteControl" };
                            else if (args[0] == "SB") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_SoundBlock", Subtype = "LargeBlockSoundBlock" };
                            else if (args[0] == "SC") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_CargoContainer", Subtype = "LargeBlockSmallContainer" };
                            else if (args[0] == "SK") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_SurvivalKit", Subtype = "SurvivalKitLarge" };
                            else if (args[0] == "SR") block = new BasicBlock(arg1, arg2) { Type = "MyObjectBuilder_Reactor", Subtype = "LargeBlockSmallGenerator" };
                            //Weird STO
                            else if (args[0] == "STO") block = new BasicBlock(arg1) { Type = "MyObjectBuilder_StoreBlock", Subtype = "StoreBlock" };
                            else if (args[0] == "TLCD") block = new OutwardFacing(arg1) { Type = "MyObjectBuilder_TextPanel", Subtype = "TransparentLCDLarge" };
                            else if (args[0] == "W") block = new WindowBlock(arg1, arg2) { Type = "MyObjectBuilder_CubeBlock", Subtype = "Window1x1Slope" };
                            else if (args[0] == "WF") block = new BasicBlock(arg1, arg2) { Type = "MyObjectBuilder_CubeBlock", Subtype = "Window1x1Face" };

                            else if (args[0] == "interior") block = new NullBlock("interior");

                            //Hard
                            //Lettering (with color)
                            //Gatling Gun
                            //Interior Turret

                            //Awkward Shapes
                            else if (args[0] == "AN")
                            {
                                block = new LargeBlock(1, 6, 2, arg1, arg2) { Type = "MyObjectBuilder_RadioAntenna", Subtype = "LargeBlockRadioAntenna" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "LG")
                            {
                                block = new LargeBlock(1, 2, 3, arg1, arg2) { Type = "MyObjectBuilder_LandingGear", Subtype = "LargeBlockLandingGear" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "JD")
                            {
                                block = new LargeBlock(3, 3, 2, arg1, arg2) { Type = "MyObjectBuilder_JumpDrive", Subtype = "LargeJumpDrive" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "AT")
                            {
                                block = new LargeBlock(3, 3, 5, arg1) { Type = "MyObjectBuilder_Thrust", Subtype = "LargeBlockLargeAtmosphericThrustSciFi" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "ATS")
                            {
                                block = new LargeBlock(1, 1, 3, arg1) { Type = "MyObjectBuilder_Thrust", Subtype = "LargeBlockSmallAtmosphericThrustSciFi" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "I")
                            {
                                block = new LargeBlock(3, 2, 4, arg1, arg2) { Type = "MyObjectBuilder_Thrust", Subtype = "LargeBlockLargeThrust" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "IS")
                            {
                                block = new LargeBlock(1, 1, 2, arg1) { Type = "MyObjectBuilder_Thrust", Subtype = "LargeBlockSmallThrust" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }

                            //2x1 Shapes
                            else if (args[0] == "OD")
                            {
                                block = new LargeBlock(1, 1, 2, arg1) { Type = "MyObjectBuilder_OreDetector", Subtype = "LargeOreDetector" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "AS")
                            {
                                block = new LargeBlock(1, 1, 2, arg1, arg2) { Type = "MyObjectBuilder_Assembler", Subtype = "LargeAssembler" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "HE")
                            {
                                block = new LargeBlock(1, 1, 2, arg1, arg2) { Type = "MyObjectBuilder_HydrogenEngine", Subtype = "LargeHydrogenEngine" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "Be")
                            {
                                block = new LargeBlock(1, 2, 1, arg1, arg2) { Type = "MyObjectBuilder_Beacon", Subtype = "LargeBlockBeacon" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "HG")
                            {
                                block = new LargeBlock(1, 2, 1, arg1, arg2) { Type = "MyObjectBuilder_OxygenGenerator", Subtype = "" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "HTs")
                            {
                                block = new LargeBlock(1, 2, 1, arg1, arg2) { Type = "MyObjectBuilder_OxygenTank", Subtype = "LargeHydrogenTankSmall" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "OT")
                            {
                                block = new LargeBlock(1, 2, 1, arg1, arg2) { Type = "MyObjectBuilder_OxygenTank", Subtype = "" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "Welder")
                            {
                                block = new LargeBlock(1, 2, 1, arg1, arg2) { Type = "MyObjectBuilder_ShipWelder", Subtype = "LargeShipWelder" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }

                            //Directional Symmetric
                            else if (args[0] == "LC")
                            {
                                block = new LargeBlock(3, arg1) { Type = "MyObjectBuilder_CargoContainer", Subtype = "LargeBlockLargeContainer" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "HT")
                            {
                                block = new LargeBlock(3, arg1) { Type = "MyObjectBuilder_OxygenTank", Subtype = "LargeHydrogenTank" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            else if (args[0] == "HTL")
                            {
                                block = new LargeBlock(3, arg1) { Type = "MyObjectBuilder_Thrust", Subtype = "LargeBlockLargeHydrogenThrust" };
                                (block as LargeBlock).ReserveSpace(x, y, z, blockDecks);
                            }
                            if (block != null)
                            {
                                block.X = x + OffSetX;
                                block.Y = y + OffSetY;
                                block.Z = z + OffSetZ;
                                blockDecks[x, y, z] = block;
                                allBlocks.Add(block);
                            }
                        }
                        x++;
                    }
                    z++;
                }
                y++;
            }
        }

        public string ShipName { get; set; }

        internal void Write(IBlueprintWriter writer)
        {
            foreach (var block in allBlocks)
            {
                block.WriteOut(writer);
            }
        }

        public BasicBlock[] Neighbors(int x, int y, int z)
        {
            x -= OffSetX;
            y -= OffSetY;
            z -= OffSetZ;
            List<BasicBlock> neighbors = new();
            neighbors.Add(z > 0 ? blockDecks[x, y, z - 1] : null);
            neighbors.Add(x < maxX-1 ? blockDecks[x+1, y, z] : null);
            neighbors.Add(z < maxZ-1 ? blockDecks[x, y, z + 1] : null);
            neighbors.Add(x > 0 ? blockDecks[x-1, y, z] : null);
            neighbors.Add(y < maxY - 1 ? blockDecks[x , y+1, z] : null);
            neighbors.Add(y > 0 ? blockDecks[x, y-1, z] : null);
            return neighbors.ToArray();
        }

        public BasicBlock[,,] LocalCube(int x, int y, int z)
        {
            x -= OffSetX;
            y -= OffSetY;
            z -= OffSetZ;
            BasicBlock[,,] localCube= new BasicBlock[3,3,3];
            for (int ix = -1; ix <= 1; ++ix)
            {
                for (int iy = -1; iy <= 1; ++iy)
                {
                    for (int iz = -1; iz <= 1; ++iz)
                    {
                        //Copy only if in bounds.
                        if ((ix + x >= 0 && ix + x < maxX) && (iy + y >= 0 && iy + y < maxY) && (iz + z >= 0 && iz + z < maxZ))
                        {
                            localCube[ix+1, iy+1, iz+1] = blockDecks[ix + x, iy + y,iz+z];
                        }
                    }
                }
            }
            return localCube;
        }
    }

}
