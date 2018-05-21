using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject mine;
    public GameObject turret;
    public GameObject enemyFighter;
    private GameObject currentEnemyToSpawn;
    
    private int numToSpawn;
    private float numToSpawnTimer;

    private int roundNumber;
    private float roundEndTime;
    private float roundEndTimer;

    public float radius;

    private Vector3 currentSpawnPosition;
    
    public float respawnRate;
    public float numToSpawnIncreaseRate;

    private float spawnTimer;

    void Start()
    {
        spawnTimer = respawnRate;
        currentEnemyToSpawn = mine;
        numToSpawn = 1;
        numToSpawnTimer = 0.0f;
        roundNumber = 1;
        roundEndTimer = 0.0f;
        roundEndTime = Random.Range(20.0f, 40.0f);
    }

    void Update()
    {
        if (roundEndTimer <= roundEndTime)
        {
            SpawnEnemies();
        } else {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= roundNumber)
            {
                roundNumber += 1;
                roundEndTimer = 0.0f;
                roundEndTime = Random.Range(20.0f * (roundNumber / 0.5f), 40.0f * (roundNumber / 0.5f));
                Debug.Log(roundNumber);
            }
        }
    }

    void SpawnEnemies()
    {
        // Spawn enemy every respawnRate seconds
        spawnTimer += Time.deltaTime;
        numToSpawnTimer += Time.deltaTime;
        roundEndTimer += Time.deltaTime;
        if (numToSpawnTimer >= numToSpawnIncreaseRate)
        {
            Debug.Log("NumToSpawn increased.");
            numToSpawn++;
            numToSpawnTimer = 0.0f;
        }
        if (spawnTimer >= respawnRate)
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
        }
    }
}