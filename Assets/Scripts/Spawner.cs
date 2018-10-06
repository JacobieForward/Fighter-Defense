using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour {

    private float spawnRadius;
    private int roundNumber;

    public GameObject mine;
    public GameObject turret;
    public GameObject fighter;
    private GameObject currentEnemyToSpawn;
    private Vector3 currentSpawnPosition;
    private float xSpawnPosition;
    private float ySpawnPosition;

    private int numAllies; // The more allies the player has, the harder things get
    private int numPlayerDeaths; // The more the player dies, the easier things get
    private float playTime; // The longer the game has been played, the harder things get, slow curve
    private int baseEnemyNum;

    private int numEnemiesThisRound;
    private int maxEnemiesActiveThisRound;
    private float defaultSpawnRate;
    private float spawnRate;

    private int enemiesSpawnedThisRound;
    private float spawnTimer;
    private int maxNumSpawnTogether;
    private bool newRound;
    private int maxPlayTimeModifier;
    private float playTimeModifier;

    void Start()
    {
        currentEnemyToSpawn = mine; // Mine is a placeholder here in order to prevent nullpointers
        roundNumber = 1;
        spawnRadius = 200;
        xSpawnPosition = 0.0f;
        ySpawnPosition = 0.0f;

        // Variables taken from gamestate to determine difficulty
        numAllies = 0;
        numPlayerDeaths = 0;
        playTime = 0.0f;
        baseEnemyNum = 20;

        // Variables that determine difficulty
        numEnemiesThisRound = 20;
        maxEnemiesActiveThisRound = 50;
        defaultSpawnRate = 1.2f;
        spawnRate = 1.2f;

        enemiesSpawnedThisRound = 0;
        spawnTimer = 0.0f;
        maxNumSpawnTogether = 2;
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


    // It all starts here. This method gathers information at the beginning of each round in order to set the variables that determine
    // That round's difficulty
    void GatherGamestateInformation()
    {
        // It is not round 1. Set all variables according to current gamestate
        numAllies = GameObject.FindGameObjectsWithTag("Player").Length - 1;
        numPlayerDeaths = Manager.instance.GetPlayerDeaths();
        maxEnemiesActiveThisRound = 50 + (numAllies * 2);
        playTime = Manager.instance.GetPlayTime();

        // Decide if it is a flip round or not
        // A flip round is a round in which the player is hard pressed to survive, difficulty is modified
        // When a flip round occurs to give the player a chance to bounce back
        if (numAllies <= 10 && playTime > 200 && roundNumber > 2 &&
            (Manager.instance.GetAlliesDestroyedThisRound() + (numPlayerDeaths * 2) + (Manager.instance.GetStationHealthLostThisRound() * 2)) >= (playTime / 25))
        {
            Debug.Log("FLIP ROUND: " + roundNumber);
            playTimeModifier += (playTime / 1.15f);
        }
        // After playtimemodifier may or may not be changed subtract it from playTime;
        playTime -= playTimeModifier;

        // Cleanup info only stored for one round
        Debug.Log("Number of Allies Destroyed: " + Manager.instance.GetAlliesDestroyedThisRound());
        Debug.Log("Station Health lost last round: " + Manager.instance.GetStationHealthLostThisRound());
        CleanupRoundInformation();
    }

    void CleanupRoundInformation()
    {
        Manager.instance.ResetAlliesDestroyedCounter();
        Manager.instance.ResetStationHealthLostCounter();
    }

    void SetDifficulty()
    {
        numEnemiesThisRound = baseEnemyNum + (int)(playTime / 3) + numAllies - (numPlayerDeaths * 3);
        spawnRate = defaultSpawnRate - (playTime / 2500) - (float)(numAllies * 0.008) + (float)(numPlayerDeaths * 0.02);
        maxNumSpawnTogether = (numEnemiesThisRound / 30);
        if (maxNumSpawnTogether < 1)
        {
            maxNumSpawnTogether = 2;
        }
        if (SceneManager.GetActiveScene().name.Equals("MapTwo"))
        {
            numEnemiesThisRound *= 1.6;
            spawnRate /= 1.6;
        }
        Debug.Log("numEnemiesThisRound: " + numEnemiesThisRound);
        Debug.Log("spawnRate: " + spawnRate);
    }

    // Wrapper for all functions that deal with spawning an enemy when the spawning criteria are met
    void Spawn()
    {
        AssignSpawnPosition();
        for (int i = 0; i < Random.Range(1, maxNumSpawnTogether); i++)
        {
            RandomizeSpawnPosition();
            if (!SceneManager.GetActiveScene().name.Equals("MapTwo")) {
                ChooseEnemyToSpawn();
            }
            InstantiateEnemy();
            enemiesSpawnedThisRound += 1;
        }
        spawnTimer = 0.0f;
    }

    void AssignSpawnPosition()
    {
        float angle = Random.Range(0, 360);
        //x = x0 + r * cos(theta)
        //y = y0 + r * sin(theta)
        xSpawnPosition = 0 + spawnRadius * Mathf.Cos(angle * Mathf.PI / 180);
        ySpawnPosition = 0 + spawnRadius * Mathf.Sin(angle * Mathf.PI / 180);
        RandomizeSpawnPosition();
    }

    void RandomizeSpawnPosition()
    {
        // Leave a little randomness in spawn position so enemies dont spawn on top of each other
        currentSpawnPosition = new Vector3(xSpawnPosition + Random.Range(0, 6), ySpawnPosition + Random.Range(0, 6), 1);
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
        Instantiate(currentEnemyToSpawn, currentSpawnPosition, Quaternion.identity);
    }
}