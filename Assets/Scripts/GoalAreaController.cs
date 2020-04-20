using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAreaController : MonoBehaviour
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        var zombie = collision.GetComponentInParent<ZombieController>();
        if (zombie != null && zombie.isActiveAndEnabled)
        {
            levelController.GoalAreaReached();
        }
    }
}
