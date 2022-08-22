using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    //array of prefabs that we can instantiate and save in this object pool.
    [SerializeField] private GameObject[] objectPrefabs;

    //list that contains every single object we have created.
    private List<GameObject> pooledGameObjects = new List<GameObject>();

    //get object from the pool
    public GameObject GetObject(string type)
    {
        //search the list for existing inactive game objects
        foreach (GameObject go in pooledGameObjects)
        {
            if (go.name == type && !go.activeInHierarchy)
            {
                go.SetActive(true);
                // Debug.Log("returning pooled object");
                return go;
            }
        }
        
        // if the pool does not contain  the object that wee need then we create a new object.
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            //check if the requested prefab exists
            if (objectPrefabs[i].name == type)
            {
                //instantiate the prefab
                GameObject newObject = Instantiate(objectPrefabs[i]);
                newObject.name = type;
                //add to object pool list
                pooledGameObjects.Add(newObject);
                
                return newObject;
            }
        }
        Debug.Log("returning null on GetObject from object pool");
        return null;
    }
    
    //release enemy form the word
    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
        
    }
    
}
