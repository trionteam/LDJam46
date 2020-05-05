using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedSpray : MonoBehaviour
{
    public GameObject CloudPrefab;
    public Transform Cone;

    public PressurePlate controlPlate;

    public float SpraySpeed = 2.0f;
    public float SprayPeriod = 5.0f;
    public int BurstSize = 10;
    public float BurstSpreadDegrees = 45.0f;

    private float LastBurstTime;

    private void Awake()
    {
        Debug.Assert(CloudPrefab != null);
    }

    private void Start()
    {
        LastBurstTime = Time.time;
    }

    Vector3 GetDirection()
    {
        return Cone.transform.up;
    }

    void Update()
    {
        if (Time.time > LastBurstTime + SprayPeriod)
        {
            if (controlPlate != null && !controlPlate.IsPressed) return;

            var pos = Cone.transform.position;
            for (int i = 0; i < BurstSize; ++i)
            {
                var spreadAngle = Random.Range(-BurstSpreadDegrees / 2.0f, BurstSpreadDegrees / 2.0f);
                Vector3 direction = Quaternion.Euler(0.0f, 0.0f, spreadAngle) * GetDirection().normalized;
                GameObject g = GameObject.Instantiate(CloudPrefab, pos, Quaternion.identity);
                g.GetComponent<CloudScript>().Init(direction * SpraySpeed);
                pos += (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * 0.05f + direction * 0.1f);
            }

            // TODO - sound wildcards
            SoundMgr.Instance?.Play($"spray_{Random.Range(0, 2)}");
            LastBurstTime = Time.time;
        }
    }
}
