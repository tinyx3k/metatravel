using System;
using static Core.Map.MercatorProjection;

namespace Core.Map.Coordinate
{
    public class WorldCoordinate : ICoordinate
    {
        public double X { get; set; }
        public double Y { get; set; }

        public WorldCoordinate(double x, double y)
        {
            X = x;
            Y = y;
        }

        public PixelCoordinate ToPixelCoordinate()
        {
            return new PixelCoordinate(
                Math.Floor(X * Scale),
                Math.Floor(Y * Scale)
            );
        }
    }
}