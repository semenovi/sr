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
            double output = 0.0;

            int wd = input.Width;
            int ht = input.Height;

            double avgI = 0.0;
            double avgP = 0.0;

            double rI = 0.0;
            double gI = 0.0;
            double bI = 0.0;
            double rP = 0.0;
            double gP = 0.0;
            double bP = 0.0;

            double varI = 0.0;
            double varP = 0.0;

            double cov = 0.0;

            double l = 0.0;

            double c1 = 0.0;
            double c2 = 0.0;

            double k1 = 0.01;
            double k2 = 0.03;

            for (int x = 0; x < wd; ++x)
            {
                for (int y = 0; y < ht; ++y)
                {
                    rI = input.GetPixel(x, y).R;
                    gI = input.GetPixel(x, y).G;
                    bI = input.GetPixel(x, y).B;
                    rP = primary.GetPixel(x, y).R;
                    gP = primary.GetPixel(x, y).G;
                    bP = primary.GetPixel(x, y).B;

                    avgI = avgI + rI + gI + bI;
                    avgP = avgP + rP + gP + bP;
                }
            }

            avgI = (avgI / 3.0) / (wd * ht);
            avgP = (avgP / 3.0) / (wd * ht);

            for (int x = 0; x < wd; ++x)
            {
                for (int y = 0; y < ht; ++y)
                {
                    rI = input.GetPixel(x, y).R;
                    gI = input.GetPixel(x, y).G;
                    bI = input.GetPixel(x, y).B;
                    rP = primary.GetPixel(x, y).R;
                    gP = primary.GetPixel(x, y).G;
                    bP = primary.GetPixel(x, y).B;

                    varI = varI + Math.Pow(((rI + gI + bI) / 3.0) - avgI, 2);
                    varP = varP + Math.Pow(((rP + gP + bP) / 3.0) - avgP, 2);
                }
            }

            varI = Math.Sqrt(varI / (wd * ht - 1));
            varP = Math.Sqrt(varP / (wd * ht - 1));

            for (int x = 0; x < wd; ++x)
            {
                for (int y = 0; y < ht; ++y)
                {
                    rI = input.GetPixel(x, y).R;
                    gI = input.GetPixel(x, y).G;
                    bI = input.GetPixel(x, y).B;
                    rP = primary.GetPixel(x, y).R;
                    gP = primary.GetPixel(x, y).G;
                    bP = primary.GetPixel(x, y).B;

                    cov = cov + ((((rI + gI + bI) / 3.0) - avgI) * (((rP + gP + bP) / 3.0) - avgP));

                }
            }

            cov = cov / (wd * ht - 1);

            if (input.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                cli.ticksConsoleWrite("WARNING! using images with this depth is unwanted, please use 8 bit");
                l = Math.Pow(2, 24) - 1;
            }
            else if (input.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb)
            {
                cli.ticksConsoleWrite("WARNING! using images with this depth is unwanted, please use 8 bit");
                l = Math.Pow(2, 32) - 1;
            }
            else if (input.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {
                l = Math.Pow(2, 8) - 1;
            }

            c1 = Math.Pow(k1 * l, 2);
            c2 = Math.Pow(k2 * l, 2);

            output = ((2 * avgI * avgP + c1) * (2 * cov + c2)) / ((avgI * avgI + avgP * avgP + c1) * (varI * varI + varP * varP + c2));

            return output;
        }
    }
}
