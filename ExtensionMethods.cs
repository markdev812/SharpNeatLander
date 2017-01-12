using System;

namespace SharpNeatLander
{
    public static class ExtensionMethods
    {
        public static bool AlmostEquals(this double val1, double val2)
        {
            return (Math.Abs(val1 - val2) < 0.0000001);
        }
    }
}