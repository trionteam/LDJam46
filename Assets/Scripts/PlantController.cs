using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    /// <summary>
    /// Prefab for the clouds emitted by the plant.
    /// </summary>
    [SerializeField]
    private GameObject _cloudPrefab = null;

    /// <summary>
    /// The cone that determines the direction in which the plan sprays.
    /// </summary>
    [SerializeField]
    private Transform _cone = null;

    /// <summary>
    /// The speed of the cloud particles.
    /// </summary>
    [SerializeField]
    private float _spraySpeed = 2.0f;

    /// <summary>
    /// The number of cloud particles in one burst.
    /// </summary>
    [SerializeField]
    private int _burstSize = 10;

    /// <summary>
    /// The spread of the burst, in degrees.
    /// </summary>
    [SerializeField]
    private float _burstSpreadDegrees = 30.0f;

    private void Awake()
    {
        Debug.Assert(_cloudPrefab != null);
        Debug.Assert(_cone != null);
    }

    /// <summary>
    /// Emit sprays from the plan in the "up" direction of the cone. This is
    /// intended to be called from an animation event.
    /// </summary>
    private void EmitSprays()
    {
        var position = _cone.position;
        for (int i = 0; i < _burstSize; ++i)
        {
            var spreadAngle = Random.Range(-_burstSpreadDegrees / 2.0f,
                                           _burstSpreadDegrees / 2.0f);
            var direction = Quaternion.Euler(0.0f, 0.0f, spreadAngle) * _cone.up;
            var cloud = GameObject.Instantiate(_cloudPrefab, position, Quaternion.identity);
            cloud.GetComponent<CloudScript>().Init(direction * _spraySpeed);
            position += (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * 0.05f + direction * 0.1f);
        }
    }
}
