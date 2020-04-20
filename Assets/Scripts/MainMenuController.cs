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
    }

    private void Start()
    {
        bool isInMainMenu = IsInMainMenu;
        exitToMenuButton.gameObject.SetActive(!isInMainMenu);
        exitGameButton.gameObject.SetActive(isInMainMenu);

        container.SetActive(isInMainMenu);
    }

    private void Update()
    {
        if (!IsInMainMenu && Input.GetKeyUp(KeyCode.Escape))
        {
            // If playing a level, escape shows/hides the menu and pauses the game.
            bool activateMenu = !container.activeSelf;
            Time.timeScale = activateMenu ? 0.0f : 1.0f;
            container.SetActive(activateMenu);
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Scenes/Level00");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}
