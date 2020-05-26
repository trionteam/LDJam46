using UnityEngine;

public class RotatingScript : MonoBehaviour
{
    [SerializeField]
	public float _rotSpeed = 10;

    void Update()
    {
		transform.Rotate(Vector3.forward, Time.deltaTime * _rotSpeed);
    }
}
