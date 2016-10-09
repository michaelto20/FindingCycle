using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindCycle
{
    class Program
    {
        static void Main(string[] args)
        {
            /*General idea:
              Run a BFS, you can find a cycle in one of two cases
                Case 1: Nodes x,y in Layer i are connected then we have a cycle
                Case 2: If node x in Layer i has an edge to two nodes in Layer i-1 then there is a cycle
                If cycle found print it out
             */

            // Set up adjacency list to represent nodes and edges, each node i is maintain in index position i and the array at that position
            // contains the nodes that i has edges to, assuming node numbers start at 0 and are all positive and consecutive
            List<List<int>> adjList = new List<List<int>>();

            //Ask user for node number and then for what edges it has
            int node;
            do
            {
                Console.Write("Enter a node. Add the nodes in sequential order starting at 0, enter -1 to finish: ");
                node = Convert.ToInt32(Console.ReadLine());
                if (node >= 0)
                {
                    Console.Write("Enter the edges separated with spaces: ");
                    //string[] temp = Console.ReadLine().Split();
                    //List<int> numArray = Array.ConvertAll(temp, elem => Convert.ToInt32(elem)).ToList();
                    //adjList[node] = numArray;
                    adjList.Add(Array.ConvertAll(Console.ReadLine().Split(), elem => Convert.ToInt32(elem)).ToList());
                }
            } while (node >= 0);

            /////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                  Run BFS                                                        //
            /////////////////////////////////////////////////////////////////////////////////////////////////////

            Queue<int> layers = new Queue<int>();
            bool[] discovered = new bool[adjList.Count()];
            int layerCount = 0;

            discovered[0] = true;
            layers.Enqueue(0);
            

            while(layers.Count() > 0)
            {
                Queue<int> nextLayer = new Queue<int>();
                while (layers.Count() > 0)
                {
                    int tempNode = layers.Dequeue();

                    foreach (int num in adjList[tempNode])
                    {
                        if (discovered[num] == false)      //haven't seen this node yet
                        {
                            discovered[num] = true;
                            nextLayer.Enqueue(num);
                        }
                        else
                        {
                            //found cycle
                        }
                    }
                    
                }
                layers = nextLayer;
                layerCount++;


            }


            Console.ReadKey();


            

            


        }
    }
}
