using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebug : MonoBehaviour
{
    private TileScript goalTile,startTile;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClickedTile();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AStar.GetPath(startTile.GridPosition);
        }
    }

    private void ClickedTile()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //we get a point on the screen over what the mouse is hovering
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                //obtain the tile script
                TileScript tileHit = hit.collider.GetComponent<TileScript>();
                //verify if the tile script is not null.
                if (tileHit != null)
                {
                    //if the start tile is null we match the found tile to the start tile
                    if (startTile == null)
                    {
                        startTile = tileHit;
                        //for visual purposes update the color.
                        startTile.spriteRenderer.color = new Color32(255, 132, 0, 255);
                        startTile.Debugging = true;
                    }
                    else if (goalTile == null)
                    {
                        goalTile = tileHit;
                        //for visual purposes update the color.
                        goalTile.spriteRenderer.color = new Color32(255, 0, 0, 255);
                        goalTile.Debugging = true;
                    }
                }
            }
        }
    }

    public void DebugPath(HashSet<Node> OpenList)
    {
        foreach (Node node in OpenList)
        {
            node.TileRef.spriteRenderer.color = Color.cyan;
        }
    }
    
}
