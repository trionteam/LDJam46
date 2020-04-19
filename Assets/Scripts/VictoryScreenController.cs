using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreenController : MonoBehaviour
{
    public LevelController levelController;

    private void Awake()
    {
        if (levelController == null)
        {
            levelController = GameObject.FindGameObjectWithTag("Managers")?.GetComponent<LevelController>();
        }
        Debug.Assert(levelController != null);
    }

    public void OnNextLevelClicked()
    {
        levelController.LoadNextLevel();
    }
}
