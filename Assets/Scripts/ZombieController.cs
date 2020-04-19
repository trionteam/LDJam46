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
            UpdateMarkers();
        }
    }
    public bool _assignedDestination;

    public float randomMovementDestinationRadius = 1.0f;

    public float destinationResetRadius = 0.01f;

    public float movementSpeed = 1.0f;

    public bool coughEnabled = true;
	public GameObject CloudPrefab;
	public int numCoughs = 4;
	public float coughTimeout = 5.0f;
	private float coughLeft = 0;

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
            UpdateMarkers();
            zombieControls.SelectedZombie(this, value);
        }
    }
    private bool _isSelected = false;

    private GameObject selectionMarker;
    private GameObject destinationMarker;

    private Rigidbody2D rigidBody;

    private ZombieControls zombieControls;

    public void AssignDestination(Vector2 destination)
    {
        HasAssignedDestination = true;
        this.destination = destination;
    }

    private void OnEnable()
    {
        UpdateMarkers();
        UpdateDestination();
    }

    private void OnDisable()
    {
        UpdateMarkers();
        // Deselect & revert to random movement when it stops being a zombie.
        IsSelected = false;
        UpdateDestination();
    }

    private void UpdateMarkers()
    {
        selectionMarker.SetActive(enabled && IsSelected);
        destinationMarker.SetActive(enabled && HasAssignedDestination);

        var destinationMarkerSprite = destinationMarker.GetComponent<SpriteRenderer>();
        var destinationMarkerColor = destinationMarkerSprite.color;
        destinationMarkerColor.a = destinationMarker.activeSelf && IsSelected ? 1.0f : 0.5f;
        destinationMarkerSprite.color = destinationMarkerColor;
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        Debug.Assert(rigidBody != null);
		Debug.Assert(CloudPrefab != null);

        zombieControls = GameObject.FindObjectOfType<ZombieControls>();
        Debug.Assert(zombieControls != null);

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

		Cough();
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
        if (!HasAssignedDestination)
        {
            UpdateDestination();
        }
    }

	private void Cough()
	{
        if (!coughEnabled) return;

		coughLeft -= Time.deltaTime;
		if (coughLeft <= 0)
		{
			for (int i = 0; i < numCoughs; ++i)
			{
				GameObject cough = GameObject.Instantiate(CloudPrefab, transform.position, Quaternion.identity);
				// cough direction is in the general movement direction
				Vector2 directionToDestination = (destination - rigidBody.position).normalized;
				Vector2 rndDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * 0.1f + directionToDestination;
				rndDir = rndDir.normalized * Random.Range(0.8f, 1.2f);
				Debug.DrawLine(rigidBody.position, rigidBody.position + rndDir);
				var cloud = cough.GetComponent<CloudScript>();
                cloud.Init(rndDir);
                cloud.SetSourceZombie(gameObject);
			}

			// TODO - wildcards to play a random sound of the selected kind
			SoundMgr.Instance?.Play($"cough_{Random.Range(0, 4)}");

			coughLeft = coughTimeout + Random.Range(-0.1f, 1.0f);
		}
	}
}
