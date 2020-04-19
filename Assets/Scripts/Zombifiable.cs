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

    public MonoBehaviour immuneBehaviour;
    public Color immuneColor = Color.blue;
    public float immuneStateDuration = 5.0f;

    public SpriteRenderer[] coloredSprites;

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
        switch (CurrentState)
        {
            case State.Immune:
                if (Time.time > currentStateTimestamp + immuneStateDuration)
                {
                    CurrentState = State.Normal;
                }
                break;
            case State.Zombie:
                if (Time.time > currentStateTimestamp + zombieStateDuration)
                {
                    CurrentState = State.Normal;
                }
                break;
        }
    }

    private void OnDestroy()
	{
		Score.Instance?.UnregisterActor(this);
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

        Color newColor = Color.white;
        switch (CurrentState)
        {
            case State.Immune:
                immuneBehaviour.enabled = true;
                newColor = immuneColor;
                break;
            case State.Normal:
                normalBehaviour.enabled = true;
                newColor = normalColor;
                break;
            case State.Zombie:
                zombieBehaviour.enabled = true;
                newColor = zombieColor;
				ShakeMgr.Instance?.Shake();
                break;
        }
        foreach(var sprite in coloredSprites)
        {
            sprite.color = newColor;
        }
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