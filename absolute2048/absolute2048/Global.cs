using System;
using System.Windows.Media;

namespace absolute2048
{
    static class Global
    {
        static public int widthX { get; set; } = 4;
        static public int heightY { get; set; } = 4;

        static public int basisValue { get; set; } = 2;
        static public int spawnValue { get; set; } = 2;
        static public bool multipleRespawn { get; set; } = false;

        static public double windowSizeModifier { get; set; } = 800;

        static public Brush backgroundColor { get; set; } = Brushes.Lavender;
        static public Brush lineColor { get; set; } = Brushes.Black;
    }
}
