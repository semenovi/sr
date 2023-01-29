
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Emit;
using System.Drawing;
using sr;
using System;

string mode = ".";
string method = ".";
string image = ".";
string pImage = ".";
string scale = ".";

if (args.Length < 1 || args[0] != "-m" || args[2] != "-e" || args[4] != "-i")
{
    Console.WriteLine("specify: \n\t-m mode\t\tinterpolation, index\n\t-e method\tnearest, bilinear - for interpolation\n\t\t\tpsnr, ssim - for index\n\t-i image\tpath\n\t-s scale\tonly for interpolation\n\t-primary\toriginal image, only for index");
    return;
}

if (args[1] == "interpolation" || args[1] == "ntr" || args[1] == "n")
{
    mode = "n";

    if (args.Length != 8 || args[0] != "-m" || args[2] != "-e" || args[4] != "-i" || args[6] != "-s")
    {
        Console.WriteLine("specify: \n\t-m mode\t\tinterpolation, index\n\t-e method\tnearest, bilinear - for interpolation\n\t\t\tpsnr, ssim - for index\n\t-i image\tpath\n\t-s scale\tonly for interpolation\n\t-primary\toriginal image, only for index");
        return;
    }

    method = args[3];
    image = args[5];
    scale = args[7];
}
else if (args[1] == "index" || args[1] == "ndx" || args[1] == "i")
{
    mode = "i";

    if (args.Length != 8 || args[0] != "-m" || args[2] != "-e" || args[4] != "-i" || args[6] != "-p")
    {
        Console.WriteLine("specify: \n\t-m mode\t\tinterpolation, index\n\t-e method\tnearest, bilinear - for interpolation\n\t\t\tpsnr, ssim - for index\n\t-i image\tpath\n\t-s scale\tonly for interpolation\n\t-primary\toriginal image, only for index");
        return;
    }

    method = args[3];
    image = args[5];
    pImage = args[7];
}

try
{
    string fn = image.Split(".")[0];
    string ext = image.Split(".")[1];

    if (ext != "bmp")
    {
        Console.WriteLine("not bmp file");
        return;
    }

    var imgIn = new Bitmap(image);
    Bitmap b = new Bitmap(imgIn);
    Console.WriteLine("[" + hpc.UtcNow.Ticks + "] image loaded: " + image);

    long st = hpc.UtcNow.Ticks;

    string outfn = ".";

    // interpolation

    if (mode == "n")
    {
        if (method == "bilinear" || method == "b")
        {
            b = bilinear.r(imgIn, int.Parse(scale));
        }
        else if (method == "nearest" || method == "n")
        {
            b = nearest.r(imgIn, int.Parse(scale));
        }
        else
        {
            Console.WriteLine("[" + hpc.UtcNow.Ticks + "] unknown method.");
            return;
        }

        long en = hpc.UtcNow.Ticks - st;
        Console.WriteLine("[" + hpc.UtcNow.Ticks + "] " + method + " scaling for " + scale + "x done in " + en.ToString() + " ticks");

        outfn = fn + "-" + method + "-" + scale + "x." + ext;

        if (ext == "bmp")
        {
            b.Save(outfn, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        Console.WriteLine("[" + hpc.UtcNow.Ticks + "] " + method + " saved to file " + outfn);
    }

    // index

    else if (mode == "i")
    {
        double o = 0.0;

        var imgPrimary = new Bitmap(pImage);
        Console.WriteLine("[" + hpc.UtcNow.Ticks + "] image loaded: " + pImage);


        if (imgIn.Size != imgPrimary.Size)
        {
            Console.WriteLine("[" + hpc.UtcNow.Ticks + "] WARNING! different size.");
            return;
        }

        if (method == "psnr" || method == "p")
        {
            o = psnr.r(imgIn, imgPrimary);
        }
        else if (method == "ssim" || method == "s")
        {
            o = ssim.r(imgIn, imgPrimary);
        }
        else
        {
            Console.WriteLine("[" + hpc.UtcNow.Ticks + "] unknown method.");
            return;
        }

        long en = hpc.UtcNow.Ticks - st;
        Console.WriteLine("[" + hpc.UtcNow.Ticks + "] " + method + " index is " + o + ", done in " + en.ToString() + " ticks");
    }
    else
    {
        Console.WriteLine("[" + hpc.UtcNow.Ticks + "] unknown mode.");
        return;
    }
}
catch (ArgumentException)
{
    Console.WriteLine("[" + hpc.UtcNow.Ticks + "] there was an argument error. Check the source code or input file.");
}