using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;


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
    
    //tower selected coloring
    private Color32 towerSelectedColor = new Color32(255, 118, 118, 255);
    // private Image img;
    [SerializeField] private Image img;

    // Start is called before the first frame update
    void Start()
    {
        priceText.text = price + "$";
        // Image img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //todo for later. little inefficeint to do this every frame better to do this on change somehow...
        //update color of button when tower is selected.
        if (GameManager.Instance.ClickedButton != null && GameManager.Instance.ClickedButton.GetInstanceID() == GetInstanceID())
        {
            ColorButton(Color.green);
            // Debug.Log("Changning back to color: "+GameManager.Instance.ClickedButton.GetInstanceID().ToString() + ", " + GetInstanceID().ToString());

        }
        // else if (GameManager.Instance.ClickedButton != null && GameManager.Instance.ClickedButton.GetInstanceID() != GetInstanceID())
        else
        {
            ColorButton(Color.white);
            // Debug.Log("Changning back to white: "+ GameManager.Instance.ClickedButton.GetInstanceID().ToString() + ", " + GetInstanceID().ToString());

        }
    }
    
    private void ColorButton(Color newColor)
    {
        // Debug.Log("Setting the color to newColor");
        img.color = newColor;
    }
}
