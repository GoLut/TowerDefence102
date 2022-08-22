using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //the speed the enemy is walking
    [SerializeField] private float speed;
    //the path to traverse
    private Stack<Node> path;
    public Point GridPosition { get; set; }

    private Vector3 destination;
    
    private bool IsActive { get; set; }

    private void Update()
    {
        Move();
    }

    public void Spawn()
    {
        transform.position = LevelManager.Instance.StartPortal.transform.position;
        
        //does something over time
        StartCoroutine(Scale(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.25f, 0.25f, 0.25f)));
        
        SetPath(LevelManager.Instance.Path);
    }

    //Interesting piece of code using IEnumerator, don't exactly know what that IEnumerator does.
    //over time increase the scale of the sprite when it is spawned. (make it seem to appear from the portal.)
    public IEnumerator Scale(Vector3 from, Vector3 to)
    {
        //disable movement during the animation moment.
        IsActive = false;
        
        float progress = 0;

        while (progress <=1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);

            progress += Time.deltaTime;

            yield return null;
        }
        transform.localScale = to;
        
        //Enable movement after spawning in
        IsActive = true;
    }

    //this function will move the monster
    private void Move()
    {
        if (IsActive)
        {
            // move from point to destination
            transform.position = Vector2.MoveTowards(transform.position,
                destination, speed * Time.deltaTime);
            //when we arrive at the destination we grab the next tile
            if (transform.position == destination)
            {
                if (path != null && path.Count > 0)
                {
                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
            }
        }

    }
//be able to set the path function from the spawn.
    private void SetPath(Stack<Node> newPath)
    {
        if (newPath != null)
        {
            this.path = newPath;
            // Debug.Log("looking for current grid position");
            GridPosition = path.Peek().GridPosition;
            destination = path.Pop().WorldPosition;
        }
        else{
            Debug.Log("no path found it is null");
        }
    }

}
