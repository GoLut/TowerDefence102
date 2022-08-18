using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
    
    //letting the unity engine know how to compare points to for example see if they are equal.
    public static bool operator == (Point first, Point second)
    {
        return first.X == second.X && first.Y == second.Y;
    }
    public static bool operator != (Point first, Point second)
    {
        return first.X != second.X || first.Y != second.Y;
    }
    public static Point operator -(Point first, Point second)
    {
        return new Point(first.X - second.X, first.Y - second.Y);
    }
        
}
