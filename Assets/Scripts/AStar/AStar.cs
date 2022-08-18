using System;
using System.Collections;
using System.Collections.Generic;
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

        //current node is the first node. the start node
        Node currentNode = nodesDict[start];
        // 1. adding the starting node to the open list.
        openList.Add(currentNode);
        //2. runs through all neighbours.
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            { 
                //get the position
                Point neighborPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);
                // Debug.Log(neighborPos.x +", "+ neighborPos.y);

                if (LevelManager.Instance.InBounds(neighborPos) && LevelManager.Instance.Tiles[neighborPos].Walkable &&
                    neighborPos != currentNode.GridPosition) //not equal to self
                {
                    int gCost = 0;
                    // [14] [10] [14]
                    // [10] [x]  [10]
                    // [14] [10] [14]
                    if (Math.Abs(x-y) == 1)
                    {
                        gCost = 10; //staight
                    }
                    else
                    {
                        gCost = 14; //diagonal
                    }

                    // 3. get the node fron the dict list based on the position
                    Node Neighbor = nodesDict[neighborPos];
                    // Neighbor.TileRef.spriteRenderer.color = Color.black;
                    if (!openList.Contains(Neighbor))
                    {
                        openList.Add(Neighbor);
                    }
                    // 4. set the parent if the g,h,f values require to do so.
                    Neighbor.CalcValues(currentNode, gCost, nodesDict[goal]);
                }
            }
        }
        
        //5. move openlist to closed list. when we checked all the neighbors of the specific current node.
        openList.Remove(currentNode);
        closedList.Add(currentNode);

        //todo this is only for debugging remove later
        //finds the debugger object and runs the show debug path.
        GameObject.Find("AStarDebugger").GetComponent<AStarDebug>().DebugPath(openList, closedList);
    }
    
}
