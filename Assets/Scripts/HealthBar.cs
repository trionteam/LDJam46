using UnityEngine;
using UnityEngine.Serialization;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    [FormerlySerializedAs("Bar")]
	private GameObject _bar = null;

    [SerializeField]
    [FormerlySerializedAs("health")]
	private float _health = 1;

    // Start is called before the first frame update
    private void Start()
    {
		Debug.Assert(null != _bar);
    }

    // Update is called once per frame
    private void Update()
    {
		// TODO - don't do every frame
		_bar.transform.localScale = new Vector3(_health, 1);
	}
}
