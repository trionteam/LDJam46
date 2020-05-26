using UnityEngine;

public class Floaty : MonoBehaviour
{
    [SerializeField]
	private float _floatDir = 90;

    [SerializeField]
	private float _floatAmp = 0.05f;

    [SerializeField]
	private float _floatSpeed = 5;

    [SerializeField]
	private float _speedRnd = 0.25f;

	private Vector2 _basePos;

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
