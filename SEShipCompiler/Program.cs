//Additional Metadata
//TODO: Create battery groups.
//TODO: Create landing gear groups
//TODO: Create a group for cameras? Cameras should not be on the toolbar or control panel.
//TODO: Hydrogen Engines should be off.
//TODO: H2O2 should be off.
//TODO: Beacons should be off.
//TODO: Vents that face "the void" should have depressurize on.
//TODO: Interior Lights should have toolbar and control panel turned off.
//TODO: Batteries should be not in toolbar or control panel.
//TODO: Cargo containers should not be in toolbar or control panel.
//TODO: Doors should be shut.
//TODO: Radio Broadcast should be off.

//Painting
//TODO: Define primary, accent, and interior paint.
//TODO: Half-block support for armor.
//TODO: Fill out the toolbar

//TODO: Preload scripts onto PBs and blueprints onto Projectors
//TODO: Handle large or small grids.
//TODO: Handle stations
//TODO: Allow subgrids

using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using SEBluePrintIO;

namespace SEShipCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = args[0];
            List<List<string[]>> decks = ShipXRawReader.Read(inputFile);
            ShipGridModel model = new(decks)
            {
                ShipName = "TestShip"
            };
            using ShipBlueprintWriter writer = new("bp.sbc");
            writer.Write(model);
        }
    }
}