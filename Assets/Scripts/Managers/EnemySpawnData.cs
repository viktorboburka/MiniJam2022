using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnData", menuName = "ScriptableObjects/WaveManager/EnemySpawnData", order = 1)]
public class EnemySpawnData : ScriptableObject
{
    public GameObject enemyPrefab;
    public int chanceToSpawn;
    public int eliteSpawnEachWave = 0;
}
