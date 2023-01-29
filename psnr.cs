namespace sr
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class psnr
    {
        private static double min(double a, double b)
        {
            if (a > b)
                return b;
            return a;
        }
        private static void hsi(double r, double g, double b, ref double hue, ref double saturation, ref double intensity)
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
        public static double r(Bitmap input, Bitmap primary)
        {
            double output = 0.0;

            double hsiMetric = 0.0;

            double di = 0.0;
            double dc = 0.0;

            double h1 = 0.0;
            double s1 = 0.0;
            double i1 = 0.0;

            double h2 = 0.0;
            double s2 = 0.0;
            double i2 = 0.0;

            double t = 0.0;

            double numerator = 0.0;

            for (int x = 0; x < input.Width; ++x)
            {
                for (int y = 0; y < input.Height; ++y)
                {
                    hsi(input.GetPixel(x, y).R, input.GetPixel(x, y).G, input.GetPixel(x, y).B, ref h1, ref s1, ref i1);
                    hsi(primary.GetPixel(x, y).R, primary.GetPixel(x, y).G, primary.GetPixel(x, y).B, ref h2, ref s2, ref i2);
                    di = Math.Abs(i1 - i2);
                    t = Math.Abs(h1 - h2);
                    if (Math.Abs(h1 - h2) >= Math.PI)
                    {
                        t = 2 * Math.PI - t;
                    }
                    dc = Math.Sqrt(s1 * s1 + s2 * s2 - 2 * s1 * s2 * Math.Cos(t));
                    hsiMetric = Math.Sqrt(di * di + dc * dc);
                    output = output + hsiMetric;
                }
            }

            if (input.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                numerator = 3 * Math.Pow(Math.Pow(2, 24), 2);
            }
            else if (input.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb)
            {
                numerator = 3 * Math.Pow(Math.Pow(2, 32), 2);
            }

            output = numerator / output;

            output = 10 * Math.Log10(output);


            return output;
        }
    }
}
