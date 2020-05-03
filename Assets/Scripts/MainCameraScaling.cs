using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScaling : MonoBehaviour
{
    static readonly float TargetAspectRatio = 16.0f / 9.0f;
    static readonly float OriginalCameraSize = 5.0f;
    new Camera camera = null;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        var screenAspect = (float)Screen.width / (float)Screen.height;

        // Screen is wider than we want. We're fine.
        if (screenAspect >= TargetAspectRatio)
        {
            camera.orthographicSize = OriginalCameraSize;
            return;
        }

        var scaling = TargetAspectRatio / screenAspect;
        camera.orthographicSize = scaling * OriginalCameraSize;
    }
}
