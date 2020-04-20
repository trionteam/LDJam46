using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAreaController : MonoBehaviour
{
    public LevelController levelController;

    // All zombies that get to the goal area will move towards this place.
    public Transform zombieDestination;

    private void Awake()
    {
        if (levelController == null)
        {
            levelController = GameObject.FindGameObjectWithTag("Managers")?.GetComponent<LevelController>();
        }
        Debug.Assert(levelController != null);
        Debug.Assert(zombieDestination != null);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var zombie = collision.GetComponentInParent<ZombieController>();
        if (zombie != null && zombie.isActiveAndEnabled)
        {
            levelController.GoalAreaReached();
            zombie.AssignDestination(zombieDestination.position);

            // Disable the zombifier to prevent the zombie from healing.
            var zombifier = zombie.GetComponent<Zombifiable>();
            if (zombifier != null)
            {
                zombifier.enabled = false;
            }
        }
    }
}
