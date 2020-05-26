using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RotatingSpray : MonoBehaviour
{
    [SerializeField]
	private GameObject _cone = null;

    [SerializeField]
    private GameObject _cloudPrefab = null;

    [SerializeField]
    private float _rotationSpeed = 100;   // rotation speed in degrees per second

    [SerializeField]
    [FormerlySerializedAs("SpraySpeed")]
    private float _spraySpeed = 2;

    [SerializeField]
    [FormerlySerializedAs("Radius")]
    private float _radius = 3;    // radius in units

    [SerializeField]
    [FormerlySerializedAs("BurstSize")]
    private int _burstSize = 10;

    [SerializeField]
    [FormerlySerializedAs("BurstSpreadDegrees")]
    private float _burstSpreadDegrees = 45.0f;

    [SerializeField]
    [FormerlySerializedAs("SprayCooldownTime")]
	private float _sprayCooldownTime = 1;

	private float _sprayCooldown = 1;

    // Start is called before the first frame update
    void Start()
    {
		Debug.Assert(null != _cone);
		Debug.Assert(null != _cloudPrefab);
	}

	Vector3 GetDir()
	{
		return _cone.transform.up;
	}

	Vector3 GetAxis()
	{
		return Vector3.forward;
	}

    // Update is called once per frame
    void Update()
    {
		// TODO - don't set every frame
		float r = _radius * 2;
		_cone.transform.localScale = new Vector3(r, r, r);
		_sprayCooldown -= Time.deltaTime;

		//DebugDraw();
		Collider2D col2 = _cone.GetComponent<Collider2D>();
		List<Collider2D> colliders = new List<Collider2D>();
		col2.GetContacts(colliders);
		if (colliders.Count > 0)
		{
			Vector3 colPos = colliders[0].transform.position;
			Vector2 toCollider = (colPos - _cone.transform.position);
			_cone.transform.LookAt(_cone.transform.position + Vector3.forward, toCollider);

			if (_sprayCooldown <= 0)
			{
				// shoot!
				_sprayCooldown = _sprayCooldownTime;

				Vector3 pos = transform.position;
				for (uint i = 0; i < _burstSize; ++i)
				{
                    var spreadAngle = Random.Range(-_burstSpreadDegrees / 2.0f, _burstSpreadDegrees / 2.0f);
                    Vector3 toColliderNorm = Quaternion.Euler(0.0f, 0.0f, spreadAngle) * (colPos - pos).normalized;
					GameObject g = GameObject.Instantiate(_cloudPrefab, pos, Quaternion.identity);
					g.GetComponent<CloudScript>().Init(toColliderNorm * _spraySpeed);
					pos += (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * 0.05f + toColliderNorm * 0.1f);
				}

				// TODO - sound wildcards
				SoundMgr.Instance?.Play($"spray_{Random.Range(0, 2)}");
			}
		}
		else
		{
			_cone.transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
		}
	}
}
