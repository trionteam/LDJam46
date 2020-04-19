using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthyPersonController : MonoBehaviour
{
    /// <summary>
    /// The destination towards which the zombie is moving.
    /// </summary>
    public Vector2 destination;

    public float randomMovementDestinationRadius = 1.0f;

    public float movementSpeed = 1.0f;

    public float destinationResetRadius = 0.01f;

    public ZombieDetection zombieDetection;

    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        Debug.Assert(rigidBody != null);
    }

    private void Start()
    {
        UpdateDestination();
    }

    private void FixedUpdate()
    {
        Vector2 directionToDestination;
        if (zombieDetection != null && zombieDetection.zombiesInSight.Count > 0)
        {
            directionToDestination = zombieDetection.EscapeDirection();
            // Reset destination, so that the person starts moving randomly when they
            // lose sight of the zombies.
            UpdateDestination();
        }
        else
        {
            if (IsAtDestination())
            {
                UpdateDestination();
            }
            directionToDestination = (destination - rigidBody.position).normalized;
        }

        var desiredPositionInFrame = rigidBody.position + movementSpeed * Time.fixedDeltaTime * directionToDestination;
        rigidBody.MovePosition(desiredPositionInFrame);
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        UpdateDestination();
    }
}
