using System;

namespace Util
{
    public class Trigonometric
    {
        public static double DegreeToRadian(double deg)
        {
            return deg * Math.PI / 180; 
        }

        public static double RadianToDegree(double rad)
        {
            return rad * Math.PI / 180;
        }
    }
}