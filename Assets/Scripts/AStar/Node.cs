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
    
}
