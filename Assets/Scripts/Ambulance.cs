using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambulance : MonoBehaviour
{
	public Waypoints waypoints;
	public float Speed = 1;
	public GameObject CloudPrefab;

	private float next = 0;
	private int n = 0;
	private Rigidbody2D rigidBody;

	public float SprayCooldownTime = 3;
	private float sprayCooldown = 1;

	public float SpraySpeed = 2;
	public int BurstSize = 20;
	public float BurstSpreadDegrees = 30.0f;

	// Start is called before the first frame update
	void Start()
	{
		Debug.Assert(null != CloudPrefab);
		if (null == waypoints)
		{
			waypoints = GameObject.Find("Waypoints")?.GetComponent<Waypoints>();
		}
		rigidBody = GetComponent<Rigidbody2D>();

	}

	void Update()
    {
		sprayCooldown -= Time.deltaTime;
		if (sprayCooldown <= 0)
		{
			// shoot!
			sprayCooldown = SprayCooldownTime;

			Vector2 pos = rigidBody.position;
			Vector2 fwd = rigidBody.transform.up;

			for (uint i = 0; i < BurstSize; ++i)
			{
				var spreadAngle = Random.Range(-BurstSpreadDegrees, BurstSpreadDegrees);
				float side = (i < BurstSize / 2) ? 1 : -1;
				Vector2 dir = Quaternion.Euler(0.0f, 0.0f, rigidBody.rotation + 90 * side + spreadAngle) * Vector2.up;

				GameObject g = GameObject.Instantiate(CloudPrefab, pos + fwd * 0.2f, Quaternion.identity);
				g.GetComponent<CloudScript>().Init((fwd * 0.2f + dir) * SpraySpeed);
				//pos += (new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * 0.05f + dir * 0.1f);
			}

			// TODO - sound wildcards
			SoundMgr.Instance?.Play($"spray_{Random.Range(0, 2)}");
		}

		if (null == waypoints) return;

		if (waypoints.Touches(n, rigidBody.position))
		{
			n = (n + 1) % waypoints.Transforms.Length;
		}


		next += Time.deltaTime;
		Vector2 target = waypoints.GetPos(n);
		Vector2 toTarget = (target - rigidBody.position).normalized;
		float a = Vector2.SignedAngle(Vector2.up, toTarget);
		float la = Mathf.LerpAngle(rigidBody.rotation, a, 0.1f);
		rigidBody.MovePosition(rigidBody.position + toTarget * Time.deltaTime * Speed);
		rigidBody.MoveRotation(la);

    }
}
