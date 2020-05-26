using UnityEngine;
using UnityEngine.Serialization;

public class RotatingScript : MonoBehaviour
{
    [SerializeField]
    [FormerlySerializedAs("RotSpeed")]
	public float _rotSpeed = 10;

    void Update()
    {
		transform.Rotate(Vector3.forward, Time.deltaTime * _rotSpeed);
    }
}
