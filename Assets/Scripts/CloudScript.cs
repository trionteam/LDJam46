using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    [SerializeField]
    private Zombifiable.State _causedState = Zombifiable.State.Zombie;

    [SerializeField]
    private List<Zombifiable.State> _acceptedStates = null;

    private GameObject _source;

    [SerializeField]
	private float _lifeTime = 5;  // seconds

    [SerializeField]
	private float _lifeLengthRandomness = 0.25f;

    [SerializeField]
	private float _rotationSpeed = 100;

	private Vector3 _velocity = Vector3.zero;

	private float _lifeLeft = 0;
	private float _actualLifeTime = 0;
	private SpriteRenderer _spriteRenderer;
	private float _rot = 1;
	private float _scale = 1;

	public void Init(Vector2 vel)
	{
		_velocity = vel;
		_rot = Random.Range(0, 2) == 0 ? -1.0f : 1.0f;
		_rot *= Random.Range(0.5f, 2.5f) * _rotationSpeed;
	}

    public void SetSourceZombie(GameObject source)
    {
        this._source = source;
    }

    // Start is called before the first frame update
    void Start()
    {
		_actualLifeTime = _lifeLeft = _lifeTime + Random.Range(-1.0f, 1.0f) * _lifeLengthRandomness;
		_spriteRenderer = GetComponent<SpriteRenderer>();

		// add a bit of variation
		float H, S, V;
		Color.RGBToHSV(_spriteRenderer.color, out H, out S, out V);
		H += Random.Range(-0.1f, 0.1f);
		_spriteRenderer.color = Color.HSVToRGB(H, S, V);

		_scale = Random.Range(0.8f, 1.2f);
		transform.localRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
		transform.localScale = Vector3.zero;
	}

    // Update is called once per frame
    void Update()
    {
		_lifeLeft -= Time.deltaTime;

		transform.position += _velocity * Time.deltaTime;
		transform.Rotate(Vector3.forward, _rot * Time.deltaTime);

		float normalizedLifeTime = (_actualLifeTime - _lifeLeft) / _actualLifeTime;

		Color c = _spriteRenderer.color;
		
		if (_lifeLeft <= 0)
		{
			Destroy(gameObject);
			return;
		}

		c.a = Curves.Instance?.CloudCurveAlpha.Evaluate(normalizedLifeTime) ?? 1.0f;
		_spriteRenderer.color = c;

		float s = _scale * (Curves.Instance?.CloudCurveSize.Evaluate(normalizedLifeTime) ?? 1.0f);
		transform.localScale = Vector3.one * s;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy the cloud if it hit a barrier.
        if (collision.gameObject.layer == Layers.SprayBarrier)
        {
            Destroy(gameObject);
            return; 
        }

        var zombifiable = collision.GetComponentInChildren<Zombifiable>() ?? collision.GetComponentInParent<Zombifiable>();
        if (zombifiable != null && zombifiable.gameObject != _source)
        {
            if (_acceptedStates.Contains(zombifiable.CurrentState))
            {
                zombifiable.CurrentState = _causedState;
            }
			// Remove itself on collision with a person.
			Destroy(gameObject);
        }
    }
}
