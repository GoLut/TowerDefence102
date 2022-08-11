using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//based on video: https://www.youtube.com/watch?v=ibOBHDgg2kg&list=PLX-uZVK_0K_4uNwvKian1bscP9mVvOp1M&index=11&ab_channel=inScopeStudios
//Simply stated it stops you haveing to go instance = findobjectoftype<something> everysingle time you would like to call
//for example the game manager. 
public abstract class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }
}
