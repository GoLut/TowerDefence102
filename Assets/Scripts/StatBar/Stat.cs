using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private BarScript bar;

    public BarScript Bar
    {
        get => bar;
    }

    private float maxVal;
    
    private float currentVal;
    
    public float MaxVal
    {
        get => maxVal;
        set
        {
            maxVal = value;
            bar.MaxValue = maxVal;

        }
    }
    
    public float CurrentVal
    {
        get => currentVal;
        set
        {
            //set between min and max (this case max = 0)
            this.currentVal = Mathf.Clamp(value, 0, MaxVal);
            bar.Value = currentVal;
        }
    }

    public void Initialize()
    {
        this.MaxVal = maxVal;
        this.CurrentVal = currentVal;
    }
    
}
