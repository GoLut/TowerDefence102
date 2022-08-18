using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Node
{

    public Point GridPosition { get; private set; }
    
    public TileScript TileRef { get; private set; }

    public Node(TileScript tileRef)
    {
        this.TileRef = tileRef;
        this.GridPosition = TileRef.GridPosition;
    }
    
    public Node Parent { get; private set; }

    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }

    //used to calc the g, h and F values.
    public void CalcValues(Node parent, int gCost, Node GoalPosition)
    {
        this.Parent = parent;
        this.G = gCost + parent.G;//gscore +parent g score. 
        this.H = (Math.Abs(GridPosition.X - GoalPosition.GridPosition.X) +
                 Math.Abs(GoalPosition.GridPosition.Y - GridPosition.Y)) * 10;
        this.F = G + H;
    }
    
}
