using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleController : MonoBehaviour
{
    Quaternion originalRotation;

    public float Amplitude = 0.05f;
    public float WobbleSpeed = 5;
    public float SpeedRnd = 0.25f;
    private float actualSpeed = 1;

    void Start()
    {
        originalRotation = transform.rotation;
        actualSpeed = WobbleSpeed * Random.Range(1.0f - SpeedRnd, 1.0f + SpeedRnd);
    }

    void Update()
    {
        var wobble = Quaternion.Euler(0.0f, 0.0f, Amplitude * Mathf.Sin(Time.time * actualSpeed));
        transform.rotation = wobble * originalRotation;
    }
}
