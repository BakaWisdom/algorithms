using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS12
{

    //Note I used Joe Zachary's starter code for this assignment.  some of the more complicated bits I rewrote the comments for to improve understanding of base code.

    class TravelingSalesMan
    {
        //min edge of each V
        private static int[] minEdges;

        // Number of vertices in graph
        private static int V;

        // Adjacency matrix for graph
        private static int[,] graph;

        // Cost of best solution found so far
        private static int bestSoFar = Int32.MaxValue;

        // Set of vertices that have already been visited
        private static HashSet<int> visited;

        private static int trueMinEdgeWeight = 0;

        /**
        * Solves the TSP Kattis problem.
*/
        public static void Main(string[] args)
        {
            // Read the graph
            V = Int32.Parse(Console.ReadLine().Trim());
            graph = new int[V, V];
            minEdges = new int[V];
            for (int i = 0; i < V; i++)
            {
                string[] tokens = Console.ReadLine().Trim().Split(' ');
                int minEdge = Int32.MaxValue;

                //loop to find minedgeweight of this node
                for (int j = 0; j < V; j++)
                {
                    graph[i, j] = Int32.Parse(tokens[j]);
                    if (i!=j)
                    {
                        minEdge = Math.Min(minEdge, Int32.Parse(tokens[j]));
                    }
                }

                minEdges[i] = minEdge;
                trueMinEdgeWeight = Math.Min(minEdge, trueMinEdgeWeight);
            }

            // Initially, only vertex 0 has been visited
            visited = new HashSet<int>();
            visited.Add(0);

            // Solve starting from vertex 0
            //TODO try starting with vertex containing shortest edge?
            TspSolve(0, 0, 0);

            // Display result
            Console.WriteLine(bestSoFar);

            Console.Read();
        }

        /**
        * Looks for solutions for the TSP problem from a given starting point. Updates bestSoFar with the
        * cost of any solution found that has a lower cost than bestSoFar. The parameters give the starting
        * point. The vertices in visited have already been incorporated into a path. The vertex most recently
        * visited is currentVertex. The cost of the path is currentTotal and its length is currentLength.
        * Assumes that the first vertex visited was vertex 0.
*/
        private static void TspSolve(int currentVertex, int currentTotal, int currentLength)
        {
            // similar to spanning tree check in class
            if (currentTotal + (V - currentLength) * trueMinEdgeWeight >= bestSoFar) return;

            // Only one edge remains to be added, which is the one back to 0 that closes the cycle.
            // Update bestSoFar accordingly.
            if (currentLength == V - 1)
            {
                bestSoFar = Math.Min(bestSoFar, currentTotal + graph[currentVertex, 0]);
            }

            // Try extending the current path using each unvisited vertex as the next vertex.
            else
            {
                //try checking cheapest edge first? TODO
                for (int vertex = 0; vertex < V; vertex++)
                {
                    if (!visited.Contains(vertex))
                    {
                        //TODO make an dictionary, key = min edge weights, storing num of that weight.  then update sorted list of keys?
                        visited.Add(vertex);
                        //TODO update lowest edge based on sorted list?
                        TspSolve(vertex, currentTotal + graph[currentVertex, vertex], currentLength + 1);
                        visited.Remove(vertex);
                        //TODO check if the vertex re-introduced has a lower edge weight
                    }
                }
            }
        }
    }
}
