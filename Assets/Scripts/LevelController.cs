using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GameObject victoryScreen = null;
    public ZombieControls zombieControls = null;

    public string nextLevelSceneName = null;

    private void Awake()
    {
        if (victoryScreen == null)
        {
            victoryScreen = GameObject.FindGameObjectWithTag("VictoryScreen");
        }
        Debug.Assert(victoryScreen != null);
        victoryScreen.SetActive(false);

        if (zombieControls == null)
        {
            zombieControls = GetComponent<ZombieControls>();
        }
        Debug.Assert(zombieControls != null);
    }

    public void GoalAreaReached()
    {
        victoryScreen.SetActive(true);
        // Disable zombie controls.
        zombieControls.enabled = false;
    }

    public void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelSceneName))
        {
            SceneManager.LoadScene(nextLevelSceneName);
        }
    }
}
