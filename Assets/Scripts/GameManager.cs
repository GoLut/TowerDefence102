using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    //The last knon tower type that we clicked on.
    public TowerButton ClickedButton { get; set; }

    //currency
    private int currency;

    public int Currency
    {
        get
        {
            return currency;
        }
        set
        {
            this.currency = value;
            //quick hack by making it a property we can avoid writing whole functions.
            this.currencyText.text = " " + this.currency.ToString() + "<color=#85BB65>$</color>";
            Debug.Log("Set starting currency");
        }
    }

    //the object pool that will contain all generated entities. every tower or enemy can acces this pool.
    public ObjectPool Pool { get; set; }

    //the text element on the canvas that displays the current currency value.
    [SerializeField] private TextMeshProUGUI currencyText;
    
    public void PickTower(TowerButton towerButton)
    {
        if (Currency >= towerButton.Price)
        {
            this.ClickedButton = towerButton;
            Hover.Instance.Activate(towerButton.Sprite);
            Debug.Log("Picked a new tower type.");
        }
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //disable hover icon.
            Hover.Instance.Deactivate();
        }
    }

    public void BuyTower()
    {
        if (Currency >= ClickedButton.Price)
        {
            //substract currency when bought.
            Currency = Currency - ClickedButton.Price;
        }
        // deactivete the mouse hover icon and the ability to place an extra tower.
        Hover.Instance.Deactivate();
    }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }


    // Start is called before the first frame update
    void Start()
    {
        Currency = 100;
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();
    }

    public void StartWave()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        //generate the path when we start the wave.
        LevelManager.Instance.GeneratePath();
        
        //spawn a random monster for now based on index 0-3
        int monsterIndex = 0; //Random.Range(0, 4);

        String type = string.Empty;

        switch (monsterIndex)
        {
            case 0:
                type = "Ninja"; 
                break;
            case 1:
                type = "SwordMaster"; 
                break;
            case 2:
                type = "SkeletonKnight"; 
                break;
            case 3:
                type = "WarriorGirl"; 
                break;
            default:
                break;
        }
        //get a reference to the latest spawned Enemy form the object pool.
        Enemy enemy = Pool.GetObject(type).GetComponent<Enemy>();
        enemy.Spawn();
        yield return new WaitForSeconds(2.5f);
    }
}
