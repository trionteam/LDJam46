using UnityEngine;

public class WobbleController : MonoBehaviour
{
    private Quaternion originalRotation;

    [SerializeField]
    private float _amplitude = 0.05f;

    [SerializeField]
    private float _wobbleSpeed = 5;

    [SerializeField]
    private float _speedRnd = 0.25f;

    private float actualSpeed = 1;

    void Start()
    {
        originalRotation = transform.rotation;
        actualSpeed = _wobbleSpeed * Random.Range(1.0f - _speedRnd, 1.0f + _speedRnd);
    }

    void Update()
    {
        var wobble = Quaternion.Euler(0.0f, 0.0f, _amplitude * Mathf.Sin(Time.time * actualSpeed));
        transform.rotation = wobble * originalRotation;
    }
}
