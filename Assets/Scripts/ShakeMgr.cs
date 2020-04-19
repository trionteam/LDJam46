using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeMgr : MonoBehaviour
{
	public static ShakeMgr Instance;
	private Camera mainCam = null;
	private float shake = -1;

	[Header("Effects")]
	public float ShakeRot = 10;
	public float ShakeMove = 2;

	void Awake()
    {
		Instance = this;
    }

	private void Start()
	{
		mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();
		Debug.Assert(null != mainCam);
	}

	void Update()
    {
		if (null == mainCam) return;

		Vector3 lea = mainCam.transform.localRotation.eulerAngles;
		if (shake > 0)
		{
			shake -= Time.deltaTime;
			Vector3 ShakeOff = new Vector3(0, Mathf.PerlinNoise(Time.fixedTime * 30, 0) * 2 - 1, Mathf.PerlinNoise(0, Time.fixedTime * 20) * 2 - 1);
			float ShakeAngle = Mathf.PerlinNoise(Time.fixedTime * 10, Mathf.Cos(Time.fixedTime)) * 2 - 1;
			//mainCam.transform.position += ShakeOff * shake * shake * shake * ShakeMove;

			lea.z = ShakeAngle * ShakeRot * shake * shake * shake;
			if (shake <= 0)
			{
				shake = -1;
				lea.z = 0;
			}
		}
		mainCam.transform.localRotation = Quaternion.Euler(lea);

	}
	public void Shake(float howMuch = 0.5f)
	{
		if (shake <= 0)
		{
			shake = howMuch;
		}
	}
}
