using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBatmanFloat : MonoBehaviour
{
	public float FloatAmp = 5;
	public float FloatSpeed = 5;
	public float SpeedRnd = 0.5f;
    //private Vector3 basePos;
    private float baseY = 0.0f;
	private float actualSpeed = 1;

	void Start()
	{
        var rectTransform = GetComponent<RectTransform>();
        baseY = rectTransform.anchoredPosition.y;
		actualSpeed = FloatSpeed * Random.Range(1.0f - SpeedRnd, 1.0f + SpeedRnd);
	}

	void Update()
	{
        //transform.localPosition = basePos + transform.up * Mathf.Sin(Time.time * actualSpeed) * FloatAmp;
        var rectTransform = GetComponent<RectTransform>();
        var pos = rectTransform.anchoredPosition;
        pos.y = baseY + Mathf.Sin(Time.time * actualSpeed) * FloatAmp;
        rectTransform.anchoredPosition = pos;
    }
}
