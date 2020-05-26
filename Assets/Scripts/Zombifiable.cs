using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Switches between different behaviours of the object depending on its health state. The
/// behaviour corresponding to the current state is activated, all other behaviours are
/// deactivated.
/// </summary>
public class Zombifiable : MonoBehaviour
{
    // The current state of the object.
    public enum State
    {
        Normal,
        Zombie,
        Immune,
    }

    public State CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState == value) return;
            _currentState = value;
            currentStateTimestamp = Time.fixedTime;
            UpdateBehavioursFromState();
        }
    }
    [SerializeField, HideInInspector]
    private State _currentState;

    private float currentStateTimestamp;

    [SerializeField]
    [FormerlySerializedAs("normalBehaviour")]
    private MonoBehaviour _normalBehaviour = null;

    [SerializeField]
    [FormerlySerializedAs("normalColor")]
    private Color _normalColor = Color.white;

    [SerializeField]
    [FormerlySerializedAs("_zombieBehaviour")]
    private MonoBehaviour zombieBehaviour = null;

    [SerializeField]
    [FormerlySerializedAs("zombieColor")]
    private Color _zombieColor = Color.green;

    [SerializeField]
    [FormerlySerializedAs("zombieStateDuration")]
    private float _zombieStateDuration = 5.0f;

    [SerializeField]
    [FormerlySerializedAs("zombieColorWeaker")]
    private Color _zombieColorWeaker = Color.green;

    [SerializeField]
    [FormerlySerializedAs("zombieColorWeakerTime")]
    private float _zombieColorWeakerTime = 5.0f;

    [SerializeField]
    [FormerlySerializedAs("zombieColorWeakest")]
    private Color _zombieColorWeakest = Color.green;

    [SerializeField]
    [FormerlySerializedAs("zombieColorWeakestTime")]
    private float _zombieColorWeakestTime = 1.0f;

    [SerializeField]
    [FormerlySerializedAs("immuneBehaviour")]
    private MonoBehaviour _immuneBehaviour = null;

    [SerializeField]
    [FormerlySerializedAs("immuneColor")]
    private Color _immuneColor = Color.blue;

    [SerializeField]
    [FormerlySerializedAs("immuneStateDuration")]
    private float _immuneStateDuration = 5.0f;

    [SerializeField, Tooltip("The list of sprite renderers that need to be colored when the current state changes.")]
    [FormerlySerializedAs("coloredSprites")]
    private SpriteRenderer[] _coloredSprites = null;

    public bool SwitchingLocked = false;

    private void Awake()
    {
        Debug.Assert(_normalBehaviour != null);
        Debug.Assert(zombieBehaviour != null);
        Debug.Assert(_immuneBehaviour != null);
    }

    private void Start()
    {
        currentStateTimestamp = Time.time;
        UpdateBehavioursFromState();

        Score.Instance?.RegisterActor(this);
    }

    private void Update()
    {
        float stateDuration = float.PositiveInfinity;
        switch (CurrentState)
        {
            case State.Immune:
                stateDuration = _immuneStateDuration;
                break;
            case State.Zombie:
                stateDuration = _zombieStateDuration;
                break;
        }
        float endStateTime = currentStateTimestamp + stateDuration;
        if (endStateTime < Time.time)
        {
            CurrentState = State.Normal;
        }
        UpdateSpriteColor();
    }

    private void OnDestroy()
    {
        Score.Instance?.UnregisterActor(this);
    }

    private void UpdateSpriteColor()
    {
        Color newColor = Color.white;
        switch (CurrentState)
        {
            case State.Immune:
                newColor = _immuneColor;
                break;
            case State.Normal:
                newColor = _normalColor;
                break;
            case State.Zombie:
                float elapsedTime = Time.time - currentStateTimestamp;
                if (elapsedTime >= _zombieColorWeakestTime)
                {
                    newColor = _zombieColorWeakest;
                }
                else if (elapsedTime >= _zombieColorWeakerTime)
                {
                    newColor = _zombieColorWeaker;
                } 
                else
                {
                    newColor = _zombieColor;
                }
                break;
        }
        foreach (var sprite in _coloredSprites)
        {
            sprite.color = newColor;
        }

    }

    private void UpdateBehavioursFromState()
    {
        Score.Instance?.UpdateUI();

        if (Application.isPlaying)
        {
            _normalBehaviour.enabled = false;
            zombieBehaviour.enabled = false;
            _immuneBehaviour.enabled = false;
        }
        switch (CurrentState)
        {
            case State.Immune:
                _immuneBehaviour.enabled = true;
                break;
            case State.Normal:
                _normalBehaviour.enabled = true;
                break;
            case State.Zombie:
                zombieBehaviour.enabled = true;
                ShakeMgr.Instance?.Shake();
                break;
        }
        UpdateSpriteColor();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Zombifiable))]
public class ZombifiableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var zombifiable = serializedObject.targetObject as Zombifiable;
        var newState = (Zombifiable.State)EditorGUILayout.EnumPopup("Current State", zombifiable.CurrentState);
        if (newState != zombifiable.CurrentState)
        {
            zombifiable.CurrentState = newState;
            EditorUtility.SetDirty(zombifiable);
        }
    }
}
#endif