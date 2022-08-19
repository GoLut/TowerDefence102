using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void Spawn()
    {
        transform.position = LevelManager.Instance.StartPortal.transform.position;
        
        //does something over time
        StartCoroutine(Scale(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.25f, 0.25f, 0.25f)));
    }

    //Interesting piece of code using IEnumerator, don't exactly know what that IEnumerator does.
    //over time increase the scale of the sprite when it is spawned. (make it seem to appear from the portal.)
    public IEnumerator Scale(Vector3 from, Vector3 to)
    {
        float progress = 0;

        while (progress <=1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);

            progress += Time.deltaTime;

            yield return null;
        }

        transform.localScale = to;
    }

}
