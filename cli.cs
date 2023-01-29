namespace sr
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class cli
    {
        public static List<string> parseArgs(List<string> input, List<string> order)
        {
            if (input.Count % 2 != 0)
            {
                throw new Exception("wrong args");
            }

            List<string> output = new List<string>(order.Count);
            for (int i = 0; i < order.Count; i++)
            {
                output.Add("");
            }


            foreach (string arg in input)
            {
                if (order.Contains(arg))
                {
                    int rightNumber = order.IndexOf(arg);
                    int wrongNumber = input.IndexOf(arg);
                    output[rightNumber] = input[wrongNumber + 1];
                }
            }
            return output;
        }

        public static void ticksConsoleWrite(string input)
        {
            Console.WriteLine(string.Format("[{0:0000000000000000}] {1}", hpc.ticks, input));
        }

        public static void helpConsoleWrite(List<string> order, List<string> helpForArgs)
        {
            Console.WriteLine("specify:");
            for (int i = 0; i < order.Count; i++)
            {
                Console.WriteLine("\t" + order[i] + " " + helpForArgs[i]);
            }
        }
    }
}
