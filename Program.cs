﻿
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Emit;
using System.Drawing;
using srntr;
using System;

if (args.Length != 6)
{
    Console.WriteLine("specify -m method, -i image to resize, -s scale");
    return;
}

string m = args[1];
string i = args[3];
string s = args[5];

try
{
    string fn = i.Split(".")[0];
    string ext = i.Split(".")[1];

    if (ext != "bmp")
    {
        Console.WriteLine("not bmp file");
        return;
    }

    var imgIn = new Bitmap(i);
    Bitmap b = new Bitmap(imgIn);
    Console.WriteLine("[" + hpc.UtcNow.Ticks + "] image loaded: " + i);

    if (m == "bilinear" || m == "b")
    {
        long st = hpc.UtcNow.Ticks;
        b = bilinear.r(imgIn, int.Parse(s));
        long en = hpc.UtcNow.Ticks - st;
        Console.WriteLine("[" + hpc.UtcNow.Ticks + "] bilinear scaling for " + s + "x done in " + en.ToString() + " ticks");
    }
    else
    {
        Console.WriteLine("unknown method.");
        return;
    }

    string outfn = fn + "-" + m + "-" + s + "x." + ext;

    if (ext == "bmp")
    {
        b.Save(outfn, System.Drawing.Imaging.ImageFormat.Bmp);
    }
    
    Console.WriteLine("[" + hpc.UtcNow.Ticks + "] bilinear saved to file " + outfn);
}
catch (ArgumentException)
{
    Console.WriteLine("[" + hpc.UtcNow.Ticks + "] there was an argument error. Check the source code or input file.");
}