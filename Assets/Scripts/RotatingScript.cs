using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingScript : MonoBehaviour
{
	public float RotSpeed = 10;

    void Update()
    {
		transform.Rotate(Vector3.forward, Time.deltaTime * RotSpeed);
    }
}
