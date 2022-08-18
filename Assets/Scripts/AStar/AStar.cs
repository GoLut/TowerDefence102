using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//made static so it can be used from other start
public static class AStar
{
    //instatniate the dicionary that cointains all nodes based on a point
    private static Dictionary<Point, Node> nodesDict;

    //create the nodes and fill the dictionary. 
    private static void CreateNodes()
    {
        nodesDict = new Dictionary<Point, Node>();
        // itterate over all know tiles in the level manager.
        foreach (TileScript tile in LevelManager.Instance.Tiles.Values)
        {
            nodesDict.Add(tile.GridPosition, new Node(tile));
        }
    }

    public static void GetPath(Point start, Point goal)
    {
        if (nodesDict == null)
        {
            CreateNodes();
        }
        
        //the open list is a hash set.
        HashSet<Node> openList = new HashSet<Node>();
        //closed list
        HashSet<Node> closedList = new HashSet<Node>();
        //the final path found by aStar
        Stack<Node> finalPath = new Stack<Node>();
        
        //current node is the first node. the start node
        Node currentNode = nodesDict[start];
        // 1. adding the starting node to the open list.
        openList.Add(currentNode);

        while (openList.Count > 0) //step 10
        {
            //2. runs through all neighbours.
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    //get the position
                    Point neighborPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);
                    // Debug.Log(neighborPos.x +", "+ neighborPos.y);

                    if (LevelManager.Instance.InBounds(neighborPos) &&
                        LevelManager.Instance.Tiles[neighborPos].Walkable &&
                        neighborPos != currentNode.GridPosition) //not equal to self
                    {
                        int gCost = 0;
                        // [14] [10] [14]
                        // [10] [x]  [10]
                        // [14] [10] [14]
                        if (Math.Abs(x - y) == 1)
                        {
                            gCost = 10; //staight
                        }
                        else
                        {
                            if (!ConnectedDiagonally(currentNode, nodesDict[neighborPos]))
                            {
                                continue;
                            }
                            gCost = 14; //diagonal
                        }

                        // 3. get the node fron the dict list based on the position
                        Node Neighbor = nodesDict[neighborPos];

                        if (openList.Contains(Neighbor))
                        {
                            if ((currentNode.G + gCost) < Neighbor.G)
                            {
                                // 4. set the parent if the g,h,f values require to do so.
                                Neighbor.CalcValues(currentNode, gCost, nodesDict[goal]); //9.4
                            }
                        }
                        //9.1 ignore nodes that are already in the closed list. 
                        else if (!closedList.Contains(Neighbor))
                        {
                            openList.Add(Neighbor); // 9.2
                            Neighbor.CalcValues(currentNode, gCost, nodesDict[goal]); //9.3
                        }
                    }
                }
            }

            //5. & 8 move openlist to closed list. when we checked all the neighbors of the specific current node.
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //7. find the node with the lowest F score
            if (openList.Count > 0)
            {
                //sorts the list by F value and selects the first value.
                currentNode = openList.OrderBy(n => n.F).First();
            }

            //find the end goal
            if (currentNode == nodesDict[goal])
            {
                Debug.Log("Path Found");
                while (currentNode.GridPosition != start)
                {
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;
                }
                break;
            }
        }
        //todo this is only for debugging remove later
        //finds the debugger object and runs the show debug path.
        GameObject.Find("AStarDebugger").GetComponent<AStarDebug>().DebugPath(openList, closedList, finalPath);
    }

    private static bool ConnectedDiagonally(Node currentNode, Node neighbor)
    {
        Point direction = neighbor.GridPosition - currentNode.GridPosition;

        //get the posisions to check for towers.
        Point first = new Point(currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y);
        Point second = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y + direction.Y);

        //check the points for towers using the information known in the level manager.
        if (LevelManager.Instance.InBounds(first) && !LevelManager.Instance.Tiles[first].Walkable)
        {
            return false;
        }
        if (LevelManager.Instance.InBounds(second) && !LevelManager.Instance.Tiles[second].Walkable)
        {
            return false;
        }
        //positions are free then we return true,
        return true;
    }
}
