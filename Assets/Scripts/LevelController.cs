using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Levels/Level metadata", fileName = "Level0")]
public class LevelMetadata : ScriptableObject
{
    public string LevelName;

    public string SceneName;
}

[CreateAssetMenu(menuName = "Levels/Level list", fileName = "LevelList")]
public class LevelList : ScriptableObject
{
    public LevelMetadata[] Levels;
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
