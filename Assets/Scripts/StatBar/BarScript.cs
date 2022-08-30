using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    private float fillAmount;

    [SerializeField] private float lerpSpeed;
    
    [SerializeField] private Image content;

    [SerializeField] private TextMeshProUGUI valueText;

    public float MaxValue { get; set; }

    public float Value
    {
        set
        {
            if (valueText != null)
            {
                string[] tmp = valueText.text.Split(':');
                valueText.text = tmp[0] + ": " + value;
            }
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleBar();
    }

    private void HandleBar()
    {
        if (fillAmount != content.fillAmount)
        {
            //a change in value over time with a certain speed.
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);;
        }

    }

    private float Map(float value, float inMin, float inMax,float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public void Reset()
    {
        Value = MaxValue;
        content.fillAmount = 1;
    }
}
