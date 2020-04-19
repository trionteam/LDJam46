using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floaty : MonoBehaviour
{
	public float FloatDir = 90;
	public float FloatAmp = 0.05f;
	public float FloatSpeed = 5;
	public float SpeedRnd = 0.25f;
	private Vector2 basePos;
	private Vector2 floatVec;
	private float actualSpeed = 1;

	void Start()
    {
		basePos = transform.position;
		float r = Mathf.Deg2Rad * FloatDir;
		floatVec = new Vector2(Mathf.Cos(r), Mathf.Sin(r)) * FloatAmp;
		actualSpeed = FloatSpeed * Random.Range(1.0f - SpeedRnd, 1.0f + SpeedRnd);
    }

    // Update is called once per frame
    void Update()
    {
		transform.position = basePos + floatVec * Mathf.Sin(Time.time * actualSpeed);
    }
}
