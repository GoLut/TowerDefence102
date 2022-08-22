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

    private Animator myAnimator;

    //blood particle effect
    [SerializeField] private GameObject blood; 

    private void Update()
    {
        Move();
    }

    public void Spawn()
    {
        //set the start position equal to the position of the spawning portal.
        transform.position = LevelManager.Instance.StartPortal.transform.position;
        
        //initialize the animator by calling to the animator component attached to the enemy
        myAnimator = GetComponent<Animator>();
        
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
                    //set the inital animation correctly
                    Animate(GridPosition, path.Peek().GridPosition);

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
            //copy global path stack to this local enemy
            this.path = newPath;
            
            //set the inital animation correctly
            Animate(LevelManager.Instance.startSpawnPoint, path.Peek().GridPosition);
            
            // Debug.Log("looking for current grid position");
            GridPosition = path.Peek().GridPosition;
            
            destination = path.Pop().WorldPosition;
            
            
        }
        else{
            Debug.Log("no path found it is null");
        }
    }

    private void Animate( Point currentPosition, Point newPosition)
    {
        if (currentPosition.Y > newPosition.Y)
        {
            //we are moving down
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", -1);
        }
        else if (currentPosition.Y < newPosition.Y)
        {
            //moving up
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", 1);
        }
        else if (currentPosition.Y == newPosition.Y)
        {
            if (currentPosition.X > newPosition.X)
            {
                //moving to the left
                myAnimator.SetInteger("Horizontal", -1);
                myAnimator.SetInteger("Vertical", 0);
            }
            else if (currentPosition.X < newPosition.X)
            {
                //moving to the right
                myAnimator.SetInteger("Horizontal", 1);
                myAnimator.SetInteger("Vertical", 0);
            }
        }
    }

    //use a coroutine so we can pause this function and wait for the animation to complete
    private IEnumerator AttackAndDeathAnimation()
    {
        AnimateAttack();
        float animationLength = myAnimator.GetCurrentAnimatorStateInfo(0).length*3;
        yield return new WaitForSecondsRealtime(animationLength);
        //Animation finished
        //signal the animator to switch to the death animation
        AnimateDeath();
        yield return new WaitForSecondsRealtime(4f);
        Release();
    }
    
    
    private void AnimateDeath()
    {
        //tell the animator to change animation
        myAnimator.SetBool("Death", true);
        //spawn blood for gore effect :)
        Instantiate(blood, transform.position, Quaternion.identity);
    }

    private void AnimateAttack()
    {
        //tell the animator to change animation.
        myAnimator.SetInteger("Attack", 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Finish")
        {
            Debug.Log("WE have collided witht he castle. ");
            StartCoroutine(AttackAndDeathAnimation());
            
        }
    }

    private void resetAnimations()
    {
        myAnimator.SetBool("Death", false);
        myAnimator.SetInteger("Attack", 0);
        myAnimator.SetInteger("Horizontal", 0);
        myAnimator.SetInteger("Vertical", -1);
        // myAnimator = gameObject.GetComponent<Animator>();
        myAnimator.Rebind();
        myAnimator.Update(0f);
        myAnimator.Play("idle", -1, 0f);
    }
    
    //releases enemy from the world and reset the status of the monster.
    private void Release()
    {
        resetAnimations();
        IsActive = false;
        
        GameManager.Instance.Pool.ReleaseObject(gameObject);
    }
    
}
