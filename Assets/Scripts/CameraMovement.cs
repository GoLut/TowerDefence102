using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 1;
    
    private float xMax;
    private float yMin;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();   
    }
    
    private void GetInput()
        //get the user key input and move the camera positions
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
            // Debug.Log("up");
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
            // Debug.Log("right");
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
            // Debug.Log("left");
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
            // Debug.Log("down");
        }

        //clamp the camera movement.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, xMax),
            Mathf.Clamp(transform.position.y, yMin, 0), -10);
    }

    public void SetLimits(Vector3 maxTile)
    {
        Vector3 wp = Camera.main.ViewportToWorldPoint(new Vector3(1, 0,0));
        
        xMax = maxTile.x - wp.x;
        yMin = maxTile.y - wp.y;

    }
}
