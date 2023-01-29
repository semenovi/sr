
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Emit;
using System.Drawing;
using srntr;
using System;


var p = new Dictionary<string, string>();


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
    Console.WriteLine("[" + hpc.UtcNow.Ticks + "] image loaded: " + p["i"]);

    long st = hpc.UtcNow.Ticks;
    Bitmap b = bilinear.r(imgIn, int.Parse(p["s"]));
    long en = hpc.UtcNow.Ticks - st;

    Console.WriteLine("[" + hpc.UtcNow.Ticks + "] bilinear scaling for " + p["s"] + "x done in " + en.ToString() + " ticks");
    
    b.Save(p["i"] + ".bl.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
    Console.WriteLine("[" + hpc.UtcNow.Ticks + "] bilinear saved to file");
}
catch (ArgumentException)
{
    Console.WriteLine("[" + hpc.UtcNow.Ticks + "] There was an error. Check the path to the image file.");
}