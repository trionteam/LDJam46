using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public Vector2 destination;

    public float randomMovementDestinationRadius = 1.0f;

    public float destinationResetRadius = 0.1f;

    public float movementSpeed = 1.0f;

    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // TODO: Move to destination if possible.
    }

    private bool IsAtDestination()
    {
        var distance = (rigidBody.position - destination).magnitude;
        return distance <= destinationResetRadius;
    }

    private void UpdateDestination()
    {
        var destinationDelta = randomMovementDestinationRadius * Random.insideUnitCircle;
        destination = rigidBody.position + destinationDelta;
    }
}
