using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AStarDebug : MonoBehaviour
{
    private TileScript goalTile,startTile;

    [SerializeField] private GameObject arrowPrefab;

    //tile that is overlayed on the game for debug purposes.
    [SerializeField] private GameObject debugTilePrefab;
    
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
            AStar.GetPath(startTile.GridPosition, goalTile.GridPosition);
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
                        //for visual purposes create a overlaying debug tile
                        // startTile.spriteRenderer.color = new Color32(255, 132, 0, 255);
                        CreateDebugTile(startTile.WorldPosition, new Color32(255, 132, 0, 255));
                        startTile.Debugging = true;
                    }
                    else if (goalTile == null)
                    {
                        goalTile = tileHit;
                        //for visual purposes create a overlaying debug tile
                        // goalTile.spriteRenderer.color = new Color32(255, 0, 0, 255);
                        CreateDebugTile(goalTile.WorldPosition, new Color32(255, 0, 0, 255));
                        goalTile.Debugging = true;
                    }
                }
            }
        }
    }

    public void DebugPath(HashSet<Node> openList, HashSet<Node> closedList, Stack<Node> finalPath)
    {
        foreach (Node node in openList)
        {
            if (node.TileRef != startTile && node.TileRef != goalTile) //not the starting node and end node.
            {
                // node.TileRef.spriteRenderer.color = Color.cyan;
                CreateDebugTile(node.TileRef.WorldPosition, Color.cyan, node);
            }

            //overlaying the arrows to point to the parent nodes.
            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach (Node node in closedList)
        {
            if (node.TileRef != startTile && node.TileRef != goalTile && !finalPath.Contains(node)) //not the starting node and end node.
            {
                Debug.Log("running entry from closed list.");
                CreateDebugTile(node.TileRef.WorldPosition, Color.blue, node);
            }

            //overlaying the arrows to point to the parent nodes.
            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach (Node node in finalPath)
        {
            CreateDebugTile(node.TileRef.WorldPosition, Color.green, node);
        }
    }

    //the node is the parent, the position is the center of the arrow position.
    private void PointToParent(Node node, Vector2 position)
    {
        if (node.Parent != null)
        {
            GameObject arrow = (GameObject)Instantiate(arrowPrefab, position, quaternion.identity);
            arrow.GetComponent<SpriteRenderer>().sortingOrder = 30;
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

    private void CreateDebugTile(Vector3 woldPos, Color32 color, Node node = null)
    {
        GameObject debugTile = (GameObject) Instantiate(debugTilePrefab, woldPos, Quaternion.identity);

        //set the color equal to what we give it.
        debugTile.GetComponent<SpriteRenderer>().color = color;

        if (node != null)
        {
            //displaying the gscore on the debug tiles.
            DebugTile tmp = debugTile.GetComponent<DebugTile>();
            
            tmp.G.text += node.G;
            tmp.H.text += node.H;
            tmp.F.text += node.F;
        }

    }
}
