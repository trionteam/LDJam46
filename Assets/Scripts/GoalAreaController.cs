using UnityEngine;
using UnityEngine.Serialization;

public class GoalAreaController : MonoBehaviour
{
    [SerializeField]
    [FormerlySerializedAs("levelController")]
    private LevelController _levelController = null;

    // All zombies that get to the goal area will move towards this place.
    [SerializeField]
    [FormerlySerializedAs("zombieDestination")]
    private Transform _zombieDestination = null;

    private void Awake()
    {
        if (_levelController == null)
        {
            _levelController = GameObject.FindGameObjectWithTag("Managers")?.GetComponent<LevelController>();
        }
        Debug.Assert(_levelController != null);
        Debug.Assert(_zombieDestination != null);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var zombie = collision.GetComponentInParent<ZombieController>();
        if (zombie != null && zombie.isActiveAndEnabled)
        {
            _levelController.GoalAreaReached();
            zombie.AssignDestination(_zombieDestination.position);

            // Disable the zombifier to prevent the zombie from healing.
            var zombifier = zombie.GetComponent<Zombifiable>();
            if (zombifier != null)
            {
                zombifier.enabled = false;
            }
        }
    }
}
