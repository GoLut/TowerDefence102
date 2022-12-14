using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    private SpriteRenderer spriteRenderer;

    
    private SpriteRenderer rangeSpriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.rangeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        if (spriteRenderer.enabled == false)
        {
            return;
        }
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    
    
    public void Activate(Sprite sprite)
    {
        this.spriteRenderer.sprite = sprite;
        //the tower to show.
        this.spriteRenderer.enabled = true;
        //The range bubble around the tower
        rangeSpriteRenderer.enabled = true;
    }

    public void Deactivate()
    {
        //disable the sprite renderer that hovers under the mouse.
        this.spriteRenderer.enabled = false;
        rangeSpriteRenderer.enabled = false;
        //clear the selected tower memeory such that no tower can be placed
        GameManager.Instance.ClickedButton = null;
    }
    
}
