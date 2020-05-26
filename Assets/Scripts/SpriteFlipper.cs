using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    [SerializeField]
    public Rigidbody2D _rigidBody;

    [SerializeField]
    public SpriteRenderer _sprite;

    private float _previousX;

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
    }

    void Update()
    {
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
