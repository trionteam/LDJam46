using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif  // UNITY_EDITOR

public class HealthyPersonController : MonoBehaviour
{
    /// <summary>
    /// The destination towards which the zombie is moving.
    /// </summary>
    public Vector2 destination;

    public bool hasBasePosition;
    public Vector2 basePosition;
    public float basePositionRadius = 1.0f;

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
        if (hasBasePosition)
        {
            var destinationDelta = basePositionRadius * Random.insideUnitCircle;
            destination = basePosition + destinationDelta;
        }
        else
        {
            var destinationDelta = randomMovementDestinationRadius * Random.insideUnitCircle;
            destination = rigidBody.position + destinationDelta;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        UpdateDestination();
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(HealthyPersonController))]
public class HealthyPersonEditor : Editor
{
    public void OnSceneGUI()
    {
        HealthyPersonController person = target as HealthyPersonController;
        if (person == null || !person.hasBasePosition) return;

        EditorGUI.BeginChangeCheck();
        var position = new Vector3(person.basePosition.x, person.basePosition.y);
        float radius = Handles.RadiusHandle(Quaternion.identity, position, person.basePositionRadius);
        position = Handles.PositionHandle(position, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed base area radius and position");
            person.basePositionRadius = radius;
            person.basePosition = new Vector2(position.x, position.y);
        }
    }
}

#endif  // UNITY_EDITOR