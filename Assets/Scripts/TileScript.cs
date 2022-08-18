using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; }
    public bool IsEmpty { get; private set; }

    //tile is full
    private Color32 tileFullColor = new Color32(255, 118, 118, 255);
    //tile is empty
    private Color32 tileEmptyColor = new Color32(96, 255, 90, 255);

    public SpriteRenderer spriteRenderer;
    
    public bool Debugging { get; set; }
    
    //if the tile is walkable by Entities
    public bool Walkable { get;  set; }

// Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent, string tileType)
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

        IsEmpty = true;
        //set the non walkability of non path tiles.
        if (int.Parse(tileType) == 0)
        {
            Walkable = false;
        }
        else
        {
            Walkable = true;
        }

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

    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedButton != null)
        {
            //colouring of the tile based on its current state.
            //ToDo (maybe extend with different types of entitys placed on the tile)
            
            if (IsEmpty && !Debugging)
            {
                ColorTile(tileEmptyColor);
            }
            else if (!IsEmpty && !Debugging) 
            {
                ColorTile(tileFullColor);
            }
            
            //if we click we place the tower if all conditions are met.
            if (Input.GetMouseButtonDown(0) && IsEmpty)
            { 
                PlaceTower();   
            }
        }
    }

    private void OnMouseExit()
    {
        if (!Debugging)
        {
            ColorTile(Color.white);
        }
    }

    private void PlaceTower()
    {
        Debug.Log("Placing at: "+ GridPosition.X + ", " + GridPosition.Y);
        //Instantiates a tower based on the last clicked on tower type known by the game manager.
        //places it at the tile location 
        GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedButton.TowerPrefab, transform.position, Quaternion.identity);
        
        //make sure the towers don't overlap towers below when placed above
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        
        //make the tile the parent of the tower. cleans up the unity scene. 
        tower.transform.SetParent(transform);
        
        // get all the buy functionality
        GameManager.Instance.BuyTower();

        //keep track if a tower is placed on the tile.
        IsEmpty = false;
        Walkable = false;
        
        ColorTile(Color.white);
    }

    private void ColorTile(Color32 newColor)
    {
        spriteRenderer.color = newColor;
    }
    
}
