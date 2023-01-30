using System.Drawing;
using sr;

List<string> order = new List<string>(3);
order.Add("-m");
order.Add("-i");
order.Add("-s");

List<string> helpArgs = new List<string>(3);
helpArgs.Add("nearest, bilinear");
helpArgs.Add("path to original image");
helpArgs.Add("interpolation scale");

if (args.Length == 0 || args[0] == "-h" || args[0] == "--help")
{
    hpc.start();
    cli.helpConsoleWrite(order, helpArgs);
    return;
}

List<string> arin = new List<string>(args);
List<string> pArgs = cli.parseArgs(arin, order);

string method = pArgs[0];
string image = pArgs[1];
string scale = pArgs[2];

bool bilinearMethod = (method is "bilinear" or "b");
bool nearestMethod = (method is "nearest" or "n");

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
catch (ArgumentException)
{
    hpc.start();
    cli.ticksConsoleWrite("there was an argument error. Check the source code or input file.");
}