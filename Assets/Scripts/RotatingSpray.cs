using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSpray : MonoBehaviour
{
	public GameObject Cone;
	public float Speed = 100;   // degrees per second
	public float Radius = 3;    // radius in units
	public int layerMask = 0x8;

    // Start is called before the first frame update
    void Start()
    {
		Debug.Assert(null != Cone);
	}

	Vector3 GetDir()
	{
		return Cone.transform.up;
	}

	Vector3 GetAxis()
	{
		return Vector3.forward;
	}

    // Update is called once per frame
    void Update()
    {
		Cone.transform.Rotate(Vector3.forward, Speed * Time.deltaTime);

		// TODO - don't set every frame
		float r = Radius * 2;
		Cone.transform.localScale = new Vector3(r, r, r);

		DebugDraw();

	}

	void DebugDraw()
	{
		Vector3 fwd = GetDir() * Radius;
		Quaternion q45 = Quaternion.AngleAxis(45, GetAxis());
		Quaternion q_45 = Quaternion.AngleAxis(-45, GetAxis());
		Vector3 right = q45 * fwd;
		Vector3 left = q_45 * fwd;
		Debug.DrawLine(transform.position, transform.position + fwd, Color.red);
		Debug.DrawLine(transform.position, transform.position + right, Color.red);
		Debug.DrawLine(transform.position, transform.position + left, Color.red);

		// TODO - just a debug, that random is bad, need a "arc query", will figure out later :)
		float rndAngle = Random.Range(-45.0f, 45.0f);
		Vector3 rndDir = Quaternion.AngleAxis(rndAngle, GetAxis()) * fwd;
		RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, rndDir, Radius, layerMask);

		if (hitInfo)
		{
			Debug.DrawLine(transform.position, hitInfo.point, Color.green);
		}
	}
}
