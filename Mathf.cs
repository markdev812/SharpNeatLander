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
    }
}