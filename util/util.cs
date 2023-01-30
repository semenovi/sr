namespace sr
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class util
    {
        public static double min(double a, double b)
        {
            if (a > b)
                return b;
            return a;
        }
        public static void hsi(double r, double g, double b, ref double hue, ref double saturation, ref double intensity)
        {
            intensity = (r + g + b) / 3;
            saturation = 1 - (3 * (min(r, min(g, b))) / (r + g + b));
            if (saturation == 0)
            {
                return;
            }
            hue = Math.Acos(0.5 * ((r - g) + (r - b)) / Math.Sqrt(((r - g) * (r - g)) + ((r - b) * (g - b))));
            if (b > g)
            {
                hue = ((360 * Math.PI) / 180.0) - hue;
            }
        }
    }
}
