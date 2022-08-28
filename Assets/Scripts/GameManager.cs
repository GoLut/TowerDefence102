using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    //The last knon tower type that we clicked on.
    public TowerButton ClickedButton { get; set; }

    //the object pool that will contain all generated entities. every tower or enemy can acces this pool.
    public ObjectPool Pool { get; set; }

    //the text element on the canvas that displays the current currency value.
    [SerializeField] private TextMeshProUGUI currencyText;

    //text incicator that displays the current wave
    [SerializeField] private TextMeshProUGUI waveText;
    //text indicator for the wave score.
    [SerializeField] private TextMeshProUGUI waveTextScore;
    
    //text incidator for the player lives.
    [SerializeField] private TextMeshProUGUI livesText;

    //the current wave we are at atm
    private int wave;
    
    //player health
    private int lives;

    private int enemyHealth;
    
    //currency
    private int currency;
    
    //game over Bool
    public bool gameOver = false;

    //the current selected tower.
    private Tower selectedTower;

    //play wave button
    [SerializeField] private GameObject waveButton;

    //gameOverMenu
    [SerializeField] private GameObject gameOverMenu;

    //list with all the active monsters in the current wave.
    private List<Enemy> activeEnemies = new List<Enemy>();
    
    public int Lives
    {
        get { return lives; }
        set
        {
            this.lives = value;
            
            if (this.lives <= 0)
            {
                this.lives = 0;
                //call game over function when lives are 0;
                GameOver();
            }
            //quick hack by making it a property we can avoid writing whole functions.
            this.livesText.text = this.lives.ToString();
        }
    }
    
    public bool WaveActive
    {
        get
        {
            return activeEnemies.Count > 0;
        }
    }
    
    public int Currency {
        get { return currency; }
        set
        {
            this.currency = value;
            //quick hack by making it a property we can avoid writing whole functions.
            this.currencyText.text = " " + this.currency.ToString() + "<color=#85BB65>$</color>";
            Debug.Log("Set starting currency");
        }
    }
    
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
        Lives = 10;
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();
    }

    public void StartWave()
    {
        //increment wave
        wave++;
        //update the screen wave text.
        this.waveText.text = "Wave: " + wave.ToString();
        StartCoroutine(SpawnWave());
        // waveButton.SetActive(false);
    }
    
    private IEnumerator SpawnWave()
    {
        //generate the path when we start the wave.
        LevelManager.Instance.GeneratePath();

        for (int i = 0; i < wave; i++)
        {
            //spawn a random monster for now based on index 0-3
            int monsterIndex = 0; //Random.Range(0, 4);

            String type = string.Empty;

            //set the monster to spawn in addition to some monster specific parameters.
            switch (monsterIndex)
            {
                case 0:
                    type = "Ninja";
                    enemyHealth = 10;
                    break;
                case 1:
                    type = "SwordMaster";
                    enemyHealth = 10;
                    break;
                
                case 2:
                    type = "SkeletonKnight";
                    enemyHealth = 10;
                    break;
                case 3:
                    type = "WarriorGirl";
                    enemyHealth = 10;
                    break;
                default:
                    break;
            }

            //get a reference to the latest spawned Enemy form the object pool.
            Enemy enemy = Pool.GetObject(type).GetComponent<Enemy>();

            //add a litle extra umpfh to the waves.
            int extraHealth = wave * 1;
            
            //spawn the enemies of this wave.
            enemy.Spawn(enemyHealth + extraHealth);

            //a list with all active monsters.
            activeEnemies.Add(enemy);

            yield return new WaitForSeconds(0.5f);
        }
    }

    //remove an enemy from the active list,
    //and if all enemys are gone we allow for new wave to start by renabling the button.
    public void RemoveEnemy(Enemy enemy)
    {
        activeEnemies.Remove(enemy);
        //enable the wave button if conditions are met.
        if (!WaveActive && !gameOver)
        {
            waveButton.SetActive(true);
        }
    }
//call to end the game.
    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            //display the game over menu;
            gameOverMenu.SetActive(true);
            //display the reached wave.
            waveTextScore.text = "Wave Reached: " + wave.ToString();
        }
    }

    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            //we toggle the range indicator of the old tower before going to the new tower to disable it.
            selectedTower.SelectToggle();
        }
        //store the current selected tower
        selectedTower = tower;
        //if we have selected a tower we toggle the range inidcator
        selectedTower.SelectToggle();
        
    }

    public void DeSelectTower()
    {
        //we toggle the range indicator to disable
        if (selectedTower != null)
        {
            selectedTower.SelectToggle();
        }
        //we have not selected a tower.
        selectedTower = null;
    }
    
    public void Restart()
    {
        //to ensure everything works when we restart.
        Time.timeScale = 1;
        //load the same leve
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        //only works when we build the game
        Application.Quit();
    }
}
