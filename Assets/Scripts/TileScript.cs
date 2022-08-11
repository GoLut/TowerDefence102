using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; }

// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        //position on the grid (eg 1,1)
        this.GridPosition = gridPos;
        //the world coordinates Eg. (100 , 123)
        transform.position = worldPos;
        // set the random rotation
        transform.Rotate( 0,0,random90DegreesInterval(),Space.Self);
        
        //add every tile created to the dictionary located in the level manager.
        LevelManager.Instance.Tiles.Add(gridPos,this);
        
        //set the parent of the tile class. this cleans up the hyrarchy in unity. 
        transform.SetParent(parent);
    }

    private int random90DegreesInterval()
    {
        return Random.Range(0, 4) * 90;
    }

    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x, transform.position.y);
            // incase we use the top left  corner (not currently implemented because of random rotation of tiles.)
            //return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x/2), transform.position.y+ (GetComponent<SpriteRenderer>().bounds.size.y/2));

        }
    }
}
