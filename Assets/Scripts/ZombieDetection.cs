using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDetection : MonoBehaviour
{
    public List<ZombieController> zombiesInSight = new List<ZombieController>();

    private new CircleCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
        Debug.Assert(collider != null);
    }

    public Vector2 EscapeDirection()
    {
        var ownPosition = new Vector2(transform.position.x, transform.position.y);

        Vector2 zombieDirections = Vector2.zero;
        foreach(var zombie in zombiesInSight)
        {
            var zombiePosition = new Vector2(zombie.transform.position.x,
                                             zombie.transform.position.y);
            zombieDirections += (zombiePosition - ownPosition);
        }
        if (zombieDirections.magnitude > 1e-6)
        {
            return -zombieDirections.normalized;
        }
        return Vector2.zero;
    }

    private void FixedUpdate()
    {
        const int maxResults = 64;
        var results = new Collider2D[maxResults];
        var numResults = collider.OverlapCollider(new ContactFilter2D().NoFilter(), results);
        zombiesInSight.Clear();
        for (int i = 0; i < numResults; ++i)
        {
            var zombie = results[i].GetComponentInParent<ZombieController>();
            if (zombie == null || !zombie.isActiveAndEnabled) continue;

            zombiesInSight.Add(zombie);
        }
    }
}
