using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{

    private UIDocument UIDocument;

    private Button startButton;
    private Button exitButton;
    private Button upgradesButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        UIDocument = GetComponent<UIDocument>();    //Gets the UIDocument component attached to the same GameObject

        startButton = UIDocument.rootVisualElement.Q<Button>("playButton");    //Used to find elements in the UI document by their name
        exitButton = UIDocument.rootVisualElement.Q<Button>("quitButton");
        upgradesButton = UIDocument.rootVisualElement.Q<Button>("upgradesButton");

        startButton.clicked += OnStartButtonClicked;   //Adds event listeners to the buttons
        exitButton.clicked += OnExitButtonClicked;
        upgradesButton.clicked += OnUpgradesButtonClicked;

    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Start Button Clicked!");   //Logs to the console when the start button is clicked
        //Add code to start the game here
        SceneManager.LoadScene("GameScene");

    }

    private void OnExitButtonClicked()
    {
        Debug.Log("Exit Button Clicked!");    //Logs to the console when the exit button is clicked
        //I would add code to save here as well, but it seems wiser to put it after every match, because how many people actually quit from a quit button on phone?
        Application.Quit();
    }

    private void OnUpgradesButtonClicked()
    {
        Debug.Log("Upgrades Button Clicked!");    //Logs to the console when the upgrades button is clicked
        //Add code to open upgrades menu here, will probably not make a new scene for this, unless that is the best choice
    }

}
