using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS11
{
    class Spiderman
    {
        static void Main(string[] args)
        {
            List<int[]> input = AcquireInput();

            string[] output = EvaluateInput(input);


            foreach (string workout in output)
            {
                Console.WriteLine(workout);
            }

            Console.ReadLine();
        }

        private static string[] EvaluateInput(List<int[]> input)
        {
            string[] results = new string[input.Count];

            //change to for loop
            int i = 0;
            foreach (int[] workSet in input)
            {
                //setting up cache for this particular workset
                Dictionary<int, Tuple<int, string>>[] cache = new Dictionary<int, Tuple<int, string>>[workSet.Length];
                for (int j = 0; j < workSet.Length; j++)
                {
                    cache[j] = new Dictionary<int, Tuple<int, string>>();
                }

                //calculate workout based on work set zeros are to indicate starting position
                WorkOutCalculator(0, 0, workSet, ref cache);

                //adding result of the calculator into results.
                results[i++] = cache[0][0].Item2;
            }

            return results;

        }

        private static int WorkOutCalculator(int myIndex, int myHeight, int[] workSet, ref Dictionary<int, Tuple<int, string>>[] cache)
        {
            //checks if the calculation has already been made
            if (cache[myIndex].ContainsKey(myHeight))
            {
                return cache[myIndex][myHeight].Item1;
            }

            //base case
            if (myIndex == workSet.Length - 1)
            {
                if (myHeight - workSet[myIndex] == 0)
                {
                    cache[myIndex].Add(myHeight, new Tuple<int, string>(myHeight, "D"));
                    return myHeight;
                }
                else
                {
                    //20000 calculated to be well below maxint to avoid overflow if pending on implimentation to but also high enough that it should necer be selected as a max height option.
                    cache[myIndex][myHeight] = new Tuple<int, string>(20000, "IMPOSSIBLE");
                    return 20000;
                }
            }

            //general case
            //calculates new next heights
            int upperHeight = myHeight + workSet[myIndex];
            int lowerHeight = myHeight - workSet[myIndex];

            //going up is always an option so it will check to max height if I go up
            int myMaxHeight = WorkOutCalculator(myIndex + 1, upperHeight, workSet, ref cache);

            //checks if the my height combined with the penalty of going up is worse than what was found
            if (myHeight + workSet[myIndex] > myMaxHeight)
            {
                myMaxHeight = myHeight + workSet[myIndex];
            }

            //checks if going down is a valid option
            if (lowerHeight >= 0)
            {
                //calculates lower maxheight
                int lowerMaxHeight = WorkOutCalculator(myIndex + 1, lowerHeight, workSet, ref cache);

                //checks if lower or upper is better TODO check if or equal is wrong
                if (lowerMaxHeight <= myMaxHeight)
                {
                    //checks to see we reached an impossible workout
                    if (lowerMaxHeight >= 20000)
                    {
                        cache[myIndex][myHeight] = new Tuple<int, string>(lowerMaxHeight, cache[myIndex + 1][lowerHeight].Item2);
                        return lowerMaxHeight;
                    }

                    cache[myIndex][myHeight] = new Tuple<int, string>( lowerMaxHeight, "D" + cache[myIndex + 1][lowerHeight].Item2);
                    return lowerMaxHeight;
                }
            }

            //if the upper solution was better or lower heigher height is negative we now check the upper solution

            //checks for impossible workout
            if (myMaxHeight >= 20000)
            {
                cache[myIndex][myHeight] = new Tuple<int, string>(myMaxHeight, cache[myIndex + 1][upperHeight].Item2);
                return myMaxHeight;
            }

            

            cache[myIndex].Add(myHeight, new Tuple<int, string>(myMaxHeight, "U" + cache[myIndex + 1][upperHeight].Item2));
            return myMaxHeight;
        }

        private static List<int[]> AcquireInput()
        {
            string thisLine = Console.ReadLine();

            int workouts = int.Parse(thisLine);
            List<int[]> output = new List<int[]>();

            for (int i = 0; i < workouts; i++)
            {
                thisLine = Console.ReadLine();

                int buildings = int.Parse(thisLine);

                thisLine = Console.ReadLine();

                string[] rawHeights = thisLine.Split(' ');

                int[] heights = new int[buildings];

                for (int j = 0; j < buildings; j++)
                {
                    heights[j] = int.Parse(rawHeights[j]);
                }

                output.Add(heights);
            }

            return output;
        }
    }
}
