using System.Drawing;
using sr;

List<string> order = new List<string>(3);
order.Add("-m");
order.Add("-i");
order.Add("-c");

List<string> helpArgs = new List<string>(3);
helpArgs.Add("psnr, ssim");
helpArgs.Add("path to original image");
helpArgs.Add("image for comparison by index");

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
string pImage = pArgs[2];

bool psnrMethod = (method is "psnr" or "p");
bool ssimMethod = (method is "ssim" or "s");

hpc.start();

try
{
    string fn = image.Split(".")[0];

    // shit
    Image img = Image.FromFile(image);
    Bitmap imgIn = new Bitmap(img);
    Bitmap b = new Bitmap(imgIn);

    cli.ticksConsoleWrite("image loaded: " + image);

    long st = 0;

    string outfn = ".";


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
catch (ArgumentException)
{
    hpc.start();
    cli.ticksConsoleWrite("there was an argument error. Check the source code or input file.");
}