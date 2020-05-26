using UnityEngine;
using UnityEngine.Serialization;

public class Floaty : MonoBehaviour
{
    [SerializeField]
    [FormerlySerializedAs("FloatDir")]
	private float _floatDir = 90;

    [SerializeField]
    [FormerlySerializedAs("FloatAmp")]
	private float _floatAmp = 0.05f;

    [SerializeField]
    [FormerlySerializedAs("FloatSpeed")]
	private float _floatSpeed = 5;

    [SerializeField]
    [FormerlySerializedAs("SpeedRnd")]
	private float _speedRnd = 0.25f;

    [SerializeField]
    [FormerlySerializedAs("basePos")]
	private Vector2 _basePos;

    [SerializeField]
    [FormerlySerializedAs("floatVec")]
	private Vector2 _floatVec;

	private float _actualSpeed = 1;

	void Start()
    {
		_basePos = transform.position;
		float r = Mathf.Deg2Rad * _floatDir;
		_floatVec = new Vector2(Mathf.Cos(r), Mathf.Sin(r)) * _floatAmp;
		_actualSpeed = _floatSpeed * Random.Range(1.0f - _speedRnd, 1.0f + _speedRnd);
    }

    // Update is called once per frame
    void Update()
    {
		transform.position = _basePos + _floatVec * Mathf.Sin(Time.time * _actualSpeed);
    }
}
