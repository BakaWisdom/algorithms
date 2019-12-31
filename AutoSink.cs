using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q4_5
{
    class AutoSink
    {
        static void Main(string[] args)
        {
            Dictionary<string, List<string>> highways;
            Dictionary<string, int[]> cities;
            string[][] trips = AcquireInput(out cities, out highways);
            int[] result = EvaluateInput(cities, highways, trips);

            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] < 0)
                {
                    Console.WriteLine("NO");
                }
                else
                {
                    Console.WriteLine(result[i]);
                }
            }

            Console.ReadLine();
        }

        /// <summary>
        /// evaluates input as desired from the assignment.
        /// </summary>
        /// <param name="cities"> holds cost, topographical location, and trip cost in its array in that order</param>
        /// <param name="highways"></param>
        /// <param name="trips"></param>
        /// <returns></returns>
        private static int[] EvaluateInput(Dictionary<string, int[]> cities, Dictionary<string, List<string>> highways, string[][] trips)
        {
            //the int[] in cities holds the following information (0, cost) prexisting, (1, post number) currently unassigned.

            int[] output = new int[trips.GetLength(0)];
            string[] reverseTopographicalOrder = FindReverseTopographicalOrder(cities, highways);

            for (int tripNum = 0; tripNum < trips.GetLength(0); tripNum++)
            {
                string startLocation = trips[tripNum][0];
                string destination = trips[tripNum][1];

                //early exit check first
                if (startLocation.Equals(destination))
                {
                    output[tripNum] = 0;
                    continue;
                }

                //reseting cost
                foreach (string city in cities.Keys)
                {
                    cities[city][2] = int.MaxValue;
                }

                cities[startLocation][2] = 0;

                while (Updatecosts(cities, highways, reverseTopographicalOrder, startLocation, destination))
                {
                    //while loop exists to run the method in its condition
                }

                if (cities[destination][2] == int.MaxValue)
                {
                    output[tripNum] = -1;
                }
                else
                {
                    output[tripNum] = cities[destination][2];
                }
            }

            return output;
        }

        private static bool Updatecosts(Dictionary<string, int[]> cities, Dictionary<string, List<string>> highways, string[] reverseTopographicalOrder, string startingCity, string destination)
        {
            bool updateSuccessful = false;

            foreach (string city in reverseTopographicalOrder.Skip<string>(cities[startingCity][1]))
            {
                
                if (cities[city][2] == int.MaxValue)
                {
                    continue;
                }
                


                if (highways.ContainsKey(city))
                {
                    foreach (string subDestination in highways[city])
                    {
                        int newCost = cities[subDestination][0] + cities[city][2];
                        if (cities[subDestination][2] <= newCost)
                        {
                            continue;
                        }
                        else
                        {
                            updateSuccessful = true;
                            cities[subDestination][2] = newCost;
                        }
                    }
                }
            }

            if (cities[destination][2]== int.MaxValue)
            {
                return false;
            }

            return updateSuccessful;
        }

        private static string[] FindReverseTopographicalOrder(Dictionary<string, int[]> cities, Dictionary<string, List<string>> highways)
        {
            string[] outputArray = new string[cities.Count];
            int backCount = 0;

            foreach (string city in cities.Keys)
            {
                if (cities[city][1] == -2)
                {
                    cities[city][1] = -1;

                    TopographySearch(ref outputArray, ref backCount, city, cities, highways);

                    int position = cities.Count - ++backCount;

                    outputArray[position] = city;
                    cities[city][1] = position;
                }
            }

            return outputArray;
        }

        private static void TopographySearch(ref string[] outputArray, ref int backCount, string city, Dictionary<string, int[]> cities, Dictionary<string, List<string>> highways)
        {
            if (highways.ContainsKey(city))
            {
                foreach (string destination in highways[city])
                {
                    if (cities[destination][1] == -2)
                    {
                        //this marks a city as visited to avoid infinite loops without giving it a real value.
                        cities[destination][1] = -1;
                        TopographySearch(ref outputArray, ref backCount, destination, cities, highways);

                        int position = cities.Count - ++backCount;

                        outputArray[position] = destination;
                        cities[destination][1] = position;
                    }
                }
            }
        }

        public static string[][] AcquireInput(out Dictionary<string, int[]> outputCities, out Dictionary<string, List<string>> outputHighways)
        {
            string thisLine;
            outputCities = new Dictionary<string, int[]>();
            outputHighways = new Dictionary<string, List<string>>();
            string[][] trips;

            thisLine = Console.ReadLine();
            int numCities = int.Parse(thisLine);

            for (int i = 0; i < numCities; i++)
            {
                thisLine = Console.ReadLine();
                string[] lineInfo = thisLine.Split(' ');
                int[] cityData = new int[3];

                //initiallizing city data
                cityData[0] = int.Parse(lineInfo[1]);
                cityData[1] = -2;
                
                outputCities.Add(lineInfo[0], cityData);
            }

            thisLine = Console.ReadLine();
            int numHighways = int.Parse(thisLine);

            for (int i = 0; i < numHighways; i++)
            {
                thisLine = Console.ReadLine();
                string[] roadInfo = thisLine.Split(' ');

                if (!outputHighways.ContainsKey(roadInfo[0]))
                {
                    outputHighways.Add(roadInfo[0], new List<string>());
                }

                outputHighways[roadInfo[0]].Add(roadInfo[1]);
            }

            thisLine = Console.ReadLine();
            int numTrips = int.Parse(thisLine);
            trips = new string[numTrips][];

            for (int i = 0; i < numTrips; i++)
            {
                thisLine = Console.ReadLine();
                string[] tripPlan = thisLine.Split(' ');

                trips[i] = tripPlan;
            }

            return trips;

        }
    }
}
