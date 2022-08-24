using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private SpriteRenderer mySpriteRenderer;

    //the current target enemy of the tower.
    private Enemy target;
    
    //queue of monsters that are in the towers range
    private Queue<Enemy> enemies = new Queue<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        Debug.Log(target);
    }

    //toggle select the sprite renderer.
    public void SelectToggle()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
    }
    
    //attack the target enemy.
    private void Attack()
    {
        if (target == null&& enemies.Count > 0)
        {
            //selets the first target from the list.
            target = enemies.Dequeue();
        }
    }
    
    //when ever an enemy enters the range circle of the tower.
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            //enque the enemies into the queue
            enemies.Enqueue(other.GetComponent<Enemy>());
            
        }
    }

    //when the target exits the circle eg. dies or is to far away.
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            target = null;
        }
    }
}
