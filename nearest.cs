namespace sr
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class nearest
    {
        // reference:
        // https://www.codeproject.com/Articles/2122/Image-Processing-for-Dummies-with-C-and-GDI-Part-4

        public static Bitmap r(Bitmap input, int c)
        {
            Bitmap bTemp = (Bitmap)input.Clone();
            Bitmap output = new Bitmap(bTemp.Width * c, bTemp.Height * c, bTemp.PixelFormat);

            double nXFactor = (Double)1 / (Double)c;
            double nYFactor = (Double)1 / (Double)c;

            for (int x = 0; x < output.Width; ++x)
                for (int y = 0; y < output.Height; ++y)
                    output.SetPixel(x, y, bTemp.GetPixel((int)(Math.Floor(x * nXFactor)), (int)(Math.Floor(y * nYFactor))));

            return output;
        }
    }
}
