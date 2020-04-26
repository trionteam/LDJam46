using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject container;
    public Button exitToMenuButton;
    public Button exitGameButton;
    public Button backToGameButton;
    public Button showMenuButton;
    public Button restartLevelButton;

    private bool IsInMainMenu
    {
        get => SceneManager.GetActiveScene().name == "MainMenu";
    }

    private void Awake()
    {
        if (container == null)
        {
            container = transform.Find("Container").gameObject;
        }
        Debug.Assert(container != null);

        if (exitToMenuButton == null)
        {
            exitToMenuButton = transform.Find("Container/BackgroundImage/ExitToMenuButton").GetComponent<Button>();
        }
        Debug.Assert(exitToMenuButton != null);

        if (exitGameButton == null)
        {
            exitGameButton = transform.Find("Container/BackgroundImage/ExitButton").GetComponent<Button>();
        }
        Debug.Assert(exitGameButton != null);

        if (backToGameButton == null)
        {
            backToGameButton = transform.Find("Container/BackgroundImage/BackToGameButton").GetComponent<Button>();
        }
        Debug.Assert(backToGameButton != null);

        if (showMenuButton == null)
        {
            showMenuButton = transform.Find("ShowMenuButton").GetComponent<Button>();
        }
        Debug.Assert(showMenuButton != null);

        if (restartLevelButton == null)
        {
            restartLevelButton = transform.Find("Container/BackgroundImage/RestartLevelButton").GetComponent<Button>();
        }
        Debug.Assert(restartLevelButton == null);
    }

    private void Start()
    {
        bool isInMainMenu = IsInMainMenu;
        exitToMenuButton.gameObject.SetActive(!isInMainMenu);
        exitGameButton.gameObject.SetActive(isInMainMenu);
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            exitGameButton.interactable = false;
        }
        backToGameButton.gameObject.SetActive(!IsInMainMenu);
        showMenuButton.gameObject.SetActive(!IsInMainMenu);
        restartLevelButton.gameObject.SetActive(!IsInMainMenu);

        container.SetActive(isInMainMenu);
    }

    private void Update()
    {
        if (!IsInMainMenu && Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleMainMenu();
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Scenes/LevelTutorial");
	}

	public void RestartLevel()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ExitGame()
    {
        Application.Quit();
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void ToggleMainMenu()
    {
        // If playing a level, escape shows/hides the menu and pauses the game.
        bool activateMenu = !container.activeSelf;
        Time.timeScale = activateMenu ? 0.0f : 1.0f;
        container.SetActive(activateMenu);
    }
}
