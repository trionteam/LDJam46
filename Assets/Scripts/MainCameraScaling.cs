using UnityEngine;
using UnityEngine.Serialization;

public class MainCameraScaling : MonoBehaviour
{
    public static readonly float TargetAspectRatio = 16.0f / 9.0f;

    [SerializeField]
    [FormerlySerializedAs("blockerUp")]
    private Transform _blockerUp = null;

    [SerializeField]
    [FormerlySerializedAs("blockerDown")]
    private Transform _blockerDown = null;

    [SerializeField]
    [FormerlySerializedAs("blockerLeft")]
    private Transform _blockerLeft = null;

    [SerializeField]
    [FormerlySerializedAs("blockerRight")]
    private Transform _blockerRight = null;

    private Camera _camera = null;

    [SerializeField]
    [FormerlySerializedAs("originalCameraSize")]
    public float _originalCameraSize = 0.0f;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        Debug.Assert(_camera != null);

        Debug.Assert(_blockerUp != null);
        Debug.Assert(_blockerDown != null);
        Debug.Assert(_blockerLeft != null);
        Debug.Assert(_blockerRight != null);
    }

    private void Start()
    {
        if (_originalCameraSize == 0.0f)
        {
            _originalCameraSize = _camera.orthographicSize;
        }
    }

    void Update()
    {
        var screenAspect = (float)Screen.width / (float)Screen.height;

        // Screen is wider than we want. We're fine.
        if (screenAspect >= TargetAspectRatio)
        {
            _camera.orthographicSize = _originalCameraSize;
        }
        else
        {
            var scaling = TargetAspectRatio / screenAspect;
            _camera.orthographicSize = scaling * _originalCameraSize;
        }

        _blockerUp.position = new Vector3(0.0f, _originalCameraSize + _blockerUp.localScale.y / 2.0f, _blockerUp.position.z);
        _blockerDown.position = new Vector3(0.0f, -_originalCameraSize - _blockerDown.localScale.y / 2.0f, _blockerDown.position.z);
        _blockerLeft.position = new Vector3(-_originalCameraSize * TargetAspectRatio - _blockerLeft.localScale.x / 2.0f, 0.0f, _blockerLeft.position.z);
        _blockerRight.position = new Vector3(_originalCameraSize * TargetAspectRatio + _blockerRight.localScale.x / 2.0f, 0.0f, _blockerRight.position.z);
    }
}
