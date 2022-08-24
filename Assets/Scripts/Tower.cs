using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private SpriteRenderer mySpriteRenderer;

    //the current target enemy of the tower.
    private Enemy target;

    //this is the projectile type.
    [SerializeField] private string projectileType;
    
    //queue of monsters that are in the towers range
    private Queue<Enemy> enemies = new Queue<Enemy>();

    private bool canAttack;

    private float attackTimer;

    //how often can we attack
    private float attackCooldown;

    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        canAttack = true;

        attackCooldown = 3; //attackspeed in seconds.
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
        if (!canAttack)
        {
            //increment the timer.
            attackTimer += Time.deltaTime;
            //if the time setpoint is reached we can enable the attack again
            if (attackTimer >= attackCooldown)
            {
                //we can attack again
                canAttack = true;
                attackTimer = 0;
            }
        }
        
        if (target == null && enemies.Count > 0)
        {
            //selets the first target from the list.
            target = enemies.Dequeue();
        }

        if (target != null && target.IsActive && canAttack)
        {
            //shoot a projectile.
            Shoot();
            //fired our bullet so we can't fire any more.
            canAttack = false;
        }
    }

    private void Shoot()
    {
        Projectile projectile = (Projectile) GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        
        //place at towers position
        projectile.transform.position = transform.position;
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
