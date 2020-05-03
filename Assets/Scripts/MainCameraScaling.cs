using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScaling : MonoBehaviour
{
    static readonly float TargetAspectRatio = 16.0f / 9.0f;

    public Transform blockerUp;
    public Transform blockerDown;
    public Transform blockerLeft;
    public Transform blockerRight;

    new Camera camera = null;

    public float originalCameraSize = 0.0f;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        Debug.Assert(camera != null);

        Debug.Assert(blockerUp != null);
        Debug.Assert(blockerDown != null);
        Debug.Assert(blockerLeft != null);
        Debug.Assert(blockerRight != null);
    }

    private void Start()
    {
        if (originalCameraSize == 0.0f)
        {
            originalCameraSize = camera.orthographicSize;
        }
    }

    void Update()
    {
        var screenAspect = (float)Screen.width / (float)Screen.height;

        // Screen is wider than we want. We're fine.
        if (screenAspect >= TargetAspectRatio)
        {
            camera.orthographicSize = originalCameraSize;
        }
        else
        {
            var scaling = TargetAspectRatio / screenAspect;
            camera.orthographicSize = scaling * originalCameraSize;
        }

        blockerUp.position = new Vector3(0.0f, originalCameraSize + blockerUp.localScale.y / 2.0f, blockerUp.position.z);
        blockerDown.position = new Vector3(0.0f, -originalCameraSize - blockerDown.localScale.y / 2.0f, blockerDown.position.z);
        blockerLeft.position = new Vector3(-originalCameraSize * TargetAspectRatio - blockerLeft.localScale.x / 2.0f, 0.0f, blockerLeft.position.z);
        blockerRight.position = new Vector3(originalCameraSize * TargetAspectRatio + blockerRight.localScale.x / 2.0f, 0.0f, blockerRight.position.z);
    }
}
