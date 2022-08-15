using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    //the price of the towers.
    [SerializeField] private int price;
    public int Price => price;

    //the cost displayed underneeth the towers . 
    [SerializeField] private TextMeshProUGUI priceText;

    [SerializeField]
    private Sprite sprite; 
    public Sprite Sprite
    {
        get => sprite;
    }
    
    [SerializeField] private GameObject towerPrefab;
    public GameObject TowerPrefab
    {
        get => towerPrefab;
        
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        priceText.text = price + "$";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
