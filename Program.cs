
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Emit;
using System.Drawing;
using Win32;
using srntr;

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

try
{

    var imgIn = new Bitmap(p["i"]);
    Console.WriteLine("[" + pt.Duration + "] image loaded: " + p["i"]);

    double st = pt.Duration;
    double en = 0.0;
    Bitmap b = bilinear.r(imgIn, int.Parse(p["s"]));
    en = pt.Duration - st;
    Console.WriteLine("[" + pt.Duration + "] bilinear done in " + en.ToString());

    b.Save(p["i"] + ".bl.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
    Console.WriteLine("[" + pt.Duration + "] bilinear saved to file");
}
catch (ArgumentException)
{
    pt.Stop();
    Console.WriteLine("[" + pt.Duration + "] There was an error. Check the path to the image file.");
}
