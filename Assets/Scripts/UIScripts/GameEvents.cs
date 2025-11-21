using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameEvents : MonoBehaviour
{

    private UIDocument UIDocument;

    private Label coinsLabel;

    private Button toMenu;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        UIDocument = GetComponent<UIDocument>();    //Gets the UIDocument component attached to the same GameObject

        coinsLabel = UIDocument.rootVisualElement.Q<Label>("CoinCount");    //Used to find elements in the UI document by their name

        toMenu = UIDocument.rootVisualElement.Q<Button>("ReturnToMenu");


        toMenu.clicked += OnToMenuClicked;   //Adds event listeners to the buttons

        UIDocument.rootVisualElement.style.display = DisplayStyle.None; //Had to do this instead of setting the GameObject inactive because it wasn't subscribing to the event properly

    }

    private void OnEnable()     //Had to move it here to avoid null reference exceptions
    {
        var gameManager = GameManager.Instance;
        gameManager.gameOver += OnGameOver;
    }

    private void OnDisable()
    {
        var gameManager = GameManager.Instance;
        gameManager.gameOver -= OnGameOver;
    }

    private void OnToMenuClicked()
    {
        Debug.Log("Return to Menu Button Clicked!");   //Logs to the console when the return to menu button is clicked
        SceneManager.LoadScene("TitleScene");  //Loads the main menu scene
    }

    private void OnGameOver()
    {
        Debug.Log("Game Over Event Triggered!"); //Logs to the console when the game over event is triggered
        UIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        coinsLabel.text = "Coins Collected: " + GameManager.Instance.coinsCollected.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
