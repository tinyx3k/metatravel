using System;
using Util;
using static Core.Map.MercatorProjection;

namespace Core.Map.Coordinate
{
    public class LatLngCoordinate: ICoordinate
    {
        public double X { get; set; }
        public double Y { get; set; }

        public LatLngCoordinate(double x, double y)
        {
            X = x;
            Y = y;
        }

        public WorldCoordinate ToWorldCoordinate()
        {
            var sinY = Math.Max(
                Math.Min(
                    Math.Sin(Trigonometric.DegreeToRadian(X)), 
                    0.9999f), 
                -0.9999f);
            var lat = TileSize * (0.5f + Y / 360);
            var lng = TileSize * (0.5f - Math.Log((1 + sinY) / (1 - sinY)) / (4 * Math.PI));
            return new WorldCoordinate(lat, lng);
        }
    }
}