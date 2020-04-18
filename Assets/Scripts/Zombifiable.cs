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

    public MonoBehaviour immuneBehaviour;
    public Color immuneColor = Color.blue;

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
        zombifiable.CurrentState = (Zombifiable.State)EditorGUILayout.EnumPopup("Current State", zombifiable.CurrentState);
    }
}
#endif