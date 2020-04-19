using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curves : MonoBehaviour
{
	public static Curves Instance;

	public AnimationCurve CloudCurveAlpha = new AnimationCurve();
	public AnimationCurve CloudCurveSize = new AnimationCurve();

	// Start is called before the first frame update
	void Awake()
    {
		Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
