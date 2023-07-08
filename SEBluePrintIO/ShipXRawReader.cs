using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEBluePrintIO
{
    public class ShipXRawReader
    {
        //https://twitter.com/ephtracy/status/653721698328551424/photo/1
        const string atmoFlame = "flame";
        const string ionFlame = "ionFlame";
        const string interior = "interior";

        public static List<List<string[]>> Read(string path)
        {
            using (FileStream fs = new(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader r = new(fs))
                {
                    int x = 0, y = 0, z = 0;
                    //Read The header
                    var formatTag = new string(r.ReadChars(4));
                    if (formatTag != "XRAW") throw new Exception("Not XRaw");
                    r.ReadBytes(4);
                    x = r.ReadInt32();
                    y = r.ReadInt32();
                    z = r.ReadInt32();
                    r.ReadBytes(4);

                    //Read all the voxels.
                    var decks = new List<List<string[]>>();
                    ReadVoxelByteCodes(r, x, y, z, decks);
                    GuessAtOrientations(x, y, z, decks);
                    return decks;
                }
            }
        }

        private static void GuessAtOrientations(int x, int y, int z, List<List<string[]>> decks)
        {
            for (int i = 0; i < z; ++i)
            {
                for (int j = 0; j < y; ++j)
                {
                    for (int k = 0; k < x; ++k)
                    {
                        var code = decks[i][j][k];
                        if (code == "OT" || code == "HTs" || code == "Be" || code == "HG")
                        {
                            if (j < (y - 1) && decks[i][j + 1][k] == code) decks[i][j][k] = $"{code}:D";
                            else if (k < (x - 1) && decks[i][j][k + 1] == code) decks[i][j][k] = $"{code}:F:R";
                        }
                        else if (code == "JD" || code == "OD" || code == "AS" || code == "HE")
                        {
                            if (j < (y - 1) && decks[i][j + 1][k] == code) decks[i][j][k] = $"{code}";
                            else if (k < (x - 1) && decks[i][j][k + 1] == code) decks[i][j][k] = $"{code}:R";
                            else if (i < (z - 1) && decks[i + 1][j][k] == code) decks[i][j][k] = $"{code}:D";
                        }
                        else if (code == "I")
                        {
                            //check up 1, back 1 == F:R
                            if (decks[i + 1][j + 4][k] == ionFlame) decks[i][j][k] = $"{code}:B:R";
                            else if (decks[i + 1][j][k + 4] == ionFlame && decks[i + 1][j][k + 3] == code) decks[i][j][k] = $"{code}:R:B";
                            else if (decks[i + 1][j][k - 1] == ionFlame) decks[i][j][k] = $"{code}:L:B";
                            //else { decks[i][j][k] = interior; }
                        }
                        else if (code == "IS")
                        {
                            if (j < (y - 1) && decks[i][j + 1][k] == ionFlame) decks[i][j][k] = $"{code}:F";
                            else if (k < (x - 1) && decks[i][j][k + 1] == ionFlame) decks[i][j][k] = $"{code}:R";
                            else if (i < (z - 1) && decks[i + 2][j][k] == ionFlame) decks[i][j][k] = $"{code}:U";
                            else if (i > 0 && decks[i - 1][j][k] == ionFlame) decks[i][j][k] = $"{code}:D:F";
                        }
                        else if (code == "ATS")
                        {
                            if (j < (y - 1) && decks[i][j + 1][k] == atmoFlame) decks[i][j][k] = $"{code}:F";
                            else if (k < (x - 1) && decks[i][j][k + 1] == atmoFlame) decks[i][j][k] = $"{code}:R";
                            else if (i < (z - 1) && decks[i + 1][j][k] == atmoFlame) decks[i][j][k] = $"{code}:U";
                            else if (i > 0 && decks[i - 1][j][k] == atmoFlame) decks[i][j][k] = $"{code}:D:F";
                        }
                        else if (code == "Welder" || code == "Welder-W")
                        {
                            string item = "Welder";
                            string tip = "Welder-W";

                            if (decks[i][j+1][k] == item) decks[i][j][k] = $"{item}:F";
                            else if (decks[i][j+1][k] == tip) decks[i][j][k] = $"{item}:B";
                            else if (decks[i][j][k + 1] == item) decks[i][j][k] = $"{item}:L";
                            else if (decks[i][j][k + 1] == tip) decks[i][j][k] = $"{item}:R";
                            else if (decks[i + 1][j][k] == tip) decks[i][j][k] = $"{item}:U";
                            else if (decks[i + 1][j][k] == item) decks[i][j][k] = $"{item}:D:F";
                        }
                    }
                }
            }
        }

        private static void ReadVoxelByteCodes(BinaryReader r, int x, int y, int z, List<List<string[]>> decks)
        {
            byte[] line = new byte[x];
            for (int i = 0; i < z; i++)
            {
                var lines = new List<string[]>();
                for (int j = 0; j < y; j++)
                {
                    line = r.ReadBytes(x);
                    string[] s = new string[x];
                    for (int k = 0; k < x; k++)
                    {
                        if (line[k] == 1) s[k] = "A";
                        else if (line[k] == 255) s[k] = interior;
                        else if (line[k] == 178) s[k] = atmoFlame;
                        else if (line[k] == 186) s[k] = ionFlame;
                        else if (line[k] == 241) s[k] = "SR";
                        else if (line[k] == 239) s[k] = "JD";
                        else if (line[k] == 233) s[k] = "B";
                        else if (line[k] == 232) s[k] = "C";
                        else if (line[k] == 225) s[k] = "CJ";
                        else if (line[k] == 221) s[k] = "LC";
                        else if (line[k] == 217) s[k] = "SC";
                        else if (line[k] == 213) s[k] = "HT";
                        else if (line[k] == 211) s[k] = "OT";
                        else if (line[k] == 209) s[k] = "HTs";
                        else if (line[k] == 202) s[k] = "Welder-W";
                        else if (line[k] == 201) s[k] = "Welder";
                        else if (line[k] == 197) s[k] = "HTL";
                        else if (line[k] == 193) s[k] = "HTS";
                        else if (line[k] == 189) s[k] = "I";
                        else if (line[k] == 185) s[k] = "IS";
                        else if (line[k] == 181) s[k] = "AT:D";
                        else if (line[k] == 177) s[k] = "ATS";
                        else if (line[k] == 169) s[k] = "AV";
                        else if (line[k] == 154) s[k] = "TLCD";
                        else if (line[k] == 153) s[k] = "LCD";
                        else if (line[k] == 145) s[k] = "SK";
                        else if (line[k] == 137) s[k] = "GY";
                        else if (line[k] == 129) s[k] = "CS";
                        else if (line[k] == 121) s[k] = "Cockpit";
                        else if (line[k] == 113) s[k] = "PB";
                        else if (line[k] == 107) s[k] = "Proj";
                        else if (line[k] == 106) s[k] = "Cam";
                        else if (line[k] == 105) s[k] = "RC";
                        else if (line[k] == 50) s[k] = "D";
                        else if (line[k] == 33) s[k] = "CW";
                        else if (line[k] == 17) s[k] = "W";
                    }
                    lines.Add(s);
                    //lines.Insert(0, s);
                }
                decks.Add(lines);
            }
        }
    }
}
