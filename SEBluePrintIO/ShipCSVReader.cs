using System.Collections.Generic;
using System.Linq;

namespace SEBluePrintIO
{
    public class ShipCSVReader

    {
        public static List<List<string[]>> Read(string path)
        {
            var decks = new List<List<string[]>>();
            var filelist = System.IO.Directory.GetFiles(path, "*.*.csv").OrderBy(x=>x).ToArray();
            foreach (string file in filelist)
            {
                var lines = new List<string[]>();
                foreach (string line in System.IO.File.ReadLines(file))
                {
                    var split = line.Split(',').Select(x => x.Trim()).ToArray();
                    lines.Add(split);
                }
                decks.Add(lines);
            }
            return decks;
        }
    }
}
