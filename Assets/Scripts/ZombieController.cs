using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    /// <summary>
    /// The destination towards which the zombie is moving.
    /// </summary>
    public Vector2 destination;

    /// <summary>
    /// True if the destination was assigned by the player. When true, the destination
    /// marker is displayed. Setting the property shows or hides the destination marker.
    /// </summary>
    public bool HasAssignedDestination
    {
        get => _assignedDestination;
        set
        {
            _assignedDestination = value;
            destinationMarker.SetActive(HasAssignedDestination);
        }
    }
    public bool _assignedDestination;

    public float randomMovementDestinationRadius = 1.0f;

    public float destinationResetRadius = 0.01f;

    public float movementSpeed = 1.0f;

    /// <summary>
    /// True if the zombie is selected for control. When true, the selection marker is
    /// displayed. Setting this property shows or hides the selection marker.
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            selectionMarker.SetActive(value);

            // Update transparency of the destination marker sprite.
            var destinationMarkerSprite = destinationMarker.GetComponent<SpriteRenderer>();
            var destinationMarkerColor = destinationMarkerSprite.color;
            destinationMarkerColor.a = value ? 1.0f : 0.5f;
            destinationMarkerSprite.color = destinationMarkerColor;
        }
    }
    private bool _isSelected = false;

    private GameObject selectionMarker;
    private GameObject destinationMarker;

    private Rigidbody2D rigidBody;

    public void AssignDestination(Vector2 destination)
    {
        HasAssignedDestination = true;
        this.destination = destination;
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        Debug.Assert(rigidBody != null);

        // Asserts are not needed - this will raise an exception if the objects
        // are not found.
        selectionMarker = transform.Find("SelectionMarker").gameObject;
        destinationMarker = transform.Find("DestinationMarker").gameObject;
    }

    private void Update()
    {
        if (HasAssignedDestination)
        {
            // The destination marker is a child of the zombie object. We need to prevent it from moving
            // with the zombie object.
            destinationMarker.transform.position = destination;
        }
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
        HasAssignedDestination = false;
        destination = rigidBody.position + destinationDelta;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        UpdateDestination();
    }
}
