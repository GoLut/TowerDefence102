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
}
