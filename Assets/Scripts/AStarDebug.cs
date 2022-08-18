using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AStarDebug : MonoBehaviour
{
    private TileScript goalTile,startTile;

    [SerializeField] private GameObject arrowPrefab;
    
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
            if (node.TileRef != startTile) //not the starting node.
            {
                node.TileRef.spriteRenderer.color = Color.cyan;
            }
            //overlaying the arrows to point to the parent nodes.
            PointToParent(node, node.TileRef.WorldPosition);
        }
    }

    //the node is the parent, the position is the center of the arrow position.
    private void PointToParent(Node node, Vector2 position)
    {
        if (node.Parent != null)
        {
            GameObject arrow = (GameObject)Instantiate(arrowPrefab, position, quaternion.identity);

            //right
            if ((node.GridPosition.X < node.Parent.GridPosition.X) &&
                (node.GridPosition.Y == node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            //top right.
            else if ((node.GridPosition.X < node.Parent.GridPosition.X) &&
                (node.GridPosition.Y > node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 45);
            }
            //up.
            else if ((node.GridPosition.X == node.Parent.GridPosition.X) &&
                     (node.GridPosition.Y > node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 90);
            }
            //top left.
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) &&
                     (node.GridPosition.Y > node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 135);
            }
            
            //left.
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) &&
                     (node.GridPosition.Y == node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            //bottom left.
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) &&
                     (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 225);
            }
            //bottom.
            else if ((node.GridPosition.X == node.Parent.GridPosition.X) &&
                     (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 270);
            }
            //bottom right.
            else if ((node.GridPosition.X < node.Parent.GridPosition.X) &&
                     (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 315);
            }
        }
    }
}
