using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS6
{
    class Dijkstra
    {
        static void Main(string[] args)
        {
            LinkedList<Dungeon> input;

            input = AcquireInput();

            double[] results = EvaluateInput(input);

            //TODO bad format
            foreach (double answer in results)
            {
                string formattedAnswer = answer.ToString("0.0000");


                Console.WriteLine(formattedAnswer);
            }
            

            Console.WriteLine();

            Console.Read();
        }

        private static double[] EvaluateInput(LinkedList<Dungeon> dungeonMaps)
        {
            double[] output = new double[dungeonMaps.Count];
            int dungeonNumber = 0;

            foreach (Dungeon thisDungeon in dungeonMaps)
            {
                PriorityQueue path = new PriorityQueue(thisDungeon.map.Length);
                int exit = thisDungeon.map.Length - 1;

                if(exit < 1)
                {
                    output[dungeonNumber++] = 0;
                    continue;
                }

                path.insertOrChange(0, 1);

                while(!path.IsEmpty())
                {
                    Node youAreHere = path.DeleteMax();

                    if (youAreHere.vertex == exit)
                    {
                        output[dungeonNumber++] = youAreHere.weight;
                        break;
                    }

                    foreach (int destination in thisDungeon.map[youAreHere.vertex])
                    {
                        path.insertOrChange(destination, youAreHere.weight * thisDungeon.traps[youAreHere.vertex, destination]);
                    }
                }
            }

            return output;
        }

        public static LinkedList<Dungeon> AcquireInput()
        {
            string thisLine;
            //linked list of dungeons.
            //the double 1st has the node (intersection) the 2nd is the destination node and the double is the weight.
            LinkedList<Dungeon> dungeonmaps = new LinkedList<Dungeon>();

            thisLine = Console.ReadLine();
            while (!string.IsNullOrEmpty(thisLine))
            {
                string[] description = thisLine.Split(' ');

                int vertex = int.Parse(description[0]);
                int edges = int.Parse(description[1]);

                if (vertex == 0 && edges ==0)
                {
                    break;
                }

                LinkedList<int>[] map = new LinkedList<int>[vertex];
                double[,] traps = new double[vertex,vertex];

                //code modelled after given example code in kattis to ensure reading/writing is done properly.
                for (int i = 0; i < edges; i++)
                {
                    thisLine = Console.ReadLine();
                    int[] corridor = new int[2];
                    string[] rawValues = thisLine.Split(' ');

                    for (int j = 0; j < 2; j++)
                    {
                        corridor[j] = int.Parse(rawValues[j]);
                    }

                    if (corridor[0] == corridor[1])
                    {
                        continue;
                    }

                    if (map[corridor[0]] == null)
                    {
                        map[corridor[0]] = new LinkedList<int>();
                    }
                    if (map[corridor[1]] == null)
                    {
                        map[corridor[1]] = new LinkedList<int>();
                    }

                    map[corridor[0]].AddLast(corridor[1]);
                    map[corridor[1]].AddLast(corridor[0]);

                    traps[corridor[0], corridor[1]] = double.Parse(rawValues[2]);
                    traps[corridor[1], corridor[0]] = double.Parse(rawValues[2]);
                }

                dungeonmaps.AddLast(new Dungeon(map, traps));
                thisLine = Console.ReadLine();
            }
            
            return dungeonmaps;

        }
    }

    internal class PriorityQueue
    {
        public Node[] core;
        public int size;
        int[] mapOfVertices;

        public PriorityQueue(int numVertex)
        {
            core = new Node[numVertex];
            mapOfVertices = new int[numVertex];
            for (int i = 0; i < numVertex; i++)
            {
                mapOfVertices[i] = -1;
            }
            size = 0;
        }

        public bool IsEmpty()
        {
            //return ReferenceEquals(core[0], null);
            return size == 0;
        }

        /// <summary>
        /// returns true if the heap was changed
        /// false if no changes were made because the weight is lower or the node has already left the heap
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public bool insertOrChange(int vertex, double weight)
        {
            if (mapOfVertices[vertex]==-1)
            {
                mapOfVertices[vertex] = size;
                core[size] = new Node(vertex, weight);

                size++;

                HeapifyUp(vertex);
                HeapifyDown(vertex);

                return true;
            }
            else if(mapOfVertices[vertex] >= 0)
            {
                double previousWeight = core[mapOfVertices[vertex]].weight;
                if (weight > previousWeight)
                {
                    core[mapOfVertices[vertex]].weight = weight;

                    HeapifyUp(vertex);
                    HeapifyDown(vertex);

                    return true;
                }
            }

            return false;
        }

        private void HeapifyUp(int vertex)
        {
            int currentLocation = mapOfVertices[vertex];
            while (currentLocation != 0)
            {
                int parent = (currentLocation - 1) / 2;
                if(core[parent].weight < core[currentLocation].weight)
                {
                    Node temp = core[parent];
                    core[parent] = core[currentLocation];
                    core[currentLocation] = temp;

                    mapOfVertices[vertex] = parent;
                    mapOfVertices[temp.vertex] = currentLocation;

                    currentLocation = parent;
                }
                else
                {
                    break;
                }
            }
        }

        private void HeapifyDown(int vertex)
        {
            int currentLocation = mapOfVertices[vertex];
            int lChild = 2 * currentLocation + 1;
            int rChild = 2 * currentLocation + 2;

            if (currentLocation < 0 || lChild>= core.Length || ReferenceEquals(core[lChild],null))
            {
                return;
            }

            if (rChild >= core.Length || ReferenceEquals(core[rChild], null) || core[lChild].weight > core[rChild].weight)
            {
                int biggestKid = lChild;

                if(biggestKid < 0)
                {
                    return;
                }

                if (core[biggestKid].weight > core[currentLocation].weight)
                {
                    Node temp = core[biggestKid];
                    core[biggestKid] = core[currentLocation];
                    core[currentLocation] = temp;

                    mapOfVertices[vertex] = biggestKid;
                    mapOfVertices[temp.vertex] = currentLocation;

                    HeapifyDown(vertex);
                }
            }
            else
            {
                int biggestKid = rChild;

                if (biggestKid < 0)
                {
                    return;
                }

                if (core[biggestKid].weight > core[currentLocation].weight)
                {
                    Node temp = core[biggestKid];
                    core[biggestKid] = core[currentLocation];
                    core[currentLocation] = temp;

                    mapOfVertices[vertex] = biggestKid;
                    mapOfVertices[temp.vertex] = currentLocation;

                    HeapifyDown(vertex);
                }
            }
        }

        public Node DeleteMax()
        {
            Node output = core[0];
            mapOfVertices[output.vertex] = -2; //-2 is the value of a node that already left queue
            size--;

            if (!this.IsEmpty())
            {
                core[0] = core[size];
                core[size] = null;
                mapOfVertices[core[0].vertex] = 0;
            }

            HeapifyDown(core[0].vertex);

            return output;
        }
    }

    internal class Node
    {
        public int vertex;
        public double weight;

        public Node(int vertex, double weight)
        {
            this.vertex = vertex;
            this.weight = weight;
        }
    }

    internal class Dungeon
    {
        /// <summary>
        /// array of vertexes holding all edges.
        /// </summary>
        public LinkedList<int>[] map;
        /// <summary>
        /// the double 1st has the node (intersection) the 2nd is the destination node and the double is the weight.
        /// </summary>
        public double[,] traps;

        public Dungeon(LinkedList<int>[] map, double[,] traps)
        {
            this.map = map;
            this.traps = traps;
        }
    }
}
