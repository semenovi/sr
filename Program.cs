
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Emit;
using System.Drawing;
using sr;
using System;

List<string> order = new List<string>(5);
order.Add("-m");
order.Add("-e");
order.Add("-i");
order.Add("-p");
order.Add("-s");

List<string> helpArgs = new List<string>(5);
helpArgs.Add("interpolation, index");
helpArgs.Add("nearest, bilinear - for interpolation; psnr, ssim - for index");
helpArgs.Add("path to original image");
helpArgs.Add("image for comparison by index");
helpArgs.Add("interpolation scale");

if (args.Length == 0 || args[0] == "-h" || args[0] == "--help")
{
    hpc.start();
    cli.helpConsoleWrite(order, helpArgs);
    return;
}

List<string> arin = new List<string>(args);
List<string> pArgs = cli.parseArgs(arin, order);

string mode = pArgs[0];
string method = pArgs[1];
string image = pArgs[2];
string pImage = pArgs[3];
string scale = pArgs[4];

bool interpolationMode = (mode is "interpolation" or "ntr" or "n");
bool indexMode = (mode is "index" or "ndx" or "i");

bool bilinearMethod = (method is "bilinear" or "b");
bool nearestMethod = (method is "nearest" or "n");

bool psnrMethod = (method is "psnr" or "p");
bool ssimMethod = (method is "ssim" or "s");

hpc.start();

try
{
    string fn = image.Split(".")[0];
    string ext = image.Split(".")[1];

    if (ext != "bmp")
    {
        cli.ticksConsoleWrite("not bmp file: " + image);
        return;
    }

    var imgIn = new Bitmap(image);
    Bitmap b = new Bitmap(imgIn);
    cli.ticksConsoleWrite("image loaded: " + image);

    long st = 0;

    string outfn = ".";

    // interpolation

    if (interpolationMode)
    {
        if (bilinearMethod)
        {
            st = hpc.ticks;
            b = bilinear.r(imgIn, int.Parse(scale));
        }
        else if (nearestMethod)
        {
            st = hpc.ticks;
            b = nearest.r(imgIn, int.Parse(scale));
        }
        else
        {
            cli.ticksConsoleWrite("unknown method.");
            return;
        }

        long en = hpc.ticks - st;
        cli.ticksConsoleWrite(method + " scaling for " + scale + "x done in " + en.ToString() + " ticks");

        outfn = fn + "-" + method + "-" + scale + "x." + ext;

        if (ext == "bmp")
        {
            b.Save(outfn, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        cli.ticksConsoleWrite(method + " saved to file " + outfn);
    }

    // index

    else if (indexMode)
    {
        double o = 0.0;

        var imgPrimary = new Bitmap(pImage);
        cli.ticksConsoleWrite("image loaded: " + pImage);

        if (imgIn.Size != imgPrimary.Size)
        {
            cli.ticksConsoleWrite("WARNING! different size.");
            return;
        }

        if (psnrMethod)
        {
            st = hpc.ticks;
            o = psnr.r(imgIn, imgPrimary);
        }
        else if (ssimMethod)
        {
            st = hpc.ticks;
            o = ssim.r(imgIn, imgPrimary);
        }
        else
        {
            cli.ticksConsoleWrite("unknown method.");
            return;
        }

        long en = hpc.ticks - st;
        cli.ticksConsoleWrite(String.Format("{0} index is {1:0.0000000000000000}, done in {2} ticks", method, o, en));
    }
    else
    {
        cli.ticksConsoleWrite("unknown mode.");
        return;
    }
}
catch (ArgumentException)
{
    hpc.start();
    cli.ticksConsoleWrite("there was an argument error. Check the source code or input file.");
}