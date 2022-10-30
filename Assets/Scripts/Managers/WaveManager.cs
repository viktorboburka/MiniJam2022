using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

public class WaveManager : MonoBehaviour
{

    [Header("Debug")]
    [SerializeField] private bool drawRays;
    [SerializeField] private bool generateUselessRandom;


    [Header("Spawner Attributes")]
    [SerializeField] private float baseWaveTime = 60;
    [SerializeField] private float spawnRadiusMax = 60;
    [SerializeField] private float spawnRadiusMin = 40;
    [SerializeField] private float aiMaxLimit = 2000;
    [SerializeField] private LayerMask layersForSpawns;

    [Header("Save Time")]
    [SerializeField] private int saveTimeAfter = 5;
    [SerializeField] private float saveTimeLenght = 10;
    [SerializeField] private float saveTimeFadeTimerLenght = 1;
    [SerializeField] private float saveTimeFadeTimerNow = 0;
    [SerializeField] private float saveTimeFade = 1;
    [SerializeField] private float saveTimePostProcessTime = 2;
    [SerializeField] private float saveTimePostProcessWeight = 0;

    [Header("Wave Modifiers")]
    [SerializeField] private int increaseWaveTimeOn = 5;
    [SerializeField] private float increaseWaveTimeBy = 10;
    [SerializeField] private float waveTimeModifier;
    [SerializeField] private int increaseGroupCountOn = 2;
    [SerializeField] private int increaseGroupCountBy = 1;
    [SerializeField] private int increaseGroupSizeOn = 10;
    [SerializeField] private int increaseGroupSizeBy = 1;

    [Header("Next Wave")]
    [SerializeField] public int nextWaveNumber = 1;
    [SerializeField] private float nextWaveIn = 15;
    [SerializeField] private int nextWaveGroupCount = 5;
    [SerializeField] private int nextWaveGroupSize = 1;

    [Header("Stats")]
    [SerializeField] public int waveNumber = 0;
    [SerializeField] public bool isSaveTime = false;
    [SerializeField] public bool isSaveTimeAfterWave = false;
    [SerializeField] public bool wasNextWaveIncomingCalled = false;
    [SerializeField] public bool wasSaveTimeIncomingCalled = false;
    [SerializeField] public bool wasLastWaveIncomingCalled = false;
    [SerializeField] public int enemiesSpawned = 0;
    [SerializeField] public int enemiesKilled = 0;
    [SerializeField] public int enemiesAlive = 0;


    [Header("Other Objects")]
    [SerializeField] private Transform playerT;
    [SerializeField] private PostProcessVolume postProcessVolume;



    [SerializeField] public List<EnemySpawnData> enemyData = new List<EnemySpawnData>();
    [SerializeField] public List<GameObject> enemyList = new List<GameObject>();

    [SerializeField] private Vector3 debugSpawn;
    private int maxSpawnAtOneTime = 100;


    void Awake()
    {
        playerT = GameObject.FindWithTag("Player").transform;
    }

    private void DrawRay(Vector3 a, Vector3 b, Color col, float dur = 1.0f)
    {
        if (drawRays)
            Debug.DrawRay(a, b, col, dur);
    }

    private void DrawSpawnArea(Color col)
    {
            int surfNumMin = 360 / (int)(spawnRadiusMin);
            for (float i = 0; i < 360; i += surfNumMin)
            {
                float radsNow = i * Mathf.Deg2Rad;
                Vector3 posStart = playerT.position + new Vector3(spawnRadiusMin * Mathf.Cos(radsNow), playerT.position.y, spawnRadiusMin * Mathf.Sin(radsNow));
                DrawRay(posStart, Vector3.up * 5, col);
            }

            int surfNumMax = 360 / (int)(spawnRadiusMax);
            for (float i = 0; i < 360; i += surfNumMax)
            {
                float radsNow = i * Mathf.Deg2Rad;
                Vector3 posStart = playerT.position + new Vector3(spawnRadiusMax * Mathf.Cos(radsNow), playerT.position.y, spawnRadiusMax * Mathf.Sin(radsNow));
                DrawRay(posStart, Vector3.up * 5, col);
            }
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

            int spawnChance = enemy.chanceToSpawn;
            if (enemy.modifyChanceToSpawnOn != 0)
                if((float)nextWaveNumber / (float)enemy.modifyChanceToSpawnOn > 1.0f)
                {
                    int modifyChanceToSpawnBy = nextWaveNumber / enemy.modifyChanceToSpawnOn;
                    spawnChance += enemy.modifyChanceToSpawnBy * modifyChanceToSpawnBy;
                    Debug.Log("Spawn Change Modified: " + spawnChance);
                }


            for(int i = 0; i < spawnChance; i++)
                enemyPrefabs.Add(enemy.enemyPrefab);
        }

        if (enemyPrefabs.Count == 0)
            return;
        int enemyToSpawn = Random.Range(0, enemyPrefabs.Count);
        if(aiMaxLimit > (enemiesSpawned - enemiesKilled))
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
        for (int i = 0; i < nextWaveGroupCount; i++)
        {
            if (GetRandomPos(out pos))
            {
                SpawnRandomEnemyGroup(pos, nextWaveGroupSize, 20);
            }
        }
    }

    public void HandleGuaranteedSpawns(int multiplier = 1)
    {
        if (enemyData.Count == 0)
            return;

        foreach (EnemySpawnData enemy in enemyData)
        {
            if (enemy == null)
                continue;



            if (enemy.guaranteedSpawnOn == 0)
                continue;

            if (nextWaveNumber % enemy.guaranteedSpawnOn == 0)
            {
                int guaranteedSpawnCount = enemy.guaranteedSpawnCount;
                if((float)nextWaveNumber / (float)enemy.modifyGuaranteedSpawnCountOn > 1.0f)
                {
                    int modifyguaranteedSpawnCountBy = nextWaveNumber / enemy.modifyGuaranteedSpawnCountOn;
                    guaranteedSpawnCount += enemy.modifyGuaranteedSpawnCountBy * modifyguaranteedSpawnCountBy;
                }
                for(int i = 0; i < multiplier * guaranteedSpawnCount; i++)
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

    void Start()
    {
        waveTimeModifier = baseWaveTime;
    }

    void HandleSaveTime()
    {
        isSaveTime = true;
        nextWaveIn = saveTimeLenght;
        wasSaveTimeIncomingCalled = false;
        wasLastWaveIncomingCalled = false;
    }

    void HandlePostProcess()
    {
        if(postProcessVolume == null)
            return;

        if(isSaveTime)
        {
            if (nextWaveIn < saveTimePostProcessTime)
                saveTimePostProcessWeight = (saveTimePostProcessTime - nextWaveIn) / saveTimePostProcessTime;
        }
        else if(isSaveTimeAfterWave)
        {
            if (nextWaveIn < saveTimePostProcessTime)
                saveTimePostProcessWeight = 1 - (saveTimePostProcessTime - nextWaveIn) / saveTimePostProcessTime;
        }
        postProcessVolume.weight = saveTimePostProcessWeight;
    }

    private void ChangeObjectAlpha(ref GameObject obj, float alpha)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        Color c = renderer.material.color;
        renderer.material.color = new Color(c.r, c.g, c.b, alpha);
    }

    void HandleSaveTimeCleanup()
    {
        GameObject[] bloodObjects = GameObject.FindGameObjectsWithTag("Blood");
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if (bloodObjects.Length == 0 && enemyObjects.Length == 0)
            return;

        if (saveTimeFadeTimerNow > saveTimeFadeTimerLenght)
        {
            foreach(GameObject blood in bloodObjects)
                Destroy(blood);
            foreach(GameObject enemy in enemyObjects)
                Destroy(enemy);
            return;
        }

        saveTimeFadeTimerNow += Time.deltaTime;
        saveTimeFade = 1 - saveTimeFadeTimerNow / saveTimeFadeTimerLenght;

        for (int i = 0; i < bloodObjects.Length; i++)
            ChangeObjectAlpha(ref bloodObjects[i], saveTimeFade);

        GameObject[] enemySprites = GameObject.FindGameObjectsWithTag("EnemySprite");
        for (int i = 0; i < enemySprites.Length; i++)
            ChangeObjectAlpha(ref enemySprites[i], saveTimeFade);
    }


    void HandleWaveModifiers()
    {
        if(nextWaveNumber % increaseGroupCountOn == 0)
            nextWaveGroupCount += increaseGroupCountBy;

        if(nextWaveNumber % increaseGroupSizeOn == 0)
            nextWaveGroupSize += increaseGroupSizeBy;

        if(nextWaveNumber % increaseWaveTimeOn == 0)
            waveTimeModifier += increaseWaveTimeBy;

        nextWaveIn = waveTimeModifier;
        if(isSaveTimeAfterWave)
            nextWaveIn += saveTimeLenght;
    }

    void StartNextWave()
    {
        EndWave();
        saveTimeFade = 1;
        saveTimeFadeTimerNow = 0;
        if (isSaveTime)
            return;

        waveNumber = nextWaveNumber;
        nextWaveNumber++;
        if(waveNumber % saveTimeAfter == 0)
        {
            isSaveTimeAfterWave = true;
        }

        SpawnWave();
        HandleGuaranteedSpawns();
        HandleWaveModifiers();
        wasNextWaveIncomingCalled = false;
    }

    void EndWave()
    {
        if (isSaveTime)
        {
            isSaveTime = false;
            isSaveTimeAfterWave = false;
            return;
        }

        if(waveNumber <= 1)
            return;
        if(waveNumber % saveTimeAfter == 0)
        {
            HandleSaveTime();
        }
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


    void NextWaveIncoming(){
        wasNextWaveIncomingCalled = true;
        Debug.Log("Next Wave Incoming: " + nextWaveNumber);
    }

    void SaveTimeIncoming(){
        wasSaveTimeIncomingCalled = true;
        Debug.Log("Save Time Incoming");
    }

    void LastWaveBeforeSaveTime(){
        wasLastWaveIncomingCalled = true;
        Debug.Log("Last Wave: " + waveNumber);
    }

    void Update()
    {
        HandlePostProcess();
        if(isSaveTime)
            HandleSaveTimeCleanup();
        HandleDeadEnemies();
        DrawSpawnArea(Color.magenta);

        if (isSaveTimeAfterWave && !isSaveTime  && !wasLastWaveIncomingCalled)
            LastWaveBeforeSaveTime();


        if(nextWaveIn < 3 && (!wasNextWaveIncomingCalled && !wasSaveTimeIncomingCalled))
        {
            if(isSaveTimeAfterWave && !isSaveTime)
                SaveTimeIncoming();
            else
                NextWaveIncoming();
        }


        if (generateUselessRandom)
        {
            Vector3 pos;
            GetRandomPos(out pos);
        }
        nextWaveIn -= Time.deltaTime;
        if (nextWaveIn <= 0)
            StartNextWave();
    }

}
