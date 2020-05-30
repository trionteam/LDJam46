using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif  // UNITY_EDITOR

public class HealthyPersonController : MonoBehaviour
{
    /// <summary>
    /// The destination towards which the zombie is moving.
    /// </summary>
    private Vector2 destination;

    [SerializeField]
    private bool _hasBasePosition = false;

    [SerializeField]
    private Vector2 _basePosition;

    [SerializeField]
    private float _basePositionRadius = 1.0f;

    [SerializeField]
    private float _randomMovementDestinationRadius = 1.0f;

    [SerializeField]
    private float _movementSpeed = 1.0f;

    [SerializeField]
    [Tooltip("The value assigned to Animator.speed on this object when the character is moving around.")]
    private float _movementAnimationSpeed = 1.0f;

    [SerializeField]
    private float _destinationResetRadius = 0.01f;

    [SerializeField]
    private ZombieDetection _zombieDetection = null;

    private Rigidbody2D _rigidBody;

    private Animator _spriteAnimation;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        Debug.Assert(_rigidBody != null);

        _spriteAnimation = GetComponent<Animator>();
    }

    private void Start()
    {
        UpdateDestination();
        UpdateAnimation();
    }

    private void OnEnable()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (_spriteAnimation != null)
        {
            _spriteAnimation.speed = _movementAnimationSpeed;
        }
    }

    private void FixedUpdate()
    {
        Vector2 directionToDestination;
        if (_zombieDetection != null && _zombieDetection.HasZombiesInSight)
        {
            directionToDestination = _zombieDetection.EscapeDirection();
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
            directionToDestination = (destination - _rigidBody.position).normalized;
        }

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
        if (_hasBasePosition)
        {
            var destinationDelta = _basePositionRadius * Random.insideUnitCircle;
            destination = _basePosition + destinationDelta;
        }
        else
        {
            var destinationDelta = _randomMovementDestinationRadius * Random.insideUnitCircle;
            destination = _rigidBody.position + destinationDelta;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        UpdateDestination();
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(HealthyPersonController))]
    [CanEditMultipleObjects]
    public class HealthyPersonEditor : Editor
    {
        public void OnSceneGUI()
        {
            HealthyPersonController person = target as HealthyPersonController;
            if (person == null || !person._hasBasePosition) return;

            EditorGUI.BeginChangeCheck();
            var position = new Vector3(person._basePosition.x, person._basePosition.y);
            float radius = Handles.RadiusHandle(Quaternion.identity, position, person._basePositionRadius);
            position = Handles.PositionHandle(position, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed base area radius and position");
                person._basePositionRadius = radius;
                person._basePosition = new Vector2(position.x, position.y);
            }
        }
    }

#endif  // UNITY_EDITOR
}