using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public MonoBehaviour normalBehaviour;
    public Color normalColor = Color.white;

    public MonoBehaviour zombieBehaviour;

    public Color zombieColor = Color.green;
    public float zombieStateDuration = 5.0f;
    public Color zombieColorWeaker = Color.green;
    public float zombieColorWeakerTime = 5.0f;
    public Color zombieColorWeakest = Color.green;
    public float zombieColorWeakestTime = 1.0f;

    public MonoBehaviour immuneBehaviour;
    public Color immuneColor = Color.blue;
    public float immuneStateDuration = 5.0f;

    public SpriteRenderer[] coloredSprites;

    public bool switchingLocked = false;

    private void Awake()
    {
        Debug.Assert(normalBehaviour != null);
        Debug.Assert(zombieBehaviour != null);
        Debug.Assert(immuneBehaviour != null);
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
                stateDuration = immuneStateDuration;
                break;
            case State.Zombie:
                stateDuration = zombieStateDuration;
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
                newColor = immuneColor;
                break;
            case State.Normal:
                newColor = normalColor;
                break;
            case State.Zombie:
                float elapsedTime = Time.time - currentStateTimestamp;
                if (elapsedTime >= zombieColorWeakestTime)
                {
                    newColor = zombieColorWeakest;
                }
                else if (elapsedTime >= zombieColorWeakerTime)
                {
                    newColor = zombieColorWeaker;
                } 
                else
                {
                    newColor = zombieColor;
                }
                break;
        }
        foreach (var sprite in coloredSprites)
        {
            sprite.color = newColor;
        }

    }

    private void UpdateBehavioursFromState()
    {
        Score.Instance?.UpdateUI();

        if (Application.isPlaying)
        {
            normalBehaviour.enabled = false;
            zombieBehaviour.enabled = false;
            immuneBehaviour.enabled = false;
        }
        switch (CurrentState)
        {
            case State.Immune:
                immuneBehaviour.enabled = true;
                break;
            case State.Normal:
                normalBehaviour.enabled = true;
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