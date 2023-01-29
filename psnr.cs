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
        public static double r(Bitmap input, Bitmap primary)
        {
            int wd = input.Width;
            int ht = input.Height;

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

            for (int x = 0; x < wd; ++x)
            {
                for (int y = 0; y < ht; ++y)
                {
                    util.hsi(input.GetPixel(x, y).R, input.GetPixel(x, y).G, input.GetPixel(x, y).B, ref h1, ref s1, ref i1);
                    util.hsi(primary.GetPixel(x, y).R, primary.GetPixel(x, y).G, primary.GetPixel(x, y).B, ref h2, ref s2, ref i2);
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
