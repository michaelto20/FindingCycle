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

            //read in file
            Console.Write("Enter file path with graph: ");
            string path = Console.ReadLine();

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string line;
            int count = 0;

            //parse file into adjacency list
            while((line=file.ReadLine()) != null)
            {
                int[] tokenized = Array.ConvertAll(line.Trim().Split(), elem => Convert.ToInt32(elem));
                if(count != 0)
                {
                    List<int> nodes = new List<int>();
                    for (int i = 0; i < tokenized.Length; i++)
                    {
                        
                        // for each i, the node number is i-1
                        if (tokenized[i] == 1 && i != 0)
                        {
                            nodes.Add(i - 1);
                        }
                        
                    }
                    adjList.Add(nodes);
                }
                count++;
            }
            
            /////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                  Run BFS                                                        //
            /////////////////////////////////////////////////////////////////////////////////////////////////////

            Queue<int> layers = new Queue<int>();
            bool[] discovered = new bool[adjList.Count()];
            List<int> visited = new List<int>();

            //ith index value is the ith node, at that position is the path to get from root to ith node
            //initialize the 
            List<int>[] paths = new List<int>[adjList.Count()];
            paths[0] = new List<int> { -1 };
            int layerCount = 0;
            bool found = false;

            discovered[0] = true;
            layers.Enqueue(0);
            

            while (layers.Count() > 0 && !found)
            {
                Queue<int> nextLayer = new Queue<int>();
                while (layers.Count() > 0 && !found)
                {
                    int currentNode = layers.Dequeue();
                    visited.Add(currentNode);
                    for(int i = 0; i < adjList[currentNode].Count(); i++)
                    {
                        int num = adjList[currentNode][i];
                        if (layers.Contains(num))
                        {
                            // return full path of cycle
                            Console.WriteLine("Cycle found.  Here is it's path.");
                            foreach(int n in analyzeCycle(paths,num,currentNode))
                            {
                                Console.Write(n + " ");
                            }
                            found = true;
                            break;
                            //found cycle
                            //Console.WriteLine("Found cycle between node: " + currentNode + " and " + num);
                        }
                        if (discovered[num] == false)      //haven't seen this node yet
                        {
                            //do I need to initailize the inner array?
                            //paths[num] = new List<int>();
                            if(paths[currentNode][0] == -1)   //node 0 is initialized without a path, indicated with -1
                            {
                                paths[num] = new List<int> { currentNode};
                            }
                            else
                            {
                                List<int> temp = new List<int>();
                                temp.AddRange(paths[currentNode]);
                                paths[num] = temp;                          // add the path of the node that just found me
                                paths[num].Add(currentNode);                // now add the node that found me, so now my path is from the root 

                                //check if newly found node completes a cycle
                                //see if it's edges connect to an edge in the current layer
                                for(int j = 0; j < adjList[num].Count(); j++)
                                {
                                    if (layers.Contains(adjList[num][j]))
                                    {
                                        int position = layers.First(elem => elem == adjList[num][j]);
                                        //cycle found
                                        found = true;
                                        // return full path of cycle
                                        Console.WriteLine("Cycle found.  Here is it's path.");
                                        foreach (int n in analyzeCycle(paths, num, position))
                                        {
                                            Console.Write(n + " ");
                                        }
                                        break;
                                    }
                                }

                                //nodes with edges to themselves are a cycle, catch those
                                if (adjList[num].Contains(num))
                                {
                                    // return full path of cycle
                                    Console.WriteLine("Cycle found.  Here is it's path.");
                                    Console.Write(num + " " + num);
                                    found = true;
                                    break;
                                }
                            }
                            discovered[num] = true;
                            nextLayer.Enqueue(num);
                        }
                        else
                        {
                            //this is here just to catch the special case of the root looping to itself
                            if (adjList[num].Contains(num))
                            {
                                // return full path of cycle
                                Console.WriteLine("Cycle found.  Here is it's path.");

                                Console.Write(num + " " + num);

                                found = true;
                                break;
                            }
                        }
                    }
                                        
                }
                layers = nextLayer;
                layerCount++;


            }
            if (!found)
            {
                Console.WriteLine("No cylces in this graph.");
            }


            Console.ReadKey();

        }

        private static List<int> analyzeCycle(List<int>[] paths, int num, int currentNode)
        {
            List<int> path1 = paths[num];
            List<int> path2 = paths[currentNode];
            List<int> cyclePath = new List<int>();
            int branch = 0;

            for (int i = 0; i < path1.Count() && i < path2.Count(); i++)
            {
                if (path1[i] == path2[i])
                {
                    //found branch in paths
                    branch = i;
                }
            }


            for (int i = branch; i < path1.Count(); i++)
            {
                cyclePath.Add(path1[i]);
            }
            cyclePath.Add(num);
            cyclePath.Add(currentNode);
            for (int i = path2.Count() - 1; i >= branch; i--)
            {

                cyclePath.Add(path2[i]);

            }


            return cyclePath;
        }
    }
}

//C:\Users\Michael Townsend\Desktop\graph.txt
/*
 Test Case 1: Cycle between 0,1,2,0
  0 1 2 3 4 5 6 7
0 0 1 1 0 0 0 0 0
1 1 0 1 1 0 0 0 0
2 1 1 0 0 1 0 1 0
3 0 1 0 0 1 0 0 0
4 0 0 1 1 0 0 0 0
5 0 0 0 0 1 0 0 0
6 0 0 1 0 0 0 0 1
7 0 0 1 0 0 0 1 0

    Test Case 2: Cycle between 0,2,4,3,1,0
  0 1 2 3 4 5 6 7
0 0 1 1 0 0 0 0 0
1 1 0 0 1 0 0 0 0
2 1 0 0 0 1 0 1 0
3 0 1 0 0 1 0 0 0
4 0 0 1 1 0 0 0 0
5 0 0 0 0 1 0 0 0
6 0 0 1 0 0 0 0 1
7 0 0 1 0 0 0 1 0

     Test Case 3: Cycle between 2,7,6,2
  0 1 2 3 4 5 6 7
0 0 1 1 0 0 0 0 0
1 1 0 0 1 0 0 0 0
2 1 0 0 0 1 0 1 1
3 0 1 0 0 0 0 0 0
4 0 0 1 0 0 0 0 0
5 0 0 0 0 1 0 0 0
6 0 0 1 0 0 0 0 1
7 0 0 1 0 0 0 1 0

    Test Case 4: Cycle between 0,1,3,2,0
  0 1 2 3 4 5 
0 0 1 1 0 0 0
1 1 0 0 1 4 0
2 1 0 0 1 0 1
3 0 1 1 0 0 0
4 0 1 0 0 0 0
5 0 0 1 0 0 0

    Test Case 5: Cycle on node 7 to itself
  0 1 2 3 4 5 6 7
0 0 1 1 0 0 0 0 0
1 1 0 0 1 0 0 0 0
2 1 0 0 0 1 0 1 1
3 0 1 0 0 0 0 0 0
4 0 0 1 0 0 0 0 0
5 0 0 0 0 1 0 0 0
6 0 0 1 0 0 0 0 0
7 0 0 1 0 0 0 0 1
    */
