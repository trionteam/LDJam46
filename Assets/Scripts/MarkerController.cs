using UnityEngine;
using UnityEngine.Serialization;

public class MarkerController : MonoBehaviour
{
    /// <summary>
    /// The state of the marker; controls how the marker is rendered.
    /// </summary>
    public enum State
    {
        /// <summary>
        /// The marker is hidden.
        /// </summary>
        Hidden,
        /// <summary>
        /// The marker is visible, but it is transparent.
        /// </summary>
        Transparent,
        /// <summary>
        /// The marker is fully visible.
        /// </summary>
        Active
    }

    /// <summary>
    /// The alpha of the marker when it is in the active state.
    /// </summary>
    [SerializeField]
    [FormerlySerializedAs("activeAlpha")]
    private float _activeAlpha = 1.0f;

    /// <summary>
    /// The alpha of the marker when it is in the transparent state.
    /// </summary>
    [SerializeField]
    [FormerlySerializedAs("transparentAlpha")]
    private float _transparentAlpha = 0.5f;

    /// <summary>
    /// The current state of the marker.
    /// </summary>
    public State CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            UpdateRendering();
        }
    }
    private State _currentState;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        Debug.Assert(_renderer != null);
    }

    private void UpdateRendering()
    {
        switch (CurrentState)
        {
            case State.Hidden:
                _renderer.enabled = false;
                break;
            case State.Transparent:
                {
                    _renderer.enabled = true;
                    var color = _renderer.color;
                    color.a = _transparentAlpha;
                    _renderer.color = color;
                }
                break;
            case State.Active:
                {
                    _renderer.enabled = true;
                    var color = _renderer.color;
                    color.a = _activeAlpha;
                    _renderer.color = color;
                }
                break;
        }
    }
}
