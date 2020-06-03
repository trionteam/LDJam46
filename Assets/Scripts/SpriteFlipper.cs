using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    [SerializeField]
    public Rigidbody2D _rigidBody;

    [SerializeField]
    public SpriteRenderer _sprite;

    [SerializeField]
    public float _minTimeBetweenFlips = 0.3f;

    private float _previousX;

    private float _earliestNextFlipTime;

    private void Awake()
    {
        if (_rigidBody == null)
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }
        Debug.Assert(_rigidBody != null);

        if (_sprite == null)
        {
            _sprite = GetComponent<SpriteRenderer>();
        }
        Debug.Assert(_sprite != null);
    }

    private void Start()
    {
        _previousX = transform.position.x;
        _earliestNextFlipTime = Mathf.NegativeInfinity;
    }

    void Update()
    {
        if (_earliestNextFlipTime > Time.time) return;

        _earliestNextFlipTime = Time.time + _minTimeBetweenFlips;
        var delta = transform.position.x - _previousX;
        if (delta < 0.0f)
        {
            _sprite.flipX = false;
        }
        else if (delta > 0.0f)
        {
            _sprite.flipX = true;
        }
        _previousX = transform.position.x;
    }
}
