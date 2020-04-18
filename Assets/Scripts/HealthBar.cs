using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
	public GameObject Bar;

	public float health = 1;

    // Start is called before the first frame update
    void Start()
    {
		Debug.Assert(null != Bar);
    }

    // Update is called once per frame
    void Update()
    {
		// TODO - don't do every frame
		Bar.transform.localScale = new Vector3(health, 1);
	}
}
