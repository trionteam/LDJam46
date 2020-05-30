using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains information about a single level in the level list.
/// </summary>
[CreateAssetMenu(menuName = "Levels/Level metadata", fileName = "Level0")]
public class LevelMetadata : ScriptableObject
{
    [SerializeField]
    private string _levelName;

    [SerializeField]
    private string _sceneName;

    /// <summary>
    /// A human-readable name of the level displayed in the UI.
    /// </summary>
    public string LevelName { get => _levelName; }

    /// <summary>
    /// The name of the scene that contains the level.
    /// </summary>
    public string SceneName { get => _sceneName; }
}
