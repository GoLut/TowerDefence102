using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;

    private Tower parent;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTargert();
    }

    public void Initialize(Tower parent)
    {
        this.parent = parent;
        this.target = parent.Target;
    }
    
    private void MoveToTargert()
    {
        if (target != null && target.IsActive)
        {
            Debug.Log("moving to target.");
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);

            //rotate the projectile in the correct direction. 
            Vector2 dir = target.transform.position - transform.position;
            float angle = MathF.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (!target)
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if (target.gameObject == other.gameObject)
            {
                target.TakeDamage(parent.Damage);
            }

            //release projectile when it has hit the monster.
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }
}
