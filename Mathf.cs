using System;

namespace SharpNeatLander
{
    public class Mathf
    {
        public static double Clamp(double val, double min, double max)
        {
            return val < min ? min : (val > max ? max : val);
        }

        /// <summary>
        /// Linearly interpolates between a and b by t.
        /// </summary>
        /// <param name="a">The start value</param>
        /// <param name="b">The end value</param>
        /// <param name="t">The interpolation value between the start and end</param>
        /// <returns></returns>
        public static double Lerp(double a, double b, double t)
        {
            return a + (b - a) * Clamp(t, 0, 1);
        }

        public static int FloorToInt(double val)
        {
            return (int)Math.Floor(val);
        }

        /// <summary>
        /// Return random double between min and max (inclusive)
        /// </summary>
        public static double RandomRange(double min, double max)
        {
            Random r = new Random();
            if (min > max)
            {
                double temp = min;
                min = max;
                max = temp;
            }
            double range = max - min + 1;
            return min + Math.Floor((range * r.NextDouble()));
        }

        /// <summary>
        /// Return random int between min and max (exclusive)
        /// </summary>
        public static int RandomRange(int min, int max)
        {
            Random r = new Random();
            if (min > max)
            {
                int temp = min;
                min = max;
                max = temp;
            }
            int range = max - min;
            return min + FloorToInt((range * r.NextDouble()));

        }
    }
}