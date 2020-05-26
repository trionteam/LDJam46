using UnityEngine;

public class ZombieController : MonoBehaviour
{
    /// <summary>
    /// The destination towards which the zombie is moving.
    /// </summary>
    private Vector2 destination;

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
    private bool _assignedDestination = false;

    [SerializeField]
    private float _randomMovementDestinationRadius = 1.0f;

    [SerializeField]
    private float _destinationResetRadius = 0.01f;

    [SerializeField]
    private float _movementSpeed = 1.0f;

    [SerializeField]
    private bool _coughEnabled = true;

    [SerializeField]
    private GameObject _cloudPrefab = null;

    [SerializeField]
    private int _numCoughs = 4;

    [SerializeField]
    private float _coughTimeout = 5.0f;

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
            _zombieControls.SelectedZombie(this, value);
        }
    }
    private bool _isSelected = false;

    /// <summary>
    /// True if the mouse cursor is over the zombie. When true, the selection marker is
    /// displayed, but with a lower alpha than when the zombie is actually selected.
    /// Setting this property shows or hides the selection marker.
    /// 
    /// The detection of zombies under the mouse cursor is done in <c href="ZombieControls"/>.
    /// </summary>
    public bool IsHovered
    {
        get => _isHovered;
        set
        {
            _isHovered = value;
            UpdateMarkers();
        }
    }
    private bool _isHovered = false;

    /// <summary>
    /// The marker displayed when the zombie is selected or the mouse is hovering over it.
    /// </summary>
    [SerializeField]
    private MarkerController _selectionMarker = null;
    /// <summary>
    /// The marker displayed at the destination of the zombie.
    /// </summary>
    [SerializeField]
    private MarkerController _destinationMarker = null;

    private Rigidbody2D _rigidBody;

    private ZombieControls _zombieControls;

    public void AssignDestination(Vector2 destination)
    {
        HasAssignedDestination = true;
        this.destination = destination;
    }

    private void OnEnable()
    {
        UpdateMarkers();
        UpdateDestination();
        // Zombies should not cough immediately after getting infected.
        coughLeft = Random.Range(0.0f, _coughTimeout);
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
        if (IsSelected)
        {
            _selectionMarker.CurrentState = MarkerController.State.Active;
        }
        else if (IsHovered)
        {
            _selectionMarker.CurrentState = MarkerController.State.Transparent;
        }
        else
        {
            _selectionMarker.CurrentState = MarkerController.State.Hidden;
        }

        if (enabled && HasAssignedDestination)
        {
            _destinationMarker.CurrentState =
                IsSelected ? MarkerController.State.Active : MarkerController.State.Transparent;
        }
        else
        {
            _destinationMarker.CurrentState = MarkerController.State.Hidden;
        }
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        Debug.Assert(_rigidBody != null);
        Debug.Assert(_cloudPrefab != null);

        _zombieControls = GameObject.FindObjectOfType<ZombieControls>();
        Debug.Assert(_zombieControls != null);

        Debug.Assert(_selectionMarker != null);
        Debug.Assert(_destinationMarker != null);
    }

    private void Update()
    {
        if (HasAssignedDestination)
        {
            // The destination marker is a child of the zombie object. We need to prevent it from moving
            // with the zombie object.
            _destinationMarker.transform.position = destination;
        }

        Cough();
    }

    private void FixedUpdate()
    {
        if (IsAtDestination())
        {
            UpdateDestination();
        }

        var directionToDestination = (destination - _rigidBody.position).normalized;
        var desiredPositionInFrame = _rigidBody.position + _movementSpeed * Time.fixedDeltaTime * directionToDestination;
        _rigidBody.MovePosition(desiredPositionInFrame);
    }

    private bool IsAtDestination()
    {
        var distance = (_rigidBody.position - destination).magnitude;
        return distance <= _destinationResetRadius;
    }

    private void UpdateDestination()
    {
        var destinationDelta = _randomMovementDestinationRadius * Random.insideUnitCircle;
        HasAssignedDestination = false;
        destination = _rigidBody.position + destinationDelta;
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
        if (!_coughEnabled) return;

        coughLeft -= Time.deltaTime;
        if (coughLeft <= 0)
        {
            for (int i = 0; i < _numCoughs; ++i)
            {
                GameObject cough = GameObject.Instantiate(_cloudPrefab, transform.position, Quaternion.identity);
                // cough direction is in the general movement direction
                Vector2 directionToDestination = (destination - _rigidBody.position).normalized;
                Vector2 rndDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * 0.1f + directionToDestination;
                rndDir = rndDir.normalized * Random.Range(0.8f, 1.2f);
                Debug.DrawLine(_rigidBody.position, _rigidBody.position + rndDir);
                var cloud = cough.GetComponent<CloudScript>();
                cloud.Init(rndDir);
                cloud.SetSourceZombie(gameObject);
            }

            // TODO - sound wildcards
            SoundMgr.Instance?.Play($"cough_{Random.Range(0, 5)}");

            coughLeft = _coughTimeout + Random.Range(-0.1f, 1.0f);
        }
    }
}
