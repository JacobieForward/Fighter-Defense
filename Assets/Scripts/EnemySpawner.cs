using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyToSpawn;
    public float spawnDistance;
    public float respawnRate;

    private float spawnTimer;

    void Start() {
        spawnTimer = respawnRate;
    }

    void Update(){
        if (Manager.instance.player != null && Vector3.Distance(Manager.instance.player.transform.position, transform.position) <= spawnDistance) {
            // Spawn enemy every respawnRate seconds
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= respawnRate) {
                Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
                spawnTimer = 0.0f;
            }
        }
    }
}
