using UnityEngine;

public class ZombieController : MonoBehaviour
{
    /// <summary>
    /// The destination towards which the zombie is moving.
    /// </summary>
    private Vector2 _destination;

    /// <summary>
    /// A healthy person that is the target of the zombie. When set, the zombie follows
    /// the target and coughs at it, even if the target moves.
    /// </summary>
    private HealthyPersonController _target;

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
            UpdateAnimation();
            UpdateMarkers();
        }
    }
    private bool _assignedDestination = false;

    [SerializeField]
    private float _randomMovementDestinationRadius = 1.0f;

    [SerializeField]
    private float _destinationResetRadius = 0.01f;

    /// <summary>
    /// When the zombie has a healthy person as a target, it will not get closer than
    /// this distance. Below this distance, the zombie will only cough at the target.
    /// </summary>
    [SerializeField]
    private float _desiredTargetDistance = 0.5f;

    [SerializeField]
    [Tooltip("The movement speed when the zombie has an assigned desination.")]
    private float _movementSpeed = 1.0f;

    [SerializeField]
    [Tooltip("The movement speed when the zombie is just moving around without a destination.")]
    private float _roamingSpeed = 0.1f;

    [SerializeField]
    [Tooltip("The value assigned to Animator.speed on this object when the character is moving towards an assigned destination.")]
    private float _movementAnimationSpeed = 1.0f;

    [SerializeField]
    [Tooltip("The value assigned to Animator.speed on this object when the character is just moving around without a destination.")]
    private float _roamingAnimationSpeed = 0.2f;

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

    private Animator _spriteAnimation;

    private ZombieControls _zombieControls;

    /// <summary>
    /// Assigns a destination to the zombie. The destination is a fixed position towards
    /// which the zombie is supposed to move. Setting a destination resets any previous
    /// destination or target assigned to the zombie.
    /// </summary>
    /// <param name="destination">The destination position for the zombie.</param>
    public void AssignDestination(Vector2 destination)
    {
        HasAssignedDestination = true;
        this._destination = destination;
        this._target = null;
    }

    /// <summary>
    /// Assigns a target to the zombie. The target is a healthy person the zombie is
    /// supposed to follow. Setting a target resets any previous targets or destinations
    /// assigned to the zombie.
    /// </summary>
    /// <param name="target">The healthy person the zombie is supposed to follow.</param>
    public void AssignTarget(HealthyPersonController target)
    {
        HasAssignedDestination = true;
        this._target = target;
        this._destination = target.transform.position;
    }

    private void OnEnable()
    {
        UpdateMarkers();
        UpdateDestination();
        UpdateAnimation();
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

    private void UpdateAnimation()
    {
        if (_spriteAnimation != null)
        {
            _spriteAnimation.speed = HasAssignedDestination ? _movementAnimationSpeed : _roamingAnimationSpeed;
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

        _spriteAnimation = GetComponent<Animator>();
    }

    private void Update()
    {
        if (HasAssignedDestination)
        {
            // The destination marker is a child of the zombie object. We need to prevent it from moving
            // with the zombie object.
            _destinationMarker.transform.position = _destination;
        }

        Cough();
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            if (!_target.enabled)
            {
                // The target is no longer healthy, let's move back to wandering around.
                _target = null;
                UpdateDestination();
            }
            else
            {
                // Update the actual destination coordinates based on the current position
                // of the target.
                _destination = _target.transform.position;
            }
        }
        else if (IsAtDestination())
        {
            // This is called only when the target is not assigned - we do not want to reset
            // the target if the zombie gets too close, but the target is not yet infected.
            UpdateDestination();
        }

        // Move towards the destination. When following a target, do not move if the target is
        // too close.
        if (_target == null ||
            Vector3.Distance(transform.position, _target.transform.position) > _desiredTargetDistance)
        {
            var speed = HasAssignedDestination ? _movementSpeed : _roamingSpeed;
            var directionToDestination = (_destination - _rigidBody.position).normalized;
            var desiredPositionInFrame = _rigidBody.position + speed * Time.fixedDeltaTime * directionToDestination;
            _rigidBody.MovePosition(desiredPositionInFrame);
        }
    }

    private bool IsAtDestination()
    {
        var distance = (_rigidBody.position - _destination).magnitude;
        return distance <= _destinationResetRadius;
    }

    private void UpdateDestination()
    {
        var destinationDelta = _randomMovementDestinationRadius * Random.insideUnitCircle;
        HasAssignedDestination = false;
        _destination = _rigidBody.position + destinationDelta;
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
                Vector2 directionToDestination = (_destination - _rigidBody.position).normalized;
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
