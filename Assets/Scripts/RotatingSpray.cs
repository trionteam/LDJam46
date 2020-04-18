using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSpray : MonoBehaviour
{
	public GameObject Cone;
	public float Speed = 100;   // degrees per second
	public float Radius = 10;	// radius in units

    // Start is called before the first frame update
    void Start()
    {
		Debug.Assert(null != Cone);
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
		Vector3 fwd = Cone.transform.up * Radius;
		Quaternion q45 = Quaternion.AngleAxis(45, Vector3.forward);
		Quaternion q_45 = Quaternion.AngleAxis(-45, Vector3.forward);
		Vector3 right = q45 * fwd;
		Vector3 left = q_45 * fwd;
		Debug.DrawLine(Cone.transform.position, Cone.transform.position + fwd, Color.red);
		Debug.DrawLine(Cone.transform.position, Cone.transform.position + right, Color.red);
		Debug.DrawLine(Cone.transform.position, Cone.transform.position + left, Color.red);
	}
}
