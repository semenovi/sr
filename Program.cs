
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Emit;
using System.Drawing;
using Win32;

var p = new Dictionary<string, string>();
hpt pt = new hpt();
pt.Start();


if (args.Length != 6)
{
    Console.WriteLine("specify -m method, -i image to resize, -s scale");
    return;
}
p.Add("m", args[1]);
p.Add("i", args[3]);
p.Add("s", args[5]);

//Console.WriteLine(p["m"]);
//Console.WriteLine(p["i"]);
//Console.WriteLine(p["s"]);

try
{
    var imgIn = new Bitmap(p["i"]);
    
    int x, y;

    for (x = 0; x < imgIn.Width; x++)
    {
        for (y = 0; y < imgIn.Height; y++)
        {
            Color pixelColor = imgIn.GetPixel(x, y);
            Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
            imgIn.SetPixel(x, y, newColor);
        }
    }
    
    pt.Stop();
    Console.WriteLine("[" + pt.Duration + "] Pixel format: " + imgIn.PixelFormat.ToString());
}
catch (ArgumentException)
{
    pt.Stop();
    Console.WriteLine("[" + pt.Duration + "] There was an error. Check the path to the image file.");
}
