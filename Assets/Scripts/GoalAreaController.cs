using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAreaController : MonoBehaviour
{
    public GameObject victorySign;

    public bool isVictory = false;

    private void Awake()
    {
        Debug.Assert(victorySign != null);
    }

    private void Start()
    {
        victorySign.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var zombie = collision.GetComponentInParent<ZombieController>();
        if (zombie != null && zombie.isActiveAndEnabled)
        {
            isVictory = true;
            victorySign.SetActive(true);
        }
    }
}
