using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains a list of levels in the game.
/// </summary>
[CreateAssetMenu(menuName = "Levels/Level list", fileName = "LevelList")]
public class LevelList : ScriptableObject
{
    [SerializeField]
    private LevelMetadata[] _levels;

    public LevelMetadata[] Levels { get => _levels; }
}