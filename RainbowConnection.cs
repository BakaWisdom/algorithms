using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS9
{
    class RainbowConnection
    {
        static void Main(string[] args)
        {
            int[] input = acquireInput();

            float output = evaluateInput(input);

            Console.WriteLine(output);

            Console.Read();
        }

        private static float evaluateInput(int[] input)
        {
            float output;
            float[] costChart = new float[input.Length];

            for (int i = 0; i < costChart.Length; i++)
            {
                costChart[i] = -1;
            }

            output = Penalty(input, 0, ref costChart);

            return output;
        }

        private static float Penalty(int[] map, int currentStop, ref float[] penaltyChart)
        {
            //base case
            if(map.Length - 1 == currentStop)
            {
                return 0;
            }

            float output = float.MaxValue;

            for (int nextStop = currentStop + 1; nextStop < map.Length; nextStop++)
            {
                int travelDistance = map[nextStop] - map[currentStop];
                float thisPenalty = (400 - travelDistance);

                //to speed up create an array for Penalty at each stop so we don't recalculate
                thisPenalty = thisPenalty * thisPenalty;

                if(penaltyChart[nextStop] == -1)
                {
                    penaltyChart[nextStop] = Penalty(map, nextStop, ref penaltyChart);
                }

                thisPenalty += penaltyChart[nextStop];

                if (thisPenalty < output)
                {
                    output = thisPenalty;
                }
            }

            return output;
        }

        private static int[] acquireInput()
        {
            string thisLine = Console.ReadLine();

            int stops = int.Parse(thisLine);
            int[] output = new int[stops+ 1];

            for (int i = 0; i < stops + 1 ; i++)
            {
                thisLine = Console.ReadLine();

                output[i] = int.Parse(thisLine);
            }

            return output;
        }
    }
}
