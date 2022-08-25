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
        }
        else if (!target)
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }
}
