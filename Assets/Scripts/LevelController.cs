using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains information about a single level in the level list.
/// </summary>
[CreateAssetMenu(menuName = "Levels/Level metadata", fileName = "Level0")]
public class LevelMetadata : ScriptableObject
{
    [SerializeField]
    private string _levelName;

    [SerializeField]
    private string _sceneName;

    /// <summary>
    /// A human-readable name of the level displayed in the UI.
    /// </summary>
    public string LevelName { get => _levelName; }

    /// <summary>
    /// The name of the scene that contains the level.
    /// </summary>
    public string SceneName { get => _sceneName; }
}

/// <summary>
/// Contains a list of levels in the game.
/// </summary>
[CreateAssetMenu(menuName = "Levels/Level list", fileName = "LevelList")]
public class LevelList : ScriptableObject
{
    [SerializeField]
    private LevelMetadata[] _levels;

    public LevelMetadata[] Levels { get => _levels; }
}

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private GameObject _victoryScreen = null;

    [SerializeField]
    private ZombieControls _zombieControls = null;

    [SerializeField]
    private string _nextLevelSceneName = null;

    private void Awake()
    {
        if (_victoryScreen == null)
        {
            _victoryScreen = GameObject.FindGameObjectWithTag("VictoryScreen");
        }
        Debug.Assert(_victoryScreen != null);
        _victoryScreen.SetActive(false);

        if (_zombieControls == null)
        {
            _zombieControls = GetComponent<ZombieControls>();
        }
        Debug.Assert(_zombieControls != null);
    }

    public void GoalAreaReached()
    {
        _victoryScreen.SetActive(true);
        // Disable zombie controls.
        _zombieControls.enabled = false;
    }

    public void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(_nextLevelSceneName))
        {
            SceneManager.LoadScene(_nextLevelSceneName);
        }
    }
}
