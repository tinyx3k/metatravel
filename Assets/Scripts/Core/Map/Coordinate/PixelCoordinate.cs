using System;
using Util;
using static Core.Map.MercatorProjection;

namespace Core.Map.Coordinate
{
    public class PixelCoordinate : ICoordinate
    {
        public double X { get; set; }
        public double Y { get; set; }

        private static PixelCoordinate PixelOrigin => new PixelCoordinate(
            TileSize / 2f, 
            TileSize / 2f
        );

        private static double PixelPerDeg => TileSize / 360f;
        private static double PixelPerRad => TileSize / (2f * Math.PI);
        public PixelCoordinate(double x, double y)
        {
            X = x;
            Y = y;
        }

        public LatLngCoordinate ToLatLngCoordinate()
        {
            var x = X / Scale;
            var y = Y / Scale;

            var lng = (x - PixelOrigin.X) / PixelPerDeg;
            var latRad = (y - PixelOrigin.Y) / -PixelPerRad;
            var lat = Trigonometric.RadianToDegree(2 * Math.Atan(Math.Exp(latRad)) - Math.PI / 2);
            return new LatLngCoordinate(lat, lng);
        }
    }
}