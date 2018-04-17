using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject mine;
    private GameObject currentEnemyToSpawn;
    private Vector3 northSpawn;
    private Vector3 southSpawn;
    private float spawnSwitchChance;

    private Vector3 currentSpawnPosition;
    
    public float respawnRate;

    private float spawnTimer;

    void Start()
    {
        northSpawn = new Vector3(0, 100, 1);
        southSpawn = new Vector3(0, -100, 1);
        spawnTimer = respawnRate;
        currentEnemyToSpawn = mine;
        currentSpawnPosition = northSpawn;
    }

    void Update()
    {
        // Spawn enemy every respawnRate seconds
        spawnTimer += Time.deltaTime;
        spawnSwitchChance = Random.Range(0.0f, 1f);
        if (spawnTimer >= respawnRate)
        {
            if (currentSpawnPosition == northSpawn && spawnSwitchChance > .5)
            {
                currentSpawnPosition = southSpawn;
            } else if (currentSpawnPosition == southSpawn & spawnSwitchChance > .5f)
            {
                currentSpawnPosition = northSpawn;
            }
            Instantiate(currentEnemyToSpawn, currentSpawnPosition, Quaternion.identity);
            spawnTimer = 0.0f;
        }
    }
}