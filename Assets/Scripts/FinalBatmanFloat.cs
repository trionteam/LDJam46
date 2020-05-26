using UnityEngine;

public class FinalBatmanFloat : MonoBehaviour
{
    [SerializeField]
	private float _floatAmp = 5;

    [SerializeField]
	private float _floatSpeed = 5;

    [SerializeField]
	private float _speedRnd = 0.5f;

    private float _baseY = 0.0f;
	private float _actualSpeed = 1;

	void Start()
	{
        var rectTransform = GetComponent<RectTransform>();
        _baseY = rectTransform.anchoredPosition.y;
		_actualSpeed = _floatSpeed * Random.Range(1.0f - _speedRnd, 1.0f + _speedRnd);
	}

	void Update()
	{
        var rectTransform = GetComponent<RectTransform>();
        var pos = rectTransform.anchoredPosition;
        pos.y = _baseY + Mathf.Sin(Time.time * _actualSpeed) * _floatAmp;
        rectTransform.anchoredPosition = pos;
    }
}
