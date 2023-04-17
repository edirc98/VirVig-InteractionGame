using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level {
    public enum LevelType
    {
        COLORS,
        CODES
    }
    public string LevelName;
    public LevelType levelType;
    public GameObject LevelPrefab;
}
public class Levels : MonoBehaviour
{
    public List<Level> GameLevels;       
}
