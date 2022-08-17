using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;
using System;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameObject[] groundTilePrefabs;

    [SerializeField] private CameraMovement cameraMovement;
    
    private Point startSpawnPoint, endSpawnPoint;
    
    [SerializeField] private GameObject starSpawnPrefab, endSpawnPrefab;
    
    public Dictionary<Point, TileScript> Tiles { get; set; }

    [SerializeField] private Transform map;

    private Point mapSize;
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //property tile size that calculates the tile size and returns the float.
    public float TileSize
    {
        get { return groundTilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    private string[] ReadLevelText()
    {  
        //import level from text document
        TextAsset rawData = Resources.Load("Level1") as TextAsset;
        //remove the /n symbols
        string data = rawData.text.Replace(Environment.NewLine, string.Empty);

        //split on the end of line characters
        return data.Split('-');
    }
    private void GenerateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();
        
        //We set how the map is generated based on this.
        string[] mapData = ReadLevelText();

        //estimates the map size based on the tile array entries given in mapData
        int mapX = (mapData[0].ToCharArray().Length) ; //-1 because of end of line char
        int mapY = mapData.Length -1;
        
        Debug.Log("X:" +(mapX).ToString() + ", y:" + (mapY).ToString());
        
        //set the map size to be known by other objects if needed.
        mapSize = new Point(mapX, mapY);

        //temp value for the maximum tile location
        Vector3 maxTile = Vector3.zero;

        //set the generation point of the level to be top left.
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        
        //generate the actual level and all its tiles
        for (int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();
            // Debug.Log(y.ToString());
            for (int x = 0; x < mapX; x++)
            {
                // Debug.Log("inline X:" +(x).ToString() + ", y:" + (y).ToString());
                PlaceTile(newTiles[x].ToString(), x, y, worldStart); 
                
            }
        }
        //make the location of the last tile known to the camera. The camera can then set its view boundary
        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;
        cameraMovement.SetLimits(maxTile);
        
        SpawnPortal();
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 wordStart)
    {
        // converts the string to a char
        // this can be of a cetrain tile tipe: eg 0 = grass, 1 = path.
        // based on this pick a random sprite index corresponding to the category
        int tileSpriteIndex = getRandomTileSprite(int.Parse(tileType));
        
        // Debug.Log(tileSpriteIndex);
        // create a new tile and makes a reference to that tile
        TileScript newGroundTile = Instantiate(groundTilePrefabs[tileSpriteIndex]).GetComponent<TileScript>();
        
        //initaiize the tile at the correct point
        newGroundTile.Setup(new Point(x, y),
            new Vector3(wordStart.x + (TileSize * x), wordStart.y - (TileSize * y), 0), map);
    }
    

    private int getRandomTileSprite(int i)
    {
        if (i == 0)
        {
            return Random.Range(0, 4);
        }
        else if (i == 1)
        {
            return Random.Range(4, 8);
        }
        else
        {
            return 0;
        }
    }

    private void SpawnPortal()
    {
        //define the spawn point.
        startSpawnPoint = new Point(1, 3);
        //spawn the object at the spawn point tile.
        Instantiate(starSpawnPrefab, Tiles[startSpawnPoint].GetComponent<TileScript>().WorldPosition,
            quaternion.identity);
        
        //set the end spawn point
        endSpawnPoint = new Point(16, 8);
        //the sprite is to big so we shift the spawning location to match the path.
        Point endSpawnPointOfsetForSprite = new Point(16, 6);
        //spawn the actual end object.
        Instantiate(endSpawnPrefab, Tiles[endSpawnPointOfsetForSprite].GetComponent<TileScript>().WorldPosition,
            quaternion.identity);
    }

    public bool InBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
    }
}

