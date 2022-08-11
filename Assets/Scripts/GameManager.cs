using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //The last knon tower type that we clicked on.
    public TowerButton ClickedButton { get; private set; }

    public void PickTower(TowerButton towerButton)
    {
        this.ClickedButton = towerButton;
        Debug.Log("Picked a new tower type.");
    }

    public void BuyTower()
    {
        ClickedButton = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
