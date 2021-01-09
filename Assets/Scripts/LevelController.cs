using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private GameObject _victoryScreen = null;

    [SerializeField]
    private ZombieControls _zombieControls = null;

    [SerializeField]
    private string _nextLevelSceneName = null;

    [SerializeField]
    private LevelList _levelList = null;

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
        int currentLevelIndex = _levelList.Levels.IndexOf(
            // SceneManager.GetActiveScene().path contains also the leading 'Assets/', which
            // is not accepted by SceneManager.LoadScene(), and thus not present in the scene
            // list.
            level => "Assets/" + level.SceneName + ".unity" == SceneManager.GetActiveScene().path);
        Debug.Assert(currentLevelIndex >= 0);
        if (currentLevelIndex < _levelList.Levels.Length - 1)
        {
            SceneManager.LoadScene(_levelList.Levels[currentLevelIndex + 1].SceneName);
        }
        else
        {
            // After reaching the last level, load the final scene.
            SceneManager.LoadScene("Scenes/FinalScene");
        }
    }
}
