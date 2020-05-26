using UnityEngine;

public class Ambulance : MonoBehaviour
{
    [SerializeField]
	private Waypoints _waypoints;

    [SerializeField]
	private float _speed = 1;

    [SerializeField]
	private GameObject _cloudPrefab = null;

	private int _n = 0;
	private Rigidbody2D _rigidBody;

    [SerializeField]
	private float _sprayCooldownTime = 3;

	private float _sprayCooldown = 1;

    [SerializeField]
	private float _spraySpeed = 2;

    [SerializeField]
	private int _burstSize = 20;

    [SerializeField]
	private float _burstSpreadDegrees = 30.0f;

	// Start is called before the first frame update
	void Start()
	{
		Debug.Assert(null != _cloudPrefab);
		if (null == _waypoints)
		{
			_waypoints = GameObject.Find("Waypoints")?.GetComponent<Waypoints>();
		}
		_rigidBody = GetComponent<Rigidbody2D>();

	}

	void Update()
    {
		_sprayCooldown -= Time.deltaTime;
		if (_sprayCooldown <= 0)
		{
			// shoot!
			_sprayCooldown = _sprayCooldownTime;

			Vector2 pos = _rigidBody.position;
			Vector2 fwd = _rigidBody.transform.up;

			for (uint i = 0; i < _burstSize; ++i)
			{
				var spreadAngle = Random.Range(-_burstSpreadDegrees, _burstSpreadDegrees);
				float side = (i < _burstSize / 2) ? 1 : -1;
				Vector2 dir = Quaternion.Euler(0.0f, 0.0f, _rigidBody.rotation + 90 * side + spreadAngle) * Vector2.up;

				GameObject g = GameObject.Instantiate(_cloudPrefab, pos + fwd * 0.2f, Quaternion.identity);
				g.GetComponent<CloudScript>().Init((fwd * 0.2f + dir) * _spraySpeed);
				//pos += (new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * 0.05f + dir * 0.1f);
			}

			// TODO - sound wildcards
			SoundMgr.Instance?.Play($"spray_{Random.Range(0, 3)}");
		}

		if (null == _waypoints) return;

		if (_waypoints.Touches(_n, _rigidBody.position))
		{
			_n = (_n + 1) % _waypoints.Transforms.Length;
		}

		Vector2 target = _waypoints.GetPos(_n);
		Vector2 toTarget = (target - _rigidBody.position).normalized;
		float a = Vector2.SignedAngle(Vector2.up, toTarget);
		float la = Mathf.LerpAngle(_rigidBody.rotation, a, 0.1f);
		_rigidBody.MovePosition(_rigidBody.position + toTarget * Time.deltaTime * _speed);
		_rigidBody.MoveRotation(la);

    }
}
