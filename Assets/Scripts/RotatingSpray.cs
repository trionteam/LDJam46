using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSpray : MonoBehaviour
{
	public GameObject Cone;
	public GameObject CloudPrefab;
	public float RotationSpeed = 100;   // rotation speed in degrees per second
	public float SpraySpeed = 2;
	public float Radius = 3;    // radius in units
	public int BurstSize = 10;

    public float BurstSpreadDegrees = 45.0f;

	public float SprayCooldownTime = 1;
	private float sprayCooldown = 1;

    // Start is called before the first frame update
    void Start()
    {
		Debug.Assert(null != Cone);
		Debug.Assert(null != CloudPrefab);
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
		// TODO - don't set every frame
		float r = Radius * 2;
		Cone.transform.localScale = new Vector3(r, r, r);
		sprayCooldown -= Time.deltaTime;

		//DebugDraw();
		Collider2D col2 = Cone.GetComponent<Collider2D>();
		List<Collider2D> colliders = new List<Collider2D>();
		col2.GetContacts(colliders);
		if (colliders.Count > 0)
		{
			Vector3 colPos = colliders[0].transform.position;
			Vector2 toCollider = (colPos - Cone.transform.position);
			Cone.transform.LookAt(Cone.transform.position + Vector3.forward, toCollider);

			if (sprayCooldown <= 0)
			{
				// shoot!
				sprayCooldown = SprayCooldownTime;

				Vector3 pos = transform.position;
				for (uint i = 0; i < BurstSize; ++i)
				{
                    var spreadAngle = Random.Range(-BurstSpreadDegrees / 2.0f, BurstSpreadDegrees / 2.0f);
                    Vector3 toColliderNorm = Quaternion.Euler(0.0f, 0.0f, spreadAngle) * (colPos - pos).normalized;
					GameObject g = GameObject.Instantiate(CloudPrefab, pos, Quaternion.identity);
					g.GetComponent<CloudScript>().Init(toColliderNorm * SpraySpeed);
					pos += (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * 0.05f + toColliderNorm * 0.1f);
				}
			}
		}
		else
		{
			Cone.transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);
		}
	}
}
