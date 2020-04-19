using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
	private Transform[] transforms;

	public Transform[] Transforms {
		get { return transforms; }
	}

	void Start()
	{
		transforms = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; ++i)
		{
			transforms[i] = transform.GetChild(i);
			transforms[i].GetComponent<Renderer>().enabled = false;
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
	// Returns a position between waypoints (t goes from 0.0 to (Transforms.Length - 1.0f))
	public Vector2 GetPos(int i)
	{
		return Transforms[i].position;
	}

	// Returns a position between waypoints (t goes from 0.0 to (Transforms.Length - 1.0f))
	public Vector2 GetPos(float t)
	{
		float f = Mathf.Floor(t);
		float c = Mathf.Ceil(t);
		if (f == c) return Transforms[(int)f].position;
		return Vector2.Lerp(Transforms[(int)f].position, Transforms[(int)c].position, c - f);
	}

	public bool Touches(int i, Vector2 pos, float r = 0.1f)
	{
		Debug.Assert(i < Transforms.Length);
		Vector2 p = Transforms[i].position;
		return (p - pos).magnitude < r;
	}
#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		for (int i = 0; i < transform.childCount; ++i)
		{
			int next = (i + 1) % transform.childCount;
			Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(next).position);
		}

		Gizmos.color = Color.white;
	}
#endif

}
