using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event System.Action gameOver;
    private playerController player;
    public static GameManager Instance { get; private set; }    //For singleton pattern
    public int coinsCollected; //Number of coins collected to be saved and loaded into this later
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //To listen to scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;

    }       //For singleton pattern

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f; //Reset time scale in case it was changed in previous scene (like in pause menu)
        if (scene.name == "GameScene")
        {
            // Initialize game state for the game scene
            Debug.Log("Game Scene Loaded");
            player = FindFirstObjectByType<playerController>();


            player.GetCoin -= OnGetCoin;    //Unsubscribes first just in case of multiple subscriptions 
            player.PlayerDeath -= OnPlayerDeath;


            player.GetCoin += OnGetCoin;
            player.PlayerDeath += OnPlayerDeath;




        }
    }

    private void OnGetCoin()
    {
        coinsCollected++;
        Debug.Log("Total Coins Collected: " + coinsCollected);
    }

    private void OnPlayerDeath()
    {
        Debug.Log("Game Over Triggered in GameManager");
        gameOver?.Invoke();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
