using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject mine;
    public GameObject turret;
    private GameObject currentEnemyToSpawn;
    private Vector3 northSpawn;
    private Vector3 southSpawn;
    private float spawnSwitchChance;
    private int numToSpawn;
    private float numToSpawnTimer;

    public float radius;

    private Vector3 currentSpawnPosition;
    
    public float respawnRate;
    public float numToSpawnIncreaseRate;

    private float spawnTimer;

    void Start()
    {
        northSpawn = new Vector3(0, 100, 1);
        southSpawn = new Vector3(0, -100, 1);
        spawnTimer = respawnRate;
        currentEnemyToSpawn = mine;
        currentSpawnPosition = northSpawn;
        numToSpawn = 1;
        numToSpawnTimer = 0.0f;
    }

    void Update()
    {
        // Spawn enemy every respawnRate seconds
        spawnTimer += Time.deltaTime;
        numToSpawnTimer += Time.deltaTime;
        if (numToSpawnTimer >= numToSpawnIncreaseRate)
        {
            Debug.Log("NumToSpawn increased.");
            numToSpawn++;
            numToSpawnTimer = 0.0f;
        }
        spawnSwitchChance = Random.Range(0.0f, 1f);
        if (spawnTimer >= respawnRate)
        {
            float angle = Random.Range(0, 360);
            //x = x0 + r * cos(theta)
            //y = y0 + r * sin(theta)
            float x = 0 + radius * Mathf.Cos(angle * Mathf.PI / 180);
            float y = 0 + radius * Mathf.Sin(angle * Mathf.PI / 180);
            for (int i = 0; i < numToSpawn; i++) {
                currentSpawnPosition = new Vector3(x + Random.Range(0, 10), y + Random.Range(0, 10), 1); // Leave a little randomness in spawn position so enemies dont spawn on top of each other
                if (numToSpawn > 3 && Random.Range(0, 100) > 80)
                {

                }
                else
                {
                    if (Random.Range(0.0f, 1f) < .25f)
                    {
                        currentEnemyToSpawn = turret;
                    }
                    Instantiate(currentEnemyToSpawn, currentSpawnPosition, Quaternion.identity);
                }
                currentEnemyToSpawn = mine;
            }
            spawnTimer = 0.0f;
        }
    }
}