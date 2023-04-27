using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Difficulty/SpawningSettings", fileName = "SpawningSettings.asset")]
public class SpawningSettings : ScriptableObject
{
    [SerializeField] int startingEnemies = 3;
    [SerializeField] int minSpawn = 1;
    [SerializeField] int maxSpawn = 3;
    [SerializeField] int spawnLimit = 8;
    [SerializeField] int maxEnemiesAtOnce = 4;

    public int MinSpawn { get => minSpawn; set => minSpawn = value; }
    public int MaxSpawn { get => maxSpawn; set => maxSpawn = value; }
    public int SpawnLimit { get => spawnLimit; set => spawnLimit = value; }
    public int MaxEnemiesAtOnce { get => maxEnemiesAtOnce; set => maxEnemiesAtOnce = value; }
    public int StartingEnemies { get => startingEnemies; set => startingEnemies = value; }
}
