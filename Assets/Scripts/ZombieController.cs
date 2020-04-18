using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public Vector2 destination;

    public float randomMovementDestinationRadius = 1.0f;

    public float destinationResetRadius = 0.01f;

    public float movementSpeed = 1.0f;

    /// <summary>
    /// True if the zombie is selected for control.
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            selectionMarker.SetActive(value);
        }
    }
    private bool _isSelected = false;

    private GameObject selectionMarker;

    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        Debug.Assert(rigidBody != null);

        selectionMarker = transform.Find("SelectionMarker").gameObject;
    }

    void FixedUpdate()
    {
        if (IsAtDestination())
        {
            UpdateDestination();
        }

        var directionToDestination = (destination - rigidBody.position).normalized;
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
