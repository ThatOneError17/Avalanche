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

        var gameManager = FindFirstObjectByType<GameManager>();

        gameManager.gameOver += OnGameOver;
    }

    private void OnToMenuClicked()
    {
        Debug.Log("Return to Menu Button Clicked!");   //Logs to the console when the return to menu button is clicked
        SceneManager.LoadScene("TitleScene");  //Loads the main menu scene
    }

    private void OnGameOver()
    {
        coinsLabel.text = "Coins Collected: " + GameManager.Instance.coinsCollected.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
