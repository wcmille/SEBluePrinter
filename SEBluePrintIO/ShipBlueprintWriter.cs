using System;
using System.Xml;

namespace SEBluePrintIO
{
    public class ShipBlueprintWriter : IDisposable, IBlueprintWriter
    {
        static readonly string builtBy = "13291234";
        static readonly string entityID = "80395965283248514";
        static readonly string ownerSteamID = "76561198033772917";
        static readonly string modelGridSize = "Large";
        readonly XmlWriter writer;

        public ShipBlueprintWriter(string filename)
        {
            var settings = new XmlWriterSettings()
            {
                Indent = true,
            };
            writer = XmlWriter.Create(filename, settings);
        }

        public void Write(ShipGridModel model)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("Definitions");
            {
                writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
                writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteStartElement("ShipBlueprints");
                {
                    writer.WriteStartElement("ShipBlueprint");
                    writer.WriteAttributeString("xsi", "type", null, "MyObjectBuilder_ShipBlueprintDefinition");

                    writer.WriteStartElement("Id");
                    writer.WriteAttributeString("Type", "MyObjectBuilder_ShipBlueprintDefinition");
                    writer.WriteAttributeString("Subtype", model.ShipName);
                    writer.WriteEndElement();

                    writer.WriteStartElement("DisplayName");
                    writer.WriteValue("BMC");
                    writer.WriteEndElement();

                    writer.WriteStartElement("CubeGrids");
                    {
                        writer.WriteStartElement("CubeGrid");
                        {
                            writer.WriteStartElement("SubtypeName");
                            writer.WriteEndElement();

                            WriteSimpleElement("EntityId", entityID);
                            WriteSimpleElement("PersistentFlags", "CastShadows InScene");

                            WritePositionAndOrientation();

                            writer.WriteStartElement("LocalPositionAndOrientation");
                            writer.WriteAttributeString("xsi", "nil", null, "true");
                            writer.WriteEndElement();

                            WriteSimpleElement("GridSizeEnum", modelGridSize);

                            WriteCubes(model);

                            WriteSimpleElement("DisplayName", model.ShipName);
                            WriteSimpleElement("DestructibleBlocks", "true");
                            WriteSimpleElement("IsRespawnGrid", "false");
                            WriteSimpleElement("LocalCoordSys", "0");

                            writer.WriteStartElement("TargetingTargets");
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    WriteSimpleElement("EnvironmentType", "None");
                    WriteSimpleElement("WorkshopId", "0");
                    WriteSimpleElement("OwnerSteamId", ownerSteamID);
                    WriteSimpleElement("Points", "0");

                    writer.WriteEndElement(); //Blueprint
                }
                writer.WriteEndElement(); //Blueprints
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        private void WritePositionAndOrientation()
        {
            writer.WriteStartElement("PositionAndOrientation");
            {
                writer.WriteStartElement("Position");
                writer.WriteAttributeString("x", "0.0");
                writer.WriteAttributeString("y", "0.0");
                writer.WriteAttributeString("z", "10.0");
                writer.WriteEndElement();

                writer.WriteStartElement("Forward");
                writer.WriteAttributeString("x", "-0");
                writer.WriteAttributeString("y", "-0");
                writer.WriteAttributeString("z", "-1");
                writer.WriteEndElement();

                writer.WriteStartElement("Up");
                writer.WriteAttributeString("x", "0");
                writer.WriteAttributeString("y", "1");
                writer.WriteAttributeString("z", "0");
                writer.WriteEndElement();

                writer.WriteStartElement("Orientation");
                WriteSimpleElement("X", "0");
                WriteSimpleElement("Y", "0");
                WriteSimpleElement("Z", "0");
                WriteSimpleElement("W", "1");
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private void WriteCubes(ShipGridModel model)
        {
            writer.WriteStartElement("CubeBlocks");
            {
                model.Write(this);
            }
            writer.WriteEndElement();
        }

        private void WriteSimpleElement(string name, string value)
        {
            writer.WriteStartElement(name);
            writer.WriteValue(value);
            writer.WriteEndElement();
        }

        private void WriteXyzElement(string name, double x, double y, double z)
        {
            writer.WriteStartElement(name);
            writer.WriteAttributeString("x", x.ToString());
            writer.WriteAttributeString("y", y.ToString());
            writer.WriteAttributeString("z", z.ToString());
            writer.WriteEndElement();
        }
        private void WriteXyzElement(string name, int x, int y, int z)
        {
            writer.WriteStartElement(name);
            writer.WriteAttributeString("x", x.ToString());
            writer.WriteAttributeString("y", y.ToString());
            writer.WriteAttributeString("z", z.ToString());
            writer.WriteEndElement();
        }
        public void Dispose()
        {
            writer.Dispose();
        }

        public void WriteOutCube(string objType, string subType, int x, int y, int z, Orientation o)
        {
            writer.WriteStartElement("MyObjectBuilder_CubeBlock");
            writer.WriteAttributeString("xsi", "type", null, objType);
            {
                WriteSimpleElement("SubtypeName", subType);
                WriteXyzElement("Min", x, y, z);

                writer.WriteStartElement("BlockOrientation");
                writer.WriteAttributeString("Forward",o.Forward);
                writer.WriteAttributeString("Up", o.Up);
                writer.WriteEndElement();

                WriteXyzElement("ColorMaskHSV", 0, -0.8, 0);
                WriteSimpleElement("SkinSubtypeId", "Clean_Armor");
                WriteSimpleElement("BuiltBy", builtBy);
            }
            writer.WriteEndElement();
        }
    }
}
