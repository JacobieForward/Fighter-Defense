using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    private float spawnRadius;
    private int roundNumber;

    public GameObject mine;
    public GameObject turret;
    public GameObject fighter;
    private GameObject currentEnemyToSpawn;
    private Vector3 currentSpawnPosition;

    private int numAllies; // The more allies the player has, the harder things get
    private int numPlayerDeaths; // The more the player dies, the easier things get
    private float playTime; // The longer the game has been played, the harder things get, slow curve
    private int baseEnemyNum;

    private int numEnemiesThisRound;
    private int maxEnemiesActiveThisRound;
    private float spawnRate;

    private int enemiesSpawnedThisRound;
    private float spawnTimer;
    private int maxNumSpawnTogether;
    private bool newRound;
    private int maxPlayTimeModifier;

    /*private int numToSpawn;
    private float numToSpawnTimer;

    private float roundEndTime;
    private float roundEndTimer;
    private float minRoundTime;
    private float maxRoundTime;
    
    public float respawnRate;
    public float numToSpawnIncreaseRate;
    private int maxEnemies;
    private GameObject[] enemies;*/

    void Start()
    {

        currentEnemyToSpawn = mine; // Mine is a placeholder here in order to prevent nullpointers
        roundNumber = 1;
        spawnRadius = 200;

        // Variables taken from gamestate to determine difficulty
        numAllies = 0;
        numPlayerDeaths = 0;
        playTime = 0.0f;
        baseEnemyNum = 20;

        // Variables that determine difficulty
        numEnemiesThisRound = 20;
        maxEnemiesActiveThisRound = 50;
        spawnRate = 1.5f;

        enemiesSpawnedThisRound = 0;
        spawnTimer = 0.0f;
        maxNumSpawnTogether = 2;
        newRound = false;
        /* OLD VARS
        minRoundTime = 20.0f;
        maxRoundTime = 40.0f;

        maxEnemies = 50;

        roundEndTime = Random.Range(minRoundTime, maxRoundTime);
        numToSpawn = 1;
        numToSpawnTimer = 0.0f;
        spawnTimer = respawnRate;
        roundEndTimer = 0.0f;
        */
    }

    // It all starts here. This method gathers information at the beginning of each round in order to set the variables that determine
    // That round's difficulty
    void GatherGamestateInformation()
    {
        // It is not round 1. Set all variables according to current gamestate
        numAllies = GameObject.FindGameObjectsWithTag("Player").Length - 1;
        numPlayerDeaths = Manager.instance.GetPlayerDeaths();
        maxEnemiesActiveThisRound = 50 + (numAllies * 2);
        playTime = Manager.instance.GetPlayTime();
    }

    void SetDifficulty()
    {
        numEnemiesThisRound = baseEnemyNum + (int)(playTime/3) + numAllies - (numPlayerDeaths * 3);
        spawnRate = 1.5f - (playTime / 2500) - (float) (numAllies * 0.007) + (float) (numPlayerDeaths * 0.02);
        maxNumSpawnTogether = (numEnemiesThisRound / 30);
        if (maxNumSpawnTogether < 1)
        {
            maxNumSpawnTogether = 2;
        }
        Debug.Log("numEnemiesThisRound: " + numEnemiesThisRound);
        Debug.Log("spawnRate: " + spawnRate);
    }

    // Wrapper for all functions that deal with spawning an enemy when the spawning criteria are met
    void Spawn()
    {
        for (int i = 0; i < Random.Range(1, maxNumSpawnTogether); i++)
        {
            RandomiseSpawnPosition();
            ChooseEnemyToSpawn();
            InstantiateEnemy();
            enemiesSpawnedThisRound += 1;
        }
        spawnTimer = 0.0f;
    }

    void RandomiseSpawnPosition()
    {
        float angle = Random.Range(0, 360);
        //x = x0 + r * cos(theta)
        //y = y0 + r * sin(theta)
        float x = 0 + spawnRadius * Mathf.Cos(angle * Mathf.PI / 180);
        float y = 0 + spawnRadius * Mathf.Sin(angle * Mathf.PI / 180);
        currentSpawnPosition = new Vector3(x + Random.Range(0, 10), y + Random.Range(0, 10), 1); // Leave a little randomness in spawn position so enemies dont spawn on top of each other
    }

    void ChooseEnemyToSpawn()
    {
        float spawnChance = Random.Range(0.0f, 1f);
        currentEnemyToSpawn = mine;
        if (spawnChance < .30f && roundNumber > 1)
        {
            currentEnemyToSpawn = turret;
        }
        if (spawnChance < .18f && roundNumber > 2)
        {
            currentEnemyToSpawn = fighter;
        }
    }

    void InstantiateEnemy()
    {
        // Mines spawn twice
        if (currentEnemyToSpawn = mine)
        {
            Instantiate(currentEnemyToSpawn, currentSpawnPosition, Quaternion.identity);
            RandomiseSpawnPosition();
        }
        Instantiate(currentEnemyToSpawn, currentSpawnPosition, Quaternion.identity);
    }

    void NewRound()
    {
        roundNumber += 1;
        GatherGamestateInformation();
        SetDifficulty();
        enemiesSpawnedThisRound = 0;
        spawnTimer = 0;
        newRound = false;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if ((spawnTimer >= spawnRate) && (enemiesSpawnedThisRound < numEnemiesThisRound) &&
            (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemiesActiveThisRound))
        {
            Spawn();
            newRound = true;
        } else if ((GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) && newRound == true)
        {
            NewRound();
        }
        /*if (Manager.instance.tutorial)
        {
            return;
        }
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (roundEndTimer <= roundEndTime)
        {
            SpawnEnemies();
        } else {
            if (enemies.Length <= 1)
            {
                roundNumber += 1;
                roundEndTimer = 0.0f;
                roundEndTime = Random.Range(minRoundTime, maxRoundTime);
                Debug.Log(roundNumber);
            }
        }*/
    }

    /* void SpawnEnemies()
     {
         // Spawn enemy every respawnRate seconds
         spawnTimer += Time.deltaTime;
         numToSpawnTimer += Time.deltaTime;
         roundEndTimer += Time.deltaTime;
         if ((numToSpawnTimer >= numToSpawnIncreaseRate) && numToSpawn < 4)
         {
             Debug.Log("NumToSpawn increased.");
             numToSpawn++;
             numToSpawnTimer = 0.0f;
         }
         if ((spawnTimer >= respawnRate) && ((enemies.Length - GameObject.FindGameObjectsWithTag("Player").Length) < maxEnemies))
         {
             float angle = Random.Range(0, 360);
             //x = x0 + r * cos(theta)
             //y = y0 + r * sin(theta)
             float x = 0 + radius * Mathf.Cos(angle * Mathf.PI / 180);
             float y = 0 + radius * Mathf.Sin(angle * Mathf.PI / 180);
             for (int i = 0; i < numToSpawn; i++)
             {
                 currentSpawnPosition = new Vector3(x + Random.Range(0, 10), y + Random.Range(0, 10), 1); // Leave a little randomness in spawn position so enemies dont spawn on top of each other
                 if (numToSpawn > 3 && Random.Range(0, 100) > 80)
                 {

                 }
                 else
                 {
                     float spawnChance = Random.Range(0.0f, 1f);
                     if (spawnChance < .25f && roundNumber >= 2)
                     {
                         currentEnemyToSpawn = turret;
                     }
                     if (spawnChance < .10f && roundNumber >= 3)
                     {
                         currentEnemyToSpawn = enemyFighter;
                     }
                     Instantiate(currentEnemyToSpawn, currentSpawnPosition, Quaternion.identity);
                 }
                 currentEnemyToSpawn = mine;
             }
             spawnTimer = 0.0f;
             if (respawnRate > 1.5f)
             {
                 respawnRate -= 0.01f;
             }
         }
     }*/
}