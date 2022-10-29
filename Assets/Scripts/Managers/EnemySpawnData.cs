using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnData", menuName = "ScriptableObjects/WaveManager/EnemySpawnData", order = 1)]
public class EnemySpawnData : ScriptableObject
{
    [Header("Enemy Data")]
    public GameObject enemyPrefab;
    [Header("Chance To Spawn")]
    public int chanceToSpawn;
    public int modifyChanceToSpawnOn = 2;
    public int modifyChanceToSpawnBy = 1;
    [Header("Guaranteed Spawn")]
    public int guaranteedSpawnOn = 0;
    public int guaranteedSpawnCount = 1;
    public int modifyGuaranteedSpawnCountOn = 0;
    public int modifyGuaranteedSpawnCountBy = 0;
}
