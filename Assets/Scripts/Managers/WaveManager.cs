using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class WaveManager : MonoBehaviour
{

    [Header("Debug")]
    [SerializeField] private bool drawRays;


    [Header("Spawner Attributes")]
    [SerializeField] private float defaultWaveTime = 60;
    [SerializeField] private float spawnRadiusMax = 60;
    [SerializeField] private float spawnRadiusMin = 40;
    [SerializeField] private LayerMask layersForSpawns;

    [Header("Wave Modifiers")]
    [SerializeField] private int increaseGroupCountEachXWave = 2;
    [SerializeField] private int increaseGroupSizeEachXWave = 10;

    [Header("Next Wave")]
    [SerializeField] public int waveNumber = 1;
    [SerializeField] private float nextWaveIn = 15;
    [SerializeField] private int nextWaveGroups = 5;
    [SerializeField] private int nextWaveEnemiesInGroup = 1;

    [Header("Stats")]
    [SerializeField] public int actualWaveNumber = 0;
    [SerializeField] public int enemiesSpawned = 0;
    [SerializeField] public int enemiesKilled = 0;
    [SerializeField] public int enemiesAlive = 0;


    [Header("Other Objects")]
    [SerializeField] private Transform playerT;



    [SerializeField] public List<EnemySpawnData> enemyData = new List<EnemySpawnData>();
    [SerializeField] public List<GameObject> enemyList = new List<GameObject>();

    [SerializeField] private Vector3 debugSpawn;
    private int maxSpawnAtOneTime = 100;

    private void DrawRay(Vector3 a, Vector3 b, Color col, float dur = 1.0f)
    {
        if (drawRays)
            Debug.DrawRay(a, b, col, dur);
    }




    public void SpawnRandomEnemyGroup(Vector3 pos, int count, int spacer = 1)
    {
        if(count > maxSpawnAtOneTime)
            count = maxSpawnAtOneTime;

        for(int i = 5; i < count + 5; i++)
        {
            float space = 60 - i * 0.25f + spacer;
            float rads = i * space * Mathf.Deg2Rad;
            Vector3 spawnPos = pos + new Vector3(i * 0.25f * Mathf.Cos(rads), pos.y, i * 0.25f * Mathf.Sin(rads));
            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPos, out hit, 50.0f, NavMesh.AllAreas))
            {
                SpawnRandomEnemy(hit.position);
            }
        }
    }



    public bool IsValidSpawnPoint(Vector3 pos)
    {

        RaycastHit hitRay;
        if (Physics.Raycast(pos, Vector3.down, out hitRay, Mathf.Infinity, layersForSpawns))
        {
            NavMeshHit hitNav;
            if (NavMesh.SamplePosition(pos, out hitNav, Mathf.Infinity, NavMesh.AllAreas))
            {
                return true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    

    public void SpawnRandomEnemy(Vector3 pos)
    {
        if (enemyData.Count == 0)
            return;

        List<GameObject> enemyPrefabs = new List<GameObject>();
        foreach (EnemySpawnData enemy in enemyData)
        {
            if (enemy == null)
                continue;

            for(int i = 0; i < enemy.chanceToSpawn; i++)
                enemyPrefabs.Add(enemy.enemyPrefab);
        }

        if (enemyPrefabs.Count == 0)
            return;
        int enemyToSpawn = Random.Range(0, enemyPrefabs.Count);
        SpawnEnemy(enemyPrefabs[enemyToSpawn], pos);
    }


    public void SpawnEnemy(GameObject enemyPrefab, Vector3 pos)
    {
        enemiesSpawned++;
        DrawRay(pos, Vector3.up * 5, Color.green, 15.0f);
        enemyList.Add(Instantiate(enemyPrefab, pos, Quaternion.identity));
    }

    public bool GetRandomPos(out Vector3 randomPos)
    {
        int numberOfTries = 0;
        Vector3 dir = new Vector3(0,0,0);
        do
        {
            Vector2 random = (Random.insideUnitCircle * spawnRadiusMax);
            randomPos = new Vector3(random.x + playerT.position.x, playerT.position.y, random.y + playerT.position.z);
            var heading = playerT.position - randomPos;
            var distance = heading.magnitude;
            dir = heading / distance;
            if (distance < spawnRadiusMin)
            {
                randomPos -= dir * (spawnRadiusMin - distance);
                heading = playerT.position - randomPos;
                distance = heading.magnitude;
            }
            if(IsValidSpawnPoint(randomPos))
            {
                DrawRay(randomPos, Vector3.up, Color.blue);
                return true;
            }
            else
            {
                randomPos += dir * distance * 2;
                if(IsValidSpawnPoint(randomPos))
                {
                    DrawRay(randomPos, Vector3.up, Color.blue);
                    return true;
                }
                else
                {
                    DrawRay(randomPos, Vector3.up, Color.red);
                }
            }

            numberOfTries++;
            if (numberOfTries > 1000)
                return false;
        } while(false);
        return false;
    }


    void SpawnWave()
    {
        Vector3 pos;
        for (int i = 0; i < nextWaveGroups; i++)
        {
            if (GetRandomPos(out pos))
            {
                SpawnRandomEnemyGroup(pos, nextWaveEnemiesInGroup, 20);
            }
        }
    }

    public void HandleElitesSpawn(int multiplier = 1)
    {
        if (enemyData.Count == 0)
            return;

        foreach (EnemySpawnData enemy in enemyData)
        {
            if (enemy == null)
                continue;



            if (enemy.eliteSpawnEachWave == 0)
                continue;

            if (waveNumber % enemy.eliteSpawnEachWave == 0)
            {
                for(int i = 0; i < multiplier; i++)
                {
                    Vector3 pos;
                    if (GetRandomPos(out pos))
                    {
                        SpawnEnemy(enemy.enemyPrefab, pos);
                    }
                }
            }
        }
    }


    void StartNextWave()
    {
        actualWaveNumber = waveNumber;
        SpawnWave();
        HandleElitesSpawn();
        waveNumber++;
        if(waveNumber % increaseGroupCountEachXWave == 0)
            nextWaveGroups++;

        if(waveNumber % increaseGroupSizeEachXWave == 0)
            nextWaveEnemiesInGroup++;
        nextWaveIn = defaultWaveTime;

    }

    void HandleDeadEnemies()
    {
        List<int> deadEnemies = new List<int>();

        for (int i = 0; i < enemyList.Count; i++)
        {
            if(enemyList[i] == null)
            {
                deadEnemies.Add(i);
            }
        }
        enemiesKilled = deadEnemies.Count;
        enemiesAlive = enemiesSpawned - enemiesKilled;
    }


    void Update()
    {
        HandleDeadEnemies();
        Vector3 pos;
        GetRandomPos(out pos);
        nextWaveIn -= Time.deltaTime;
        if (nextWaveIn <= 0)
            StartNextWave();
    }

}
