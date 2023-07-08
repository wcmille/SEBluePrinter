using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEBluePrintIO
{
    public class Orientation
    {
        public static readonly string[] DirectionMap = { "Forward", "Right", "Backward", "Left", "Up", "Down" };
        private static readonly string[] opposite  = { "Backward", "Left", "Forward", "Right", "Down", "Up" };

        public static string Convert(string arg)
        {
            if (arg == "F") return "Forward";
            if (arg == "B") return "Backward";
            if (arg == "L") return "Left";
            if (arg == "R") return "Right";
            if (arg == "U") return "Up";
            if (arg == "D") return "Down";
            throw new ArgumentException("Invalid Argument");
        }

        public Orientation(string arg, string arg2)
        {
            Forward = Convert(arg);
            Up = (arg2 != null) ? Convert(arg2) : "";
            if (arg == "U" || arg == "D")
            {
                if (arg == "U" && Up == "") { Up = "Backward"; }
                else if (arg == "D" && Up == "") { Up = "Forward"; }
            }
            else
            {
                if (Up == "") Up = "Up";
            }
        }

        public string Forward { get; set; }
        public string Up { get; set; }

        internal static string Opposite(int nullDir)
        {
            return opposite[nullDir];
        }
    }
}
