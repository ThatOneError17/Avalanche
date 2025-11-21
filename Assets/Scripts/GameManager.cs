using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event System.Action gameOver;
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
        if (scene.name == "GameScene")
        {
            // Initialize game state for the game scene
            Debug.Log("Game Scene Loaded");
            var player = FindFirstObjectByType<playerController>();
            player.GetCoin += () =>
            {
                coinsCollected++;   //Will need to save these in a save manager later
                Debug.Log("Total Coins Collected: " + coinsCollected);
            };

            player.playerDeath += () =>
            {
                death();
            };

        }
    }

    void death()
    {
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
