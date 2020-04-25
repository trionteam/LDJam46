using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private GameObject container;
    private Button exitToMenuButton;
    private Button exitGameButton;
    private Button backToGameButton;
    private Button showMenuButton;

    private bool IsInMainMenu
    {
        get => SceneManager.GetActiveScene().name == "MainMenu";
    }

    private void Awake()
    {
        container = transform.Find("Container").gameObject;
        Debug.Assert(container != null);

        exitToMenuButton = transform.Find("Container/ExitToMenuButton").GetComponent<Button>();
        Debug.Assert(exitToMenuButton != null);

        exitGameButton = transform.Find("Container/ExitButton").GetComponent<Button>();
        Debug.Assert(exitGameButton != null);

        backToGameButton = transform.Find("Container/BackToGameButton").GetComponent<Button>();
        Debug.Assert(backToGameButton != null);

        showMenuButton = transform.Find("ShowMenuButton").GetComponent<Button>();
        Debug.Assert(showMenuButton != null);
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
