namespace sr
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class ssim
    {
        // reference: https://medium.com/srm-mic/all-about-structural-similarity-index-ssim-theory-code-in-pytorch-6551b455541e

        public static double r(Bitmap input, Bitmap primary)
        {
            int wd = input.Width;
            int ht = input.Height;

            double ave1 = 0.0;
            double con1 = 0.0;
            double stc = 0.0;

            double ave2 = 0.0;
            double con2 = 0.0;

            double avec = 0.0;
            double conc = 0.0;
            double stcc = 0.0;

            double c1 = 0.0;
            double c2 = 0.0;
            double c3 = 0.0;

            double l = 0.0;
            double k1 = 0.01;
            double k2 = 0.03;

            double h1 = 0.0;
            double s1 = 0.0;
            double i1 = 0.0;

            double h2 = 0.0;
            double s2 = 0.0;
            double i2 = 0.0;

            double output = 0.0;

            for (int x = 0; x < wd; ++x)
            {
                for (int y = 0; y < ht; ++y)
                {
                    util.hsi(input.GetPixel(x, y).R, input.GetPixel(x, y).G, input.GetPixel(x, y).B, ref h1, ref s1, ref i1);
                    util.hsi(primary.GetPixel(x, y).R, primary.GetPixel(x, y).G, primary.GetPixel(x, y).B, ref h2, ref s2, ref i2);

                    ave1 = ave1 + i1;
                    ave2 = ave2 + i2;
                }
            }

            ave1 = ave1 / (wd * ht);
            ave2 = ave2 / (wd * ht);

            for (int x = 0; x < wd; ++x)
            {
                for (int y = 0; y < ht; ++y)
                {
                    util.hsi(input.GetPixel(x, y).R, input.GetPixel(x, y).G, input.GetPixel(x, y).B, ref h1, ref s1, ref i1);
                    util.hsi(primary.GetPixel(x, y).R, primary.GetPixel(x, y).G, primary.GetPixel(x, y).B, ref h2, ref s2, ref i2);

                    con1 = con1 + Math.Pow(i1 - ave1, 2);
                    con2 = con2 + Math.Pow(i2 - ave2, 2);
                }
            }

            con1 = con1 / (wd * ht - 1);
            con1 = Math.Sqrt(con1);

            con2 = con2 / (wd * ht - 1);
            con2 = Math.Sqrt(con2);

            for (int x = 0; x < wd; ++x)
            {
                for (int y = 0; y < ht; ++y)
                {
                    util.hsi(input.GetPixel(x, y).R, input.GetPixel(x, y).G, input.GetPixel(x, y).B, ref h1, ref s1, ref i1);
                    util.hsi(primary.GetPixel(x, y).R, primary.GetPixel(x, y).G, primary.GetPixel(x, y).B, ref h2, ref s2, ref i2);

                    stc = stc + (i1 - ave1) * (i2 - ave2);
                }
            }

            stc = stc / (wd * ht - 1);

            if (input.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                l = Math.Pow(2, 24) - 1;
            }
            else if (input.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb)
            {
                l = Math.Pow(2, 32) - 1;
            }

            c1 = Math.Pow(k1 * l, 2);
            c2 = Math.Pow(k2 * l, 2);
            c3 = c2 / 2;

            avec = (2 * ave1 * ave2 + c1) / (ave1 * ave1 + ave2 * ave2 + c1);
            conc = (2 * con1 * con2 + c2) / (con1 * con1 + con2 * con2 + c2);
            stcc = (stc + c3) / (con1 * con1 + con2 * con2 + c3);

            output = Math.Pow(avec, 1) * Math.Pow(conc, 1) * Math.Pow(stcc, 1);

            return output;
        }
    }
}
