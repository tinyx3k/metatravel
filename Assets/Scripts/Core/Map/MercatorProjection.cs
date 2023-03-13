using System;

namespace Core.Map
{
    public static class MercatorProjection
    {
        internal static int TileSize { get; set; }= 256;
        internal static double Zoom { get; set; } = 3;
        internal static double Scale => 1 << (int)Zoom;
    }
}
