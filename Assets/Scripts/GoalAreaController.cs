using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAreaController : MonoBehaviour
{
    public bool isVictory = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var zombie = collision.GetComponentInParent<ZombieController>();
        if (zombie != null && zombie.isActiveAndEnabled)
        {
            isVictory = true;
            // TODO: Victory!
        }
    }
}
