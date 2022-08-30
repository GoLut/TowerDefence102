using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;

    private Tower parent;

    private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
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
        else if (!target || !target.IsActive)
        {
            Release();
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
    private void Release()
    {
        resetAnimations();
        GameManager.Instance.Pool.ReleaseObject(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if (target.gameObject == other.gameObject)
            {
                target.TakeDamage(parent.Damage);
                
                //activate the trigger animation.
                myAnimator.SetTrigger("Impact");
            }
            //we release the object when the on state exit script is called 
        }
    }
}
