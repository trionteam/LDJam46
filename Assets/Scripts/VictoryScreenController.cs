using UnityEngine;

public class VictoryScreenController : MonoBehaviour
{
    private LevelController _levelController;

    private void Awake()
    {
        _levelController = GameObject.FindGameObjectWithTag("Managers")?.GetComponent<LevelController>();
        Debug.Assert(_levelController != null);
    }

    public void OnNextLevelClicked()
    {
        _levelController.LoadNextLevel();
    }
}
