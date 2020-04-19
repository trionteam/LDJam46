using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAreaController : MonoBehaviour
{
    public GameObject victoryScreen;

    public bool isVictory = false;

    private void Awake()
    {
        if (victoryScreen == null)
        {
            victoryScreen = GameObject.FindGameObjectWithTag("VictoryScreen");
        }
        Debug.Assert(victoryScreen != null);
    }

    private void Start()
    {
        victoryScreen.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var zombie = collision.GetComponentInParent<ZombieController>();
        if (zombie != null && zombie.isActiveAndEnabled)
        {
            isVictory = true;
            victoryScreen.SetActive(true);
        }
    }
}
