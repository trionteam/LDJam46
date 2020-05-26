using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject _container;

    [SerializeField]
    private Button _exitToMenuButton;

    [SerializeField]
    private Button _exitGameButton;

    [SerializeField]
    private Button _backToGameButton;

    [SerializeField]
    private Button _showMenuButton;

    [SerializeField]
    private Button _restartLevelButton;

    private bool IsInMainMenu
    {
        get => SceneManager.GetActiveScene().name == "MainMenu";
    }

    private void Awake()
    {
        if (_container == null)
        {
            _container = transform.Find("Container").gameObject;
        }
        Debug.Assert(_container != null);

        if (_exitToMenuButton == null)
        {
            _exitToMenuButton = transform.Find("Container/BackgroundImage/ExitToMenuButton").GetComponent<Button>();
        }
        Debug.Assert(_exitToMenuButton != null);

        if (_exitGameButton == null)
        {
            _exitGameButton = transform.Find("Container/BackgroundImage/ExitButton").GetComponent<Button>();
        }
        Debug.Assert(_exitGameButton != null);

        if (_backToGameButton == null)
        {
            _backToGameButton = transform.Find("Container/BackgroundImage/BackToGameButton").GetComponent<Button>();
        }
        Debug.Assert(_backToGameButton != null);

        if (_showMenuButton == null)
        {
            _showMenuButton = transform.Find("ShowMenuButton").GetComponent<Button>();
        }
        Debug.Assert(_showMenuButton != null);

        if (_restartLevelButton == null)
        {
            _restartLevelButton = transform.Find("Container/BackgroundImage/RestartLevelButton").GetComponent<Button>();
        }
        Debug.Assert(_restartLevelButton != null);
    }

    private void Start()
    {
        bool isInMainMenu = IsInMainMenu;
        _exitToMenuButton.gameObject.SetActive(!isInMainMenu);
        _exitGameButton.gameObject.SetActive(isInMainMenu);
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            _exitGameButton.interactable = false;
        }
        _backToGameButton.gameObject.SetActive(!IsInMainMenu);
        _showMenuButton.gameObject.SetActive(!IsInMainMenu);
        _restartLevelButton.gameObject.SetActive(!IsInMainMenu);

        _container.SetActive(isInMainMenu);
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
        Time.timeScale = 1.0f;
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
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void ToggleMainMenu()
    {
        // If playing a level, escape shows/hides the menu and pauses the game.
        bool activateMenu = !_container.activeSelf;
        Time.timeScale = activateMenu ? 0.0f : 1.0f;
        _container.SetActive(activateMenu);
        // Reset UI selection so that highlight works on the 'show menu' button.
        EventSystem.current.SetSelectedGameObject(null);
    }
}
