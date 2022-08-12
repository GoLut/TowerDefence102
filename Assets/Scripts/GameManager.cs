using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //The last knon tower type that we clicked on.
    public TowerButton ClickedButton { get; set; }

    public void PickTower(TowerButton towerButton)
    {
        this.ClickedButton = towerButton;
        Hover.Instance.Activate(towerButton.Sprite);
        Debug.Log("Picked a new tower type.");
        
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
        // deactivete the mouse hover icon and the ability to place an extra tower.
        Hover.Instance.Deactivate();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();
    }
}
