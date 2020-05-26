using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDetection : MonoBehaviour
{
    private List<ZombieController> _zombiesInSight = new List<ZombieController>();

    private CircleCollider2D _collider;

    public bool HasZombiesInSight
    {
        get => _zombiesInSight.Count > 0;
    }

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        Debug.Assert(_collider != null);
    }

    public Vector2 EscapeDirection()
    {
        var ownPosition = new Vector2(transform.position.x, transform.position.y);

        Vector2 zombieDirections = Vector2.zero;
        foreach(var zombie in _zombiesInSight)
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
        var numResults = _collider.OverlapCollider(new ContactFilter2D().NoFilter(), results);
        _zombiesInSight.Clear();
        for (int i = 0; i < numResults; ++i)
        {
            var zombie = results[i].GetComponentInParent<ZombieController>();
            if (zombie == null || !zombie.isActiveAndEnabled) continue;

            _zombiesInSight.Add(zombie);
        }
    }
}
