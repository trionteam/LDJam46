using UnityEngine;
using UnityEngine.Serialization;

public class FixedSpray : MonoBehaviour
{
    [SerializeField]
    private GameObject _cloudPrefab = null;

    [SerializeField]
    private Transform _cone = null;

    [SerializeField]
    [FormerlySerializedAs("controlPlate")]
    private PressurePlate _controlPlate = null;

    [SerializeField]
    [FormerlySerializedAs("SpraySpeed")]
    private float _spraySpeed = 2.0f;

    [SerializeField]
    [FormerlySerializedAs("SprayPeriod")]
    private float _sprayPeriod = 5.0f;

    [SerializeField]
    [FormerlySerializedAs("BurstSize")]
    private int _burstSize = 10;

    [SerializeField]
    [FormerlySerializedAs("BurstSpreadDegrees")]
    private float _burstSpreadDegrees = 45.0f;

    private float _lastBurstTime;

    private void Awake()
    {
        Debug.Assert(_cloudPrefab != null);
    }

    private void Start()
    {
        _lastBurstTime = Time.time;
    }

    Vector3 GetDirection()
    {
        return _cone.transform.up;
    }

    void Update()
    {
        if (Time.time > _lastBurstTime + _sprayPeriod)
        {
            if (_controlPlate != null && !_controlPlate.IsPressed) return;

            var pos = _cone.transform.position;
            for (int i = 0; i < _burstSize; ++i)
            {
                var spreadAngle = Random.Range(-_burstSpreadDegrees / 2.0f, _burstSpreadDegrees / 2.0f);
                Vector3 direction = Quaternion.Euler(0.0f, 0.0f, spreadAngle) * GetDirection().normalized;
                GameObject g = GameObject.Instantiate(_cloudPrefab, pos, Quaternion.identity);
                g.GetComponent<CloudScript>().Init(direction * _spraySpeed);
                pos += (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * 0.05f + direction * 0.1f);
            }

            // TODO - sound wildcards
            SoundMgr.Instance?.Play($"spray_{Random.Range(0, 2)}");
            _lastBurstTime = Time.time;
        }
    }
}
